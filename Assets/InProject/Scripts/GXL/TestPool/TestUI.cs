using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using EnergyBarToolkit;

public class TestUI: MonoBehaviour {
    public Button btn=null;
    private void Awake() {

        #if(DEBUG)
            print("dddd");
        #endif

        MyClass my=new MyClass();
        Debug.Log(my.name);
    }
    private void Start()
    {   
        if(btn!=null){
            btn.onClick.AddListener(()=>{
            });
        }
    }

    void DoSomething(){
        Debug.Log("DoSomething");
    }
}
public class MyClass{
    public string name="name";
}