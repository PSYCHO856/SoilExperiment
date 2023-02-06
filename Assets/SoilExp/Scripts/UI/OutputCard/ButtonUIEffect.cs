using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUIEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image highlight;

    private void Awake()
    {
        highlight = transform.GetChild(0).GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.enabled = false;
    }
}
