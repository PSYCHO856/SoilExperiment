using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 认知模块的配置文件
/// </summary>
[CreateAssetMenu(menuName ="GXL/CfgKonwledge")]
public class ObjectCfgKonwledge : ScriptableObject
{
    [Header("标题")]
    public string title;
    public string content;
    //内容图集
    public List<Sprite> contentSprs=new List<Sprite>();
    //视频
    public VideoClip videoclip;
    [Header("演示模型")]
    public GameObject aniPrefab;
    [Header("设计参数")]
    public List<Sprite> designSprs=new List<Sprite>();
}
