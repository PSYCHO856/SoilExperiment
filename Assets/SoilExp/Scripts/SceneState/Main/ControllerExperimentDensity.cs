using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public partial class ControllerExperiment
{
    void OpenTIANPING(Transform tianping)
    {
        Renderer[] renderers;
        renderers = tianping.GetComponents<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                if (renderers[i].materials[j].name.Equals("Material #42 (Instance)"))
                {
                    renderers[i].materials[j].DOColor(Color.white, 1.5f);
                }
            }
        }
    }
    
        //涂抹凡士林
    void BrushCircleKnife(Transform selecteTrans, Transform targetTrans)
    {
        if (!selecteTrans) return;
        var position = selecteTrans.position;
        var position1 = targetTrans.position;

        float moveDuration = 0.5f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(
                selecteTrans.DOMove(new Vector3(position.x, position.y + brushMoveHeight, position.z),
                    moveDuration)) // 添加动画到队列中
            .Append(targetTrans.DOMove(new Vector3(position1.x, position.y + 0.4f, position1.z), moveDuration))
            .Append(
                selecteTrans.DOMove(new Vector3(position1.x, position.y + brushMoveHeight, position1.z),
                    moveDuration))
            
            .Append(
                selecteTrans.DOMove(new Vector3(position1.x, position.y + brushMoveHeight - 0.05f, position1.z), moveDuration))
            
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position.y + brushMoveHeight - 0.05f, position1.z + 0.05f), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position.y + brushMoveHeight - 0.05f, position1.z - 0.02f), moveDuration))
            .Append(
                selecteTrans.DOMove(new Vector3(position1.x, position.y + brushMoveHeight, position1.z), moveDuration))
            .Append(
                selecteTrans.DOMove(new Vector3(position.x, position.y + brushMoveHeight, position.z), moveDuration))
            .Append(
                selecteTrans.DOMove(new Vector3(position.x, position.y, position.z), moveDuration))
            ;
        ReturnOriginPos(targetTrans.gameObject,4.5f, true);
        
    }

    void MoveCircleKnifeToSoil(Transform selecteTrans, Transform targetTrans)
    {
        if (!selecteTrans) return;

        var position = selecteTrans.position;
        var position1 = targetTrans.position;
        float selectedTransHeight = selecteTrans.GetComponent<BoxCollider>().size.y * selecteTrans.localScale.y;
        float targetTransHeight = targetTrans.GetComponent<BoxCollider>().size.y * targetTrans.localScale.y;
        float moveDuration = 0.4f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position.y + 0.4f, position.z), moveDuration)) // 添加动画到队列中
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position.y + 0.4f, position1.z), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(180, 0, 0), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + (selectedTransHeight + targetTransHeight)/2, position1.z), moveDuration))
            .AppendCallback(MoveEquipmentCallback);
    }
    
    void MoveCircleKnifeInSoil(Transform selecteTrans)
    {
        var position = selecteTrans.position;
        float selectedTransHeight = selecteTrans.GetComponent<BoxCollider>().size.y * selecteTrans.localScale.y;
        float moveDuration = 0.4f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position.y - 0.01f, position.z), moveDuration)) // 添加动画到队列中
            .AppendInterval(moveDuration) // 添加时间间隔
            .AppendCallback(MoveEquipmentCallback);
        
        //(selectedTransHeight + targetTransHeight)/2
    }
    
    void MoveXIAOKnifeToSoil(Transform selecteTrans, Transform targetTrans)
    {
        if (!selecteTrans) return;
        
        var position = selecteTrans.position;
        var position1 = targetTrans.position;
        float selectedTransHeight = selecteTrans.GetComponent<BoxCollider>().size.y * selecteTrans.localScale.y;
        float targetTransHeight = targetTrans.GetComponent<BoxCollider>().size.y * targetTrans.localScale.y / 2;

        float targetTransRadius = targetTrans.GetComponent<BoxCollider>().size.z * targetTrans.localScale.z / 2;
        
        
        float moveDuration = 0.5f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position.y + 0.2f, position.z),
                moveDuration)) // 添加动画到队列中
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position.y + 0.2f, position1.z), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(45, 90, 0), moveDuration))

            .Append(selecteTrans.DOMove(new Vector3(position1.x + xiaoKnifeMoveOffsetX,
                position1.y + targetTransHeight + 0.01f, position1.z + targetTransRadius), moveDuration))
            .AppendInterval(0.5f)
            .Append(selecteTrans.DOMove(new Vector3(position1.x + xiaoKnifeMoveOffsetX,
                position1.y + targetTransHeight - 0.01f, position1.z + targetTransRadius), moveDuration))
            .AppendCallback(XIAOKnifeCallback)
            .Append(circleKnifeTrans.DOMove(
                new Vector3(circleKnifeTrans.position.x, circleKnifeTrans.position.y - 0.05f,
                    circleKnifeTrans.position.z), moveDuration))
            ;
        
        ReturnOriginPos(selecteTrans.gameObject, 3.5f, true);
    }
    
    void MoveGUAKnifeToSoil(Transform selecteTrans, Transform targetTrans)
    {
        if (!selecteTrans) return;
        
        var position = selecteTrans.position;
        var position1 = targetTrans.position;
        float selectedTransHeight = selecteTrans.GetComponent<BoxCollider>().size.z * selecteTrans.localScale.z;
        float targetTransHeight = targetTrans.GetComponent<BoxCollider>().size.y * targetTrans.localScale.y;
        float targetTransRadis = targetTrans.GetComponent<BoxCollider>().size.x * targetTrans.localScale.x / 2;
        float heightOffset = targetTransHeight / 2 + selectedTransHeight / 2;
        
        float moveDuration = 0.5f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position1.y + heightOffset, position.z), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(-90,0,0), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + heightOffset, position1.z + targetTransRadis * 1.3f), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(-60,0,0), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + heightOffset, position1.z + targetTransRadis * 0.5f), moveDuration))
            
            
            //向前移动 左边对齐
            .Append(selecteTrans.DOMove(new Vector3(position1.x - targetTransRadis, 
                position1.y + heightOffset, position1.z + targetTransRadis), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(-80,-45,0), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(-60,-45,0), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x - targetTransRadis * 0.5f, 
                position1.y + heightOffset, position1.z + targetTransRadis * 0.5f), moveDuration))
            
            .Append(selecteTrans.DOMove(new Vector3(position1.x - targetTransRadis, 
                position1.y + heightOffset, position1.z + targetTransRadis), moveDuration))
            //环刀翻转
            .Append(targetTrans.DORotate(new Vector3(0, 0, 0), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(-80,-45,0), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(-60,-45,0), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x - targetTransRadis * 0.5f, 
                position1.y + heightOffset, position1.z + targetTransRadis * 0.5f), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(-90,0,0), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + heightOffset, position1.z + targetTransRadis * 1.3f), moveDuration))
            .Append(selecteTrans.DORotate(new Vector3(-60,0,0), moveDuration))
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + heightOffset, position1.z + targetTransRadis * 0.5f), moveDuration))

            ;
        
        ReturnOriginPos(selecteTrans.gameObject, moveDuration * 18, true);
    }
    
    
    void XIAOKnifeCallback()
    {
        if (stepsIndex == 6)
        {
            soilObj.SetActive(false);
            circleKnifeTrans.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            soilObj2.SetActive(false);
            circleKnifeTrans.GetChild(1).gameObject.SetActive(true);
        }

    }
    

    
    
        
}
