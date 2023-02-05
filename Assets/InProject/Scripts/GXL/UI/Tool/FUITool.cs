using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 工具栏 1
/// </summary>
public class FUITool : MonoBehaviour
{
    string nowIcon="";
    /// <summary>
    /// 当前UI
    /// </summary>
    public Transform switchTool;
    public GameObject tgItem;//
    public Obj_ActiveStatue activeStatue;
    public List<Transform> camPos=new List<Transform>();
    /// <summary>
    /// 生成模型的节点1
    /// </summary>
    public Transform modelsNode;
    /// 切换模块
    private void Start() {
        Init();
        UtilityTool.Instance.WaitForFrame(2,()=>{
            CreatTool(0);
        });
    }

    void Init(){
        var btns=switchTool.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < btns.Length; i++)
        {
            int index=i;
            ///切换四个模块
            btns[i].onValueChanged.AddListener((isOn)=>{
                if(isOn){
                    SetPos(index);//设置摄像机位置1
                    CreatTool(index);//重新创建背包栏道具/清除场景的模型
                    activeStatue.ActiveByAt(index);//激活对应高亮插入预制件
                }
            });
        }
         btns[2].onValueChanged.AddListener((isOn)=>{
            if(isOn){
                MessageCenter.Instance.BoradCastMessage(EMsg.GuideBig_Emit,"260002");
            }
        });
    }
    /// <summary>
    /// 创建tg对象
    /// </summary>
    /// <param name="index"></param>
    public void CreatTool(int index){
        Clear();
        string searchID=$"{index}0";
        tgItem.SetActive(false);
        var listDics=GlobalPropsMgr.Instance.iconTool.
        Where(c=>c.Key.Substring(0,searchID.Length)==searchID);
        // Debug.Log("searchID-"+index);
        int m=0;
        foreach (var item in listDics)
        {
            var obj=Instantiate(tgItem,tgItem.transform.parent);
            var toolUnit=obj.GetComponent<FUIToolUnit>();
            obj.SetActive(true);
            toolUnit.title.text=StringEx.SplitToIndex(item.Key,"_",1);
            toolUnit.icon.sprite=item.Value;
            toolUnit.tg=obj.GetComponent<Toggle>();
            toolUnit.UID=item.Key;
            toolUnit.index=m;
            toolUnit.tg.onValueChanged.AddListener((isOn)=>{
                if(isOn){
                    CreatObj(toolUnit);
                }
            });
            m++;
        }
    }
    /// <summary>
    /// 清空道具icon 跟场景模型
    /// </summary>
    public void Clear(){
         int childs=tgItem.transform.parent.childCount;
         for (int i = 0; i < childs; i++)
         {
            if(i!=0){
                Destroy(tgItem.transform.parent.GetChild(i).gameObject);
            }
         }
         var modelChilds=modelsNode.GetComponentsInChildren<UnitBase>(false);
         Debug.Log("清除全部模型-"+modelChilds.Count());
         //清除modelNode下的模型
         foreach (var item in modelChilds)
         {
            Destroy(item.gameObject);
         }
    }
    /// <summary>
    /// 创建预制件
    /// </summary>
    /// <param name="toolUnit"></param>
    public void CreatObj(FUIToolUnit toolUnit){
        Debug.Log($"Prefab：{toolUnit.UID}");
        var pref=Resources.Load<GameObject>(ResPath.prefab_tool+toolUnit.UID);
        if(pref!=null){
            GameObject  obj=Instantiate(pref,modelsNode);
        }
        toolUnit.gameObject.SetActive(false);
    }
    /// <summary>
    /// 根据点位设置摄像机位置
    /// </summary>
    /// <param name="index"></param>
    private void SetPos(int index){
        Camera.main.transform.position = camPos[index].transform.position;
        Camera.main.transform.eulerAngles = camPos[index].transform.eulerAngles;
    }
}

