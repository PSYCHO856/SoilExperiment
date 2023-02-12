using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : UIBasePage
{
    public Button btnDensity;
    public Button btnMoisture;
    public Button btnBorderMoisture;
    public Button btnBorderPlastic;
    
    // Start is called before the first frame update
    void Start()
    {
        btnDensity.onClick.AddListener(() =>
        {
            SceneStateController.Instance.SetState(new DensityExperimentSceneState());
            ToolManager.Instance.sceneNumber = 0;
        });
        
        btnMoisture.onClick.AddListener(() =>
        {
            SceneStateController.Instance.SetState(new MoistureExperimentSceneState());
            ToolManager.Instance.sceneNumber = 1;
        });
        
        btnBorderMoisture.onClick.AddListener(() =>
        {
            SceneStateController.Instance.SetState(new BorderMoistureExperimentSceneState());
            ToolManager.Instance.sceneNumber = 2;
        });
        
        btnBorderPlastic.onClick.AddListener(() =>
        {
            SceneStateController.Instance.SetState(new BorderPlasticExperimentSceneState());
            ToolManager.Instance.sceneNumber = 3;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
