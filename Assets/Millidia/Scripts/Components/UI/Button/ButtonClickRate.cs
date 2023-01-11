using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 按钮点击后冷却一定时间才可以再次点击
/// </summary>
public class ButtonClickRate : MonoBehaviour
{
    /// <summary>
    /// 需要控制的btn
    /// </summary>
    public Button rateBtn;
    /// <summary>
    /// 按钮点击冷却时间
    /// </summary>
    public float btnCoolDownTime = 1;

    private float lastClickTime;

    private void Awake()
    {
        if(rateBtn == null)
        {
            rateBtn = gameObject.GetComponent<Button>();
        }
        if(rateBtn != null)
        {
            rateBtn.onClick.AddListener(RateBtn_Click);
        }
        lastClickTime = btnCoolDownTime;
    }

    public void RateBtn_Click()
    {
        lastClickTime = 0;
        rateBtn.enabled = false;
    }

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rateBtn != null && rateBtn.enabled == false)
        {
            if(lastClickTime < btnCoolDownTime)
            {
                lastClickTime += Time.deltaTime;
            }
            else
            {
                rateBtn.enabled = true;
            }
        }
    }
}
