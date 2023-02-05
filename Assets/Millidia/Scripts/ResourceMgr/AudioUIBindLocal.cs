using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// 本地加载绑定音效播放
/// </summary>
public class AudioUIBindLocal: AudioUIBind {
    public int resPathId=1;
    // public bool isPlayer;//是否播放完毕
    [SerializeField] AudioSource _au_main;

    protected override void Start() {
        if(!_au_main)
            _au_main=this.GetComponent<AudioSource>();
        ui=GetComponent<EventListener>();
        ui.onClick+=(eventData)=>{
            OnAudio(clickSound);
        };
    }
    protected virtual void OnAudio(string  name){
         if(String.IsNullOrEmpty(name)){
            return;
        }
        _au_main.clip=Resources.Load<AudioClip>(ResPath.audio_scene+name);
        _au_main.Play();
    }
}
