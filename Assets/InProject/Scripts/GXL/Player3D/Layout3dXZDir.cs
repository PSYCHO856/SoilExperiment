using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 3d布局-XZ轴排版
/// </summary>
[ExecuteInEditMode] 
public class Layout3dXZDir : MonoBehaviour
{
    public int totles=1;
    public int row=0;//行数
    public int column=0;//列数
    /// x轴-间距 z轴-间距
    public Vector2 spacing=new Vector2(0,0);
    public DirType dirType=DirType.Horizon;
    public GameObject obj3d;

    private Vector3 originPos;
     private void OnEnable()
     {
        CreateObj();
        Debug.Log("打开3d布局!");
     }

     private void CreateObj(){
        originPos=obj3d.transform.localPosition;
        Vector3 pos=originPos;
        if(dirType==DirType.Horizon){
            for(int i=0;i<totles;i++){
                pos=Vector3.right*spacing.x*(i+1)+originPos;
                var obj= Instantiate(obj3d,obj3d.transform.parent);
                obj.transform.localPosition=pos;
            }
        }else if(dirType==DirType.Vertial){
            for(int i=0;i<totles;i++){
                pos=Vector3.forward*spacing.y*(i+1)+originPos;
                var obj= Instantiate(obj3d,obj3d.transform.parent);
                obj.transform.localPosition=pos;
            }
        }
     }

}

public  enum DirType{
    Horizon=0,
    Vertial=1
}
