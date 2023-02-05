
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 设备信息类
/// </summary>
public  class ExperimentInstructionConfigInfo : APP.Core.Singleton<ExperimentInstructionConfigInfo>
{
    static Dictionary<string, ExperimentInstructionConfig> datas = new Dictionary<string, ExperimentInstructionConfig>();

    public static Dictionary<string, ExperimentInstructionConfig> Datas
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
    public ExperimentInstructionConfig GetPlayer(string id)
    {
        return Datas[id];
    }
}