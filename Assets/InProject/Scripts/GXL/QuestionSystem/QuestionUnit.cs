using System;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// 答题元素1
/// </summary>
public class QuestionUnit : UnitBase
{
    public  Toggle tg;//是否选中tg
    public Text descp;//文字描述
    public void Init(){
        if(String.IsNullOrEmpty(descp.text)){
            this.gameObject.SetActive(false);
        }else{
            this.gameObject.SetActive(true);
        }
    }
    public void SetABC(string desc,ToggleGroup tGroup){
        LetterType str=(LetterType)(Convert.ToInt32(index));
        descp.text=$"{str}.{desc}";
        this.gameObject.SetActive(true);
        if(String.IsNullOrEmpty(desc)){
            this.gameObject.SetActive(false);
        }
       tg.group=tGroup;
    }
}
