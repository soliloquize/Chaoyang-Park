using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBblAdaptor : MonoBehaviour {

    //public string msgStr;
    public Text msgText;
    public RectTransform msgBbl;
    public float msgHeight;
    //public float bblWidth;
    public float bblHeight;
    public float heightOff;


	public void SetString (string s) { 
        //print(msgBbl.rect + "," + msgBbl.rect.height + "," + msgBbl.sizeDelta);
        //msgText = this.transform.GetComponentInChildren<Text>();
        heightOff = msgBbl.rect.height - msgHeight;
        msgText.text = s;
        msgHeight = msgText.preferredHeight;
        
        bblHeight = msgHeight + heightOff;
        var curBbl = msgBbl.GetComponent<RectTransform>();
        curBbl.sizeDelta = new Vector2(msgBbl.sizeDelta.x, bblHeight);
        print("new bbl height is" + msgBbl.rect.height);
	}

}
