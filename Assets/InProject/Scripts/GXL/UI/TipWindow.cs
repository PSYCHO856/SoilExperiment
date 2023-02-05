using System.Linq.Expressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
 
using System;
/// <summary>
/// 设置页面
/// </summary>
public class TipWindow: MonoBehaviour {
    public static TipWindow instanes;
    public CanvasGroup canvasGroup;
    private TaskEmit taskBoard;
    public int taskID=-1;
    private string eqUID="1001";
    public List<Button> btns=new List<Button>();
    
   private void Awake()
   {
        instanes=this;
   }

    void Start() {
        btns[0].onClick.AddListener(()=>{
            MUIMgr.Instance.OpenUI(EMUI.MUI_Konw,taskBoard.taskID,taskBoard.index);//触发到MUIKnow 页面1
            MessageCenter.Instance.BoradCastMessage(EMsg.Guide_Finish,taskID+2);
            Close();
        });
        btns[1].onClick.AddListener(()=>{
            MUIMgr.Instance.OpenUI(EMUI.MUI_Question,taskID+4,taskBoard.taskID);
            Close();
        });
        //例题参数-
        btns[2].onClick.AddListener(()=>{
            MUIMgr.Instance.OpenUI(EMUI.MUI_Sample,taskBoard.index);
            Close();
        });
        canvasGroup.blocksRaycasts=false;
    }

    public void Open(TaskEmit emit){
        this.gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.3f);
        taskBoard=emit;
        taskID=Convert.ToInt32(EquipmentConfigInfo.Datas[emit.taskID].preId+"00");
        eqUID=emit.taskID;
        canvasGroup.blocksRaycasts=true;

    }
    public void Close(){
        canvasGroup.DOFade(0, 0.3f).OnComplete(()=> {
            this.gameObject.SetActive(false);
        });
        canvasGroup.blocksRaycasts=false;
    }
    /// <summary>
    /// 到当前第几个任务
    /// </summary>
    /// <param name="index"></param>
    public void ToTask(int index){
        MessageCenter.Instance.BoradCastMessage(EMsg.Guide_Finish,taskID+index);
    }
}