using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YaoHuang : MonoBehaviour {
	public float zhouqiTime = 1;
	public float stopTime = 0f;
	public int zhouqiCishu = 1;
	public Vector3 movePosition = new Vector3(1,1,1);
	private float playI = 0f;
	private float stopI = 0f;
	private int cishuI = 0;
	private Vector3 oldPosition;

	// Use this for initialization
	void Start () {
		oldPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (cishuI < zhouqiCishu) {
			playI += Time.deltaTime;
			if (playI > zhouqiTime) {
				playI = 0;
				cishuI++;
			}
			transform.localPosition = oldPosition + movePosition/2 - movePosition * Mathf.Sin (playI / zhouqiTime * Mathf.PI * 2+Mathf.PI/2)/2;
		} else if (stopI <= stopTime) {
			stopI += Time.deltaTime;
			if (stopI > stopTime) {
				stopI = 0;
				cishuI = 0;
			}
		}
	}
}
