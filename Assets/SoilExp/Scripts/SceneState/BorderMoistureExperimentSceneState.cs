using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BorderMoistureExperimentSceneState : ISceneState
{
    public BorderMoistureExperimentSceneState() : base("BorderMoistureContentExperimentScene")
    {

    }

    public override void StateStart()
    {
        base.StateStart();

        //主界面
        // PanelManager.Instance.MainPanelManager.MainMenuPanel.Hide_DisplayUI(true);

        // PanelManager.Instance.CommonPanelManager.Hide_DisplayTotalPanel();

        UIController.Open(UIPageId.ExperimentPage);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();    
    }
    public override void StateEnd()
    {
        base.StateEnd();

        // PanelManager.Instance.Clear();
    }

}

