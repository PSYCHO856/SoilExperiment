using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Linq;
/// <summary>
/// 对话核心组件
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class FUITalk: MonoBehaviour {
    public Text txt_content;//对话内容 Content
    public Text txt_name;

    public CanvasGroup dialog;//对话框

    [SerializeField]private float interval=0.1f;//每个字显示间隔
    [SerializeField]private float fade=1f;//渐显时间
    [SerializeField] private bool isTimeLine=false;//是否是当前文本-自动播放。 不可外部触发播放下一个
    private int talkIndex=-1;//当前对话的索引
    private AudioSource _au_main;
    private Action endCb=null;

    //当前对话数据列表
    private List < TalkConfig > curLists = new List < TalkConfig > ();
  
    protected virtual void Awake()
    {
        _au_main=this.GetComponent<AudioSource>();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="talkIdPre">前置对话ID</param>
    /// <param name="endCB">对话完成回调方法</param>
    public void Init(string talkIdPre,Action endCB=null){
        IsEnable(true);
        talkIndex=-1;
        endCb=endCB;
        curLists.Clear();
        var preTask = TalkConfigInfo.Datas.Where(c => c.Value.Id.Substring(0, talkIdPre.Length) == talkIdPre).ToDictionary(k => k.Key, v => v.Value);
        foreach(var itor in preTask) {
           curLists.Add(itor.Value);
        }
    }
    public void Close(Action ac=null){
        DOTween.To(()=>dialog.alpha,(x)=>dialog.alpha=x,0,fade).OnComplete(()=>{
            ac?.Invoke();
            txt_content.text="";
            txt_name.text="";
            IsEnable(false);
        });
    }
    private void IsEnable(bool isOn){
        if(isOn){
            dialog.alpha=1;
            dialog.blocksRaycasts=true;
        }else{
            dialog.alpha=0;
            dialog.blocksRaycasts=false;
        }
    }
    /// <summary>
    /// 自动点击播放 使用的UI
    /// </summary>
    public void NextTalk(){
        IsEnable(true);
        talkIndex++;
        if(talkIndex>=curLists.Count){
            //关闭页面
            dialog.alpha=0;
            Debug.Log($"关闭对话面板");
            endCb?.Invoke();
            return;
        }
        if(DOTween.IsTweening(txt_content)){
            txt_content.DOKill();//销毁动画
        }
        txt_content.text="";
        txt_name.text=curLists[talkIndex].name;
        OnAudio(curLists[talkIndex].audio);
        txt_content.DOText(curLists[talkIndex].Content,interval*curLists[talkIndex].Content.Length).SetEase(Ease.Linear).
        OnComplete(()=>
        {
            if(isTimeLine){
                NextTalk();
            }
        });
    }
    /// <summary>
    /// 按照ID播放
    /// </summary>
    public void NextTalk(TalkConfig config,Action ac=null){
        IsEnable(true);
        if(DOTween.IsTweening(txt_content)){
            txt_content.DOKill();//销毁动画
        }
        txt_content.text="";
        txt_name.text=config.name;

        OnAudio(config.audio);
        txt_content.DOText(config.Content,interval*config.Content.Length).OnComplete(()=>{
            if(ac!=null)
                ac.Invoke();
        });
    }

    private void OnAudio(string  name){
        if(String.IsNullOrEmpty(name)){
            return;
        }
        _au_main.clip=Resources.Load<AudioClip>(ResPath.audio_talk+name);
        _au_main.Play();
    }
}