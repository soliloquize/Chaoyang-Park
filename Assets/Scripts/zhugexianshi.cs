using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zhugexianshi : MonoBehaviour {
	public float delayTime = 1;
	public float showTime = 0.5f;
	public float stopTime = 2;
	private int neirongI = 0;
	private float playI = 0;
	// Use this for initialization
	void Start () {
		transform.gameObject.GetComponent<Image> ().color = new Color(1,1,1,0);
	}
	
	// Update is called once per frame
	void Update () {
			if (playI < delayTime) {
				playI += Time.deltaTime;
			} else if (playI <= delayTime + showTime) {
				playI += Time.deltaTime;
			transform.gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, (playI - delayTime) / showTime);
			} else if (playI <= delayTime + showTime + stopTime) {
				playI += Time.deltaTime;
			} else if (playI <= delayTime + showTime * 2 + stopTime) {
				playI += Time.deltaTime;
			transform.gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, 1-(playI - delayTime - showTime - stopTime) / showTime);
			}
	}
}
