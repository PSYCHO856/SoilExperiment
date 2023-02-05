using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : APP.Core.MonoSingleton<SceneMgr> ,ManagerBase{

    public event OnPercentEventHandler OnPercent;
    public event Action OnLoadLevel;
    public event Action OnDone;
    //这里只做展示
    [SerializeField]
    private  string SceneName;
    AsyncOperation ao;
    float percent;

    public void Init()
    {
        //弹框
        //版本判断
        Debug.Log(" Application.version：" + Application.version);
        //如果是android平台，需要等网络回复是否需要更新

        ResourceMgr.Instance.OnDone += () => {
        };
    }

    /// <summary>
    /// 异步加载场景 加载场景需要传入指定的场景名 会自动调用进度条界面 当加载完关闭进度条 转场
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneAsync(string sceneName,Action cb=null)
    {
        MUIMgr.Instance.Dispose();
        MUIMgr.Instance.OpenUI("MUILoading");
        if (OnLoadLevel != null)
            OnLoadLevel();
        SceneName = sceneName;
        StartCoroutine(LoadSceneAsy(sceneName,cb));
        
    }

    IEnumerator LoadSceneAsy(string sceneName,Action cb)
    {
        percent = 0;//进度
       
        ao = SceneManager.LoadSceneAsync(sceneName);

        SceneName = sceneName;
        ao.allowSceneActivation = false;
        if(OnPercent != null)
        {
            OnPercent(0f, "开始加载场景" + sceneName);
        }
        while (ao.progress < 0.9f || percent < 1)
        {
            percent += 0.01f;
            if (percent > 1)
                percent = 1;
            if (OnPercent != null)
            {
                OnPercent(percent, String.Format("开始加载场景{0}%" , (int)(percent * 100)));
            }
           
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2);

        if(OnPercent != null)
            OnPercent(percent, String.Format("加载完毕", (int)(percent * 100)));

        ao.allowSceneActivation = true;
        if (OnDone != null)
            OnDone();
        System.GC.Collect();
        yield return new WaitForSeconds(0.1f);
        if(cb!=null){
            cb();//加载完后回调
         }
    }
}

public enum EScene{
    Login=0,
    Level_Select,
    Level_See,
    Level_Exam,
    Level_Main,

}
