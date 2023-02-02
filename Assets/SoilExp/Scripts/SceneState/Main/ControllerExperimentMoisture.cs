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
            if (stepsIndex == 3 || stepsIndex == 11)
            {
                //第三步 环刀刷凡士林 有特殊动画
                BrushCircleKnife(selectedTrans, hit1.collider.transform);
            }
            else
            {
                MoveEquipment(selectedTrans, hit1.collider.transform);
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
}