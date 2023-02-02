using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : UIBasePage
{
    public Button btnDensity;
    public Button btnMoisture;
    public Button btnBorderMoisture;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
