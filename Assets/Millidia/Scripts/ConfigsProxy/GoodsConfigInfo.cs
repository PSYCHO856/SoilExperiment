using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 物品代理类
/// </summary>
public  class GoodsConfigInfo : APP.Core.Singleton<GoodsConfigInfo>
{
    static Dictionary<string, GoodsConfig> goods = new Dictionary<string, GoodsConfig>();

    public static Dictionary<string, GoodsConfig> Goods
    {
        get
        {
            return goods;
        }
        set
        {
            goods = value;
        }
    }

    /// <summary>
    /// 获得指定id的角色初始化配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GoodsConfig GetPlayer(string id)
    {
        return Goods[id];
    }
}
