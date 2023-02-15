
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 设备信息类
/// </summary>
public  class ExperimentIntroductionConfigInfo : APP.Core.Singleton<ExperimentIntroductionConfigInfo>
{
    static Dictionary<string, ExperimentIntroductionConfig> datas = new Dictionary<string, ExperimentIntroductionConfig>();

    public static Dictionary<string, ExperimentIntroductionConfig> Datas
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
    public ExperimentIntroductionConfig GetPlayer(string id)
    {
        return Datas[id];
    }
}