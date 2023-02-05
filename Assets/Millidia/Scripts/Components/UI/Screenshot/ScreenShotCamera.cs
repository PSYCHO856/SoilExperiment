using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>
/// 选择摄像机截图并保存本地
/// </summary>
public class ScreenShotCamera : MonoBehaviour {

//     public Camera mCamera;
//     public Image mImage;

//     byte[] mbyte;

//     private void Start()
//     {
//         //StartCoroutine(Shot(new Rect(0, 0, Screen.width, Screen.height), mCamera, Application.streamingAssetsPath + "/screenShot.png"));
//         //Fun(mCamera, Application.streamingAssetsPath + "/UDPTexture/screenShot.jpg");
//     }

//     private void Update()
//     {
//         if (Input.GetKeyDown("w"))
//         {
//             OpenDirectory();
//             //UDPClient.instance.SocketSend(Convert.ToBase64String(mbyte));
//         }
//     }

//     void Screenshot(Camera m_Camera, string filePath)
//     {
//         RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 16);
//         m_Camera.targetTexture = rt;
//         m_Camera.Render();
//         RenderTexture.active = rt;
//         Texture2D t = new Texture2D(Screen.width, Screen.height);

//         t.ReadPixels(new Rect(0, 0, t.width, t.height), 0, 0);
//         t.Apply();

//         Sprite sprite = Sprite.Create(t, new Rect(0, 0, Screen.width, Screen.height), Vector2.zero);
//         // mImage.sprite = sprite;

//         //mbyte = t.EncodeToJPG();
//         mbyte = new byte[t.EncodeToJPG().Length];
//         mbyte = t.EncodeToJPG();
//         // Debug.Log(mbyte.Length);
//         System.IO.File.WriteAllBytes(filePath, t.EncodeToJPG());

// #if UNITY_EDITOR
//         UnityEditor.AssetDatabase.Refresh();
// #endif

//         //还原各项截图前得初始设定
//         m_Camera.targetTexture = null;
//         RenderTexture.active = null;
//         Destroy(rt);
//     }
//     string path;
//     /// <summary>
//     /// // 打开文件夹路径
//     /// </summary>
//     public  string OpenDirectory()
//     {
//         path = UnityEditor.EditorUtility.OpenFolderPanel("保存路径",  Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "");
//         Screenshot(mCamera,path + "/实验截图1.jpg");
//         Debug.Log(path);
//         return path;
//     }
}