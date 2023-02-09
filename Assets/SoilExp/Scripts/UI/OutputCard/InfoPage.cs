using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class InfoPage : UIBasePage
{

    public Button btnClose;

    private void Awake()
    {
        btnClose.onClick.AddListener(() =>
        {
            openRefAction.Invoke();
            base.OnClose();
        });
    }
}
