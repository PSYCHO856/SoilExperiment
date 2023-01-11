using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家数据类
/// </summary>
public  class EquipmentConfigInfo : APP.Core.Singleton<EquipmentConfigInfo>
{
    static Dictionary<string, EquipmentConfig> datas = new Dictionary<string, EquipmentConfig>();

    public static Dictionary<string, EquipmentConfig> Datas
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
    public EquipmentConfig GetPlayer(string id)
    {
        return Datas[id];
    }
}
