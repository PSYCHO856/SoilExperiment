using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPage : UIBasePage
{

    public Button btnClose;

    private void Awake()
    {
        btnClose.onClick.AddListener((() => base.OnClose()));
    }
}
