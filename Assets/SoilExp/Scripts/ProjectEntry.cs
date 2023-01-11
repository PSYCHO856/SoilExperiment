using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ProjectEntry : SingletonBaseComponent<ProjectEntry>
{
    // Use this for initialization
    protected override void OnAwake()
    {
        DontDestroyOnLoad(gameObject);

        //加载视频播放器和音乐控制器
        // GameObject canvasVideo = Instantiate(AssetManager.Instance.ResourceAsset.LoadGameObject("Managers/CanvasVideo"));
        // canvasVideo.name = "CanvasVideo";

        // GameObject audioManager = Instantiate(AssetManager.Instance.ResourceAsset.LoadGameObject("Managers/AudioManager"));
        // audioManager.name = "AudioManager";

        //加载工具类
        GameObject toolManager = Instantiate(AssetManager.Instance.ResourceAsset.LoadGameObject("Managers/ToolManager"));
        toolManager.name = "ToolManager";

        //数据初始化
        // DataManager.Instance.Init();
    }

    protected override void OnStart()
    {
        SceneStateController.Instance.SetState(new StartSceneState(), false);       
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneStateController.Instance != null)
            SceneStateController.Instance.StateUpdate();       
    }    

}



