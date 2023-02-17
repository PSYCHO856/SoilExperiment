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
            
            if (stepsIndex == 0)
            {
                ObjectsWithAnime[0].GetComponent<Animation>().Play();
                DOTween.Sequence() // 返回一个新的Sequence
                    .AppendInterval(40f)
                    .AppendCallback(MoveEquipmentCallback);
                
                isSelect = false;
                Invoke("DeselectAllGobj",0.5f);
            }

        }
        //当前步骤中两个物体 选中第二个时
        else if (currentStepEquipment.Count == 2 &&
                 hit1.collider.gameObject.name.Equals(currentStepEquipment[1].name) &&
                 isSelect)
        {
            // if (stepsIndex == 0)
            // {
            //     // MoveShovelToSieve(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            // }
            // else if (stepsIndex == 1)
            // {
            //     // plate1 = hit1.collider.transform;
            //     // AddSoilToPlate(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            // }
            // else if (stepsIndex == 2)
            // {
            //     // plate2 = hit1.collider.transform;
            //     // AddSoilToPlate(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            // }
            // else if (stepsIndex == 3)
            // {
            //     // plate3 = hit1.collider.transform;
            //     // AddSoilToPlate(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            // }
            // else if (stepsIndex == 4)
            // {
            //     // AddWaterToPlate(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            // }
            if (stepsIndex == 1)
            {
                UseTIAOKnifeToPlate1(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            }
            else if (stepsIndex == 2)
            {
                UseTIAOKnifeToPlate2(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            }
            else if (stepsIndex == 3)
            {
                AddSoilToCup(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
            }
            else if (stepsIndex == 4)
            {
                MoveCupToUnityEquipment(selectedTrans, hit1.collider.transform, MoveEquipmentCallback);
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

            if (stepsIndex == 5)
            {
                HighUnityEquipment(hit1.collider.transform);
            }
            if (stepsIndex == 6)
            {
                UnityEquipmentActive(hit1.collider.transform);
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
    
    Transform shovel;
    Transform sieveTop;
    Transform sieveMiddle;
    Transform sieveBottom;
    void MoveShovelToSieve(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
    {
        shovel = selecteTrans.GetChild(0);
        sieveTop = targetTrans.GetChild(0);
        sieveMiddle = targetTrans.GetChild(1);
        sieveBottom = targetTrans.GetChild(2);
        Vector3 shovelRotation = shovel.rotation.eulerAngles;
        
        var position = shovel.position;
        var position1 = sieveTop.position;
        float moveDuration = 0.5f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(shovel.DOMove(new Vector3(position.x, position.y + 0.2f, position.z),
                moveDuration)) // 添加动画到队列中

            .Append(sieveTop.DOMove(new Vector3(position1.x, position1.y + 0.3f, position1.z), moveDuration))
            .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z - 0.1f), moveDuration))
            .Append(shovel.DORotate(new Vector3(0, 90, 60), moveDuration))
            .Append(shovel.DOMove(new Vector3(position1.x, position1.y + 0.05f, position1.z - 0.1f), moveDuration)
            .OnComplete(() =>
            {
                shovel.gameObject.GetComponent<ControlDissolve>().StartDisSolve();
                // shovel.gameObject.SetActive(false);
            }))
            // .Append(boxTop.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
            .AppendCallback(SieveCallback)
            .AppendInterval(1.5f).OnComplete(()=>
        {
            sieveBottom.GetChild(1).gameObject.SetActive(true);
        })
            .AppendCallback(callback);
    }

    
    void SieveCallback()
    {
        sieveTop.GetComponent<ControlDissolve>().StartDisSolve();
        sieveMiddle.GetComponent<ControlDissolve>().StartDisSolve();
        // sieveMiddle.gameObject.SetActive(false);
        sieveBottom.GetChild(0).gameObject.SetActive(true);
        
    }

    Transform shovel2;
    Transform plateSoil;
    
    void AddSoilToPlate(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
    {
        shovel2 = sieveBottom.GetChild(1);
        plateSoil = targetTrans.GetChild(1);
        Quaternion shovelRotation = shovel2.rotation;
        var position = shovel2.position;
        var position1 = targetTrans.position;
        float moveDuration = 0.5f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(shovel2.DOMove(new Vector3(position.x, position.y + 0.2f, position.z),
                moveDuration)) // 添加动画到队列中

            .Append(shovel2.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z - 0.06f), moveDuration))
            .Append(shovel2.DORotate(new Vector3(0, 90, 60), moveDuration))
            .Append(shovel2.DOMove(new Vector3(position1.x, position1.y + 0.05f, position1.z - 0.06f), moveDuration)
                .OnComplete(() =>
                {
                    shovel2.GetComponent<ControlDissolve>().StartDisSolve();
                    plateSoil.gameObject.SetActive(true);
                    plateSoil.GetComponent<ControlDissolve>().BackNormal();
                    // plateSoil.gameObject.SetActive(true);
                }))
            .AppendInterval(1.5f).OnComplete(()=>
            {
            shovel2.position = position;
            shovel2.rotation = shovelRotation;
            shovel2.gameObject.SetActive(true);
            shovel2.GetComponent<ControlDissolve>().SetActive();
            })
            
            .AppendCallback(callback);
    }

    public Transform plate1;
    public Transform plate2;
    public Transform plate3;
    void AddWaterToPlate(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
    {
        Transform cylinder = selecteTrans;
        
        var position = cylinder.position;
        var position1 = plate1.position;
        var position2 = plate2.position;
        var position3 = plate3.position;
        
        float plateHeight = targetTrans.GetComponent<BoxCollider>().size.y * targetTrans.localScale.y;
        
        float moveDuration = 0.5f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(cylinder.DOMove(new Vector3(position.x, position.y + 0.2f, position.z),
                moveDuration)) // 添加动画到队列中

            .Append(cylinder.DOMove(new Vector3(position1.x - 0.1f, position1.y + 0.1f, position1.z - 0.05f), moveDuration))
            .Append(cylinder.DOMove(new Vector3(position1.x - 0.1f, position1.y + plateHeight, position1.z - 0.05f), moveDuration))
            .Append(cylinder.DORotate(new Vector3(0, 60, 0), moveDuration))
            .Append(cylinder.DORotate(new Vector3(-90, 0, 0), moveDuration))
            
            .Append(cylinder.DOMove(new Vector3(position2.x - 0.1f, position2.y + plateHeight, position2.z - 0.05f), moveDuration))
            .Append(cylinder.DORotate(new Vector3(0, 60, 0), moveDuration))
            .Append(cylinder.DORotate(new Vector3(-90, 0, 0), moveDuration))
            
            .Append(cylinder.DOMove(new Vector3(position3.x - 0.1f, position3.y + plateHeight, position3.z - 0.05f), moveDuration))
            .Append(cylinder.DORotate(new Vector3(0, 60, 0), moveDuration))
            .Append(cylinder.DORotate(new Vector3(-90, 0, 0), moveDuration))
            
            .Append(cylinder.DOMove(position, moveDuration))
            .AppendCallback(callback);
    }

    private Vector3 tiaoPos;
        void UseTIAOKnifeToPlate1(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
    {
        var position = selecteTrans.position;
        tiaoPos = position;
        var position1 = plate1.position;
        var position2 = plate2.position;
        var position3 = plate3.position;
        
        float plateRadius = 0.05f;
        // float plateRadius = targetTrans.GetComponent<BoxCollider>().size.x * targetTrans.localScale.x;
        float plateHeight = 0.02f;
        
        float moveDuration = 0.5f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position.y + 0.1f, position.z),
                moveDuration)) // 添加动画到队列中

            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + 0.1f, position1.z - plateRadius), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + plateHeight, position1.z - plateRadius), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(-20, -50, -50), moveDuration))
            //搅拌
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + plateHeight, position1.z - plateRadius/2), moveDuration/2))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + plateHeight, position1.z - plateRadius), moveDuration/2))
            
            .Append(selecteTrans.DOMove(new Vector3(position2.x, position2.y + 0.15f, position2.z - plateRadius), moveDuration)
                .OnComplete(() =>
                {
                    plate1.GetChild(2).GetComponent<ControlDissolve>().BackNormal();//布
                    // plate1.GetChild(2).gameObject.SetActive(true);//布
                }))
            .Append(selecteTrans.DOMove(new Vector3(position2.x, position2.y + plateHeight, position2.z - plateRadius), moveDuration))
            
            .Append(selecteTrans.DOMove(new Vector3(position2.x, position2.y + plateHeight, position2.z - plateRadius/2), moveDuration/2))
            .Append(selecteTrans.DOMove(new Vector3(position2.x, position2.y + plateHeight, position2.z - plateRadius), moveDuration/2))

            
            .Append(selecteTrans.DOMove(new Vector3(position3.x, position3.y + 0.15f, position3.z - plateRadius), moveDuration)
                .OnComplete(() =>
                {
                    plate2.GetChild(2).GetComponent<ControlDissolve>().BackNormal();
                    // plate2.GetChild(2).gameObject.SetActive(true);//布
                }))
            .Append(selecteTrans.DOMove(new Vector3(position3.x, position3.y + plateHeight, position3.z - plateRadius), moveDuration))
            
            .Append(selecteTrans.DOMove(new Vector3(position3.x, position3.y + plateHeight, position3.z - plateRadius/2), moveDuration/2))
            .Append(selecteTrans.DOMove(new Vector3(position3.x, position3.y + plateHeight, position3.z - plateRadius), moveDuration/2))
            .Append(selecteTrans.DOMove(new Vector3(position3.x, position3.y + 0.15f, position3.z - plateRadius), moveDuration)
                .OnComplete(() =>
                {
                    plate3.GetChild(2).GetComponent<ControlDissolve>().BackNormal();
                    // plate3.GetChild(2).gameObject.SetActive(true);//布
                }))
            .Append(selecteTrans.DORotate(Vector3.zero, moveDuration))
            .Append(selecteTrans.DOMove(position, moveDuration))
            
            .AppendCallback(callback);
    }
        
        void UseTIAOKnifeToPlate2(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
    {
        var position = selecteTrans.position;
        var position1 = plate1.position;
        
        float plateRadius = 0.05f;
        // float plateRadius = targetTrans.GetComponent<BoxCollider>().size.x * targetTrans.localScale.x;
        float plateHeight = 0.02f;
        
        float moveDuration = 0.5f;
        plate1.GetChild(2).gameObject.SetActive(false);
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position.y + 0.1f, position.z),
                moveDuration)) // 添加动画到队列中

            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + 0.1f, position1.z - plateRadius), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(-20, -50, -50), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + plateHeight, position1.z - plateRadius), moveDuration))

            //搅拌
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + plateHeight, position1.z - plateRadius/2), moveDuration/2))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + plateHeight, position1.z - plateRadius), moveDuration/2)
                .OnComplete(() =>
                {
                    selecteTrans.GetChild(2).gameObject.SetActive(true);
                })
            )
            .AppendCallback(callback);
    }
        
        void AddSoilToCup(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
        {
            var position = selecteTrans.position;
            var position1 = targetTrans.position;
        
            float plateRadius = 0.04f;
            float plateHeight = 0.02f;
        
            float moveDuration = 0.5f;
            DOTween.Sequence() // 返回一个新的Sequence
                .Append(selecteTrans.DOMove(new Vector3(position.x, position.y + 0.13f, position.z),
                    moveDuration)) // 添加动画到队列中

                .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + 0.13f, position1.z - plateRadius), moveDuration))
                .Append(selecteTrans.DORotate(new Vector3(-20, -70, -70), moveDuration))
                .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + plateHeight, position1.z - plateRadius), moveDuration))
                
                //搅拌
                .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + plateHeight, position1.z - plateRadius/2), moveDuration/2))
                .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + plateHeight, position1.z - plateRadius), moveDuration/2)
                    .OnComplete(() =>
                    {
                        targetTrans.GetChild(1).GetComponent<ControlDissolve>().BackNormal();
                        // targetTrans.GetChild(1).gameObject.SetActive(true);
                        selecteTrans.GetChild(2).gameObject.SetActive(false);
                    }))
                .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + 0.2f, position1.z - plateRadius), moveDuration))
                .Append(selecteTrans.DORotate(Vector3.zero, moveDuration))
                .Append(selecteTrans.DOMove(tiaoPos, moveDuration))
                
                .AppendCallback(callback);
        }
        
        void MoveCupToUnityEquipment(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
        {
            var position = selecteTrans.position;
            var position1 = targetTrans.GetChild(0).GetChild(0).position;

            float moveDuration = 0.5f;
            DOTween.Sequence() // 返回一个新的Sequence
                .Append(selecteTrans.DOMove(new Vector3(position.x, position1.y, position.z),
                    moveDuration)) // 添加动画到队列中

                .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
                .AppendCallback(callback);
            
            selecteTrans.SetParent(targetTrans.GetChild(0));
        }

        void HighUnityEquipment(Transform selecteTrans, TweenCallback callback = null)
        {
            Transform light = selecteTrans.GetChild(1).GetChild(0);
            var position = selecteTrans.position;
            var aimPosition = selecteTrans.GetChild(2).position;
            float moveDuration = 0.5f;
            DOTween.Sequence() // 返回一个新的Sequence
                .Append(selecteTrans.GetChild(0).DOMove(aimPosition,
                    moveDuration)
                    .OnComplete(() =>
                    {
                        // light
                        selecteTrans.GetChild(1).GetChild(0).GetComponent<Renderer>().material.color = Color.red;
                    })) // 添加动画到队列中
                .AppendCallback(callback);
        }
        
        void UnityEquipmentActive(Transform selecteTrans, TweenCallback callback = null)
        {
            var position = selecteTrans.position;
            var aimPosition = selecteTrans.GetChild(2).position;
            float moveDuration = 0.5f;
            // DOTween.Sequence() // 返回一个新的Sequence
            //     .Append(selecteTrans.DOMove(aimPosition,
            //             moveDuration)
            //         .OnComplete(() =>
            //         {
            //             // light
            //         })) // 添加动画到队列中
            //     .AppendCallback(callback);
            
        }
}