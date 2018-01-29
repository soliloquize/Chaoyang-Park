using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class DataConvert {

	public static string GetString(object value)
	{
		return value.ToString();
	}

	public static int GetInt(object value)
	{
		int result = 0;
		return int.TryParse(GetString(value), out result)? result : 0;
	}

	public static float GetFloat(object value)
	{
		float result = 0.0f;
		return float.TryParse(GetString(value), out result)? result : 0.0f;
	}

	public static float GetFloatRound(object value, int digits)
	{
		return (float)Math.Round((decimal)GetFloat(value), digits, MidpointRounding.AwayFromZero);
	}

	public static bool GetIntBool(object value)
	{
		return GetInt(value) == 0 ? false : true;
	}

	public static bool GetBool(object value)
	{
		bool result = false;
		return bool.TryParse(GetString(value), out result)? result : false;
	}

}

public class JsonDataParser {

    public static string GetString(JsonObject config, string key)
    {
		object value = null;
//		return config.TryGetValue(key, out value) ? value.ToString() : "";

		bool res = config.TryGetValue(key, out value);
		if (res)
		{
			return (value != null)? value.ToString() : "";
		}
		else
		{
			return "";
		}
    }

    public static int GetInt(JsonObject config, string key)
	{
		int value = 0;
		return int.TryParse(GetString(config, key), out value) ? value : 0;
    }

    public static float GetFloat(JsonObject config, string key)
	{
		float value = 0.0f;
		return float.TryParse(GetString(config, key), out value) ? value : 0.0f;
    }

	public static float GetFloatRound(JsonObject config, string key, int digits)
	{
		return (float)Math.Round((decimal)GetFloat(config, key), digits, MidpointRounding.AwayFromZero);
	}

    public static bool GetIntBool(JsonObject config, string key)
    {
        return GetInt(config, key) == 0 ? false : true;
    }

    public static bool GetBool(JsonObject config, string key)
	{
		bool value = false;
		return bool.TryParse(GetString(config, key), out value) ? value : false;
    }

    public static JsonObject GetJsonObject(JsonObject config, string key)
    {
		object value = null;
		JsonObject obj = config.TryGetValue(key, out value) ? (value as JsonObject) : null;
		return obj != null ? obj : new JsonObject();
    }

    public static JsonArray GetJsonArray(JsonObject config, string key)
	{
		object value = null;
		JsonArray arr = config.TryGetValue(key, out value) ? (value as JsonArray) : null;
		return arr != null ? arr : new JsonArray();
    }

    public static string[] GetStringArray(JsonObject config, string key)
    {
		string str = GetString(config, key);
//		return str != "" && str != "0" ? str.Split(";"[0]) : new string[0];
		return str != "" ? str.Split(';') : new string[0];
    }

	public static string[] GetStringArray(JsonObject config, string key, char split)
	{
		string str = GetString(config, key);
//		return str != "" && str != "0" ? str.Split(split) : new string[0];
		return str != "" ? str.Split(split) : new string[0];
	}

	public static Vector3 GetVector3(JsonObject config, string key)
	{
		JsonObject obj = GetJsonObject(config, key);
		return new Vector3(GetFloat(obj, "x"), GetFloat(obj, "y"), GetFloat(obj, "z"));
	}

	public static Quaternion GetQuaternion(JsonObject config, string key)
	{
		JsonObject obj = GetJsonObject(config, key);
		return new Quaternion(GetFloat(obj, "x"), GetFloat(obj, "y"), GetFloat(obj, "z"), GetFloat(obj, "w"));
	}
}

public class DialogDataPaser
{
    //格式
    //1;2;3...
    public static List<int> IntList(string strParser)
    {
        List<int> myList = new List<int>();

        string[] arrStrings = strParser.Split(';');
        for (int i = 0; i < arrStrings.Length; ++i )
        {
            int nTest = 0;
            if (int.TryParse(arrStrings[i], out nTest))
            {
                myList.Add(nTest);
            }
        }

        return myList;
    }

    //格式
    //100101;100102;100103....
    public static List<string> StringList(string strParser)
    {
        List<string> myList = new List<string>();

        string[] arrStrings = strParser.Split(';');
        for (int i = 0; i < arrStrings.Length; ++i)
        {
            myList.Add(arrStrings[i]);
        }

        return myList;
    }

    //格式
    //[100101,100102,100103...]
    //张撕栋钦点专属格式
    public static List<string> StringList2(string strParser)
    {
        List<string> myList = new List<string>();
        string strArr = strParser;
        if ('[' != strArr[0] || ']' != strArr[strArr.Length - 1])
        {
            return myList;
        }

        strArr.Remove(0);
        strArr.Remove(strArr.Length - 1);

        string[] arrStrings = arrStrings = strArr.Split(',');
        for (int i = 0; i < arrStrings.Length; ++i)
        {
            myList.Add(arrStrings[i]);
        }

        return myList;
    }

    //格式
    //100101,100102,100103....
    public static List<string> stringList3(string strParser)
    {
        List<string> myList = new List<string>();

        string[] arrStrings = strParser.Split(',');
        for (int i = 0; i < arrStrings.Length; ++i)
        {
            myList.Add(arrStrings[i]);
        }

        return myList;
    }

    //格式
    //100101,1;100102,2;100103,3.....
    public static Dictionary<string, int> StringIntDic(string strParser)
    {
        Dictionary<string, int> myList = new Dictionary<string, int>();

        string[] arrStrings = strParser.Split(';');
        for (int i = 0; i < arrStrings.Length; ++i)
        {
            string[] oneString = arrStrings[i].Split(',');
            if (oneString.Length != 2)
            {
                continue;
            }

            int nTestValue;
            if (!int.TryParse(oneString[1], out nTestValue))
            {
                continue;
            }

            myList.Add(oneString[0], nTestValue);
            
        }

        return myList;
    }

    //格式
    //1,2;1,3;2,4....
    public static List<int[]> Int2ArrayList(string strParser)
    {
        List<int[]> myList = new List<int[]>();

        string[] arrStrings = strParser.Split(';');
        for (int i = 0; i < arrStrings.Length; ++i)
        {
            string[] oneString = arrStrings[i].Split(',');
            int nTestOne;
            int nTestTwo;

            if (!int.TryParse(oneString[0], out nTestOne) || !int.TryParse(oneString[1], out nTestTwo))
            {
                continue;
            }

            int[] oneArray = {nTestOne,nTestTwo};
            myList.Add(oneArray);
    
        }

        return myList;
    }
}