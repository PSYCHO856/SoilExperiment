using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
/// <summary>
/// 选择页面的做成循环列表形式
/// </summary>
public class MUISelectCtl: MonoBehaviour {
    //按钮列表
    public Transform uiRoot;
    [SerializeField]
    private List<Button> btns=new List<Button>();
    void OnAwake() {
     
    }

    void Start() {
        init();
    }

    public void init(){
        AudioManager.Instance?.PlayAudio(0,EAudio.bgm_main_01);//播放BGM-设置音量大小
        AudioManager.Instance?.SetVolume(PersistentDataMgr.Instance.PlayerData.set_bgm,0);
        btns[0].onClick.AddListener(()=>{
            Debug.Log("Level_PPT01");
        });
        btns[1].onClick.AddListener(()=>{
            Debug.Log("Level_PPT01");

        });
        btns[2].onClick.AddListener(()=>{
            Debug.Log("Level_PPT01");

        });
        btns[3].onClick.AddListener(()=>{
            Debug.Log("Level_PPT01");
            // SceneMgr.Instance.LoadSceneAsync(EScene.Level_Main.ToString(),()=>{
            //     Debug.Log("进入3D污水漫游");
            // });
        });
        btns[4].onClick.AddListener(()=>{
            Debug.Log("Level_PPT01");
        });
    }
}