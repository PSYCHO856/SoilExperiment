using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;
  
  
public abstract class MUIBase : MonoBehaviour {


    public bool useLua = true;

    /// <summary>
    /// 映射委托
    /// </summary>
    /// <param name="objs"></param>
    public delegate void Objects (params object[] objs);
    public string path;
    public TextAsset luaScript;
    private Action luaStart;
    private Action luaUpdate;
    private Objects luaOpen;
    private Objects luaClose;
    private Objects luaRefresh;
  
    private Action luaDestroy;


    /// <summary>
    /// 静态窗口 默认在场景中需要勾上
    /// </summary>
    public bool StaticWindow;

    public Canvas innerCanvas;

    public GraphicRaycaster ghRaycaster;

    public static int sortOrder = 20000;

    private void Awake()
    {
        OnAwake();
        // Debug.Log(UiName);
        if(StaticWindow)
         MUIMgr.Instance.PushUI(this);
    }

    void Start()
    {
        if (luaStart != null)
        {
            luaStart();
        }
    }

    void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate();
        }

    }

    public bool NoClose;
 
    public virtual void OnAwake()
    {
        MUIMgr.Instance.AddUI(this);
        transform.SetParent(MUIMgr.Instance.Canvas, false);

        if (useLua == false)
            return;


        luaScript = ResourceMgr.Load<TextAsset>("Luas/" + path + ".lua");
        if (luaScript == null)
            return;
       
    }

    public string UiName;
	public virtual void Open(params object[] parms)
    {
        transform.SetParent(MUIMgr.Instance.Canvas, false);
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        if(innerCanvas == null)
        {
            innerCanvas = gameObject.GetComponent<Canvas>();
            if (innerCanvas == null)
            {
                innerCanvas = gameObject.AddComponent<Canvas>();
            }
        }
        innerCanvas.overrideSorting = true;
        sortOrder++;
        innerCanvas.sortingOrder = sortOrder;
        if(ghRaycaster == null)
        {
            ghRaycaster = gameObject.GetComponent<GraphicRaycaster>();
            if (ghRaycaster == null)
            {
                ghRaycaster = gameObject.AddComponent<GraphicRaycaster>();
            }
        }

        if (!NoClose)
            MUIMgr.Instance.PushUI(this);

        if(luaOpen != null)
        {
            Debug.Log("参XXX数" + parms.Length);
            luaOpen(parms);
        }
        GetAllButtonAddListener();
    }

     void GetAllButtonAddListener()
     {
        {
            if(!AudioManager.Instance){
                return;
            }
            // //获取场景所有物体
            // GameObject[] allObj = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            // int btnCount = 0;
            // Button tmpButton;
            // Toggle tmpToggle;
        
            // for (int i = 0; i < allObj.Length; i++)
            // {
            //     tmpButton = allObj[i].GetComponent<Button>();
            //     tmpToggle = allObj[i].GetComponent<Toggle>();
            //     //如果是技能按钮不要音效

            //     if (tmpButton != null && skillItemBtn == null)
            //     {
            //         btnCount++;
            //         tmpButton.onClick.AddListener(()=>{
            //             AudioManager.Instance.PlayAudio(1,EAudio.se_btn_01);
            //         });
            //     }
            //     if (tmpToggle != null)
            //     {
            //         btnCount++;
            //         tmpToggle.onValueChanged.AddListener((Value)=>{
            //             if(Value){
            //                  AudioManager.Instance.PlayAudio(1,EAudio.se_btn_01);
            //             }
                       
            //         });
            //     }
            // }
        }
    }
    public virtual void Close(params object[] parms)
    {        
        if (NoClose)
            return;
        sortOrder--;
        gameObject.SetActive(false);
        transform.SetAsFirstSibling();

        if(luaClose != null)
        {
            luaClose(parms);
        }
    }

    public virtual void Refresh(params object[] parms)
    {
        Debug.Log(gameObject.name + "Refresh");
        if(luaRefresh != null)
        {
            luaRefresh(parms);
        }
    }

    public virtual void OnDestroy()
    {
        MUIMgr.Instance.RemoveUI(UiName);
    }

    public void Back()
    {
        MUIMgr.Instance.PopUI();
    }
}
