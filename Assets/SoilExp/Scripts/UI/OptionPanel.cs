using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Button homeBtn;
    public Button settingsBtn;
    public Button outputFormBtn;
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
        SetBackScene(new MainSceneState());
        homeBtn.onClick.AddListener(OnHome);
        settingsBtn.onClick.AddListener(OnSettings);
        outputFormBtn.onClick.AddListener(() =>
        {
            UIController.Open(UIPageId.CardPage);
        });
        helpBtn.onClick.AddListener(OnHelp);
        quitBtn.onClick.AddListener(OnQuit);
        closeBtnHelp.onClick.AddListener(OnCloseBtnHelp);
        closeBtnSettings.onClick.AddListener(OnCloseBtnSettings);
        quitBtnConfirm.onClick.AddListener(OnQuitBtnConfirm);
        quitBtnCancel.onClick.AddListener(OnQuitBtnCancel);
        
    }
    // PanelsInit();
    #region optionExtraFunction
    
    void PanelsInit()
    {
        helpPanel.SetActive(false);
        quitPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
    
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
