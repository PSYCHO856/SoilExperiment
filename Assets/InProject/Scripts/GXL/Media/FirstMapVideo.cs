using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
/// <summary>
/// 截取第一帧视频截图
/// </summary>
public class FirstMapVideo : MonoBehaviour
{
     public VideoPlayer vp;
     Texture2D videoFrameTexture;
     RenderTexture renderTexture;
     public RawImage img;
     public void init(string url)
     {
         vp.url=url;
         videoFrameTexture = new Texture2D(350,280);
         vp = GetComponent<VideoPlayer>();
         vp.playOnAwake = false;
         vp.waitForFirstFrame = true;
 
         vp.sendFrameReadyEvents = true;
         vp.frameReady += OnNewFrame;
         vp.Play();
         //vp.Pause();
 
     }
     int framesValue=0;//获得视频第几帧的图片
    void OnNewFrame(VideoPlayer source, long frameIdx)
    {
        framesValue++;
        if (framesValue==1) {
            renderTexture = source.texture as RenderTexture;
            // Debug.Log(source.texture.width+"s视频分辨率"+source.texture.height);
            if (videoFrameTexture.width != renderTexture.width || videoFrameTexture.height != renderTexture.height) {
                videoFrameTexture.Reinitialize (renderTexture.width, renderTexture.height);
            }
            RenderTexture.active = renderTexture;
            videoFrameTexture.ReadPixels (new Rect (0, 0, renderTexture.width, renderTexture.height), 0, 0);
            videoFrameTexture.Apply ();
            RenderTexture.active = null;
            vp.frameReady -= OnNewFrame;
            vp.sendFrameReadyEvents = false;
            img.texture=videoFrameTexture;
        }
    }
}
