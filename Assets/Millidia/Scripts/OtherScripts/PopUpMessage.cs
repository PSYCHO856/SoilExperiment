using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PopUpMessage
{
    public static void ShowPopUp(string message,Vector3 pos,float flyTime = 1,float disAppearTime = 1)
    {
        var go = ResourceMgr.CreateUIPrefab("GUIs/PopMessage", MUIMgr.Instance.Canvas);
        var myText =  UtilityTool.FindChild<Text>(go.transform, "Text");
        myText.text = message;
        Sequence seq = DOTween.Sequence();
        seq.Append(go.transform.DOLocalMove(pos, flyTime));
        seq.Append(go.GetComponent<Image>().DOFade(0, disAppearTime));
        seq.Append(myText.DOFade(0, disAppearTime));
        seq.OnComplete(() => {
            GameObject.Destroy(go);
        });
    }
}
