using System;
using System.Collections;
using System.Collections.Generic;
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
        
        Messenger<string>.AddListener(GameEvent.ON_INSTRUCTION_UPDATE, ShowInstruction);
        Messenger<Vector3>.AddListener(GameEvent.ON_OPERATE_UPDATE, SetOperateUIPos);
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

    void OnOperate()
    {
        
    }
    
    void SetOperateUIPos(Vector3 equipmentWorldPos)
    {
        //世界坐标转屏幕坐标 和Camera.main.WorldToScreenPoint(carWorldPos)的区别 返回Vector3 rectTransform.position=transform.position
        var equipmentScreenPos = RectTransformUtility.WorldToScreenPoint(ControllerExperiment.Instance.mainCam, equipmentWorldPos);//Vector2
        //屏幕坐标转UI坐标 out返回
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), equipmentScreenPos,
            ControllerExperiment.Instance.uiCam, out var equipmentLocalPos);
        operateBtn.GetComponent<RectTransform>().anchoredPosition = equipmentLocalPos + new Vector2(40, -40);
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
