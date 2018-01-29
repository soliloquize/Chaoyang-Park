using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectToUICoor : MonoBehaviour {
    [SerializeField]
    Camera UICam;
    [SerializeField]
    Transform worldObj;
    [SerializeField]
    Transform UIObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () { 
        
    }

    public void SetWorldObjTrans(Transform world)
    {
        worldObj = world;
        UIObj.localPosition = UICam.WorldToScreenPoint(worldObj.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0) + new Vector3(-20, 20, 0);
    }
}
