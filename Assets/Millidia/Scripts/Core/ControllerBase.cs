using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 控制器的基类
/// </summary>
public abstract class ControllerBase: MonoBehaviour
{
    #region 预加载的配置数据
    protected Dictionary < string, MapPosConfig > _mapdatas = new Dictionary < string, MapPosConfig > ();
    protected Dictionary<string ,EquipmentConfig> _equipDatas=new Dictionary<string, EquipmentConfig>();
    #endregion
    public abstract void Init();
    /// <summary>
    /// 初始化所需配置
    /// </summary>
    public void InitConfig(){
        _mapdatas = MapPosConfigInfo.Datas;
        _equipDatas=EquipmentConfigInfo.Datas;
    }
}
