using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
using UnityEngine;
/// <summary>
/// 图片适配字长度
/// </summary>
[ExecuteInEditMode]
public class UIChildSizeMgr : UIBehaviour
{
    public FollowItem[] follows;
    private bool bChange = true;

    [Serializable]
    public class FollowItem
    {
        public RectTransform follower;
        public RectOffset padding;
        public bool followWidth = true;
        public bool followHeight = true;

        public void SetSize(RectTransform trans)
        {
            if (null == trans || null == follower)
                return;
            if (followWidth)
            {
                float width = padding.left + padding.right + trans.rect.width;
                follower.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            }
            if (followHeight)
            {
                float height = padding.top + padding.bottom + trans.rect.height;
                follower.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            }
        }

    }

    protected override void Awake()
    {
        base.Awake();
        bChange = true;
        ResetSize();
    }


    void ResetSize()
    {
        if (!bChange) return;
        RectTransform rectTrans = transform as RectTransform;

        if (null == follows) return;
        for (int i = 0, max = follows.Length; i < max; i++)
        {
            follows[i].SetSize(rectTrans);
        }
       
        bChange = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        bChange = true;
        ResetSize();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        bChange = true;
        ResetSize();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
        {
            bChange = true;
            ResetSize();
        }
    }
#endif
}
