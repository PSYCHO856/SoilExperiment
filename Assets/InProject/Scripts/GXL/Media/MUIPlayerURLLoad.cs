using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO; 
using UnityEngine.Video;
// using LitJson;
/// <summary>
/// 加载URL多媒体 
/// </summary>
using UnityEngine.Networking;
public class MUIPlayerURLLoad : MonoBehaviour
{
    public VideoPlayer vPlayer;
    public AudioSource sPlayer;
    public static MUIPlayerURLLoad instance;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        instance=this;
    }
    void Start()
    {
        //HTTP请求图片
        // HTTPRequest req=new HTTPRequest(new Uri(urlImg),OnRequestImg);
        // req.Send();
        //string url = GameData.CustomServiceURL + "/familyCircle.htm?uid" + ToolsUtil.IM_AccountID_UserID + "&searchStart=" + 1;
      
        // StartCoroutine(GetTxt(url,(str)=>{
        //     Debug.Log("获取文本");
        // }));
        //HTTPRequestManager.Instance.WebRequestCustom(url, new byte[0], OnGetSpaceDyn, 0, UnityWebRequest.kHttpVerbGET);
    }

//    private void OnGetSpaceDyn(ReceiveData data)
//     {
//         if (data.code == 200)
//         {
//             Debug.Log(data.data.ToJson());
//             SpaceDynData infoData = JsonMapper.ToObject<SpaceDynData>(data.data.ToJson());
//             foreach(var itor in infoData.results){
//                 Debug.Log(itor.circleId);
//             }
//         }
//         else
//         {
//             Debug.LogError("请求用户信息接口出错");
//         }
//     }

    /// <summary>
    /// 下载单个图片 urlImg-链接 img-要适配的父节点 isBigMode-点击查看大图模式
    /// </summary>
    /// <param name="urlImg"></param>
    /// <param name="img"></param>
    /// <returns></returns>
    public void StartGetPic(string urlImg,Image img,Vector2 imgShell,bool isBigMode=false){
         StartCoroutine(GetImagePic(urlImg,img,imgShell,isBigMode));
    }
    ///
    public void StartJsonTxt(string url,Action<string> cb){
         StartCoroutine(GetTxt(url,cb));
    }

    public void StartGetVideo(RawImage img,Vector2 imgRect,Vector2 resol){
        float targetWidth=1334;
        float targetHeight=750; 

        if(true){
            var rateH=resol.y/imgRect.y;
            targetWidth=resol.x/rateH;
            targetHeight=resol.y/rateH;
        }
        // Debug.Log(targetWidth+"dd"+targetHeight);
        img.GetComponent<RectTransform>().sizeDelta=new Vector2((int)targetWidth,(int)targetHeight);
    }
    public void StartGetSound(string urlS,Toggle tg){
        StopGetSound();
        StartCoroutine(GetAudioClip(urlS,tg));
    }
    public void StopGetSound(){
        sPlayer.Stop();
    }

    private IEnumerator GetAudioClip (string urlSound,Toggle tg) {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(urlSound, AudioType.MPEG)) {
            yield return request.SendWebRequest();
 
            if (request.isHttpError || request.isNetworkError) {
                Debug.LogError("音频加载失败"+request.error);
            } else {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(request);
                sPlayer.clip=myClip;
                sPlayer.Play();
                StartCoroutine(AudioPlayFinished( sPlayer.clip.length,tg));
                // Debug.Log($"音频大小-{request.downloadedBytes/(1024)}kb");
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator GetTxt (string url,Action<string> callback) {
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            yield return request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError) {
                Debug.LogError("json文本加载失败"+request.error);
            } else {
                // Debug.Log("json文本加载成功"+request.downloadHandler.text);
                callback(request.downloadHandler.text);
            }
        }
    }
    private IEnumerator GetImagePic(string urlImg,Image img,Vector2 imgRect,bool isBigMode)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(urlImg))
        {
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError){
                Debug.LogError("图片加载失败"+request.error);
                GetImagePic(urlImg,img,imgRect,isBigMode);
            }else{
                Texture2D texture = (request.downloadHandler as DownloadHandlerTexture).texture;
                Sprite sprite =  Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                img.sprite = sprite;
                ScaleTexture(texture,img,imgRect,isBigMode);
                //  Debug.Log($"图片大小-{request.downloadedBytes}");
            }
        }
    }
    /// <summary>
    /// 根据纹理图比例 缩放img
    /// </summary>
    /// <param name="source"></param>
    /// <param name="img"></param>
    /// <returns></returns>
     public  void ScaleTexture(Texture2D source,Image img ,Vector2 imgRect,bool isBigMode)
    {
        float targetWidth=100;
        float targetHeight=100; 

        if(source.width<source.height){
            var rateW=source.width/imgRect.x;
            targetWidth=source.width/rateW;
            targetHeight=source.height/rateW;
        }else{
            var rateH=source.height/imgRect.y;
            targetWidth=source.width/rateH;
            targetHeight=source.height/rateH;
        }
        if(isBigMode){
            var rateH=source.height/imgRect.y;
            targetWidth=source.width/rateH;
            targetHeight=source.height/rateH;
        }
        // Debug.Log(targetWidth+"dd"+targetHeight);
        img.GetComponent<RectTransform>().sizeDelta=new Vector2((int)targetWidth,(int)targetHeight);
    }

    //执行协成函数 并且返回时间
    private IEnumerator AudioPlayFinished(float time, Toggle tg)
    {

        yield return new WaitForSeconds(time);
        //声音播放完毕后之下往下的代码  

        # region   声音播放完成后执行的代码

        print("声音播放完毕，继续向下执行");
        tg.isOn=false;
    
        #endregion
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
