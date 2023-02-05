using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 物品代理类
/// </summary>
public  class ContentConfigInfo : APP.Core.Singleton<ContentConfigInfo>
{
    static Dictionary<string, ContentConfig> datas = new Dictionary<string, ContentConfig>();

    public static Dictionary<string, ContentConfig> Datas
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
     //构建引导数据
    public List<ContentConfig> BuildData(string preId){
        List<ContentConfig> datas=new List<ContentConfig>();
        var preTask= Datas.Where(c=>c.Value.Id.Substring(0,preId.Length)==preId).ToDictionary(k=>k.Key,v=>v.Value);
        foreach (var itor in preTask)
        {
            var v= DeepCopyEx.DeepCopyByReflection<ContentConfig>(itor.Value);
            v.Id=itor.Value.Id;
            datas.Add(v);
        }
        return datas;
    }
}
