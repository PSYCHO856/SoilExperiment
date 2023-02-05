using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 进度条显示组件
/// </summary>
public class MUILoading : MUIBase {
    public Image BGP;
    public Text messageT;
    public Material mat;

    public GameObject loadSceneProgressBarGb;
    public Image pro_bar;
    public Text pro_title;

    public GameObject updatePanelGb;

    public Text contentText;

    public Slider loadProgressSlider;

    public Text loadProgressText;

    public Button installBtn;
    public Text versionTxt;
    // public VersionController versionController;

    

    public override void OnAwake()
    {
        base.OnAwake();
        ResourceMgr.Instance.OnPercent += OnPercentView;//资源加载需要用到进度条
        SceneMgr.Instance.OnPercent += OnPercentView;
        versionTxt.text=" v"+Application.version;
    }

    private void OnEnable()
    {
        MessageCenter.Instance.BoradCastMessage(EMUI.Loading_Open);
    }

    private void OnDisable()
    {
        MessageCenter.Instance.BoradCastMessage(EMUI.Loading_Close);
    }
    /// <summary>
    /// 显示进度
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="message"></param>
    private void OnPercentView(float percent, string message)
    {
        mat.SetFloat("_FillLevel", percent);
        //loadImage.fillAmount = percent;
        // messageT.text = message;
        var rect= pro_bar.GetComponent<RectTransform>();
        float length=pro_bar.transform.parent.GetComponent<RectTransform>().rect.width;
        rect.sizeDelta=new Vector2(percent*length,rect.rect.height);
        pro_title.text=(int)(percent*100)+"%";
        // Debuger.Log(percent);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ResourceMgr.Instance.OnPercent -= OnPercentView;
        SceneMgr.Instance.OnPercent -= OnPercentView;
        // if (versionController != null)
        // {
        //     Destroy(versionController);
        // }
    }
    //切换背景图
    private void SetBGPImg(Sprite spr){
        BGP.sprite=spr;
    }

    public void SetUpdateMessage(string msg)
    {
        loadSceneProgressBarGb.SetActive(false);
        updatePanelGb.SetActive(true);
        contentText.text = msg;
    }

    public void SetLoadProgress(ulong currentloadbyte, ulong totalloadbyte)
    {

        if(currentloadbyte == totalloadbyte)
        {
            installBtn.gameObject.SetActive(true);
            loadProgressSlider.gameObject.SetActive(false);
        }
        loadProgressSlider.value = currentloadbyte * 1f / totalloadbyte;
        loadProgressText.text = (currentloadbyte / 1024) + "KB/" + (totalloadbyte / 1024) + "KB";
    }
}
