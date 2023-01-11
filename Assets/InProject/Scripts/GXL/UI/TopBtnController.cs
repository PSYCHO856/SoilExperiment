using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
/// <summary>
/// 常驻顶部页面列表-.
/// </summary>
public class TopBtnController : MonoBehaviour
{
    [Header("TopBtns")]
    public List<Button> topBtns=new List<Button>(); //0-设置 1-退出 2-home选择页面
    void Start()
    {
        init();
    }
     public void init(){
        topBtns[0].onClick.AddListener(()=>{
            SceneChoise();
        });
        topBtns[1].onClick.AddListener(()=>{
            MUIMgr.Instance.OpenUI(EMUI.MUI_Set);
        });
        topBtns[2].onClick.AddListener(()=>{
             //打开选择弹窗
            MUIMgr.Instance.ShowAlert(OpenType.TwoButton, () => { 
                Debug.Log("通用弹窗-退出窗口回调");
                ExitGame();
            }, null, "", "", "确定", "取消");
        });
        topBtns[3].onClick.AddListener(()=>{
            MUIMgr.Instance.OpenUI(EMUI.MUI_Help);
        });
        
    }
    private void SceneChoise()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "":
                //IOCThought.Instance.GetSceneClass<StartScene>();//例子
                break;
        }
    }
    private void ExitGame(){
        #if UNITY_EDITOR  
            EditorApplication.isPlaying=false;
        #elif UNITY_ANDROID  
            Application.Quit();
        #elif UNITY_STANDALONE_WIN 
            Application.Quit();
        #endif 
    }
}