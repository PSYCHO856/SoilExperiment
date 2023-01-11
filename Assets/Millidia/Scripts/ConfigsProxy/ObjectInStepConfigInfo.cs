
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 设备信息类
/// </summary>
public  class ObjectInStepConfigInfo : APP.Core.Singleton<ObjectInStepConfigInfo>
{
    static Dictionary<string, ObjectInStepConfig> datas = new Dictionary<string, ObjectInStepConfig>();

    public static Dictionary<string, ObjectInStepConfig> Datas
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
    public ObjectInStepConfig GetPlayer(string id)
    {
        return Datas[id];
    }
}