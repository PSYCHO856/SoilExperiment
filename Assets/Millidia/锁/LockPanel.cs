using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//if (!HaspLock.Instance.LoginHasp()) return;
public class LockPanel :SingletonBaseComponent<LockPanel>
{
    private Text Tip;

    protected override void OnAwake()
    {
        Tip = transform.GetComponentInChildren<Text>();            

        gameObject.SetActive(false);
    }

    protected override void OnStart()
    {
        
    }

    public void Show(string str)
    {
        gameObject.SetActive(true);
        Tip.text = str;

        Exit();
    }

    //3秒钟之后退出程序
    public void Exit()
    {
        StartCoroutine(IE_exit(3));
    }

    IEnumerator IE_exit(float time)
    {
        yield return new WaitForSeconds(time);

        Application.Quit();
    }

}
