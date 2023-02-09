﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneState : ISceneState
{
    public MainSceneState() : base("MainScene")
    {

    }

    public override void StateStart()
    {
        base.StateStart();

        //主界面
        // PanelManager.Instance.MainPanelManager.MainMenuPanel.Hide_DisplayUI(true);

        // PanelManager.Instance.CommonPanelManager.Hide_DisplayTotalPanel();

        if (ToolManager.Instance.isLogin)
        {
            UIController.Open(UIPageId.MainPage);
        }
        else
        {
            UIController.Open(UIPageId.LoginPage);

        }
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

