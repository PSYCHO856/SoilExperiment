using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ConfirmBtnClick : MonoBehaviour
{
    public void btnClick()
    {
        transform.parent.gameObject.GetComponent<CanvasGroup>().DOFade(0, 1f);
    }
}
