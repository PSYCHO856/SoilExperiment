using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
/// <summary>
/// 正则表达式限制
/// </summary>
public class InputFieldLimit : MonoBehaviour
{
    public InputField m_InputField;
    public ContentType type=ContentType.F_Plus;
    private void Awake()
    {
        if(!m_InputField){
            m_InputField = GetComponent<InputField>();
        }
       
        m_InputField.onValueChanged.AddListener(OnInputFieldValueChang);
        // m_InputField.onEndEdit.AddListener((strs)=>{
        //     Debug.Log("wsl");
        // });
    }
    string content="";
    private void OnInputFieldValueChang(string inputInfo)
    {   
        switch(type){
        case ContentType.INT_Plus:
            {
                if(inputInfo.CompareTo("-")==0||String.IsNullOrEmpty(inputInfo)){
                    m_InputField.text = "";
                }else{
                    float nums=Convert.ToInt32(inputInfo);
                    if(nums<0){
                        nums=-nums;
                        m_InputField.text = nums.ToString();
                    }
                } 
            }
            break;
        case ContentType.F_Plus:
            {
                if(inputInfo.CompareTo("-")==0||String.IsNullOrEmpty(inputInfo)){
                    m_InputField.text = "";
                }else{
                    float nums=Convert.ToSingle(inputInfo);
                    if(nums<0){
                        nums=-nums;
                        m_InputField.text = nums.ToString();
                    }
                } 
            }
            break;
        }


    }

}
public enum ContentType{
    INT_Plus,//浮点+
    F_Plus,//浮点-

}
