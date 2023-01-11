using UnityEngine;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// 手动绑定音效播放
/// </summary>
[RequireComponent(typeof(EventListener))]
public class AudioUIBind: MonoBehaviour {
    /// <summary>
    /// 设置点击音效
    /// </summary>
    public string clickSound=EAudio.se_btn_01;
    protected EventListener ui;
    protected virtual void Start() {
        ui=GetComponent<EventListener>();
        ui.onClick=(eventData)=>{
            AudioManager.Instance?.PlayAudio(1,clickSound);
        };
    }
}