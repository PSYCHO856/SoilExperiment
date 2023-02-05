using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 设置页面1
/// </summary>
public class MUIKnow: MUIBase {
    public Text titeMain;
    public Text content;
    public RectTransform textContent;
    public PlayVideo playVideo;
    [Header("底部列表")]
    public GameObject equipsItem;
    public TgGroup bot_equips;//底部tg按钮
    public List<Toggle> left_tgls=new List<Toggle>();//左侧tg按钮
    public List<Transform> nodes=new List<Transform>();//0-图文节点 1-模型节点
    [Header("TipWIndow")]
    public TipWindow tipWindow;
    private List<GameObject> _botList=new List<GameObject>();
    public override void OnAwake() {
        UiName = EMUI.MUI_Konw;
        base.OnAwake();
        this.gameObject.SetActive(false);
        nodes[1]=GameObject.Find("ModelsNode").transform;
        tipWindow=GameObject.Find("TipWindow").GetComponent<TipWindow>();
    }
    //只执行一次
    void Start() {
        left_tgls[1].onValueChanged.AddListener((isON)=>{
            if(isON){
               playVideo.Init(); //切换视频
            }
        });
        left_tgls[2].onValueChanged.AddListener((isON)=>{
            if(isON){
                Debug.Log("切换到模型动画");
                tipWindow.ToTask(3);
            }
        });
    }
    public void OnTg(int index){
         left_tgls[index].isOn=true;
    }
    /// <summary>
    /// 刷新文本-视频-动画
    /// </summary>
    /// <param name="key"></param>
    private void DoRefresh(string key){
        left_tgls[0].isOn=true;
        Clear();

        if(!GlobalPropsMgr.Instance._eqipdata.ContainsKey(key)){
            Debug.Log("不存在此设备UID-"+key);
            return;
        }
        var data=GlobalPropsMgr.Instance._eqipdata[key];
        //文本刷新
        content.text=data.content;
        titeMain.text=data.title;
        //图片资源刷新
        //视频
        if(playVideo.Init(data.videoclip)){
            left_tgls[1].gameObject.SetActive(false);
        }else{
            left_tgls[1].gameObject.SetActive(true);
        }
        //模型
        Instantiate(data.aniPrefab,nodes[1]);
        Debug.Log($"刷新图标文本-{key}");
        LayoutRebuilder.ForceRebuildLayoutImmediate(textContent); 
    }
    private void Clear(){
        nodes[1].transform.localScale=Vector3.one;
        if(nodes[1].childCount>0){
            Destroy(nodes[1].GetChild(0).gameObject);
            Debug.Log(nodes[1].GetChild(0).gameObject);
        }
    }
    int changeIndex=0;
    bool isCreat=true;
    /// <summary>
    ///打开界面
    /// </summary>
    public override void Open(params object[] parms) {
        base.Open(parms);
        string equipId=parms[0].ToString();
        changeIndex=(int)parms[1];
        bot_equips.OnChange(changeIndex);
        DoRefresh(equipId);
        if(isCreat){
            CreatEquip();
            isCreat=false;
        }
    }
    private void CreatEquip(){
        equipsItem.SetActive(false);
        foreach(var itor in EquipmentConfigInfo.Datas){
            var obj=Instantiate(equipsItem,equipsItem.transform.parent);
            var equipId=itor.Value.Id;
            obj.GetComponentInChildren<Text>().text=itor.Value.Name;
            obj.GetComponent<EventListener>().UIID=equipId;
            obj.SetActive(true);
            _botList.Add(obj);
        }
        UtilityTool.Instance.WaitForSecond(0.2f,()=>{
            bot_equips.tgIndex=changeIndex;
            bot_equips.Init();
            foreach(var itor in bot_equips.listeners){
                itor.onClick+=(p)=>{
                   DoRefresh(itor.UIID);
                };
            }
        });
    }
    public void CloseSelf()
    {
        MUIMgr.Instance.CloseUI(UiName);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="parms"></param>
    public override void Close(params object[] parms) {
        base.Close(parms);
    }
    /// <summary>
    /// 刷新界面
    /// </summary>
    /// <param name="parms"></param>
    public override void Refresh(params object[] parms) {
        base.Refresh(parms);
    }
}