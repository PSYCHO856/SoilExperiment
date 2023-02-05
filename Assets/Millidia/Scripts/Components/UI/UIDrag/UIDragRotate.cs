using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace App.Tool
{
    /// <summary>
    /// UI拖拽旋转
    /// </summary>
    public class UIDragRotate : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public class BeginDrag : UnityEvent { }

        public BeginDrag beginDrag { get; set; } = new BeginDrag();

        public class Draging : UnityEvent { }

        public Draging draging { get; set; } = new Draging();

        public class EndDrag : UnityEvent { }

        public EndDrag endDrag { get; set; } = new EndDrag();

        [ContextMenu("快速配置 对角")]
        public void QSetDuijiao()
        {
            List<UIDragRotate> uIDragRotatesList = new List<UIDragRotate>(transform.parent.GetComponentsInChildren<UIDragRotate>(true));
            float _maxDistance = 0;
            for (int i = 0; i < uIDragRotatesList.Count; i++)
            {
                float distance = Mathf.Abs(Vector2.Distance(uIDragRotatesList[i].transform.localPosition, transform.localPosition));
                if (distance >= _maxDistance)
                {
                    _maxDistance = distance;
                    rotatePoint = uIDragRotatesList[i].GetComponent<RectTransform>();
                }
            }
        }

        public Transform tangramTran;
        /// <summary>
        /// 是否限制角度
        /// </summary>
        public bool limitAngle = true;
        /// <summary>
        /// 限制的角度  如果值为90  那么每次的旋转角度以90度为一个周期     
        /// </summary>
        public int fixedAngle = 45;
        public RectTransform rotatePoint;

        float angle;
        /// <summary>
        /// 当前旋转的角度和开始拖动的角度的旋转差值
        /// </summary>
        public float offest;
        Vector3 startV;

        //public float rotateAngle;

        Vector2 lastV;

        public Vector2 min = Vector2.zero;
        public Vector2 max = Vector2.zero;

        public RectTransform dragRangeRect;

        Canvas canvas;

        [Header("回弹系数")]
        public float Coefficient_restitution = 1.0f;

        void Start()
        {
            if (rotatePoint == null)
                rotatePoint = tangramTran as RectTransform;
            if (if_Litmit_RotateArea) //参考这个脚本 UIDragRotateCustom
            {
                canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                if (dragRangeRect == null)
                    dragRangeRect = canvas.GetComponent<RectTransform>();
                Set();
            }
        }
        public void Set()
        {

            float f = canvas.transform.localScale.x;

            Vector2 position = dragRangeRect.position;

            Vector2 size = dragRangeRect.sizeDelta * f;
            float width = size.x / 2;
            float height = size.y / 2;

            float min_x = position.x - width;
            float min_y = position.y - height;

            float max_x = position.x + width;
            float max_y = position.y + height;

            min = new Vector2(min_x, min_y);
            max = new Vector2(max_x, max_y);
        }

        [Header("是否限制 旋转范围")]
        public bool if_Litmit_RotateArea;

        public void OnBeginDrag(PointerEventData eventData)
        {
            startV = tangramTran.localEulerAngles;
            lastV = Input.mousePosition - rotatePoint.position;
            angle = GetAngle(lastV);
            //rotateAngle = 0;
            offest = 0;
            beginDrag?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 nowV = Input.mousePosition - rotatePoint.position;
            //offest = GetAngle(nowV) - angle;
            //Vector3 vector = startV + new Vector3(0, 0, offest);

            float a = GetAngle(lastV, nowV);
            offest += a;
            lastV = nowV;
            //if (rotatePoint == null)
            //{
            //    tangramTran.localEulerAngles = startV - new Vector3(0, 0, offest);
            //}
            //else
            {
                tangramTran.RotateAround(rotatePoint.position, Vector3.back, a);
            }
            draging?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            if (limitAngle)
            {
                float last = offest;
                if (Mathf.Abs(offest) % fixedAngle > fixedAngle / 2)
                {
                    if (offest > 0)
                        offest = (int)offest / fixedAngle * fixedAngle + fixedAngle;
                    else
                        offest = (int)offest / fixedAngle * fixedAngle - fixedAngle;
                }
                else
                {
                    offest = (int)offest / fixedAngle * fixedAngle;
                }
                //if(rotatePoint == null)
                //{
                //    Vector3 vector = startV - new Vector3(0, 0, offest);
                //    tangramTran.localEulerAngles = vector;
                //}
                //else
                {
                    tangramTran.RotateAround(rotatePoint.position, Vector3.back, offest - last);
                }
                //offest = 0;
            }

            if (if_Litmit_RotateArea)
            {
                Vector2 size = tangramTran.GetComponent<RectTransform>().sizeDelta / 2 * GameObject.Find("Canvas").transform.localScale.x;

                Vector2 endVector = tangramTran.position;
                if (endVector.x + size.x > max.x)
                {
                    endVector.x = max.x - size.x * Coefficient_restitution;
                }
                else if (endVector.x - size.x < min.x)
                {
                    endVector.x = min.x + size.x * Coefficient_restitution;
                }

                if (endVector.y + size.y > max.y)
                {
                    endVector.y = max.y - size.y * Coefficient_restitution;
                }
                else if (endVector.y - size.y < min.y)
                {
                    endVector.y = min.y + size.y;
                }
                tangramTran.position = endVector * Coefficient_restitution;
            }
            endDrag?.Invoke();
        }


        /// <summary>
        /// 获取与（0,0,1）的夹角
        /// </summary>
        /// <param name="to"></param>
        float GetAngle(Vector3 to)
        {
            float angle = Vector3.Angle(Vector3.right, to); //求出两向量之间的夹角
            Vector3 normal = Vector3.Cross(Vector3.right, to);//叉乘求出法线向量
            angle *= Mathf.Sign(Vector3.Dot(normal, Vector3.forward));  //求法线向量与物体上方向向量点乘，结果为1或-1，修正旋转方向
            if (angle < 0)
                angle += 360;
            return angle;
        }

        /// <summary>
        /// 获取与（0,0,1）的夹角
        /// </summary>
        /// <param name="to"></param>
        float GetAngle(Vector3 form, Vector3 to)
        {
            float angle = Vector3.Angle(form, to); //求出两向量之间的夹角
            Vector3 normal = Vector3.Cross(form, to);//叉乘求出法线向量
            angle *= Mathf.Sign(Vector3.Dot(normal, Vector3.back));  //求法线向量与物体上方向向量点乘，结果为1或-1，修正旋转方向
                                                                     //if (angle < 0)
                                                                     //    angle += 360;
            return angle;
        }

        private void Reset()
        {
            tangramTran = transform.parent;
        }
    }
}
