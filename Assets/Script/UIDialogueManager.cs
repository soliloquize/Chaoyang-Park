using SimpleJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueManager : GameSingleton<UIDialogueManager> {

    [HideInInspector]
    public Transform teller;
    [HideInInspector]
    public Transform listener;

    public float dialogueStopTime = 1.5f;

    public GameObject UIDiaTeller;
	public GameObject UIDiaListener;

    public delegate void DialogueEndDelegate();
    public DialogueEndDelegate onDialogueEnd;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (teller != null && listener != null)
        {
            UIDiaTeller.GetComponent<ObjectToUICoor>().SetWorldObjTrans(teller);
            UIDiaListener.GetComponent<ObjectToUICoor>().SetWorldObjTrans(listener);
        }
    }

    public void MakeDialogue(Transform teller,Transform listener, DialogueManager.DiaGroupType type)
    {
        this.teller = teller;
        this.listener = listener;
        if (teller != GameObject.FindWithTag("Player").transform)
            GlobalTool.FindChild<SpriteRenderer>(teller, "SpriteIcon").sprite = Resources.Load<Sprite>("Icon/" + GlobalTool.FindChild<SpriteRenderer>(teller, "SpriteIcon").sprite.name[0] + "B");
		if (listener != GameObject.FindWithTag("Player").transform&& listener != GameObject.FindWithTag("Target").transform)
            GlobalTool.FindChild<SpriteRenderer>(listener, "SpriteIcon").sprite = Resources.Load<Sprite>("Icon/" + GlobalTool.FindChild<SpriteRenderer>(listener, "SpriteIcon").sprite.name[0] + "B");
        StopCoroutine("DialogueBegin");
        StartCoroutine(DialogueBegin(type));
    }

    IEnumerator DialogueBegin(DialogueManager.DiaGroupType type)
    {
        List<JsonObject> joList = DialogueManager.Singleton.GetCertainDiaGroupTypeData(type);
        int i = Random.Range(0, joList.Count);
        string[] contents = JsonDataParser.GetStringArray(joList[i], "contents", '|');
        string singer = JsonDataParser.GetString(joList[i], "singer");

        int j = 0;
        while (j<singer.Length)
        {
            UIDiaTeller.SetActive(false);
            UIDiaListener.SetActive(false);
            if (singer.Substring(j,1) == ((int)DialogueManager.Singer.teller).ToString())
            {
                UIDiaTeller.SetActive(true);
                GlobalTool.FindChild<Text>(UIDiaTeller.transform, "Text").text = DialogueManager.Singleton.GetWordsById(contents[j]);
                UIDiaTeller.GetComponent<ChatBblAdaptor>().SetString(DialogueManager.Singleton.GetWordsById(contents[j]));
            }
            else
            {
                UIDiaListener.SetActive(true);
                GlobalTool.FindChild<Text>(UIDiaListener.transform, "Text").text = DialogueManager.Singleton.GetWordsById(contents[j]);
                UIDiaListener.GetComponent<ChatBblAdaptor>().SetString(DialogueManager.Singleton.GetWordsById(contents[j]));
            }
            ++j;
            yield return new WaitForSeconds(dialogueStopTime);
        }
        UIDiaTeller.SetActive(false);
        UIDiaListener.SetActive(false);
        if (teller != GameObject.FindWithTag("Player").transform)
            GlobalTool.FindChild<SpriteRenderer>(teller, "SpriteIcon").sprite = Resources.Load<Sprite>("Icon/" + GlobalTool.FindChild<SpriteRenderer>(teller, "SpriteIcon").sprite.name[0] + "A");
		if (listener != GameObject.FindWithTag("Player").transform&& listener != GameObject.FindWithTag("Target").transform)
            GlobalTool.FindChild<SpriteRenderer>(listener, "SpriteIcon").sprite = Resources.Load<Sprite>("Icon/" + GlobalTool.FindChild<SpriteRenderer>(listener, "SpriteIcon").sprite.name[0] + "A");

        if (onDialogueEnd != null)
            onDialogueEnd();
        onDialogueEnd = null;
    }
}
