using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 任务事件触发接口
/// </summary>
public class TaskRayInter : MonoBehaviour,IRayInteraction
{
    private bool IsRay=true;
  
    private void Start()
    {
        MessageCenter.Instance.RegiseterMessage(EMsg.Player_IsMove,this,MoveAction);//注册消息
    }
    public void OnHover(RaycastHit hitInfo)
    {
        Transform tr = hitInfo.collider.transform;
        if (tr == null) { return; }
        Debug.Log("模型射线检测是否打开-"+IsRay);
        if (Input.GetMouseButtonUp(0)&&IsRay)
        {
            if(tr.TryGetComponent(out TaskEmit comp)){
                comp.OnEmit();
            }
        }
    }
    void MoveAction(params object[] parms){
        IsRay=((bool)parms[0]);
    }
}
