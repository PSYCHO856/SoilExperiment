using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentPage : UIBasePage
{
    public GameObject equipmentInstruction;
    private CanvasGroup equipmentInstructionCG;
    public TypeWriter equipmentInstructionTypeWriter;

    public Button testBtn;
    public Button instructionCloseBtn;
    public Button operateBtn;

    private void Awake()
    {
        equipmentInstruction = transform.Find("EquipmentInstruction").gameObject;
        equipmentInstructionCG = equipmentInstruction.GetComponent<CanvasGroup>();
        //设备介绍和操作按钮显示
        Messenger<string>.AddListener(GameEvent.ON_INSTRUCTION_UPDATE, ShowInstruction);
        Messenger<Vector3,string,Action>.AddListener(GameEvent.ON_OPERATE_UPDATE, SetOperateUIPos);
        Messenger.AddListener(GameEvent.ON_INSTRUCTION_FADE, OnInstructionClose);
        testBtn.onClick.AddListener(OnTest);
        instructionCloseBtn.onClick.AddListener(OnInstructionClose);
        operateBtn.onClick.AddListener(OnOperate);
        
        operateBtnTextTypeWriter = operateBtn.transform.GetChild(0).GetComponent<TypeWriter>();
        operateBtnCG = operateBtn.transform.GetComponent<CanvasGroup>();
        operateBtnCG.alpha = 0;
    }

    private void OnDestroy()
    {
        Messenger<string>.RemoveListener(GameEvent.ON_INSTRUCTION_UPDATE, ShowInstruction);
        Messenger<Vector3,string,Action>.RemoveListener(GameEvent.ON_OPERATE_UPDATE, SetOperateUIPos);
        Messenger.RemoveListener(GameEvent.ON_INSTRUCTION_FADE, OnInstructionClose);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        equipmentInstructionCG.alpha = 0;
        equipmentInstructionTypeWriter.Run("", equipmentInstructionTypeWriter.text);

        Messenger.Broadcast(GameEvent.ON_STEPS_UPDATE);
    }

    void ShowInstruction(string instruction)
    {
        
        equipmentInstructionCG.DOFade(1, 1f);
        equipmentInstructionTypeWriter.Run(instruction, equipmentInstructionTypeWriter.text);
    }

    void OnTest()
    {
        ControllerExperiment.Instance.stepsIndex++;
    }
    
    void OnInstructionClose()
    {
        equipmentInstructionCG.DOFade(0, 0.5f);
    }

    private Action refreshAction;
    void OnOperate()
    {
        HideOperateBtn();
        refreshAction.Invoke();
        UIController.Open(UIPageId.CardPage);
    }
    
    void SetOperateUIPos(Vector3 equipmentWorldPos,string operateTextContent, Action refreshNextStep)
    {
        //世界坐标转屏幕坐标 和Camera.main.WorldToScreenPoint(carWorldPos)的区别 返回Vector3 rectTransform.position=transform.position
        var equipmentScreenPos = RectTransformUtility.WorldToScreenPoint(ControllerExperiment.Instance.mainCam, equipmentWorldPos);//Vector2
        //屏幕坐标转UI坐标 out返回
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), equipmentScreenPos,
            ControllerExperiment.Instance.uiCam, out var equipmentLocalPos);
        operateBtn.GetComponent<RectTransform>().anchoredPosition = equipmentLocalPos + new Vector2(-150, 200);
        
        ShowOperateBtn();
        operateBtnTextTypeWriter.Run(operateTextContent, operateBtnTextTypeWriter.text);
        
        refreshAction = null;
        refreshAction += refreshNextStep;
    }

    private TypeWriter operateBtnTextTypeWriter;
    private CanvasGroup operateBtnCG;
    void ShowOperateBtn()
    {
        operateBtnCG.alpha = 0;
        
        operateBtn.enabled = true;
        operateBtnCG.DOFade(1, 0.5f);
    }
    
    void HideOperateBtn()
    {
        operateBtn.enabled = false;
        
        operateBtnCG.DOFade(0, 0.5f);
    }

}
