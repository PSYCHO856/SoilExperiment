using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 场景状态基类
/// </summary>
public class ISceneState
{
    protected Transform mCanvas;
    public string sceneName;

    public ISceneState (string scenename)
    {
        sceneName = scenename;
    }

    /// <summary>
    /// 状态开始调用
    /// </summary>
    public virtual void  StateStart() { mCanvas = GameObject.Find("UICanvas").transform; }
    /// <summary>
    /// 状态更新调用
    /// </summary>
    public virtual void StateUpdate() { }
    /// <summary>
    /// 状态结束调用
    /// </summary>
    public virtual void StateEnd() { }
}
