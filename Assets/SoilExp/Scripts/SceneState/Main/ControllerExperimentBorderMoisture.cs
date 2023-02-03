using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public partial class ControllerExperiment
{
    void BorderMoistureStepsJudge()
    {
        //当前步骤中两个物体 选中了第一个时
        if (hit1.collider.gameObject.name.Equals(currentStepEquipment[0].name) &&
            currentStepEquipment.Count == 2)
        {
            isSelect = true;
            selectedTrans = hit1.collider.transform;
            RefreshSuggestGobj(currentStepEquipment[1]);

            if (stepsIndex == 7)
            {
                hit1.collider.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        //当前步骤中两个物体 选中第二个时
        else if (currentStepEquipment.Count == 2 &&
                 hit1.collider.gameObject.name.Equals(currentStepEquipment[1].name) &&
                 isSelect)
        {
            if (stepsIndex == 1 || stepsIndex == 2 || stepsIndex == 5 || stepsIndex == 6 || stepsIndex == 16 || stepsIndex == 17 )
            {
                //第二步 称重显示ui
                //显示交互按钮的流程？ 
                MoveEquipment(selectedTrans, hit1.collider.transform, MoveEquipmentCallbackWithUIOptionAndEquipmentReturn);
            }
            else if (stepsIndex == 0)
            {
                MoveShovelToSieve(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
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

            if (stepsIndex == 0)
            {
                
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
    
    // void MoveEquipmentCallbackWithUIOptionAndEquipmentReturn()
    // {
    //     Messenger<Vector3,string,Action>.Broadcast(GameEvent.ON_OPERATE_UPDATE, hit1.collider.transform.position,nowBtnText,MoveEquipmentCallback);
    //
    //     ReturnOriginPos(selectedTrans.gameObject, 1.5f, true, false);
    //
    // }
    //
    // void MoveShovelToBox(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
    // {
    //     Transform shovel = selecteTrans.GetChild(0);
    //     Transform boxTop = targetTrans.GetChild(1);
    //     
    //     var position = shovel.position;
    //     var position1 = boxTop.position;
    //     float moveDuration = 0.5f;
    //     DOTween.Sequence() // 返回一个新的Sequence
    //         .Append(shovel.DOMove(new Vector3(position.x, position.y + 0.2f, position.z),
    //             moveDuration)) // 添加动画到队列中
    //
    //         .Append(boxTop.DOMove(new Vector3(position1.x, position1.y + 0.3f, position1.z), moveDuration))
    //
    //         .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z + 0.05f), moveDuration))
    //         .Append(shovel.DORotate(new Vector3(0, -90, 60), moveDuration))
    //         .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.05f, position1.z + 0.05f), moveDuration)
    //         .OnComplete(() =>
    //         {
    //             targetTrans.GetChild(2).gameObject.SetActive(true);
    //             shovel.GetChild(2).gameObject.SetActive(false);
    //         }))
    //
    //         //回位
    //         .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z + 0.05f), moveDuration))
    //         .Append(shovel.DOMove(new Vector3(position.x, position.y + 0.2f, position.z),
    //             moveDuration))
    //         .Append(shovel.DORotate(new Vector3(5.408f, -9.438f, 29.553f), moveDuration))
    //         .Append(shovel.DOMove(new Vector3(position.x, position.y, position.z), moveDuration)
    //             .OnComplete(() =>
    //             {
    //                 shovel.GetChild(2).gameObject.SetActive(true);
    //             })
    //         )
    //         .Append(boxTop.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
    //         
    //         .Append(boxTop.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
    //         
    //         .AppendCallback(callback);
    // }
    //
    // void PutBoxesInBakingBox(Transform selecteTrans, Transform targetTrans, int pos, TweenCallback callback = null)
    // {
    //     var position = selecteTrans.position;
    //     var position1 = targetTrans.transform.GetChild(pos).position;
    //     
    //     float moveDuration = 0.5f;
    //     DOTween.Sequence() // 返回一个新的Sequence
    //         .Append(selecteTrans.DOMove(new Vector3(position.x, position1.y, position.z),
    //             moveDuration)) // 添加动画到队列中
    //         .Append(selecteTrans.DOMove(new Vector3(position.x, position1.y, position1.z), moveDuration))
    //         .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
    //         .AppendCallback(callback);
    // }
    //
    // public Animator bakingBoxDoor;
    // void CloseBakingBox()
    // {
    //     bakingBoxDoor.SetTrigger("closeDoor");
    //     MoveEquipmentCallback();
    // }
    //
    // void OpenBakingBox()
    // {
    //     bakingBoxDoor.SetTrigger("openDoor");
    //     bakingBoxDoor.transform.parent.GetComponent<BoxCollider>().enabled = false;
    //     MoveEquipmentCallback();
    // }
    //
    // void PutBoxesInDryBox(Transform selecteTrans, Transform targetTrans, int pos, TweenCallback callback = null)
    // {
    //     var position = selecteTrans.position;
    //     var position1 = targetTrans.transform.GetChild(pos).position;
    //     
    //     float moveDuration = 0.5f;
    //     if (pos == 4)
    //     {
    //         DOTween.Sequence() // 返回一个新的Sequence
    //             .Append(selecteTrans.DOMove(new Vector3(position.x + 0.2f, position.y, position.z),
    //                 moveDuration)) // 添加动画到队列中
    //             .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z), moveDuration))
    //             .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
    //             .AppendCallback(callback);
    //     }
    //     else
    //     {
    //         DOTween.Sequence() // 返回一个新的Sequence
    //             .Append(selecteTrans.DOMove(new Vector3(position.x + 0.2f, position.y, position.z),
    //                 moveDuration)) // 添加动画到队列中
    //             .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z), moveDuration))
    //             .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
    //             .AppendCallback(CloseDryBoxTop)
    //             .AppendCallback(callback);
    //     }
    // }
    //
    // public Animator dryBoxAnimator;
    // void OpenDryBoxTop()
    // {
    //     dryBoxAnimator.SetTrigger("openTop");
    // }
    //
    // void CloseDryBoxTop()
    // {
    //     dryBoxAnimator.SetTrigger("closeTop");
    // }
    
    void MoveShovelToSieve(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
    {
        Transform shovel = selecteTrans.GetChild(0);
        Transform boxTop = targetTrans.GetChild(1);
        Vector3 shovelRotation = shovel.rotation.eulerAngles;
        
        var position = shovel.position;
        var position1 = boxTop.position;
        float moveDuration = 0.5f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(shovel.DOMove(new Vector3(position.x, position.y + 0.2f, position.z),
                moveDuration)) // 添加动画到队列中

            .Append(boxTop.DOMove(new Vector3(position1.x, position1.y + 0.3f, position1.z), moveDuration))

            .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z + 0.05f), moveDuration))
            .Append(shovel.DORotate(new Vector3(0, -90, 60), moveDuration))
            .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.05f, position1.z + 0.05f), moveDuration)
            .OnComplete(() =>
            {
                targetTrans.GetChild(2).gameObject.SetActive(true);
                shovel.GetChild(2).gameObject.SetActive(false);
            }))

            //回位
            .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z + 0.05f), moveDuration))
            .Append(shovel.DOMove(new Vector3(position.x, position.y + 0.2f, position.z),
                moveDuration))
            .Append(shovel.DORotate(shovelRotation, moveDuration))
            .Append(shovel.DOMove(new Vector3(position.x, position.y, position.z), moveDuration)
                .OnComplete(() =>
                {
                    shovel.GetChild(2).gameObject.SetActive(true);
                })
            )
            .Append(boxTop.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
            
            .Append(boxTop.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
            
            .AppendCallback(callback);
    }
}