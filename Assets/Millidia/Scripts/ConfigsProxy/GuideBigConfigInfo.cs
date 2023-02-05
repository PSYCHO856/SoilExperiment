using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 玩家数据类
/// </summary>
public  class GuideBigConfigInfo : APP.Core.Singleton<GuideBigConfigInfo>
{
    static Dictionary<string, GuideBigConfig> datas = new Dictionary<string, GuideBigConfig>();
    public static Dictionary<string, GuideBigConfig> Datas
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
