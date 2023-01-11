
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollChild : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect upperscroll;

    private void Awake()
    {
        Transform parent = transform.parent;
        if (parent)
        {
            upperscroll = parent.GetComponentInParent<ScrollRect>();
        }
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (upperscroll != null)
        {
            upperscroll.OnBeginDrag(eventData); 
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (upperscroll != null)
        {
            upperscroll.OnDrag(eventData);
        }

        
    }

    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (upperscroll != null)
        {
            upperscroll.OnEndDrag(eventData);
        }
    }
}


