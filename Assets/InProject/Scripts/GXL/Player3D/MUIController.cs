using System;
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LitJson;
/// <summary>
/// MUIMAin2`
/// </summary>
public class MUIController: MUIBase {
    [Header("底部工具")]
    public FUITool fUITool;
    [Header("玩家")]
    public GameObject obj_tools;
    public GuideController guides;
    //计时器
    public Text txt_timer;
    public override void OnAwake() {
        UiName = EMUI.MUI_Main;
        base.OnAwake();
    }
    //11
    private void Start() {
        InitOnce();
        MessageCenter.Instance.RegiseterMessage("ActiveMain",this,ActiveMain);

        MessageCenter.Instance.RegiseterMessage(EMsg.Player_IsMove, this, MoveAction); //注册消息
        // MessageCenter.Instance.RegiseterMessage(EMsg.Player_tools, this, Objtools); //注册消息工具开启

        guides.Init(); 
        AudioManager.Instance?.PlayAudio(0,EAudio.bgm_park);
        // CloseSelf();
    }
    void MoveAction(params object[] parms) {
        // ActiveMobleTouch((bool) parms[0]);//只有声音停止-12
    }
    void Objtools(params object[] parms) {
       obj_tools.SetActive((bool) parms[0]);
    }
    void ActiveMain(params object[] parms) {
        var isBool=(bool) parms[0];
        this.gameObject.SetActive(isBool);
        // ActiveMobleTouch((bool) parms[0]);//只有声音停止-12
    }
    public void InitOnce() {
        // fUITool.CreatTool();
    }
   
    /// <summary>
    /// 根据地图索引获取值
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector3 GetEquitPos(int index) {
        return Vector3.back;
    }

    /// <summary>
    /// 是否启动摇杆
    /// </summary>
    /// <param name="isEnable"></param>
    public void ActiveMobleTouch(bool isEnable) {
        if (isEnable)
            AudioManager.Instance?.SetVolume(1, 0);
        else
            AudioManager.Instance?.SetVolume(0, 0);
    }

    public override  void OnDestroy() {
        MessageCenter.Instance.RemoveAllMessageByName(EMsg.Player_IsMove);
        MessageCenter.Instance.RemoveAllMessageByName(EMsg.Player_tools);
        MessageCenter.Instance.RemoveAllMessageByName("ActiveMain");
        Debug.Log($"销毁{UiName}");
    }
    int count = 0;
    bool isTime=true;
    /// <summary>
    ///打开界面
    /// </summary>
    public override void Open(params object[] parms) {
        base.Open(parms);
        var innerCanvas = gameObject.GetComponent<Canvas>();
        innerCanvas.overrideSorting = false;
        // OnStartTiemer();
        AudioManager.Instance?.PlayAudio(0,EAudio.bgm_park);

    }

    public void CloseSelf() {
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