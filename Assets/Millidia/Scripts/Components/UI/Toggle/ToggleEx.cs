using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// 简单的toggle效果-有一个image组件即可
/// </summary>
public class ToggleEx: 
MonoBehaviour,
IPointerClickHandler
{
    public bool isOn=false;
    public List<Sprite> spris=new List<Sprite>();//on off切换
    private Image img;
    public Action<bool> onChange;

    private void Awake() {
        img=GetComponent<Image>();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData){
        isOn=!isOn;
        if(onChange!=null)
            onChange(isOn);
        if(isOn){
            img.sprite=spris[0];
        }else{
            img.sprite=spris[1];
        }
    }
    public void Reset(){
        img.sprite=spris[1];
    }
}