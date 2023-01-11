using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
/*
 *UI拖拽移动工具
 * 
 */
namespace App.Tool
{
    /// <summary>
    /// UI拖拽移动工具  
    /// </summary>
    public class UI_DragMoveTool : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler
    {

        [Header("限制在 此区域内拖拽")]
        public RectTransform DragLimitAreaRect;

        public class OnDragDeal : UnityEvent { }
        public OnDragDeal draging { get; set; } = new OnDragDeal();//拖拽中回调

        public class OnBeginDeagDeal : UnityEvent { }
        public OnBeginDeagDeal beginDrag { get; set; } = new OnBeginDeagDeal();//开始拖拽回调

        public class OnEndDragDeal : UnityEvent { }
        public OnEndDragDeal endDrag { get; set; } = new OnEndDragDeal();//拖拽结束回调

        Vector3 offestValue;//这个是 刚开始拖拽时 UI位置和鼠标位置的差值 记录这个值 以保证在拖拽过程中 这个位置始终是恒定的
        public Canvas canvas;
        private void Start()
        {
            
        }

        [Header("拖拽对象")]
        public RectTransform _DragRect;

        //限制UI拖拽范围 如果父对象是Canvas 那么就是 需要算出四个顶点的坐标值 然后限制

         void Awake()
        {
            if (DragLimitAreaRect == null)
            {
                DragLimitAreaRect =canvas.GetComponent<RectTransform>();
            }
            if (_DragRect == null)
                _DragRect = GetComponent<RectTransform>();
            SetDragRange();
        }


        /// <summary>
        /// 开始拖拽时的监听
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //把鼠标坐标 转换为UI坐标系的世界坐标
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_DragRect, eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos))
                {
                    //计算偏移量
                    offestValue = _DragRect.position - globalMousePos;
                    beginDrag?.Invoke();
                }
            }
        }

        [Header("是否 限制")]
        public bool if_Litmit = true;

        public float minX, maxX, minY, maxY;

        /// <summary>
        /// 设置最大 最小坐标 这里考虑到 中枢轴的影响 所以 计算极值时 直接计算其中枢轴的偏移量 来同化对最值的影响
        /// </summary>
        void SetDragRange()
        {
            if (if_Litmit)
            {
                // 最小x坐标 = 容器当前x坐标 - 容器轴心距离左边界的距离 + UI轴心距离左边界的距离
                minX = DragLimitAreaRect.position.x
                    - DragLimitAreaRect.pivot.x * DragLimitAreaRect.rect.width
                    + _DragRect.rect.width * _DragRect.pivot.x;

                // 最大x坐标 = 容器当前x坐标 + 容器轴心距离右边界的距离 - UI轴心距离右边界的距离
                maxX = DragLimitAreaRect.position.x
                    + (1 - DragLimitAreaRect.pivot.x) * DragLimitAreaRect.rect.width
                    - _DragRect.rect.width * (1 - _DragRect.pivot.x);

                // 最小y坐标 = 容器当前y坐标 - 容器轴心距离底边的距离 + UI轴心距离底边的距离
                minY = DragLimitAreaRect.position.y
                    - DragLimitAreaRect.pivot.y * DragLimitAreaRect.rect.height
                    + _DragRect.rect.height * _DragRect.pivot.y;

                // 最大y坐标 = 容器当前x坐标 + 容器轴心距离顶边的距离 - UI轴心距离顶边的距离
                maxY = DragLimitAreaRect.position.y
                    + (1 - DragLimitAreaRect.pivot.y) * DragLimitAreaRect.rect.height
                    - _DragRect.rect.height * (1 - _DragRect.pivot.y);
            }
            //就算不设限 也应该让UI在屏幕上 保留一些
            else
            {
                minX = _DragRect.rect.width * _DragRect.pivot.x;
                //默认 再左移 半个UI大小的身位
                minX -= _DragRect.rect.width / 2;

                maxX = Screen.width - _DragRect.rect.width * (1 - _DragRect.pivot.x);
                maxX += _DragRect.rect.width / 2;
                minY = _DragRect.rect.height * _DragRect.pivot.y;
                minY -= _DragRect.rect.height / 2;
                maxY = Screen.height - _DragRect.rect.height * (1 - _DragRect.pivot.y);
                maxY += _DragRect.rect.height / 2;

            }

        }

        Vector3 DragRangeLimit(Vector3 pos)
        {
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            return pos;
        }

        /// <summary>
        /// 拖动中监听
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // 将屏幕空间上的点转换为位于给定RectTransform平面上的世界空间中的位置
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_DragRect, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
                {
                    // 加上偏移量保证相对位置不变
                    //_rect.position = globalMousePos + offestValue;

                    SetDragRange();
                    //限制拖拽范围
                    _DragRect.position = DragRangeLimit(globalMousePos + offestValue);
                    draging?.Invoke();
                }
            }

        }

        /// <summary>
        /// 缓动移动到一个合适的位置
        /// </summary>
        public void SlowMoveToRightPos()
        {
            _DragRect.DOMove(DragRangeLimit(_DragRect.transform.position), 0.5f).SetEase(Ease.Linear);
        }

        /// <summary>
        /// 结束拖拽监听
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                endDrag?.Invoke();
            }
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            SetDragRange();
        }
    }
}