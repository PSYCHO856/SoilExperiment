using System.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
/// <summary>
/// 视频播放器
/// </summary>
public class PlayVideo : MonoBehaviour
{
    [Header("VideoPlayer组件")]
    public VideoPlayer videoPlayer;
    [Header("开始播放开关")]
    public Toggle startToggle;
    public RawImage bigRawImg;
    public MUIPlayerURLLoad urlLoad;//加载媒体的核心组件

    private RenderTexture renderTexture ;
    
    private RectTransform rt ;
    
    private void Awake()
    {
        if(!videoPlayer){
            videoPlayer = GetComponent<VideoPlayer>();
        }
        rt = GetComponent<RectTransform>();
        
        startToggle.onValueChanged.AddListener((isOn) =>
        {
            if (videoPlayer != null)
            {
                if(isOn){
                    Play();
                }else{
                    Pause();
                }
            }
        });
    }
 
    private void Start()
    {
        // Init(0);
        videoPlayer.loopPointReached+=(VideoPlayer source)=>{
            startToggle.isOn=false;
        };
    }
    //播放当前视频
    public void Init(int index=0){
        // videoPlayer.clip=clips[index];
        videoPlayer.Play();
        videoPlayer.sendFrameReadyEvents = true;
        StartCoroutine(Video());
        videoPlayer.Pause();
        bigRawImg.enabled=false;
        startToggle.isOn=true;
    }
    //加载资源
    public bool Init(VideoClip clip){
        if(!clip){
            videoPlayer.clip=null;
            Debug.Log("clip is null");
            return true;
        }
        videoPlayer.clip=clip;
        startToggle.isOn=false;
        return false;
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        videoPlayer.Pause();
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        videoPlayer.Stop();
    }
 
    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        videoPlayer.Play();
    }
 
    /// <summary>
    /// 获取视频时长
    /// </summary>
    /// <returns></returns>
    public float GetTime()
    {
        return (videoPlayer.frameCount / videoPlayer.frameRate );
    }
    
    /// <summary>
    /// 设置播放时间点
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(float time)
    {
        videoPlayer.frame = (long)(time * videoPlayer.frameCount);
        //videoPlayer.frame=100;
        // Debug.Log($"{time}gxl"+videoPlayer.frame+"dd"+time * videoPlayer.frameCount);
        if(startToggle.isOn){
            Play();
        }else{
            Pause();
        }
    }
    public float screenH=0;
    IEnumerator Video()
    {
        yield return new WaitForSeconds(0.5f);
        if(videoPlayer.texture==null){
            StartCoroutine(Video());
            Debug.Log("视频加载ing---");
            yield  break;
        }
        bigRawImg.enabled=true;
        renderTexture= new RenderTexture(videoPlayer.texture.width, videoPlayer.texture.height, 16, RenderTextureFormat.ARGB32);
        bigRawImg.texture=renderTexture;
        videoPlayer.targetTexture=renderTexture;
        //设置播放器视频分辨率
        if(screenH==0){
            screenH=Screen.height;
        }
        urlLoad.StartGetVideo(bigRawImg,new Vector2(Screen.width,screenH)
        ,new Vector2(videoPlayer.texture.width,videoPlayer.texture.height));
        Debug.Log($"视频分辨率:{videoPlayer.texture.width}x{videoPlayer.texture.height}");
        videoPlayer.Play();
    }
}