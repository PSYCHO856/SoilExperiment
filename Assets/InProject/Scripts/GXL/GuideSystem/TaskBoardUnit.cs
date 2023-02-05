using UnityEngine;
/// <summary>
/// 牌子任务的元件
/// </summary>
public class TaskBoardUnit : TaskEmit
{
    /// <summary>
    /// 射线触发
    /// </summary>
    public override void OnEmit(){
        TipWindow.instanes.Open(this);
        AudioManager.Instance.PlayAudio(1,EAudio.se_btn_01);
    }
}
