using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;
 
using System;
/// <summary>
/// 设置页面
/// </summary>
public class MUISet: MUIBase {
    private PersistentDataMgr p_datas;
    public Image igIcon;
    public List < Toggle > tgsV = new List < Toggle > ();
    public Toggle tgFull;
    public Slider audioS;
    public override void OnAwake() {
        UiName = EMUI.MUI_Set;
        base.OnAwake();
    }

    void Start() {
        // tgsV[0].onValueChanged.AddListener((Value) => {
        //     p_datas.PlayerData.set_addFri = (Value == true) ? 1 : 0;
        //     p_datas.SavePlayerJson();
        // });
        tgFull.onValueChanged.AddListener((Value) => {
           if(Value){
                Screen.SetResolution(1920,1080,true);//全屏模式
           }else{
                Debug.Log("窗口模式");
                Screen.SetResolution(1280,720,false);//窗口模式
           }
           p_datas.PlayerData.set_fullScreen = (Value == true) ? 1 : 0;
           p_datas.SavePlayerJson();
        });
        audioS.onValueChanged.AddListener((Value)=>{
            p_datas.PlayerData.set_bgm =Value;
            p_datas.SavePlayerJson();
            AudioManager.Instance.SetVolume(Value,0);
        });
        Open();
    }

    /// <summary>
    ///打开界面
    /// </summary>
    public override void Open(params object[] parms) {
        base.Open(parms);
        //获取本地数据
        p_datas = PersistentDataMgr.Instance;
        tgFull.isOn = p_datas.PlayerData.set_fullScreen == 1;//是否全屏
        audioS.value=p_datas.PlayerData.set_bgm;
    }

    public void CloseSelf()
    {
        MUIMgr.Instance.CloseUI(UiName);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="parms"></param>
    public override void Close(params object[] parms) {
        base.Close(parms);
    }
    /// <summary>
    /// 刷新界面
    /// </summary>
    /// <param name="parms"></param>
    public override void Refresh(params object[] parms) {
        base.Refresh(parms);
    }
}