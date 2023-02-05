using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 答题代理
/// </summary>
public  class QuestionConfigInfo : APP.Core.Singleton<QuestionConfigInfo>
{
    static Dictionary<string, QuestionConfig> datas = new Dictionary<string, QuestionConfig>();

    public static Dictionary<string, QuestionConfig> Datas
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

    /// <summary>
    /// 获得指定id的角色初始化配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public QuestionConfig GetPlayer(string id)
    {
        return Datas[id];
    }
}
