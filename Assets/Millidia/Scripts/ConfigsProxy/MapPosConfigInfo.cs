using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家数据类
/// </summary>
public  class MapPosConfigInfo : APP.Core.Singleton<MapPosConfigInfo>
{
    static Dictionary<string, MapPosConfig> datas = new Dictionary<string, MapPosConfig>();

    public static Dictionary<string, MapPosConfig> Datas
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
    public MapPosConfig GetPlayer(string id)
    {
        return Datas[id];
    }
}
