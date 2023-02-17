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
            ToolManager.Instance.sceneNumber = 0;
            UIController.Open(UIPageId.IntroductionPage);
        });
        
        btnMoisture.onClick.AddListener(() =>
        {
            ToolManager.Instance.sceneNumber = 1;
            UIController.Open(UIPageId.IntroductionPage);
        });
        
        btnBorderMoisture.onClick.AddListener(() =>
        {
            ToolManager.Instance.sceneNumber = 2;
            UIController.Open(UIPageId.IntroductionPage);
        });
        
        btnBorderPlastic.onClick.AddListener(() =>
        {
            ToolManager.Instance.sceneNumber = 3;
            UIController.Open(UIPageId.IntroductionPage);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
