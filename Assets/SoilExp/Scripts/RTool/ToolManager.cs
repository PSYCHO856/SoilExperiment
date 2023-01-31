using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景工具类，单例
/// </summary>
public class ToolManager : SingletonBaseComponent<ToolManager>
{

    public bool isElevationToolEnable = true;
    public bool isDialogueOpen;
    public bool isPackPanelDisplay;
    public int sceneNumber = 0;

    protected override void OnAwake()
    {
        DontDestroyOnLoad(gameObject);
    }

    protected override void OnStart()
    {
        
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }

    /// <summary>
    /// 设置物体高亮,高亮时间默认3秒钟
    /// </summary>
    /// <param name="t"></param>
    /// <param name="time"></param>
    public void SetHighlightOn(Transform t, float time = 0)
    {
        SetHighlightOff();
        UserHelper.SetHighlightOn(t);
        if (time != 0)
        {
            CancelInvoke("SetHighlightOff");
            Invoke("SetHighlightOff", time);
        }
            
    }
    private void SetHighlightOff()
    {
        UserHelper.SetHighlightOff();
    }


    /// <summary>
    /// 截取屏幕内的像素
    /// </summary>
    /// <param name="rect">截取区域：屏幕左下角为0点</param>
    /// <param name="fileName">文件名及存储路径</param>
    /// <param name="callBack">截图完成回调</param>
    public void SaveScreenCapture(Rect rect, string fileName, Action callBack = null)
    {
        StartCoroutine(ScreenCapture(rect, fileName, callBack));
    }


    /// <summary>
    /// 截取屏幕内的像素 
    /// </summary>
    /// <param name="rect">截取区域：屏幕左下角为0点</param>
    /// <param name="fileName">文件名及存储路径</param>
    /// <param name="callBack">截图完成回调</param>
    /// <returns></returns>
    IEnumerator ScreenCapture(Rect rect, string fileName, Action callBack = null)
    {
        yield return new WaitForEndOfFrame();//等到帧结束，不然会报错
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);//新建一个Texture2D对象
        tex.ReadPixels(rect, 0, 0);//读取像素，屏幕左下角为0点
        tex.Apply();//保存像素信息

        byte[] bytes = tex.EncodeToPNG();//将纹理数据，转化成一个png图片


        System.IO.File.WriteAllBytes(fileName, bytes);//写入数据 

        Debug.Log(string.Format("截取了一张图片: {0}", fileName));

        callBack?.Invoke();
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//刷新Unity的资产目录
#endif
    }


}
