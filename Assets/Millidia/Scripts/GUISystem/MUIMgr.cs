using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理整个UI的对象的
/// </summary>
public class MUIMgr : APP.Core.Singleton<MUIMgr>{

    Dictionary<string, MUIBase> Uis = new Dictionary<string, MUIBase>();
    Stack<MUIBase> stack = new Stack<MUIBase>();

    public void PushUI(MUIBase ba)
    {
        stack.Push(ba);
         
    }
 
    public void PopUI()
    {
        if(stack.Count > 0)
        {
            while (stack.Count != 0)
            {
                MUIBase go = stack.Peek();
                if (go == null)
                    stack.Pop(); //如果为空删除
                else
                {
                    if(go.gameObject.activeSelf == false) //删除关闭的
                    {
                        stack.Pop();
                    }
                    if (go.gameObject.activeSelf == true) //发现有开启的
                        break;
                }
            }

            if (stack.Count == 0)
                return;

            MUIBase ui = stack.Pop();
            ui.Close();
        }
      
    }

    Transform canvas;
    public Transform Canvas
    {
        get { if(canvas == null)
            {
                canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
                if(canvas == null)
                   canvas = GameObject.Find("Canvas").transform;
                if (canvas == null)
                    canvas = ResourceMgr.CreateUIPrefab("GUIs/Canvas", null).transform;
            }
            return canvas;
        }
    }
 
    public void AddUI(MUIBase ba)
    {
        if(!Uis.ContainsKey(ba.UiName))
            Uis.Add(ba.UiName, ba);
    }
    
    public void OpenUI(string UiName,params object[] prams)
    {
         if(UiName.Equals(EMUI.MessageTip)){
            Debug.Log("打开UI："+UiName);
            // MessageCenter.Instance.BoradCastMessage(EMsg.Player_IsMove,true);
        }else{
            // MessageCenter.Instance.BoradCastMessage(EMsg.Player_IsMove,false);
        }
        if (Uis.ContainsKey(UiName))
        {
            // Debug.Log("存在：" + prams.Length);
            Uis[UiName].Open(prams);
        }
        else
        {
            // Debug.Log("不存在：" + UiName);
            var ui = GameObject.Instantiate<GameObject>(ResourceMgr.Load<GameObject>("GUIs/" + UiName));
            MUIBase mb = ui.GetComponent<MUIBase>();
            mb.Open(prams);
        }
    }

    public void CloseUI(string UiName, params object[] prams)
    {
        MessageCenter.Instance.BoradCastMessage(EMsg.Player_IsMove,true);
        // Debug.Log("关闭UI："+UiName);
        if (Uis.ContainsKey(UiName))
        {
            Uis[UiName].Close(prams);
        }
    }

    public void RefreshUI(string UiName, params object[] prams)
    {
        if (Uis.ContainsKey(UiName))
        {
            Uis[UiName].Refresh(prams);
        }
    }

    public void RemoveUI(string UiName)
    {
        if (Uis.ContainsKey(UiName))
        {
            Uis.Remove(UiName);
        }
    }
    /// <summary>
    /// 通过名字获取
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public MUIBase GetUI(string uiName) 
    {
        return Uis[uiName];
    }
    /// <summary>
    /// 获取UI对象
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public T GetUI<T>(string uiName) where T:MUIBase
    {
        return Uis[uiName] as T;
    }

    /// <summary>
    /// 显示提示框
    /// </summary>
    /// <param name="type"></param>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="oneText"></param>
    /// <param name="twoText"></param>
    public MUIAlert ShowAlert(OpenType type,Action one,Action two = null,string title = "",string content = "", string oneText = "",string twoText = "")
    {
        MUIAlert alert = ResourceMgr.CreateUIPrefab("GUIs/Alert/Alert", Canvas).GetComponent<MUIAlert>();
        var innerCanvas = alert.gameObject.AddComponent<Canvas>();
        innerCanvas.overrideSorting = true;
        innerCanvas.sortingOrder = 30003;
        alert.gameObject.AddComponent<GraphicRaycaster>();

        switch (type)
        {
            case OpenType.Long:
                alert.OpenLong(one, content, title, oneText);
                break;
            case OpenType.TwoButton:
                alert.OpenTwoButton(one, two, content, title, oneText, twoText);
                break;
        }
        return alert;
    }

    public override void Dispose()
    {
        base.Dispose();
        stack.Clear();
        Uis.Clear();
    }

    public bool GetIsOpenUI()
    {
        foreach (var item in Uis)
        {
            if (item.Key != EMUI.MUI_Main)
            {
                if (item.Value.gameObject.activeInHierarchy)
                {
                    return true;
                } 
            }
        }
        return false;
    }
}
