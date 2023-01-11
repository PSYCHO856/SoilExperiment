using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
/// <summary>
/// 消息提示-对象池 简单显示UI弹窗-暂时没加对象池-
/// </summary>
public class MessageTip : MUIBase
{
    [SerializeField]
    private Text title_tip;
    private Animation ani;
    private List<string> listStr = new List<string>();
    private static MessageTip instance;

    /// <summary>
    /// 表情
    /// </summary>
    /// <summary>
    /// 表情资源
    /// </summary>
    public Sprite[] emojis;
    public static MessageTip Instance
    {
        get
        {
            if (instance == null)
            {
                MUIMgr.Instance.OpenUI(EMUI.MessageTip);
                instance = MUIMgr.Instance.GetUI<MessageTip>(EMUI.MessageTip);
            }
            return instance;
        }
    }
    private AnimationState state;
    private float intervalTime;
    float tiem;
    private int i = 0;
    public override void OnAwake()
    {
        UiName = EMUI.MessageTip;
        base.OnAwake();
        this.gameObject.SetActive(false);
        ani = this.GetComponent<Animation>();
        state = ani["MessageTipAni"];
    }

    public void ShowAlertBox(List<string> list, float f = 2f)
    {
        if (list.Count <= 0)
        {
            this.gameObject.SetActive(false);
            return;
        }
        this.gameObject.SetActive(true);
        listStr = list;
        i = 0;
        intervalTime = f;
        title_tip.text = listStr[i++];
        ResetAnimation();
    }

    /// <summary>
    /// 单条消息-可以改变颜色
    /// </summary>
    /// <param name="str"></param>
    /// <param name="color16"></param>
    /// <param name="emoji">表情，0为微笑，1为悲伤</param>
    /// <param name="isLocal"></param>
    public void ShowAlertBox(string str, string color16="FFFFFF")
    {
        title_tip.text = $"<color=#{color16}>{str}</color>";
        transform.SetAsLastSibling();
        ResetAnimation();
    }

    public void Update()
    {
        if (listStr == null || listStr.Count <= 0)
        {
            return;
        }
        if (listStr.Count > i)
        {
            tiem += Time.deltaTime;
            if (tiem >= intervalTime)
            {
                title_tip.text = listStr[i++];
                ResetAnimation();
                tiem = 0;
            }
        }
    }

    private void ResetAnimation()
    {       
        ani.Play("MessageTipAni");
        state.time = 0;
        ani.Sample();
    }
}