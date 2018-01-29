
using UnityEngine;
using System.Collections;

public class GlobalTool : MonoBehaviour
{

    public static string LoadUIName = "";
    public static string LoadSceneName = "";

    public static bool Contain3DScene = false;

    public static GameObject FindChild(Transform trans, string childName)
    {
        Transform child = trans.Find(childName);
        if (child != null)
        {
            return child.gameObject;
        }

		GameObject go = null;
        int count = trans.childCount;
        for (int i = 0; i < count; ++i)
        {
            child = trans.GetChild(i);
            go = FindChild(child, childName);
            if (go != null)
			{
                return go;
			}
        }

        return null;
    }

    public static T FindChild<T>(Transform trans, string childName) where T : Component
    {
        GameObject go = FindChild(trans, childName);
        if (go == null)
		{
            return null;
		}

        return go.GetComponent<T>();
    }

}
