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

            if (stepsIndex == 6)
            {
                // hit1.collider.transform.GetChild(1).gameObject.SetActive(false);
                hit1.collider.transform.GetChild(1).GetComponent<ControlDissolve>().StartDisSolve();
            }
            else if (stepsIndex == 14)
            {
                // hit1.collider.transform.GetChild(1).gameObject.SetActive(true);
                hit1.collider.transform.GetChild(1).GetComponent<ControlDissolve>().BackNormal();
            }
            else if (stepsIndex == 3/* || stepsIndex == 4*/)
            {
                MoveShovelToBox(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            }
        }
        //当前步骤中两个物体 选中第二个时
        else if (currentStepEquipment.Count == 2 &&
                 hit1.collider.gameObject.name.Equals(currentStepEquipment[1].name) &&
                 isSelect)
        {
            if (stepsIndex == 1 || stepsIndex == 2 || stepsIndex == 4 || stepsIndex == 5 || stepsIndex == 15 || stepsIndex == 16)
            {
                //第二步 称重显示ui
                MoveBoxToBalanceEquipment(selectedTrans, hit1.collider.transform, MoveEquipmentCallbackWithUIOptionAndEquipmentReturn
                ,BoxToBalanceEndHeighOffset);
            }
            // else if (stepsIndex == 5 || stepsIndex == 6 || stepsIndex == 16 || stepsIndex == 17 )
            // {
            //     MoveEquipment(selectedTrans, hit1.collider.transform, MoveEquipmentCallbackWithUIOptionAndEquipmentReturn);
            // }
            // else if (stepsIndex == 3/* || stepsIndex == 4*/)
            // {
            //     MoveShovelToBox(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            // }
            else if(stepsIndex == 6)
            {
                // hit1.collider.transform.GetChild(1).gameObject.SetActive(false);
                hit1.collider.transform.GetChild(1).GetComponent<ControlDissolve>().StartDisSolve();
                MoveEquipmentCallback();
            }
            else if (stepsIndex == 14)
            {
                hit1.collider.transform.GetChild(1).GetComponent<ControlDissolve>().BackNormal();
                // hit1.collider.transform.GetChild(1).gameObject.SetActive(true);
                MoveEquipmentCallback();
            }
            else if(stepsIndex == 7 || stepsIndex == 8)
            {
                PutBoxesInBakingBox(selectedTrans, hit1.collider.transform, stepsIndex - 8, MoveEquipmentCallback);
            }
            else if(stepsIndex == 11 || stepsIndex == 12)
            {
                PutBoxesInDryBox(selectedTrans, hit1.collider.transform, stepsIndex - 8, MoveEquipmentCallback);
                if (stepsIndex == 12)
                {
                    OpenDryBoxTop();
                }
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

            if (stepsIndex == 9)
            {
                CloseBakingBox();
            }
            else if (stepsIndex == 10)
            {
                OpenBakingBox();
            }
            else if (stepsIndex == 13)
            {
                OpenDryBoxTop();
                dryBoxAnimator.transform.parent.GetComponent<BoxCollider>().enabled = false;
                MoveEquipmentCallback();
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
            Invoke("DeselectAllGobj",0.5f);
        }
    }
    
    void MoveEquipmentCallbackWithUIOptionAndEquipmentReturn()
    {
        Messenger<Vector3,string,Action>.Broadcast(GameEvent.ON_OPERATE_UPDATE, hit1.collider.transform.position,nowBtnText,MoveEquipmentCallback);

        ReturnOriginPos(selectedTrans.gameObject, 1.5f, true, false);

    }

    void MoveShovelToBox(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
    {
        // Transform shovel = selecteTrans.GetChild(0);
        // Transform boxTop = targetTrans.GetChild(1);
        // Vector3 shovelRotation = shovel.rotation.eulerAngles;
        //
        // var position = shovel.position;
        // var position1 = boxTop.position;
        // float moveDuration = 0.5f;
        // DOTween.Sequence() // 返回一个新的Sequence
        //     .Append(shovel.DOMove(new Vector3(position.x, position.y + 0.2f, position.z),
        //         moveDuration)) // 添加动画到队列中
        //
        //     .Append(boxTop.DOMove(new Vector3(position1.x, position1.y + 0.3f, position1.z), moveDuration))
        //
        //     .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z + 0.05f), moveDuration))
        //     .Append(shovel.DORotate(new Vector3(0, -90, 60), moveDuration))
        //     .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.05f, position1.z + 0.05f), moveDuration)
        //     .OnComplete(() =>
        //     {
        //         targetTrans.GetChild(2).GetComponent<ControlDissolve>().BackNormal();
        //         // targetTrans.GetChild(2).gameObject.SetActive(true);
        //         shovel.GetChild(2).gameObject.SetActive(false);
        //     }))
        //
        //     //回位
        //     .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z + 0.05f), moveDuration))
        //     .Append(shovel.DOMove(new Vector3(position.x, position.y + 0.2f, position.z),
        //         moveDuration))
        //     .Append(shovel.DORotate(shovelRotation, moveDuration))
        //     .Append(shovel.DOMove(new Vector3(position.x, position.y, position.z), moveDuration)
        //         .OnComplete(() =>
        //         {
        //             shovel.GetChild(2).gameObject.SetActive(true);
        //         })
        //     )
        //     .Append(boxTop.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
        //     
        //     .Append(boxTop.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
        //     
        //     .AppendCallback(callback);
        
                        ObjectsWithAnime[0].GetComponent<Animation>().Play();
                        DOTween.Sequence() // 返回一个新的Sequence
                            .AppendInterval(20f)
                            .AppendCallback(MoveEquipmentCallback);
                        isSelect = false;
                        Invoke("DeselectAllGobj",0.5f);
                        
        
    }

    void PutBoxesInBakingBox(Transform selecteTrans, Transform targetTrans, int pos, TweenCallback callback = null)
    {
        var position = selecteTrans.position;
        var position1 = targetTrans.transform.GetChild(pos).position;
        
        float moveDuration = 0.5f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position1.y, position.z),
                moveDuration)) // 添加动画到队列中
            .Append(selecteTrans.DOMove(new Vector3(position.x, position1.y, position1.z), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
            .AppendCallback(callback);
    }

    public Animator bakingBoxDoor;
    void CloseBakingBox()
    {
        bakingBoxDoor.SetTrigger("closeDoor");
        MoveEquipmentCallback();
    }
    
    void OpenBakingBox()
    {
        bakingBoxDoor.SetTrigger("openDoor");
        bakingBoxDoor.transform.parent.GetComponent<BoxCollider>().enabled = false;
        MoveEquipmentCallback();
    }
    
    void PutBoxesInDryBox(Transform selecteTrans, Transform targetTrans, int pos, TweenCallback callback = null)
    {
        var position = selecteTrans.position;
        var position1 = targetTrans.transform.GetChild(pos).position;
        
        float moveDuration = 0.5f;
        if (pos == 4)
        {
            DOTween.Sequence() // 返回一个新的Sequence
                .Append(selecteTrans.DOMove(new Vector3(position.x + 0.2f, position.y, position.z),
                    moveDuration)) // 添加动画到队列中
                .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z), moveDuration))
                .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
                .AppendCallback(callback);
        }
        else
        {
            DOTween.Sequence() // 返回一个新的Sequence
                .Append(selecteTrans.DOMove(new Vector3(position.x + 0.2f, position.y, position.z),
                    moveDuration)) // 添加动画到队列中
                .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z), moveDuration))
                .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
                .AppendCallback(CloseDryBoxTop)
                .AppendCallback(callback);
        }
    }

    public Animator dryBoxAnimator;
    void OpenDryBoxTop()
    {
        dryBoxAnimator.SetTrigger("openTop");
    }
    
    void CloseDryBoxTop()
    {
        dryBoxAnimator.SetTrigger("closeTop");
    }

    public float BoxToBalanceEndHeighOffset = 0;
    void MoveBoxToBalanceEquipment(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null,
        float endHeighOffset = 0)
    {
        MoveEquipment(selecteTrans, targetTrans, callback, endHeighOffset);
    }
    
}