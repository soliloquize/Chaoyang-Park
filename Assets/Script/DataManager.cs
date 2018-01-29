using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleJson;

public delegate bool DataToModule(JsonObject json);

public class DataManager: GameSingleton<DataManager>  
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public JsonArray GetJSONArrayFromFile(string filePath)
    {
        TextAsset textAsset = Resources.Load(filePath) as TextAsset;
		if (textAsset == null)
		{
			return null;
		}

        JsonArray array = SimpleJson.SimpleJson.DeserializeObject<JsonArray>(textAsset.text);
        return array;
    }

    public JsonObject GetJSONObjectFromFile(string filePath)
    {
        TextAsset textAsset = Resources.Load(filePath) as TextAsset;
		if (textAsset == null)
		{
			return null;
		}

        JsonObject json = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(textAsset.text);
        return json;
    }
}