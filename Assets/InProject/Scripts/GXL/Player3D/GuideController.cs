using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Compts.UI;
using HighlightingSystem;
using LitJson;
/// <summary>
/// 任务引导====
/// 通过消息主动修改进度---全局消息实现
/// </summary>
public class GuideController : ControllerBase
{   
    public int _progress{get;set;}=0;//默认进度0 第一个模块任务
    public Transform taskFind;//根节点对象 objection。
    public Text tmp_title;
    //告示牌的文字
    //任务进度
    [Header("任务进度")]
    public RectTransform textContent;
    public List<TextExpand> t_guide=new List<TextExpand>();
    public List<Toggle> tg_progress=new List<Toggle>();
    //任务d
    private List<GuideTask> save_guides =new List<GuideTask>();
    private List<string> taskIDPre=new List<string>();//前置ID-嗯
    private GuideTask  nowGuide;
     /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public virtual void Awake()
    {
        t_guide.AddRange(textContent.transform.GetComponentsInChildren<TextExpand>(true));
    }
   
    private void OnEnable() {
        tg_progress[_progress].isOn=true;
    }
    //初始化
    public  override void Init()
    {    
        Debug.Log("==配置任务引导数据"+GlobalPropsMgr.Instance._eqID);
        var preID=EquipmentConfigInfo.Datas[GlobalPropsMgr.Instance._eqID].preId;
        BuildData(preID);
        Refresh();
        // MovePos(save_guides[0].datas.First().Value,false);
        MessageCenter.Instance.RegiseterMessage(EMsg.Guide_Finish,this,Msg_Finished);
        MessageCenter.Instance.RegiseterMessage(EMsg.Guide_Next,this,Msg_Next);//触发下个任务
    }
    //构建引导数据
    private void BuildData(string preId){
        taskIDPre.AddRange(preId.Split("|"));
        foreach (var item in taskIDPre)
        {
            var preTask= GuideConfigInfo.Datas.Where(c=>c.Value.Id.Substring(0,3)==item).ToDictionary(k=>k.Key,v=>v.Value);
            save_guides.Add(new GuideTask(){
                isFinish=false,
                guideName=preTask.First().Value.GoupDescribe,
                guideID=item
            });
            foreach (var itor in preTask)
            {
                var v= DeepCopyEx.DeepCopyByReflection<GuideConfig>(itor.Value);
                v.Id=itor.Value.Id;
                save_guides.Last().datas.Add(itor.Key,v);
            }
        }
        Debug.Log("当前任务模块ID组-"+JsonMapper.ToJson(taskIDPre));
    }
    /// <summary>
    /// 刷新引导进度-当前数据项
    /// </summary>
    private void Refresh(){
        if(save_guides.Count<=0){
            return;
        }
        //刷新高亮 刷新传送圈
        nowGuide=save_guides[_progress];
        tmp_title.text=nowGuide.guideName;
        //刷新字体
        int x=0;
        Clear();
        foreach(var guide in nowGuide.datas){
            t_guide[x].text.text= $"    {guide.Value.Describe}";
            if(guide.Value.isFinish){
                t_guide[x].text.color=Color.yellow;
                 t_guide[x].transform.GetChild(1).gameObject.SetActive(true);
            }else{
                t_guide[x].text.color=Color.white;
                t_guide[x].transform.GetChild(1).gameObject.SetActive(false);
            }
            t_guide[x].gameObject.SetActive(true);
            x++;
        }
        //更新ContentSizeFitter 下一帧刷新
        var roots=textContent.GetComponentsInChildren<ContentSizeFitter>();
        foreach(var itor in roots){
            LayoutRebuilder.ForceRebuildLayoutImmediate(itor.GetComponent<RectTransform>());
        }
    }

    /// 此模块-弃用非顺序
    private void Modify(string guideID){
        var saveDT=save_guides[_progress];
        if(!saveDT.datas.ContainsKey(guideID)){
            Debug.Log($"此进度None-{guideID}");
            return;
        }
        var taskUnits=saveDT.datas[guideID];
        if(taskUnits.isFinish){
            return;
        }
        MovePos(taskUnits);
        ToTaskFind(saveDT.datas[guideID].taskPath);//taskPath不为空-激活对应节点
        saveDT.datas[guideID].isFinish=true;
        if(isPass(saveDT.datas)){
            _progress++;
            tg_progress[_progress].isOn=true;
            // MovePos(save_guides[_progress].datas.First().Value);
            Debug.Log($"{guideID}完成");
        }else{
        }
        Refresh();
    }
    int seq=0;
    /// <summary>
    /// 修改进度
    /// </summary>
    private void Modify(){
        var taskUnits=save_guides[_progress].datas;
        seq++;//20101 开始任务
        int seqnext=Mathf.Clamp(seq+1,0,taskUnits.Count);
        string guideID=$"{ taskIDPre[_progress]}0{seq}";
        string nextGuideID=$"{ taskIDPre[_progress]}0{seqnext}";

        if(!taskUnits.ContainsKey(guideID)){
            Debug.Log($"此进度ID不存在-{guideID}");
            return;
        }
        ToTaskFind(taskUnits[guideID].taskPath);//taskPath不为空-激活对应节点
        //其他判断
        if(taskUnits[guideID].isFinish){
            return;
        }
        if(taskUnits[nextGuideID].isTool)
        {
            MessageCenter.Instance.BoradCastMessage(EMsg.Player_tools,true);
        }else{
            MessageCenter.Instance.BoradCastMessage(EMsg.Player_tools,false); 
        }
        if(taskUnits[nextGuideID].isHelp){
            //MUIMgr.Instance.OpenUI(EMUI.MUI_Help);
        }
        taskUnits[guideID].isFinish=true;
        tg_progress[_progress].isOn=true;
        Debug.Log("任务Id-"+guideID);

        if(isPass(taskUnits)){
            seq=0;
            _progress++;
            Debug.Log($"当前场景任务总数-{taskIDPre.Count} 进度-{_progress}");
            if(_progress>=taskIDPre.Count){
                // MessageTip.Instance.ShowAlertBox(EMUI.Tip_Exit,EMUI.yellow16);
                return;
            }
            MUIMgr.Instance.CloseUI(EMUI.MUI_Main);
            MUIMgr.Instance.CloseUI(EMUI.MUI_Help);

            MovePos(save_guides[_progress].datas.First().Value);
            
        }else{
            MovePos(taskUnits[nextGuideID]);
        }
        Refresh();
       
    }
    private bool isPass( Dictionary<string, GuideConfig> datas){
        foreach (var item in datas)
        {   
            if(!item.Value.isFinish){
                return false;
            }
        }
        return true;
    }
    private void ToTaskFind(string taskPath){
        if(!String.IsNullOrEmpty(taskPath)){
            Debug.Log("taskPath-"+taskPath);
            var taskNode= taskFind.Find(taskPath);
            taskNode.gameObject.SetActive(true);
        }
    }
    ///
    ///设置NavAgent新位置-坐标-旋转
    public void MovePos(GuideConfig mapPos,bool isfade=true) {
        // Transform mainCam=Camera.main.transform;
        if(String.IsNullOrEmpty(mapPos.pos)){
            MessageCenter.Instance.BoradCastMessage(EMsg.Nav_IsActive,false);//关闭连线
            Debug.Log($"关闭寻路位置");
        }else{
            MessageCenter.Instance.BoradCastMessage(EMsg.Nav_IsActive,true);//打开连线
            var configMove=StringEx.SplitToFloat(mapPos.pos,"|");
            var pos = new Vector3(configMove[0], configMove[1], configMove[2]);
            // var configRotate=StringEx.SplitToFloat(mapPos.rotate,"|");
            MessageCenter.Instance.BoradCastMessage(EMsg.Nav_Start,pos);
            Debug.Log($"打开寻线位置-{mapPos.pos}");
        }
    }

    private void Clear(){
        foreach (var item in t_guide)
        {
            item.gameObject.SetActive(false);
        }
    }
    private void Msg_Finished(params object[] parms){

        Modify(parms[0].ToString());
    }
    private void Msg_Next(params object[] parms){
       Modify();
    }
    private void OnDestroy()
    {
        MessageCenter.Instance.RemoveAllMessageByName(EMsg.Guide_Finish);
        MessageCenter.Instance.RemoveAllMessageByName(EMsg.Guide_Next);
        Debug.Log("清除任务注册");
    }
}

public class GuideTask{
    public bool isFinish=false;
    public string guideID="-1";
    public string guideName="";
    public  Dictionary<string, GuideConfig> datas = new Dictionary<string, GuideConfig>();//子任务列表

}
