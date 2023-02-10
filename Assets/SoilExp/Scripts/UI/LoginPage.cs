using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPage : UIBasePage
{
    public InputField ipf1;
    public InputField ipf2;
    public Button btnLogin;
    
    // Start is called before the first frame update
    void Start()
    {
        btnLogin.onClick.AddListener(() =>
        {
            SaveStudentData();
            
            base.OnClose();
            UIController.Open(UIPageId.MainPage);
        });
        
    }

    void SaveStudentData()
    {
        ToolManager.Instance.stuName = ipf1.text;
        ToolManager.Instance.stuNumber = ipf2.text;
        ToolManager.Instance.isLogin = true;

    }
}