
using UnityEngine;
using System;
using System.Collections;

using SimpleJson;
using System.Collections.Generic;

public class DialogueManager : GameSingleton<DialogueManager>
{
	public enum Singer
    {
        teller = 0,
        listener = 1
    }

    public enum DiaGroupType
    {
        Idle = 0,
        ToPlayer = 1,
        ToNPC = 2,
        Weiguan = 3,
        Boss = 4,
        Success = 5,
        Fail = 6  
    }
    
	private JsonObject dataGroup = null;
    private JsonObject dataWords = null;

    public override void Initialize()
	{
		base.Initialize();
        dataGroup = DataManager.Singleton.GetJSONObjectFromFile("Tab/data_group");
        dataWords = DataManager.Singleton.GetJSONObjectFromFile("Tab/data_words");
    }
	
	public List<JsonObject> GetCertainDiaGroupTypeData(DiaGroupType type)
    {
        List<JsonObject> joList = new List<JsonObject>();
        foreach (var key in dataGroup.Keys)
        {
            JsonObject jo;
            jo = JsonDataParser.GetJsonObject(dataGroup, key);
            string s = JsonDataParser.GetString(jo, "groupType");
            if (((int)type).ToString() == s)
                joList.Add(jo);
        }
        return joList;
    }

    public string GetWordsById(string key)
    {
        JsonObject jo = JsonDataParser.GetJsonObject(dataWords, key);
        string s = JsonDataParser.GetString(jo, "content");
        return s;
    }
	
}
