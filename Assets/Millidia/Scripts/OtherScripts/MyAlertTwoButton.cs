using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MyAlertTwoButton : MonoBehaviour
{
    public Text title;
    public Text content;
    public Text leftBtnText;
    public Text rightBtnText;

    public Button leftBtn;
    public Button rightBtn;

    private void Awake()
    {
        title =  UtilityTool.FindChild<Text>(transform, "title");
        content =  UtilityTool.FindChild<Text>(transform, "content");
        leftBtnText =  UtilityTool.FindChild<Text>(transform, "leftBtnText");
        rightBtnText =  UtilityTool.FindChild<Text>(transform, "rightBtnText");
        leftBtn =  UtilityTool.FindChild<Button>(transform, "LeftButton");
        rightBtn =  UtilityTool.FindChild<Button>(transform, "RightButton");
    }

    /// <summary>
    /// 创建消息窗口 工具方法
    /// </summary>
    /// <returns></returns>
    public static MyAlertTwoButton CreateAlert(string title, string content, string leftBtnText, string rightBtnText, Action leftAction, Action rightAction = null)
    {
        var go = ResourceMgr.CreateUIPrefab("GUIs/TwoButtonAlert",MUIMgr.Instance.Canvas);
        ///获取组件
        var alert = go.GetComponent<MyAlertTwoButton>();
        alert.title.text = title;
        alert.content.text = content;
        alert.leftBtnText.text = leftBtnText;
        alert.rightBtnText.text = rightBtnText;
        alert.leftBtn.onClick.AddListener(() => {
            if(leftAction != null)
            {
                leftAction();
            }
            Destroy(go);
        });
        alert.rightBtn.onClick.AddListener(() => {
            if (rightAction != null)
            {
                rightAction();
            }
            Destroy(go);
        });
        return alert;
    }

}
