using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对话数据类
/// </summary>
public  class TalkConfigInfo : APP.Core.Singleton<TalkConfigInfo>
{
    static Dictionary<string, TalkConfig> datas = new Dictionary<string, TalkConfig>();

    public static Dictionary<string, TalkConfig> Datas
    {
        get
        {
            return datas;
        }
        set
        {
            datas = value;
        }
    }
}
