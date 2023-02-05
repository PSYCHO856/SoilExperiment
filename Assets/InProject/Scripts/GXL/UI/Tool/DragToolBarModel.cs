using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 拖动工具栏到模型
/// </summary>
public class DragToolBarModel : DragGridCom
{
    public LayerMask layerMask;
    public string UID;//当前设备名称
    private void Start()
    {
        onEndDrag+=(point)=>{
            IsTargetObj();
        };
    }

    private bool IsTargetObj(){
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //
        if(Physics.Raycast(ray,out hitInfo,Mathf.Infinity,layerMask.value)){
            var targetModel=hitInfo.collider;
            Debug.Log("清除："+targetModel.name);
           // MessageCenter.Instance.BoradCastMessage(EMsg.Player_tools,false);
        }
        return false;
    }
}
