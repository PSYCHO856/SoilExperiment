using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class DragGridCom: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public Action < PointerEventData > onBeginDrag;
    public Action < PointerEventData > onDrag;
    public Action < PointerEventData > onEndDrag;
    public void OnBeginDrag(PointerEventData eventData) {
        //获取格子中的物品
        if (transform.childCount > 0) {
            Transform child = transform.GetChild(0);
            eventData.selectedObject = child.gameObject;
            Canvas vanvas = child.GetComponent < Canvas > (); //每个可以拖动的物体需要有canvas 组件 以调节显示层级
            vanvas.sortingOrder = vanvas.sortingOrder + 1; //避免遮挡
        }
        if (onBeginDrag != null)
            onBeginDrag(eventData);
    }


    public void OnDrag(PointerEventData eventData) {
        //  Debug.Log("结束拖动1");
        if (eventData.selectedObject != null) {
            eventData.selectedObject.transform.position = Input.mousePosition; //物体随鼠标移动

        }
        if (onDrag != null)
            onDrag(eventData);
    }


    public void OnEndDrag(PointerEventData eventData) {

        if (onEndDrag != null)
        {
            // Debug.Log("结束拖动");
            onEndDrag(eventData);
        }
        if (eventData.selectedObject != null) {
            //如果拖拽到了外面 或者停留的地方不是grid
            if (eventData.pointerEnter == null || eventData.pointerEnter.tag != "grid") {
                //还原到初始位置
                eventData.selectedObject.transform.SetParent(eventData.pointerDrag.transform);
                //清零本地坐标值
                eventData.selectedObject.transform.localPosition = Vector2.zero;
                return;
            }

            //如果当前停留的地方是格子
            if (eventData.pointerEnter.tag == "grid") {
                //判断当前格子是否已经存在物体
                if (eventData.pointerEnter.transform.childCount == 0) {
                    //设置拖动物体的父组件为当前格子
                    eventData.selectedObject.transform.SetParent(eventData.pointerEnter.transform);
                    Debug.Log(eventData.pointerEnter.name);
                } else //当前格子已经存放物体了
                {
                    //获取当前格子的物体
                    Transform item = eventData.pointerEnter.transform.GetChild(0);
                    //把当前格子的物体放到拖动物体的格子中
                    item.SetParent(eventData.pointerDrag.transform);
                    //清零本地坐标值
                    item.localPosition = Vector2.zero;
                    //设置拖动物体的父组件为当前格子
                    eventData.selectedObject.transform.SetParent(eventData.pointerEnter.transform);
                    Debug.Log(eventData.pointerEnter.name);

                }

                //清零理拖动物体的本地坐标值
                eventData.selectedObject.transform.localPosition = Vector2.zero;
                //还原vanvas.sortingOrder值
                Canvas vanvas = eventData.selectedObject.GetComponent < Canvas > ();
                vanvas.sortingOrder = vanvas.sortingOrder - 1;
            }
        }
    }


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}