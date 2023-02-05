using System;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 移动到指定位置的元件
/// </summary>
public class TaskMoveUnit : TaskEmit
{
    [SerializeField]
    public UnityEvent ac;
    public override void OnEmit(){
        MessageCenter.Instance.BoradCastMessage(EMsg.Guide_Finish,taskID);
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            MessageCenter.Instance.BoradCastMessage(EMsg.Guide_Finish,taskID);
            transform.position=Vector3.zero;
            this.gameObject.SetActive(false);
            ac.Invoke();
        }
    }
}
