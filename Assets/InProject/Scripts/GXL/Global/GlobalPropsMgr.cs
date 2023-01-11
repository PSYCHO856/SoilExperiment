using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
/// <summary>
/// 全局参数-管理器
/// </summary>
public class GlobalPropsMgr : APP.Core.MonoSingleton<GlobalPropsMgr>,ManagerBase
{
    public string _eqID="1006";
    //学生数据
    private StudentInfo _studentInfo=new StudentInfo();
    //获取ScriptObject配置数据
    public Dictionary<string,ObjectCfgKonwledge> _eqipdata=new Dictionary<string, ObjectCfgKonwledge>();
    //工具栏全部图片
    public Dictionary<string,Sprite> iconTool=new Dictionary<string, Sprite>();
    private void Start() {
        Init();
    }
    #region 反射遍历类的数据成员
    public List<string > GetStudentInfo(){
        List<string> temps=new List<string>();
        Type type=typeof(StudentInfo);
        PropertyInfo[] props=type.GetProperties();
        foreach(var itor in props){
            string prop=itor.GetValue(_studentInfo).ToString();
            temps.Add(prop);
            Debug.Log($"student-{prop}");
        }
        return temps;
    }
    public void SetStudentInfo(List<string > datas){
        Type type=typeof(StudentInfo);
        PropertyInfo[] props=type.GetProperties();
        int i=0;
        foreach(var prop in props){
            prop.SetValue(_studentInfo,datas[i]);
            i++;
        }
    }
    #endregion
    public  void Init(){
        var lists=ResourceMgr.LoadAll<Sprite>(ResPath.ui_tool);
        foreach (var item in lists)
        {
            iconTool.Add(item.name,item);
        }
        Debug.Log(iconTool);
    }
    private ObjectCfgKonwledge LoadRes(string name){
        return Resources.Load<ObjectCfgKonwledge>(ResPath.cf_konwledge+name);
    }
}

/// <summary>
/// 学生数据
/// </summary>
public class StudentInfo{
    public string classroom{set;get;}="";
    public string name{set;get;}="";
    public string number{set;get;}="";
    public string level{set;get;}="";
    public string grade{set;get;}="";
    
}

/// <summary>
/// 全局保存存储数据
/// </summary>
public class SaveData{
    //任务进度数据
    public Action a;
}