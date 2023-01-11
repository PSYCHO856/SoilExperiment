using UnityEngine;
using System.Collections;
using System.Xml;
using System.Linq;
using UnityEngine.EventSystems;
using HighlightingSystem;
using System.Collections.Generic;
using System;

public class UserHelper
{
    public static string[] SpritStr(string str)
    {
        return str.Split('|');
    }
    public static string[] SpritStr(string str, char c)
    {
        return str.Split(c);
    }
    /// <summary>
    /// linq 比较
    /// </summary>
    /// <param name="arr1"></param>
    /// <param name="arr2"></param>
    /// <returns></returns>
    public static bool CompareArr(string[] arr1, string[] arr2)
    {
        var q = from a in arr1 join b in arr2 on a equals b select a;
        bool flag = arr1.Length == arr2.Length && q.Count() == arr1.Length;
        return flag;//内容相同返回true,反之返回false。
    }
    /// <summary>
    /// 笨方法比较
    /// </summary>
    /// <param name="arr1"></param>
    /// <param name="arr2"></param>
    /// <returns></returns>
    public static bool CompareARR(string[] arr1, string[] arr2)
    {
        bool[] flag = new bool[arr1.Length];  //初始化一个bool数组，初始值全为false
        for (int i = 0; i < arr1.Length; i++)
        {
            if (arr1[i].Equals(arr2[i]))
            {
                flag[i] = true;
            }
        }
        if (flag.Contains(false))   //判断bool数组中是否包含false;
            return false;
        return true;
    }
    public static Vector3 TransitionToV3(string targetV3, char c = ',')
    {
        string[] strArry = SpritStr(targetV3, c);
        Vector3 v3 = new Vector3(float.Parse(strArry[0]), float.Parse(strArry[1]), float.Parse(strArry[2]));
        return v3;
    }
    public static string GetSystemSet(string key)//获取配置信息
    {
        string keyvalue = "0";
        string xFile = Application.streamingAssetsPath + "/Config.xml";

        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(xFile);
        XmlNode MusicKeyNode = xDoc.SelectSingleNode("config/SystemSet");

        foreach (XmlNode xChild in MusicKeyNode.ChildNodes)
        {
            XmlElement xe = (XmlElement)xChild;
            if (xe.GetAttribute("SetName") == key)
            {
                keyvalue = xe.GetAttribute("value");
            }

        }
        return keyvalue;
    }
    public static void SetSystemSet(string key, string keyvalue)//保存配置信息
    {
        string xFile = Application.streamingAssetsPath + "/Config.xml";

        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(xFile);
        XmlNode MusicKeyNode = xDoc.SelectSingleNode("config/SystemSet");
        foreach (XmlNode xChild in MusicKeyNode.ChildNodes)
        {
            XmlElement xe = (XmlElement)xChild;
            if (xe.GetAttribute("SetName") == key)
            {
                xe.SetAttribute("value", keyvalue);
            }
        }
        xDoc.Save(xFile);
    }

    private static List<Highlighter> htlist = new List<Highlighter>();
    public static void SetHighlightOn(Transform t)
    {
        if (t == null)
        {
            throw new Exception("没有设置高亮部分物体");
        }
        #region
        if (t.GetComponent<Renderer>() != null)
        {
            Renderer renderer = t.GetComponent<Renderer>();
            if (renderer != null)
            {
                Highlighter ht = t.GetComponent<Highlighter>();
                if (ht == null)
                {
                    ht = t.gameObject.AddComponent<Highlighter>();
                }
                ht.FlashingOn(new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 0f, 1f), 1.25f);
                htlist.Add(ht);
            }
        }

        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).GetComponent<Renderer>() != null)
            {
                Renderer renderer = t.GetChild(i).GetComponent<Renderer>();
                if (renderer != null)
                {
                    Highlighter ht = t.GetChild(i).GetComponent<Highlighter>();
                    if (ht == null)
                    {
                        ht = t.GetChild(i).gameObject.AddComponent<Highlighter>();
                    }
                    ht.FlashingOn(new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 0f, 1f), 2);
                    htlist.Add(ht);
                }
            }
            else
            {
                SetHighlightOn(t.GetChild(i));
            }
        }
        #endregion
    }
    public static void SetHighlightOff()
    {
        for (int i = 0; i < htlist.Count; i++)
        {
            htlist[i].FlashingOff();
        }
        htlist.Clear();
    }


}
