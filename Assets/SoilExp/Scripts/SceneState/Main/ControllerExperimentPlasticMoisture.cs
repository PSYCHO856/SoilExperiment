using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public partial class ControllerExperiment
{
    public float BoxToBalanceEndHeighOffset4 = 0.14f;
    void BorderPlasticStepsJudge()
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
            if (stepsIndex == 0)
            {
                MoveSoilToGlass(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            }
            else if (stepsIndex == 2)
            {
                MoveSoilToBox(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            }
            else if (stepsIndex == 3)
            {
                MoveBoxToBalanceEquipment(selectedTrans, hit1.collider.transform, MoveEquipmentCallbackWithUIOption
                ,BoxToBalanceEndHeighOffset4);
                // MoveEquipment(selectedTrans, hit1.collider.transform, MoveEquipmentCallbackWithUIOption);
            }
            else
            {
                MoveEquipment(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            }

            isSelect = false;
            Invoke("DeselectAllGobj",0.5f);
        }
        
        
        //当前步骤中只有一个物体 选中时
        else if (!isSelect && currentStepEquipment.Count == 1 &&
                 hit1.collider.gameObject.name.Equals(currentStepEquipment[0].name))
        {
            if (stepsIndex == 1)
            {
                RollSoilCylinder(hit1.collider.transform);
            }
            
            //若出现需要点击确认的交互操作 ui点击消失后再读取下一步的信息
            if (!nowBtnText.Equals(""))
            {
                MoveEquipmentCallbackWithUIOption();
            }
            else
            {
                MoveEquipmentCallback();
            }
            Invoke("DeselectAllGobj",0.5f);
        }
    }

    public Vector3 SoilCylinderEndPos;
    void MoveSoilToGlass(Transform select,Transform target, TweenCallback callback = null)
    {
        Transform soilCylinder = FindSceneEquipmentObject("土条");
        soilCylinder.GetComponent<ControlDissolve>().BackNormal();

        target.GetChild(0).GetComponent<ControlDissolve>().BackNormal();
        target.GetChild(1).GetComponent<ControlDissolve>().BackNormal();
        DOTween.Sequence() // 返回一个新的Sequence
            .AppendCallback(callback);
    }

    public Transform FindSceneEquipmentObject(string gObjName)
    {
        foreach (var gobj in sceneEquipmentObjects)
        {
            if (gobj.name.Equals(gObjName)) return gobj.transform;
        }

        Debug.Log("gobj not finddd");
        return null;
    }
    
    void RollSoilCylinder(Transform select, TweenCallback callback = null)
    {
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(select.DOMove(SoilCylinderEndPos, 1f))
            .AppendCallback(callback);
    }
    
    void MoveSoilToBox(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
    {
        
        if (!selecteTrans) return;

        var position = selecteTrans.position;
        var position1 = targetTrans.position;
        float selectedTransHeight = selecteTrans.GetComponent<BoxCollider>().size.y * selecteTrans.localScale.y;
        float targetTransHeight = targetTrans.GetComponent<BoxCollider>().size.y * targetTrans.localScale.y;
        float moveDuration = 0.4f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position.y + 0.2f, position.z), moveDuration)) // 添加动画到队列中
            .AppendInterval(0.2f)
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position.y + 0.2f, position1.z), moveDuration))
            
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y - 0.02f, position1.z), moveDuration)
                .OnComplete(() =>
                {
                    targetTrans.GetChild(1).GetComponent<ControlDissolve>().BackNormal();
                    selecteTrans.SetParent(targetTrans);
                })
            )
            .AppendInterval(0.1f)
            .AppendCallback(callback);
    }
    
}