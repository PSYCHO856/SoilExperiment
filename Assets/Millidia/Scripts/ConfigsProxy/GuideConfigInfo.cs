using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家数据类
/// </summary>
public  class GuideConfigInfo : APP.Core.Singleton<GuideConfigInfo>
{
    static Dictionary<string, GuideConfig> datas = new Dictionary<string, GuideConfig>();
    static Dictionary<string, GuideConfig> save_datas = new Dictionary<string, GuideConfig>();

    public static Dictionary<string, GuideConfig> Datas
    {
        get
        {
            return datas;
        }
        set
        {
            datas = value;
            GetSaveData();
        }
    }

    /// <summary>
    /// 获得指定id的角色初始化配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GuideConfig GetPlayer(string id)
    {
        return Datas[id];
    }

    public static Dictionary<string, GuideConfig> GetSaveData(){
        save_datas.Clear();
        save_datas=datas;
        return save_datas;
    }
}
