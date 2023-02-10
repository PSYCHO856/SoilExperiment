using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;
using System;
using Debug = UnityEngine.Debug;

public partial class Card1Page : CardPage
{
    private MyCharacterMove playerMove;
    //overrider init and execute
    
    void DisablePlayerMove()
    {
        if (!playerMove)
            playerMove = ControllerExperiment.Instance.playerObj.GetComponent<MyCharacterMove>();

        playerMove.enabled = false;
    }
    
    void EnablePlayerMove()
    {
        playerMove.enabled = true;
    }

    public override void PageOpenInit()
    {
        base.PageOpenInit();
        DisablePlayerMove();
    }
    
    public override void PanelClose()
    {
        base.PanelClose();
        EnablePlayerMove();
    }

    
    // 创建一条射线

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Ray ray = ControllerExperiment.Instance.uiCam.ScreenPointToRay(Input.mousePosition);
    //         RaycastHit hit;
    //         // 射线碰撞检测
    //         if (Physics.Raycast(ray, out hit))
    //         {
    //             // 检测是否碰撞到UI
    //             if (hit.collider.gameObject.GetComponent<CanvasRenderer>() != null)
    //             {
    //                 // 在这里添加代码，当鼠标点击到UI上时执行。
    //                 Debug.Log("jiance dao ui!!!");
    //                 //禁用3d检测
    //             }
    //         }
    //     }
    // }
}
