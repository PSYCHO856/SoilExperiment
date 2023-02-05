using System;
using System.IO;
using UnityEngine;
using System.Net.Mime;
using I2.Loc;
/// <summary>
/// 可持久化数据管理类
/// </summary>
public class PersistentDataMgr {
    private static PersistentDataMgr instance;

    public static PersistentDataMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PersistentDataMgr();
            }
            
            return instance;

        }
    }

    private PlayerSetData player;

    public PlayerSetData PlayerData{
        get{
            LoadPlayerJson();
            return player;
        }
        set{
            player=value;
        }
    }
    public void SetView(int index)
    {
        PlayerData.set_playerView = index;
        SavePlayerJson();
    }

    public  void SavePlayerJson () {
        //string path = Application.persistentDataPath + ResPath.localPlayer;
        // var content = JsonUtility.ToJson(player, true);
        // File.WriteAllText(path, content);
        PlayerPrefs.SetInt("id",player.id);
        PlayerPrefs.SetInt("set_fullScreen",player.set_fullScreen);
        PlayerPrefs.SetInt("set_showName",player.set_showName);
        PlayerPrefs.SetFloat("set_bgm",player.set_bgm);
        PlayerPrefs.SetInt("set_req",player.set_req);
        PlayerPrefs.SetInt("set_playerView",player.set_playerView);
      
    }
    public  PlayerSetData LoadPlayerJson() {

        // string path = Application.persistentDataPath + ResPath.localPlayer; //默认路径
        // if (File.Exists(path)) {
        //     var content = File.ReadAllText(path);
        //     player= JsonUtility.FromJson < Player > (content);
        //     return player;
        // } else {
        //     Debug.LogError("Save file not found in  " + path);
        //     return null;
        // }
        if (player == null)
        {

            player = new PlayerSetData();
           
            player.id = PlayerPrefs.GetInt("id", player.id);
            player.set_fullScreen = PlayerPrefs.GetInt("set_fullScreen", player.set_fullScreen);
            player.set_showName = PlayerPrefs.GetInt("set_showName", player.set_showName);
            player.set_bgm = PlayerPrefs.GetFloat("set_bgm", player.set_bgm);
            player.set_req = PlayerPrefs.GetInt("set_req", player.set_req);
            player.set_playerView = PlayerPrefs.GetInt("set_playerView", player.set_playerView);

        }
        
        return player;
    }

    public void Init()
    {
        LoadPlayerJson();
        //if (LocalizationManager.CurrentLanguage != LocalizationManager.GetAllLanguages()[player.set_language])
        //{
        //    LocalizationManager.CurrentLanguage = LocalizationManager.GetAllLanguages()[player.set_language];
        //}
    }
}

public class PlayerSetData {
    public int id=0;
    public int set_fullScreen=1;
    public int set_showName=1; 
    public float set_bgm=0.5f; 
    public int set_req=1; //1 true  0falseint
    /// <summary>
    /// 0为第三人称，1为第一人称
    /// </summary>
    public int set_playerView=0; //视角设置

    public int set=0;
}