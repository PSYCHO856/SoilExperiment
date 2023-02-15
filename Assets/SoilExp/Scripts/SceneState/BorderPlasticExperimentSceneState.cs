using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BorderPlasticExperimentSceneState : ISceneState
{
    public BorderPlasticExperimentSceneState() : base("BorderPlasticContentExperimentScene")
    {

    }

    public override void StateStart()
    {
        base.StateStart();

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

