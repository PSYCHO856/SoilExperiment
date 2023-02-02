using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public partial class ControllerExperiment
{
    void MoistureStepsJudge()
    {
        //当前步骤中两个物体 选中了第一个时
        if (hit1.collider.gameObject.name.Equals(currentStepEquipment[0].name) &&
            currentStepEquipment.Count == 2)
        {
            isSelect = true;
            selectedTrans = hit1.collider.transform;
            RefreshSuggestGobj(currentStepEquipment[1]);
        }
        //当前步骤中两个物体 选中第二个时
        else if (currentStepEquipment.Count == 2 &&
                 hit1.collider.gameObject.name.Equals(currentStepEquipment[1].name) &&
                 isSelect)
        {
            if (stepsIndex == 1 || stepsIndex == 2)
            {
                //第二步 称重显示ui
                //显示交互按钮的流程？ 
                MoveEquipment(selectedTrans, hit1.collider.transform, MoveEquipmentCallbackWithUIOptionAndEquipmentReturn);
            }
            else if (stepsIndex == 2)
            {
                MoveEquipment(selectedTrans, hit1.collider.transform, MoveEquipmentCallbackWithUIOption);
            }
            else
            {
                MoveEquipment(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            }

            isSelect = false;
        }
        //当前步骤中只有一个物体 选中时
        else if (!isSelect && currentStepEquipment.Count == 1 &&
                 hit1.collider.gameObject.name.Equals(currentStepEquipment[0].name))
        {

            if (stepsIndex == 5 || stepsIndex == 13)
            {
                //第五步 环刀插进土里
                MoveCircleKnifeInSoil(selectedTrans);
            }

            //若出现需要点击确认的交互操作 ui点击消失后再读取下一步的信息
            else if (!nowBtnText.Equals(""))
            {
                MoveEquipmentCallbackWithUIOption();
            }
            else
            {
                MoveEquipmentCallback();
            }
        }
    }
    
    void MoveEquipmentCallbackWithUIOptionAndEquipmentReturn()
    {
        Messenger<Vector3,string,Action>.Broadcast(GameEvent.ON_OPERATE_UPDATE, hit1.collider.transform.position,nowBtnText,MoveEquipmentCallback);

        ReturnOriginPos(selectedTrans.gameObject, 1.5f, true, false);

    }
}