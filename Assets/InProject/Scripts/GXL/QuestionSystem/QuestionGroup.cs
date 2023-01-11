using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 答题组-判断-清除-显示正确答案
/// </summary>
[RequireComponent(typeof(ToggleGroup))]
public class QuestionGroup : UnitBase
{
    public string QID="40104";//当前题目ID
    public int buildTotle=6;//生成个数
    public bool isMult=false;//是否多选
    [Header("配置参数")]
    public Text titleHead;
    public EventListener btnConfirm;
    public Action a_confirm;
    public GameObject q1;//答题生成的单元

    private QuestionConfig qData;
    private List<QuestionUnit> units=new List<QuestionUnit>();
    private ToggleGroup tgGroup;
    private String s_answer;
    
    private void Awake()
    {
        tgGroup=this.GetComponent<ToggleGroup>();
      
    }
   private void Start()
   {
      btnConfirm.onClick+=OnConfirm;
   }
    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="data">单个答题配置数据-</param>
    public void Init(QuestionConfig data){
        qData=data;
        Clear();
        OnUpdate();
    }
    /// <summary>
    /// 构造对象池
    /// </summary>
    public  void CreatPools(){
        q1.SetActive(false);
        units.Clear();

        for(int i=0;i<buildTotle;i++){
            var obj=Instantiate(q1,q1.transform.parent);
            var qUnit=obj.GetComponent<QuestionUnit>();
            qUnit.index=i;
            units.Add(qUnit);
            obj.SetActive(true);
        }
    }
    //数据更新2
    private void OnUpdate(){
        bool isMult=IsMult();
        titleHead.text=qData.Describe+(isMult?" <color=red>多选题</color>":"");
        QID=qData.Id;
        ToggleGroup mult=isMult?null:tgGroup;
        units[0].SetABC(qData.T0,mult);
        units[1].SetABC(qData.T1,mult);
        units[2].SetABC(qData.T2,mult);
        units[3].SetABC(qData.T3,mult);
        units[4].SetABC(qData.T4,mult);
        units[5].SetABC(qData.T5,mult);
        
        RebuildLayout();
    }
    private void RebuildLayout(){
         //更新ContentSizeFitter 下一帧刷新11
        var roots=this.GetComponentsInChildren<ContentSizeFitter>(true);
        roots.Reverse();
        foreach(var itor in roots){
            LayoutRebuilder.ForceRebuildLayoutImmediate(itor.GetComponent<RectTransform>());
        }
        Debug.Log("更新ContentSizeFitter 下一帧刷新");
    }
    public Text mTitle;//正确答案
    //确认选择
    private void OnConfirm(PointerEventData p){
        var ratio=GetScore();
        mTitle.gameObject.SetActive(true);
        if(ratio==0){
            mTitle.color=Color.red;
            mTitle.text=$"回答错误 正确答案:{s_answer}";
        }else if(ratio==1){
            mTitle.color=Color.white;
            mTitle.text=$"回答正确！";
            Debug.Log("回答正确！");
        }else{
            mTitle.color=Color.white;
            mTitle.text=$"不完全正确 正确答案:{s_answer}";
        }
        a_confirm?.Invoke();
    }
    //清空
    public void Clear(){
        mTitle.gameObject.SetActive(false);

        foreach(var itor in units){
            itor.tg.isOn=false;
            itor.descp.text="";
        }
    }
    //计算分数比率-等于1是全答对-0是全错
    public float GetScore(){
        var array=qData.Answer.Split("|");
        float counter=0;
        s_answer="";
        var chooesed=units.Where(c=>c.tg.isOn==true);
        foreach(var itor in chooesed){
            if(qData.Answer.IndexOf(itor.index.ToString())!=-1){
                counter++;
            }else{
                counter=0;
                break;
            }
        }
        foreach(var itor in array){
            LetterType str=(LetterType)(Convert.ToInt32(itor));
            s_answer+=$" {str}";
        }
       // Debug.Log("counter-"+counter+"totle-"+totle);
        return counter/array.Length;
    }
    //判断是否多选
    private bool IsMult(){
        float totle=qData.Answer.Split("|").Length;
        if(totle==1){
            return false;
        }else{
            return true;
        }
    }
    private void OnDestroy(){
    }
}
/// <summary>
/// 字母枚举
/// </summary>
public enum LetterType{
    A=0,
    B,
    C,
    D,
    E,
    F
}
