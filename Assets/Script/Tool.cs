using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour {
    private static LineRenderer GetLineRenderer(Transform t)
    {
        LineRenderer lr = t.GetComponent<LineRenderer>();
        if (lr == null)
        {
            lr = t.gameObject.AddComponent<LineRenderer>();
        }
        lr.SetWidth(0.1f, 0.1f);
        return lr;
    }

    public static void DrawLine(Transform t, Vector3 start, Vector3 end)
    {
        LineRenderer lr = GetLineRenderer(t);
        lr.SetVertexCount(2);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public static void DrawSector(Transform t, Vector3 center, float angle, float radius)
    {
        LineRenderer lr = GetLineRenderer(t);
        int pointAmount = 100;//点的数目，值越大曲线越平滑    
        float eachAngle = angle / pointAmount;
        Vector3 forward = t.forward;

        lr.SetVertexCount(pointAmount);
        lr.SetPosition(0, center);
        lr.SetPosition(pointAmount - 1, center);

        for (int i = 1; i < pointAmount - 1; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, -angle / 2 + eachAngle * (i - 1), 0f) * forward * radius + center;
            lr.SetPosition(i, pos);
        }
    }

    public static bool CheckIfSeen(Transform t,Vector3 center,float angle, float radius,Transform player)
    {
        //Debug.Log(Physics.Raycast(center + 0.5f * Vector3.up, player.position - center, radius, 1 << 8).ToString()
        //    + center.ToString()+player.position.ToString());
        RaycastHit hit;
        //Vector3 pos = Quaternion.Euler(0f, -angle / 2, 0f) * t.forward * radius + center;
        if (Mathf.Abs(Vector3.Angle(t.forward, player.position - t.position)) < angle / 2
            && Vector3.Distance(t.position, player.position) < radius
            && Physics.Raycast(center, player.position - center, out hit, radius, 1 << 9))
        {
            if ( hit.distance > Vector3.Distance(t.position, player.position))
                return true;
            else
                return false;
        }
        else
            return false;
    }
}
