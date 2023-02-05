using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Obj_ActiveStatue: MonoBehaviour  {
    public List < GameObject > lists = new List < GameObject > ();
    public int openID = 0;
    public bool isAwake=false;
    private void Start(){
        if(isAwake)
            ActiveByAt(openID);
    }
    /// <summary>
    /// 激活对应UI
    /// </summary>
    public void ActiveByAt(int openID) {
        for (int i = 0; i < lists.Count; i++) {
            if(i==openID){
                lists[i].SetActive(true);
            }else{
                lists[i].SetActive(false);
            }
        }
    }
}