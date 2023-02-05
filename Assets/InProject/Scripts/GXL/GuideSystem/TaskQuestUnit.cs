using System;
using UnityEngine;
/// <summary>
/// 答题任务牌子的元件
/// </summary>
public class TaskQuestUnit : TaskEmit
{
    public string QId;//答题ID
    public Action ac;
    /// <summary>
    /// 是否已经打过题
    /// </summary>
    public bool isOn=true;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        ac=OnEnd;
    }
    /// <summary>
    /// 射线检测事件-触发事件
    /// </summary>
    public override void OnEmit(){
        MUIMgr.Instance.OpenUI(EMUI.MUI_QuestionSingle,ac,QId);
    }
    
    public virtual void OnEnd(){
        MessageCenter.Instance.BoradCastMessage(EMsg.Guide_Finish,taskID);
    }

}
