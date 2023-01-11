using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 单个答题页面1
/// </summary>
public class MUISample: MUIBase {
    public List<GameObject> obj_QAs=new List<GameObject>();
    public override void OnAwake() {
        UiName = EMUI.MUI_Sample;
        base.OnAwake();
    }
    
    List<object> myParam=new List<object>();

    /// <summary>
    ///打开界面-参数0为回调方法 参数1为单个答题ID
    /// </summary>
    public override void Open(params object[] parms) {
        base.Open(parms);
        foreach (var item in obj_QAs)
        {
            item.SetActive(false);
        }

        obj_QAs[(int)parms[0]].SetActive(true);
    }
   
    public void CloseSelf()
    {
        MUIMgr.Instance.CloseUI(UiName);
    }
    void Clear(){
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