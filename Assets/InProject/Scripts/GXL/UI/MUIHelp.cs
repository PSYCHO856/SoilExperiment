using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;
 
using System;
/// <summary>
/// 设置页面1
/// </summary>
public class MUIHelp: MUIBase {

    public override void OnAwake() {
        UiName = EMUI.MUI_Help;
        base.OnAwake();
    }

    void Start() {
        Open();
    }

    /// <summary>
    ///打开界面
    /// </summary>
    public override void Open(params object[] parms) {
        base.Open(parms);
    
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