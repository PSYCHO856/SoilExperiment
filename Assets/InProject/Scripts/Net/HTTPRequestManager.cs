using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
public delegate void HTTPCallBack(object[] param);
public delegate void HTTPDownLoadProgress(float progress,ulong downSize, ulong fileSize);

public delegate void HTTPCustomCallBack(ReceiveData data);

public class HTTPRequestManager : APP.Core.MonoSingleton<HTTPRequestManager>
{
    /// <summary>
    /// 单词下载大小，防止协程下载卡死
    /// </summary>
    private double loadedBytes = 10000000;
    // Start is called before the first frame update
    void Start()
    {
        //#if UNITY_EDITOR
        //        byte[] data = System.Text.Encoding.UTF8.GetBytes("{ \"appType\": \"0\", \"version\": \"" + Application.version + "\"}");
        //        WebRequest("http://112.74.39.226:8888/api/init", data, HTTPCallBack);
        //#endif
        //DownLoadFromURL("https://app.middangeard.cc/_Middangeard_Online_V1.0.14_3.apk", new byte[0], HTTPDownLoadProgress, HTTPCallBack, "");
        
    }
    public void HTTPCallBack(object[] param)
    {
        Debug.Log("param[0]=" + param[0] + ",param[1]=" + param[1]);
    }
    public void HTTPDownLoadProgress(float progress, ulong downSize, ulong fileSize){
        Debug.Log("progress=" + progress + ",downSize=" + downSize + ",fileSize=" + fileSize);

    }


    /// <summary>
    /// http请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="data">请求数据</param>
    /// <param name="callBack">请求回调</param>
    /// <param name="callCount">请求次数</param>
    /// <param name="method">http请求方式post/get</param>
    public void WebRequest(string url, byte[] data, HTTPCallBack callBack, int callCount = 0, string method = UnityWebRequest.kHttpVerbPOST)
    {
        StartCoroutine(WebRequest_Wait(url, data, callBack, callCount, method));
    }
    /// <summary>
    /// http请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="jsonMsg">请求json字符串数据</param>
    /// <param name="callBack">请求回调</param>
    /// <param name="callCount">请求次数</param>
    /// <param name="method">http请求方式post</param>
    public void WebRequest(string url, string jsonMsg, HTTPCallBack callBack, int callCount = 0, string method = UnityWebRequest.kHttpVerbPOST)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonMsg);
        WebRequest(url, data, callBack, callCount, method);
    }

    /// <summary>
    /// http请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="jsonMsg">请求数据</param>
    /// <param name="callBack">请求回调</param>
    /// <param name="callCount">请求次数</param>
    /// <param name="method">http请求方式post/get</param>
    public void WebRequestCustom(string url, string jsonMsg, HTTPCustomCallBack callBack, int callCount = 0, string method = UnityWebRequest.kHttpVerbPOST)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonMsg);
        Debug.Log("jsonMsg=" + jsonMsg);
        WebRequestCustom(url, data, callBack, callCount, method);
    }

    /// <summary>
    /// http请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="data">请求数据</param>
    /// <param name="callBack">请求回调</param>
    /// <param name="callCount">请求次数</param>
    /// <param name="method">http请求方式post/get</param>
    public Coroutine WebRequestCustom(string url, byte[] data, HTTPCustomCallBack callBack, int callCount = 0, string method = UnityWebRequest.kHttpVerbPOST)
    {
        return StartCoroutine(WebRequest_Wait(url, data, (object[] param) => {
            ReceiveData receiveData;
            if(param.Length > 0)
            {
                //Debug.Log("param[0]=" + param[0]);
                //string msg = "{\"code\":200,\"data\":{\"birthday\":\"2017 - 07 - 17\",\"nickName\":\"5bCP5ZKV5ZSn\",\"photo\":\"http://pond.waterdrop.xin/%E5%B0%8FGUJi.png\",\"uid\":565},\"message\":\"成功\",\"serverTimestamp\":1656142550012}";
                //字符串处理一下
                string jsonmsg = param[0].ToString();
                //Debug.Log("jsonmsg=" + jsonmsg);
                receiveData = LitJson.JsonMapper.ToObject<ReceiveData>(jsonmsg);
                //Debug.Log("receiveData.data.ToJson()=" + receiveData.data.ToJson() + ",receiveData.data.ToString()=" + receiveData.data.ToString());
                //JsonData jsondata = JsonMapper.ToObject(jsonmsg);
                //receiveData.data = jsondata["data"].ToJson();
            }
            else
            {

                receiveData = new ReceiveData() { code = 0,message="http请求出错"};
            }
            try
            {
                if (callBack != null)
                {
                    callBack(receiveData);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("url=" + url + ",callBackError:" + e);
            }
        }, callCount, method));
    }





    /// <summary>
    /// 协程http请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="data">请求数据</param>
    /// <param name="callBack">请求回调</param>
    /// <param name="callCount">请求次数</param>
    /// <param name="method">http请求方式post/get</param>
    private IEnumerator WebRequest_Wait(string url, byte[] data, HTTPCallBack callBack, int callCount, string method)
    {
        //拿到最后的名称
        //http://app.middangeard.cc/com.Army.ZHW_2022.04.09_07_Online.apk
        Debug.Log("url=" + url + ",callCount=" + callCount);

        UnityWebRequest request = UnityWebRequest.Get(url);
        //request.SetRequestHeader("Content-Type", "application/json");
        request.method = method;

        request.SetRequestHeader("Content-Type", "application/json;charset=utf-8");

        //request body
        if (data.Length > 0)
        {
            request.uploadHandler = new UploadHandlerRaw(data);
        }
        yield return request.SendWebRequest();
        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.LogError("headRequest error:" + request.error + ",url=" + url);
            yield return new WaitForSeconds(1);
            //yield break;
            WebRequest(url, data, callBack, callCount + 1);
        }
        else
        {
            if (callBack != null)
            {
                Debug.Log("request.downloadHandler.text=" + request.downloadHandler.text);
                
                callBack(new object[] { request.downloadHandler.text });
            }
        }
        request.Dispose();

    }

    /// <summary>
    /// 断点下载
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <param name="downLoadProgress"></param>
    /// <param name="callBack"></param>
    /// <param name="saveFolder">Application.persistentDataPath + 账号 + 自己文件夹 + 文件名称</param>
    /// <param name="callCount"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    public void DownLoadFromURL(string url, byte[] data, HTTPDownLoadProgress downLoadProgress, HTTPCallBack callBack, string saveFolder = "", int callCount = 0, string method = UnityWebRequest.kHttpVerbPOST)
    {
        StartCoroutine(BreakPointDownloadFromURL(url, data, downLoadProgress, callBack, saveFolder, callCount, method));
    }

    /// <summary>
    /// 断点下载
    /// </summary>
    /// <param name="loadPath"></param>
    /// <param name="data"></param>
    /// <param name="downLoadProgress"></param>
    /// <param name="callBack"></param>
    /// <param name="saveFolder">Application.persistentDataPath + 账号 + 自己文件夹 + 文件名称</param>
    /// <param name="callCount"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    private IEnumerator BreakPointDownloadFromURL(string loadPath, byte[] data, HTTPDownLoadProgress downLoadProgress, HTTPCallBack callBack,string saveFolder = "", int callCount = 0, string method = UnityWebRequest.kHttpVerbPOST)
    {
        //拿到最后的名称
        //http://app.middangeard.cc/com.Army.ZHW_2022.04.09_07_Online.apk
        string[] loadinfo = loadPath.Split('/');
        string savePath = "";//Path.Combine(Utility.FileSaveFolder, saveFolder, loadinfo[loadinfo.Length - 1]);

        Debug.Log(savePath);
        bool isLoadSuccess = false;
        bool isOver = true;
        UnityWebRequest headRequest = UnityWebRequest.Head(loadPath);
        yield return headRequest.SendWebRequest();
        if (!string.IsNullOrEmpty(headRequest.error))
        {
            Debug.LogError("headRequest error:" + headRequest.error);
            //yield break;
        }
        else
        {
            ulong totalLength = ulong.Parse(headRequest.GetResponseHeader("Content-Length"));
            Debug.Log("文件大小：" + totalLength);
            headRequest.Dispose();
            UnityWebRequest request = UnityWebRequest.Get(loadPath);
            request.downloadHandler = new DownloadHandlerFile(savePath, true);

            FileInfo file = new FileInfo(savePath);

            ulong fileLength = (ulong)file.Length;
            request.SetRequestHeader("Range", "bytes=" + fileLength + "-");
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError("下载失败" + request.error);
            }
            else
            {
                
                if (fileLength < totalLength)
                {
                    request.SendWebRequest();
                    ulong lastdownloadedbytes = 0;
                    while (!request.isDone)
                    {
                        float progress = (request.downloadedBytes + fileLength) / (float)totalLength;
                        if (downLoadProgress != null)
                        {
                            downLoadProgress(progress, request.downloadedBytes + fileLength, totalLength);
                        }
                        Debug.Log("下载进度：" + progress + ",downloadedBytes:" + request.downloadedBytes + ",error:" + request.error + ",isNetworkError:" + request.isNetworkError + ",responseCode:" + request.responseCode);
                        if (request.downloadedBytes >= loadedBytes || (lastdownloadedbytes == request.downloadedBytes && lastdownloadedbytes != 0))
                        {
                            //StopCoroutine("BreakPointResume");
                            request.Abort();
                            if (!string.IsNullOrEmpty(request.error))
                            {
                                Debug.LogError("下载失败：" + request.error);
                            }
                            isOver = false;
                            yield return StartCoroutine(BreakPointDownloadFromURL(loadPath, data, downLoadProgress, callBack,saveFolder, callCount, method));
                        }
                        lastdownloadedbytes = request.downloadedBytes;
                        yield return new WaitForSeconds(0.5f);
                    }
                    
                }
                if (downLoadProgress != null && isOver)
                {
                    downLoadProgress(1, totalLength, totalLength);
                }
                isLoadSuccess = true;
            }
        

            request.Dispose();
        }
        if (callBack != null && isOver)
        {
            callBack(new object[] { isLoadSuccess, savePath });
        }
    }


    //public void HTTPCallBack(object[] param)
    //{
    //    string json = param[0].ToString();
    //    Debug.Log(json);
    //    AppVersionRes res = JsonMapper.ToObject<AppVersionRes>(json);
    //    Debug.Log("res=" + res.ToString());
    //}
}
public class ReceiveData{
    public int  code=0;
    public string message="";
}