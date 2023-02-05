using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
/// <summary>
/// 设置页面
/// </summary>
public class MUIQuestion: MUIBase {
    public QuestionGroup questionGroup;
    public GameObject obj_tg;//生成题目的单元
    private Dictionary<string, QuestionConfig> nowJosn=new Dictionary<string, QuestionConfig>();
    private string _searchID="401";

    void CreatQ(QuestionConfig nowQ) {
        questionGroup.Init(nowQ);
    }

    /// <summary>
    ///打开界面1
    /// </summary>
    public override void Open(params object[] parms) {
        base.Open();
        _searchID=EquipmentConfigInfo.Datas[parms[1].ToString()].answerID;
        var ansList=
            QuestionConfigInfo.Datas.Where(c=>c.Value.Id.Substring(0,_searchID.Length)==_searchID);
        nowJosn=ansList.ToDictionary(k=>k.Key,v=>v.Value);
        MultQuestion();
        
        if(parms.Length>0)
        {
            myParam.Clear();
            myParam.Add(parms[0]);
        }
    }
    bool isCreat=true;
    List<GameObject> pools=new List<GameObject>();
    /// <summary>
    /// 构造多题
    /// </summary>
    private void MultQuestion(){
        int index=1;
        obj_tg.SetActive(false);
        Clear();

        foreach (var item in nowJosn)
        {
            var obj=Instantiate(obj_tg,obj_tg.transform.parent);
            var qcom=item.Value;
            obj.SetActive(true);
            obj.GetComponent<Toggle>().onValueChanged.AddListener(( isVale)=>{
                if(isVale)
                    CreatQ(qcom);
            });
            obj.GetComponentInChildren<Text>().text=""+index;
            pools.Add(obj);
            index++;
        }
        if(isCreat){
            questionGroup.CreatPools();
            isCreat=false;
        }
        pools[0].GetComponent<Toggle>().isOn=true;
        
    }
    void Clear(){
        foreach (var item in pools)
        {
            Destroy(item);
        }
        pools.Clear();
    }
    List<object> myParam=new List<object>();
    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="parms"></param>
    public override void Close(params object[] parms) {
        base.Close();
        if(myParam.Count>0){
            MessageCenter.Instance.BoradCastMessage(EMsg.Guide_Finish,(int)myParam[0]);
        }
    }
}