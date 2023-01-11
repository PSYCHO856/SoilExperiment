using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : UIBasePage
{
    public Button btnDensity;
    
    // Start is called before the first frame update
    void Start()
    {
        btnDensity.onClick.AddListener((() =>
        {
            SceneStateController.Instance.SetState(new DensityExperimentSceneState());
        }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
