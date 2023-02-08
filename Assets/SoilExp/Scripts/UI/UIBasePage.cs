using DG.Tweening;
using UnityEngine;

public abstract class UIBasePage : MonoBehaviour
{
    private CanvasGroup cg;
    public virtual void OnOpen()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.DOFade(1, 1f);
    }

    public virtual void OnClose()
    {
        cg.DOFade(0, 0.5f);
    }
}