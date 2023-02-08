using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;
using System;
using Debug = UnityEngine.Debug;

public class CardPage : UIBasePage
{
    /// <summary>
    /// 存放word中需要替换的关键字以及对应要更改的内容
    /// </summary>
    protected Dictionary<string, string> wordReplaceDic = new Dictionary<string, string>();

    /// <summary>
    /// 存放word中需要替换（或者添加）的图片资源
    /// </summary>
    protected Dictionary<string, string> imgReplaceDic = new Dictionary<string, string>();

    /// <summary>
    /// 存放word中需要替换的关键字符（复选框状态）
    /// </summary>
    protected Dictionary<string, bool> toggleReplaceDic = new Dictionary<string, bool>();

    [SerializeField] protected Button BtnClose;

    [SerializeField] protected Transform Chooses;
    [SerializeField] protected Transform Inputs;
    // [SerializeField] protected Button BtnAppendixClose;

    /// <summary>
    /// 画图
    /// </summary>
     protected Button BtnClear1;
     protected Button BtnClear2;
     // protected Button BtnClearAll1;
     // protected Button BtnClearAll2;
     // protected Button BtnCut1;
     // protected Button BtnCut2;

    [SerializeField] protected GameObject Panel;
    // [SerializeField] protected GameObject AppendixPanel;

     protected Transform DrawLine1;
     protected Transform DrawLine2;
     protected Transform BG1;
     protected Transform BG2;

    [SerializeField] protected CardPageMenu cardPageMenu;
    protected GameObject nowInputField;
    protected List<GameObject> inputorderList = new List<GameObject>();

    [SerializeField] protected string iptName;
    [SerializeField] protected string toggleName;
    [SerializeField] protected string cardName;
    [SerializeField] protected bool isFirstOpen = true;

    [SerializeField] protected Button BtnCard;

    protected bool smallSceneState;
    [SerializeField] protected GameObject bg;
    // protected PlayerBehavior myPlayerBehavior;
    // protected SmallSceneCamera _smallSceneCamera;

    // protected GameObject toolPanel;

    [SerializeField] protected Vector3 disasterPos;
    [SerializeField] protected Vector3 disasterAngle;
    private void Awake()
    {
        PageInit();
        ButtonAndWordListenerInit();
    }

    HighlightingSystem.HighlightingRenderer cameraHighlightingRenderer;
    HighlightingSystem.HighlightingRenderer smallCameraHighlightingRenderer;

    public override void OnOpen()
    {
        base.OnOpen();
        cardPageMenu.MenuInit();
    }

    public virtual void PageInit()
    {
        // Panel.SetActive(false);
        // AppendixPanel.SetActive(false);
        //bg.SetActive(false);
        inputorderList.Clear();
        //cardPageMenu.card1Page = this;

        BtnClear1 = transform.Find("Panel/Scroll View/Viewport/Content/BtnClear1").GetComponent<Button>();
        BtnClear2 = transform.Find("Panel/Scroll View/Viewport/Content/BtnClear2").GetComponent<Button>();
        // BtnClearAll1 = transform.Find("Panel/Scroll View/Viewport/Content/BtnClearAll1").GetComponent<Button>();
        // BtnClearAll2 = transform.Find("Panel/Scroll View/Viewport/Content/BtnClearAll2").GetComponent<Button>();
        // BtnCut1 = transform.Find("Panel/Scroll View/Viewport/Content/BtnCut1").GetComponent<Button>();
        // BtnCut2 = transform.Find("Panel/Scroll View/Viewport/Content/BtnCut2").GetComponent<Button>();

        DrawLine1 = transform.Find("Panel/Scroll View/Viewport/Content/Mask1/DrawLine");
        DrawLine2 = transform.Find("Panel/Scroll View/Viewport/Content/Mask2/DrawLine");
        BG1 = transform.Find("Panel/Scroll View/Viewport/Content/BG1");
        BG2 = transform.Find("Panel/Scroll View/Viewport/Content/BG2");
        

        Messenger<GameObject>.AddListener(GameEvent.INPUTFIELD_RECORD, SelectedInputFieldChange);
        Messenger.AddListener(GameEvent.INPUTFIELD_WITHDRAW, SelectedInputFieldWithdraw);

        // myPlayerBehavior = GameObject.Find("Player").GetComponent<PlayerBehavior>();



    }


    /// <summary>
    /// 面板数据初始化
    /// </summary>
    protected void InitData()
    {
        //Inputs.transform.GetChild(35).GetComponent<InputField>().text = "46°";//坡度
        //Inputs.transform.GetChild(36).GetComponent<InputField>().text = "东南朝向";//坡向

        imgReplaceDic.Clear();
        imgReplaceDic.Add("{Image1}", Application.streamingAssetsPath + "/Image1.jpg");
        imgReplaceDic.Add("{Image2}", Application.streamingAssetsPath + "/Image2.jpg");
    }


    protected void Save(string docName)
    {
        //要替换文档的路径
        //string path = Path.Combine(Application.streamingAssetsPath, "地质灾害调查报告.docx");
        string path = Path.Combine(Application.streamingAssetsPath, docName + ".docx");

        //导出的替换后的文档路径
        //string outpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "地质灾害调查报告.docx");
        string outpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), docName + ".docx");

        //文本字典
        wordReplaceDic.Clear();
        //添加相应字段到字典 Inputs.transform.childCount
        for (int i = 0; i < Inputs.transform.childCount; i++)
        {
            string key = "$" + Inputs.transform.GetChild(i).gameObject.name + "$";
            string value = Inputs.transform.GetChild(i).GetComponent<InputField>().text;
            wordReplaceDic.Add(key, value);
        }

        //图片字典
        //imgReplaceDic.Clear();
        //imgReplaceDic.Add("{Image1}", Application.streamingAssetsPath + "/5.jpg");

        //关键字符字典
        toggleReplaceDic.Clear();
        for (int i = 0; i < Chooses.transform.childCount; i++)
        {
            Transform t = Chooses.transform.GetChild(i);
            for (int j = 0; j < t.transform.childCount; j++)
            {
                string key = "{" + t.transform.GetChild(j).gameObject.name + "}";
                bool value = t.transform.GetChild(j).GetComponent<Toggle>().isOn;
                toggleReplaceDic.Add(key, value);
            }
        }


        IOTool.ReplaceToWord(path, outpath, wordReplaceDic, imgReplaceDic, toggleReplaceDic);

    }

    /// <summary>
    /// 面板初始化
    /// </summary>
    protected void WordInit()
    {
        int m = 0, n = 0;
        //初始化Toggle
        for (int i = 0; i < Chooses.transform.childCount; i++)
        {
            Transform t = Chooses.transform.GetChild(i);
            for (int j = 0; j < t.transform.childCount; j++)
            {
                t.transform.GetChild(j).GetComponent<Toggle>().isOn = false;
                t.transform.GetChild(j).gameObject.name = toggleName + m.ToString();
                m++;
            }
        }

        //初始化InputField
        for (int i = 0; i < Inputs.transform.childCount; i++)
        {
            Inputs.transform.GetChild(i).GetComponent<InputField>().text = "";
            Inputs.transform.GetChild(i).gameObject.name = iptName + n.ToString();
            n++;
        }

        InitData();
    }

    protected void SelectedInputFieldChange(GameObject inputField)
    {
        nowInputField = inputField;
        if (inputorderList.Count == 0)
        {
            inputorderList.Add(inputField);
        }

        if (inputorderList[inputorderList.Count - 1] != inputField)
        {
            inputorderList.Add(inputField);
        }
    }

    protected void SelectedInputFieldWithdraw()
    {
        if (!nowInputField) return;
        nowInputField.GetComponent<InputFieldListener>().ClearInputField();
        inputorderList.RemoveAt(inputorderList.Count - 1);
        if (inputorderList.Count == 0)
        {
            nowInputField = null;
            return;
        }
        if (nowInputField != null)
            nowInputField = inputorderList[inputorderList.Count - 1];
    }

    protected void ButtonAndWordListenerInit()
    {

        //BtnClear.onClick.AddListener(delegate {
        cardPageMenu.btnWithDrawAll.onClick.AddListener(delegate {
            //初始化Toggle
            for (int i = 0; i < Chooses.transform.childCount; i++)
            {
                Transform t = Chooses.transform.GetChild(i);
                for (int j = 0; j < t.transform.childCount; j++)
                {
                    t.transform.GetChild(j).GetComponent<Toggle>().isOn = false;
                }
            }

            //初始化InputField
            for (int i = 0; i < Inputs.transform.childCount; i++)
            {
                Inputs.transform.GetChild(i).GetComponent<InputField>().text = "";
            }

            InitData();

        });
  
        cardPageMenu.BtnSave.onClick.AddListener(delegate
        {
            // UIController.Open(UIPageId.InfoPage);
            UIController.Open(UIPageId.InfoPage,delegate { Save(cardName);});
            // Save(cardName);
            UnityEngine.Debug.Log("btnsave clicked.");
            // PanelManager.Instance.ToolPanelManager.Hide_DisplayMessagePanel("保存表格数据到桌面？", delegate { Save(cardName); });
        });

        //BtnAppendix.onClick.AddListener(delegate {
        //    AppendixPanel.SetActive(true);
        //});

        // BtnAppendixClose.onClick.AddListener(delegate {
        //     // AppendixPanel.SetActive(false);
        // });

        BtnClear1.onClick.AddListener(delegate {
            DrawLine1.GetComponent<DrawLine>().WithdrawLastLine();
        });

        BtnClear2.onClick.AddListener(delegate {
            DrawLine2.GetComponent<DrawLine>().WithdrawLastLine();

        });

        // BtnClearAll1.onClick.AddListener(delegate {
        //     DrawLine1.GetComponent<DrawLine>().ClearAllLines();
        // });
        //
        // BtnClearAll2.onClick.AddListener(delegate {
        //     DrawLine2.GetComponent<DrawLine>().ClearAllLines();
        // });

        // BtnCut1.onClick.AddListener(delegate {
        //     //截屏测试
        //     //ToolManager.Instance.SaveScreenCapture(new Rect(0, 0, 1920, 1080), Application.streamingAssetsPath + "/5.jpg");
        //     Transform trans = BG1;
        //     //Vector3 pos = GameObject.Find("Eyes").GetComponent<Camera>().WorldToScreenPoint(trans.position);
        //     Camera cameraTrans = GameObject.Find("screenShotEyes").GetComponent<Camera>();
        //
        //     //Vector3 v3 = BG1.GetComponent<RectTransform>().anchoredPosition3D;
        //     //Debug.Log(v3);
        //
        //     Vector3 pos = cameraTrans.WorldToScreenPoint(trans.position);
        //     //Debug.Log("pos " + pos);
        //
        //     //获取bg在ui坐标的位置
        //     if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").transform as RectTransform, pos,
        //         cameraTrans,
        //         //GameObject.Find("Eyes").GetComponent<Camera>(),
        //         out Vector2 spos))
        //     {
        //         //Debug.Log("spos " + spos);
        //         Rect rect = trans.GetComponent<RectTransform>().rect;
        //         rect.position = new Vector2(spos.x + Screen.width / 2, spos.y + Screen.height / 2);
        //         Debug.Log(rect.position + "  " + rect.size);
        //         if (imgReplaceDic.TryGetValue("{Image1}", out string value))
        //         {
        //             ToolManager.Instance.SaveScreenCapture(rect, value);
        //         }
        //     }
        // });
        //
        // BtnCut2.onClick.AddListener(delegate {
        //     //截屏测试
        //     //ToolManager.Instance.SaveScreenCapture(new Rect(0, 0, 1920, 1080), Application.streamingAssetsPath + "/5.jpg");
        //     Transform trans = BG2;
        //
        //     Camera cameraTrans = GameObject.Find("screenShotEyes").GetComponent<Camera>();
        //     Vector3 pos = cameraTrans.WorldToScreenPoint(trans.position);
        //
        //     //Vector3 pos = GameObject.Find("Eyes").GetComponent<Camera>().WorldToScreenPoint(trans.position);
        //     if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").transform as RectTransform, pos,
        //         cameraTrans, out Vector2 spos))
        //     {
        //         Rect rect = trans.GetComponent<RectTransform>().rect;
        //         rect.position = new Vector2(spos.x + Screen.width / 2, spos.y + Screen.height / 2);
        //         Debug.Log(rect.position + "  " + rect.size);
        //         if (imgReplaceDic.TryGetValue("{Image2}", out string value))
        //         {
        //             ToolManager.Instance.SaveScreenCapture(rect, value);
        //         }
        //     }
        // });

        BtnCard.onClick.AddListener(delegate {
            UserHelper.SetHighlightOff();
            Invoke("DelayOpenCard", 0.1f);
            
            Panel.SetActive(true);
            //toolPanel.SetActive(false);
            //BtnCard.gameObject.SetActive(false);

        });

        BtnClose.onClick.AddListener(delegate {
            PanelClose();
        });



        WordInit();
    }

    void DelayOpenCard()
    {
        // if (Panel.activeSelf)
        // {
        //     PanelClose();
        // }
        // else
        // {
        //     Panel.SetActive(true);
        //     if (isFirstOpen)
        //     {
        //         myPlayerBehavior.Move(disasterPos, disasterAngle, 3, null);
        //         isFirstOpen = false;
        //
        //         cameraHighlightingRenderer = myPlayerBehavior.myCamera.transform.GetComponent<HighlightingSystem.HighlightingRenderer>();
        //         smallCameraHighlightingRenderer = myPlayerBehavior.smallSceneCamera.transform.GetComponent<HighlightingSystem.HighlightingRenderer>();
        //     }
        //     SmallSceneControl();
        //     PanelManager.Instance.ToolPanelManager.PackPanel.OpenPanel();
        //
        // }
        // PanelManager.Instance.ToolPanelManager.PackPanel.ClearLines();
    }

    void PanelClose()
    {
        base.OnClose();
        // Panel.SetActive(false);
        // toolPanel.SetActive(true);
        // BtnCard.gameObject.SetActive(true);
        SmallSceneControl();
    }

    void SmallSceneControl()
    {
        if (smallSceneState)
        {
            // cameraHighlightingRenderer.enabled = true;
            // smallCameraHighlightingRenderer.enabled = false;
            // myPlayerBehavior.myCamera.enabled = true;
            // myPlayerBehavior.smallSceneCamera.enabled = false;

            // smallSceneState = false;
            // ToolManager.Instance.isElevationToolEnable = true;
        }
        else
        {
            // cameraHighlightingRenderer.enabled = false;
            // smallCameraHighlightingRenderer.enabled = true;
            // myPlayerBehavior.myCamera.enabled = false;
            // myPlayerBehavior.smallSceneCamera.enabled = true;

            // smallSceneState = true;
            // ToolManager.Instance.isElevationToolEnable = false;
        }
    }

    //相机旋转导致WorldToScreenPoint方法出问题
    void CameraRotateProblem(Vector3 world2ScreenPos)
    {
        if (world2ScreenPos.z < 0)
        {

        }
    }

}
