using System.Collections;
using System.Collections.Generic;
using APP.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneStateController : Singleton<SceneStateController>
{
    public string SceneName;
    public AsyncOperation asyncOperation;
    public  bool isRunState;

    private  ISceneState sceneSate;
    private LoadingPanel loadingPanel;
    
    /// <summary>
    /// 跳转场景
    /// </summary>
    /// <param name="scenestate"></param>场景名字
    /// <param name="isLoading"></param>是否需要加载界面
    /// <param name="isLoadScene"></param>时候卸载场景换新场景
    public void SetState(ISceneState scenestate,bool isLoadScene = true, string loadingName = "Loading")
    {
        //if (!HaspLock.Instance.LoginHasp()) 
        //    return;

        if (sceneSate != null)
        {
            sceneSate.StateEnd(); //上一个场景的清理工作
        }
        sceneSate = scenestate;
        SceneName = sceneSate.sceneName;


        if (isLoadScene)
        {
            isRunState = false;
            GameObject loading = GameObject.Instantiate(AssetManager.Instance.ResourceAsset.LoadPanelObject(loadingName));
            loadingPanel = loading.AddComponent<LoadingPanel>();
            loadingPanel.SceneSate = sceneSate;    
        }
        else
        {
            StateRun();
        }
    }
    public void StateUpdate()
    {
        if (sceneSate != null && isRunState)
        {
            sceneSate.StateUpdate();
        }
       if(asyncOperation!=null&&asyncOperation.isDone)
        {
            StateRun();
            asyncOperation = null;
        }
    }
    public  void StateRun( )
    {       
        isRunState = true;
        sceneSate.StateStart();
    }   
}
