using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionPage : UIBasePage
{
    Image expPreviewImg;
    Button startExpBtn;
    Button returnBtn;
    public Text introductionText;
    Text nameText;

    private void Awake()
    {
            expPreviewImg = GameObject.Find("expPreviewImg").GetComponent<Image>();

            startExpBtn = GameObject.Find("startExpBtn").GetComponent<Button>();
            returnBtn = GameObject.Find("returnBtn").GetComponent<Button>();
            startExpBtn.onClick.AddListener(LoadScene);
            returnBtn.onClick.AddListener(base.OnClose);

            // introductionText = transform.Find("introductionText").GetComponent<Text>();
            nameText = GameObject.Find("nameText").GetComponent<Text>();
        // startExpBtn.onClick.AddListener(OnClose);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        ReadExcelData();
        // Application.streamingAssetsPath + "/Image1.jpg"
        expPreviewImg.sprite = Resources.Load<Sprite>("ScreenShot/" + nowData.previewImageName) as Sprite;
        // expPreviewImg.SetNativeSize();
        introductionText.text = nowData.Instruction;
        nameText.text = nowData.Name;


    }

    public void LoadScene()
    {
        gameObject.SetActive(false);
        switch (ToolManager.Instance.sceneNumber)
        {
            case 0:
                SceneStateController.Instance.SetState(new DensityExperimentSceneState());
                break;
            case 1:
                SceneStateController.Instance.SetState(new MoistureExperimentSceneState());
                break;
            case 2:
                SceneStateController.Instance.SetState(new BorderMoistureExperimentSceneState());
                break;
            case 3:
                SceneStateController.Instance.SetState(new BorderPlasticExperimentSceneState());
                break;
        }
        
    }
    
    ExperimentIntroductionConfig nowData = null;
    ExperimentIntroductionConfig ReadExcelData()
    {
        
        foreach (var experimentIntroductionConfigInfo in ExperimentIntroductionConfigInfo.Datas)
        {
            if (experimentIntroductionConfigInfo.Value.sceneId == ToolManager.Instance.sceneNumber)
            {
                nowData = experimentIntroductionConfigInfo.Value;
                break;
            }
        }

        return nowData;
    }
    
}
