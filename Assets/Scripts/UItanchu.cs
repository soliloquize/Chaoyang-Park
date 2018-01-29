using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UItanchu : MonoBehaviour {
	public bool play= true;
	public float time = 0.3f;
	private  float playI = 0f;
	private  Vector3 oldTran;
	// Use this for initialization
	void Start () {
		oldTran = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z);
	}

	void OnEnable()
	{
		play= true;
	}
	// Update is called once per frame
	void Update () {
		if (play == true && playI <= time) {
			playI += Time.deltaTime;
			transform.localScale =  new Vector3(oldTran.x,oldTran.y,oldTran.z)*Mathf.Sin(playI/time*Mathf.PI/2);
			if (playI > time) {
				transform.localScale = new Vector3(oldTran.x,oldTran.y,oldTran.z);
				play = false;
				playI = 0;
			}
		} 
	}
}
