using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.Reflection;
 
public class Config : ConfigBase
{
    string className;
    string path;
    string container;

    public string ClassName
    {
        get
        {
            return className;
        }

        set
        {
            className = value;
        }
    }

    public string Path
    {
        get
        {
            return path;
        }

        set
        {
            path = value;
        }
    }

    public string Container
    {
        get
        {
            return container;
        }

        set
        {
            container = value;
        }
    }
}

public partial class ConfigInfo : APP.Core.MonoSingleton<ConfigInfo> {

    //public bool IsLoadLocal;

    Dictionary<string, Config> configs = new Dictionary<string, Config>();
    public Dictionary<string, Config> Configs
    {
        get
        {
            return configs;
        }

        set
        {
            configs = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

   /// <summary>
   /// 注册数据
   /// </summary>
    public void ReadConfig()
    {
        MapPosConfigInfo.Datas=ReadConfigs<MapPosConfig>();
        EquipmentConfigInfo.Datas=ReadConfigs<EquipmentConfig>();
        GuideConfigInfo.Datas=ReadConfigs<GuideConfig>();
        QuestionConfigInfo.Datas=ReadConfigs<QuestionConfig>();
        
        ExperimentEquipmentConfigInfo.Datas=ReadConfigs<ExperimentEquipmentConfig>();
        ExperimentInstructionConfigInfo.Datas=ReadConfigs<ExperimentInstructionConfig>();
        ObjectInStepConfigInfo.Datas=ReadConfigs<ObjectInStepConfig>();
        
        ExperimentIntroductionConfigInfo.Datas=ReadConfigs<ExperimentIntroductionConfig>();
        
        // Configs = ReadConfigs<Config>("Configs/Configs");
        // Type ty = typeof(ConfigInfo);
        // foreach (var data in Configs.Values)
        // {
        //     MethodInfo method = ty.GetMethod("ReadConfigs");
        //     Type tragetClass = Type.GetType(data.ClassName);
        //     // Debug.Log(tragetClass);
        //     method = method.MakeGenericMethod(tragetClass);//创建泛型方法  Dictionary<string, ItemConfig> ReadConfigs<ItemConfig>(string path) where T : ConfigBase
        //     PropertyInfo pinfp = ty.GetProperty(data.Container);//找到 ItemDir 字典

        //    Debug.Log("method-"+method + "pinfp-" + pinfp + " Instance-" + Instance + " Path-" + data.Path);//注册配置信息
        //     pinfp.SetValue(Instance, method.Invoke(Instance, new object[] { data.Path }), null);
        // }
    }
    /// <summary>
    /// 加载Json数据-文件名==类名
    /// </summary>
    public Dictionary<string,T> ReadConfigs<T>()where T:ConfigBase
    {
        Type fileName = typeof(T);
        Dictionary<string, T> configs = new Dictionary<string, T>();
        TextAsset ta = ResourceMgr.Load<TextAsset>($"Configs/{fileName}");
      
        JsonData ar = JsonMapper.ToObject(ta.text);
        foreach (var data in ar)
        {
            T config = JsonMapper.ToObject<T>(JsonMapper.ToJson(data));
            configs.Add(config.Id, config);
        }
        Debug.Log("加载表-"+fileName);
        // Debug.Log(ar.ToJson());
        return configs;
    }
    /// <summary>
    /// 注册表格
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<T> RigistConfigs<T>(string path)where T:ConfigBase
    {
        List<T> configs = new List<T>();
        TextAsset ta = ResourceMgr.Load<TextAsset>(path);
        Debug.Log("加载表" + ta);
        JsonData ar = JsonMapper.ToObject(ta.text);
        foreach (var data in ar)
        {
            JsonData j = data as JsonData;
            T unit = JsonMapper.ToObject<T>(JsonMapper.ToJson(data));
            configs.Add(unit);
        }
        return configs;
    }

    /// <summary>
    /// 获取指定字典的键对应的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <param name="dataName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static T GetDataById<T>(string id,string dataName,T type) where T : ConfigBase
    {
       foreach(var f in typeof(ConfigInfo).GetFields())
        {
            Debug.Log(f);
        }
       var value = typeof(ConfigInfo).GetField(dataName).GetValue(ConfigInfo.Instance);
       Dictionary<string,T> temp = value as Dictionary<string,T>;
       foreach(var data in temp)
        {
            Debug.Log(data.Key);
        }
       return temp[id];
    }
}
