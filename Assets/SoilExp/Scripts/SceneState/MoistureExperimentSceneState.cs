﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoistureExperimentSceneState : ISceneState
{
    public MoistureExperimentSceneState() : base("MoistureContentExperimentScene")
    {

    }

    public override void StateStart()
    {
        base.StateStart();

        //主界面
        // PanelManager.Instance.MainPanelManager.MainMenuPanel.Hide_DisplayUI(true);

        // PanelManager.Instance.CommonPanelManager.Hide_DisplayTotalPanel();
        UIController.Open(UIPageId.IntroductionPage);

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

