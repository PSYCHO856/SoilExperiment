using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneState : ISceneState
{   
    public StartSceneState() : base("00StartScene")
    {

    }

    public override void StateStart()
    { 
        base.StateStart();
        SceneStateController.Instance.SetState(new MainSceneState());
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
