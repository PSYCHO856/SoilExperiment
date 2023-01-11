using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// ToggleGroup 基于EventListener脚本-
/// </summary>
public class TgGroup: MonoBehaviour {
    /// <summary>
    /// 默认第一个打开
    /// </summary>
    public int tgIndex=0;
    // [HideInInspector]
    public List<EventListener> listeners=new List<EventListener> ();
    /// <summary>
    /// 是否默认选择一个
    /// </summary>
    public bool isOpen=false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        if(isOpen)
            Init();
    }
    public void Init() {
        var childs=transform.GetComponentsInChildren<EventListener>();
        listeners.AddRange(childs);
        Open();
    }
    /// <summary>
    ///打开界面
    /// </summary>
    public virtual void Open() {
        for(int i=0;i<listeners.Count;i++)
        {
            if(i == tgIndex)
            {
                if (listeners[i].transform.childCount > 0)
                {
                    listeners[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }else{
                if (listeners[i].transform.childCount > 0)
                {
                    listeners[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            var index=i;
            listeners[index].onClick+=(eventData)=>{
               OnChange(index);
            };
        }
        Debug.Log("认知监听初始化完成！");
    }
    public void OnChange(int index){
        if(listeners.Count<=0){
            return;
        }
        for (int i = 0; i < listeners.Count; i++)
        {
            if(index==i){
                listeners[i].transform.GetChild(0).gameObject.SetActive(true);
            }else{
                listeners[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    private void OnDestroy() {
    }
}