using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 登录界面
/// </summary>
public class LoginSystem : MUIBase
{
    InputField input_UserName;
    Button btn_Login;
    private string errorMessage;

    public override void OnAwake()
    {
        base.OnAwake();
        input_UserName = UtilityTool.FindChild<InputField>(transform, "input_UserName");
        btn_Login = UtilityTool.FindChild<Button>(transform, "btn_Login");
    }

    private void Start()
    {
        input_UserName.text = PlayerPrefs.GetString("UserName");
        btn_Login.onClick.AddListener(LoginReq);
        base.Open();
        // Debug.Log(GoodsConfigInfo.Goods["50000"].Icon);
    }


    public void OpenRegister()
    {
        Close();
        MUIMgr.Instance.OpenUI("Main/RegisterSystem");
    }

    //public void LoginReq()
    //{
    //    LoginReq req = new LoginReq();
    //    req.WlxqUid = input_UserName.text;

    //    ServerEngine.Instance.SendRequest((byte)MessageId.EloginReq, req, LoginResCallback);
    //}

    public void LoginReq()
    {
        // AutoLoginReq req = new AutoLoginReq();
        // req.WlxqUid = input_UserName.text;
        // req.Sex = "男";
        // req.NickName = input_UserName.text;
        // req.AccId = "guji_" + input_UserName.text;
        // ServerEngine.Instance.SendRequest((byte)MessageId.EautoLoginReq, req, LoginResCallback);
        MessageTip.Instance.ShowAlertBox("登录成功：");
        SceneMgr.Instance.LoadSceneAsync("Level_Main");
    }
}
