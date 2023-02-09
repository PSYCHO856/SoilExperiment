using System;
using DG.Tweening;
using UnityEngine;

public abstract class UIBasePage : MonoBehaviour
{
    private CanvasGroup cg;
    public Action openRefAction;
    public float closeDuration = 0.5f;
    public virtual void OnOpen()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.DOFade(1, 1f);
    }
    
    public virtual void OnOpen(Action action)
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.DOFade(1, 1f);
        openRefAction = action;
    }

    public virtual void OnClose()
    {
        cg.DOFade(0, closeDuration);
        Invoke("Close",closeDuration);
    }

    void Close()
    {
        gameObject.SetActive(false);
    }
}