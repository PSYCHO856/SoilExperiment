using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 单个答题页面
/// </summary>
public class MUIQuestionSingle: MUIBase {
    public QuestionConfig questionData;
    public QuestionGroup questionGroup;
    private Dictionary<string, QuestionConfig> nowJosn=new Dictionary<string, QuestionConfig>();
    private string _searchID="41101";
    public override void OnAwake() {
        UiName = EMUI.MUI_QuestionSingle;
        base.OnAwake();
    }
    
    List<object> myParam=new List<object>();

    /// <summary>
    ///打开界面-参数0为回调方法 参数1为单个答题ID
    /// </summary>
    public override void Open(params object[] parms) {
        base.Open(parms);
        myParam.Clear();
        if(parms.Length>0)
        {
            _searchID=parms[1].ToString();
            questionData=QuestionConfigInfo.Datas[_searchID];
            myParam.Add(parms[0]);
            myParam.Add(parms[1]);
            SingleQuestion();
        }
    }
    bool isCreat=true;
    List<GameObject> pools=new List<GameObject>();

    /// <summary>
    /// 构造单个题
    /// </summary>
    private void SingleQuestion(){
        if(isCreat){
            questionGroup.CreatPools();
            isCreat=false;
        }
        questionGroup.Init(questionData);//构造单选数据
    }
    public void CloseSelf()
    {
        MUIMgr.Instance.CloseUI(UiName);
    }
    void Clear(){
    }
    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="parms"></param>
    public override void Close(params object[] parms) {
        base.Close(parms);
        if(myParam.Count>0){
            Action ac=(Action)myParam[0];
            if(ac==null){
            }else{
                MessageTip.Instance.ShowAlertBox($"答题完毕！",EMUI.yellow16);
                ac?.Invoke();
            }
        }
    }
    /// <summary>
    /// 刷新界面
    /// </summary>
    /// <param name="parms"></param>
    public override void Refresh(params object[] parms) {
        base.Refresh(parms);
    }
}