using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentPage : UIBasePage
{
    public GameObject equipmentInstruction;
    public Text equipmentInstructionText;

    public Button testBtn;
    public Button instructionCloseBtn;
    public Button operateBtn;
    
    public Button homeBtn;
    public Button settingsBtn;
    public Button helpBtn;
    public Button quitBtn;
    
    public Button closeBtnHelp;
    public Button closeBtnSettings;
    public Button quitBtnConfirm;
    public Button quitBtnCancel;
    
    public GameObject helpPanel;
    public GameObject quitPanel;
    public GameObject settingsPanel;

    private void Awake()
    {
        equipmentInstruction = transform.Find("EquipmentInstruction").gameObject;
        //设备介绍和操作按钮显示
        Messenger<string>.AddListener(GameEvent.ON_INSTRUCTION_UPDATE, ShowInstruction);
        Messenger<Vector3,string,Action>.AddListener(GameEvent.ON_OPERATE_UPDATE, SetOperateUIPos);
        testBtn.onClick.AddListener(OnTest);
        instructionCloseBtn.onClick.AddListener(OnInstructionClose);
        operateBtn.onClick.AddListener(OnOperate);
        
        homeBtn.onClick.AddListener(OnHome);
        settingsBtn.onClick.AddListener(OnSettings);
        helpBtn.onClick.AddListener(OnHelp);
        quitBtn.onClick.AddListener(OnQuit);
        closeBtnHelp.onClick.AddListener(OnCloseBtnHelp);
        closeBtnSettings.onClick.AddListener(OnCloseBtnSettings);
        quitBtnConfirm.onClick.AddListener(OnQuitBtnConfirm);
        quitBtnCancel.onClick.AddListener(OnQuitBtnCancel);

        operateBtnImage = operateBtn.transform.GetComponent<Image>();
        operateBtnImage.color = new Color(operateBtnImage.color.r, operateBtnImage.color.g, operateBtnImage.color.b, 0);
        operateBtnText = operateBtn.transform.GetChild(0).GetComponent<Text>();
    }

    private void OnDestroy()
    {
        Messenger<string>.RemoveListener(GameEvent.ON_INSTRUCTION_UPDATE, ShowInstruction);
        Messenger<Vector3,string,Action>.RemoveListener(GameEvent.ON_OPERATE_UPDATE, SetOperateUIPos);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        equipmentInstruction.SetActive(false);
        equipmentInstructionText.text = "";

        PanelsInit();
        SetBackScene(new MainSceneState());
        
        Messenger.Broadcast(GameEvent.ON_STEPS_UPDATE);
    }

    void PanelsInit()
    {
        helpPanel.SetActive(false);
        quitPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    void ShowInstruction(string instruction)
    {
        equipmentInstruction.SetActive(true);
        equipmentInstructionText.text = instruction;
    }

    void OnTest()
    {
        ControllerExperiment.Instance.stepsIndex++;
    }
    
    void OnInstructionClose()
    {
        equipmentInstruction.SetActive(false);
    }

    private Action refreshAction;
    void OnOperate()
    {
        HideOperateBtn();
        refreshAction.Invoke();
    }
    
    void SetOperateUIPos(Vector3 equipmentWorldPos,string operateTextContent, Action refreshNextStep)
    {
        //世界坐标转屏幕坐标 和Camera.main.WorldToScreenPoint(carWorldPos)的区别 返回Vector3 rectTransform.position=transform.position
        var equipmentScreenPos = RectTransformUtility.WorldToScreenPoint(ControllerExperiment.Instance.mainCam, equipmentWorldPos);//Vector2
        //屏幕坐标转UI坐标 out返回
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), equipmentScreenPos,
            ControllerExperiment.Instance.uiCam, out var equipmentLocalPos);
        operateBtn.GetComponent<RectTransform>().anchoredPosition = equipmentLocalPos + new Vector2(-100, 100);
        
        ShowOperateBtn();
        operateBtnText.text = operateTextContent;
        
        refreshAction = null;
        refreshAction += refreshNextStep;
    }

    private Image operateBtnImage;
    private Text operateBtnText;
    void ShowOperateBtn()
    {
        operateBtnImage.DOColor(new Color(operateBtnImage.color.r,operateBtnImage.color.g,operateBtnImage.color.b,1), 1f);
        operateBtnText.DOColor(new Color(operateBtnText.color.r, operateBtnText.color.g, operateBtnText.color.b, 1),
            1f);
    }
    
    void HideOperateBtn()
    {
        operateBtnImage.DOColor(new Color(operateBtnImage.color.r,operateBtnImage.color.g,operateBtnImage.color.b,0), 1f);
        operateBtnText.DOColor(new Color(operateBtnText.color.r, operateBtnText.color.g, operateBtnText.color.b, 0),
            1f);
    }


    #region optionExtraFunction
    
    public void SetBackScene(ISceneState backScene)
    {
        backSceneState = backScene;
    }
    
    private ISceneState backSceneState;
    void OnHome()
    {
        UIController.Clear();

        SceneStateController.Instance.SetState(backSceneState);           
    }
    
    void OnSettings()
    {
        settingsPanel.SetActive(true);
    }
    
    void OnHelp()
    {
        helpPanel.SetActive(true);
    }
    
    void OnQuit()
    {
        quitPanel.SetActive(true);
    }

    void OnCloseBtnHelp()
    {
        helpPanel.SetActive(false);
    }
    
    void OnCloseBtnSettings()
    {
        settingsPanel.SetActive(false);
    }
    
    void OnQuitBtnConfirm()
    {
        Application.Quit();
    }
    
    void OnQuitBtnCancel()
    {
        quitPanel.SetActive(false);
    }

    #endregion
    
}
