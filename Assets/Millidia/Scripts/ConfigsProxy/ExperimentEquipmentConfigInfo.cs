
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 设备信息类
/// </summary>
public  class ExperimentEquipmentConfigInfo : APP.Core.Singleton<ExperimentInstructionConfigInfo>
{
    static Dictionary<string, ExperimentEquipmentConfig> datas = new Dictionary<string, ExperimentEquipmentConfig>();

    public static Dictionary<string, ExperimentEquipmentConfig> Datas
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
    public ExperimentEquipmentConfig GetPlayer(string id)
    {
        return Datas[id];
    }
}