using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;
 
using System;
/// <summary>
/// 学生设置页面
/// </summary>
public class MUIStudentCtl: MonoBehaviour {
    [SerializeField]
    private List<InputField> inputTexts=new List<InputField>();
    [SerializeField]
    private List<InputField> inputResults=new List<InputField>();
    [SerializeField]
    private Button _btnResult;
    private List<string> datas=new List<string>();
    private void Start()
    {
        Init();
        _btnResult.onClick.AddListener(GetResult);
        
    }
    /// <summary>
    /// 初始化数据-
    /// </summary>
    public void Init(){
        List<string> temps=GlobalPropsMgr.Instance.GetStudentInfo();
        for (int i = 0; i < inputTexts.Count; i++)
        {
            var index=i;
            datas.Add(temps[i]); 
            inputTexts[i].text=temps[i];
            inputTexts[index].onEndEdit.AddListener((strs)=>{
               datas[index]=strs;
            });
        }
    }
    private void GetResult(){
        for (int i = 0; i < inputResults.Count; i++)
        {
           ScaleText(inputResults[i].textComponent,3);
        }
    }

    private void ScaleText(Text text,int times){
        var unitScale=text.GetComponent<Eff_UIScale>();//字体放大提示
        unitScale.toScale=1.3f;
        unitScale.LoopPlay(times);
    }



    public void Close(){
        GlobalPropsMgr.Instance.SetStudentInfo(datas);
    }
}