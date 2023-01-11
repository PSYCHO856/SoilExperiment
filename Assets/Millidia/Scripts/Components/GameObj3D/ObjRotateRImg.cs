using UnityEngine;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// 根据EventListener-控制物品的旋转 缩放
/// </summary>
public class ObjRotateRImg: MonoBehaviour  {
    public EventListener r_event;
    public Transform rectStart;
      // 缩放限制系数
    public float MinScale = 0.5f;
    public float MaxScale = 2.5f;
    public float speed = 6f;
    public float speedScale = 3f;


    Vector3 originPos;
    //鼠标沿水平方向拖拽的增量
    private float X;
    
    private void Awake() {
        if (!rectStart) {
            rectStart = GameObject.Find("ModelsNode").transform;
        }
        originPos=rectStart.localEulerAngles;
    }
    private void OnEnable()
    {
        Clear();
    }
    private void Start()
    {
       r_event.onDrag+=onDrag;
       r_event.onBeginDrag+=onBeginDrag;
       r_event.onEndDrag+=onEndDrag;
       r_event.onScroll+=onScroll;
    }
    void onDrag(PointerEventData p){
        RotateSmooth();
    }
    void onScroll(PointerEventData p){
        ScaleSmooth( p.scrollDelta.y);
    }
    void onBeginDrag(PointerEventData p){
         X = 0f;
    }
    void onEndDrag(PointerEventData p){
    }
    public void Clear(){
       rectStart.localEulerAngles=originPos;
    }

    private void RotateSmooth(){
        X = -Input.GetAxis("Mouse X");
        rectStart.Rotate(new Vector3(0, X, 0) * speed, Space.World);
    }
    //缩放模型
    private void ScaleSmooth(float scrollDeltaY)
    {
        //鼠标滚轮物体缩放

        if (scrollDeltaY>0)
        {
            float localSacleX = rectStart.localScale.x + 0.01f*speedScale;
            float localSacleY = rectStart.localScale.y + 0.01f*speedScale;
            float localSacleZ = rectStart.localScale.z + 0.01f*speedScale;

            rectStart.localScale = new Vector3(localSacleX, localSacleY, localSacleZ);
            if (localSacleX >= MaxScale)
            {
                rectStart.localScale = new Vector3(MaxScale, MaxScale, MaxScale);
            }
        }
        if (scrollDeltaY<0)
        {
            float localSacleX = rectStart.localScale.x - 0.01f*speedScale;
            float localSacleY = rectStart.localScale.y - 0.01f*speedScale;
            float localSacleZ = rectStart.localScale.z - 0.01f*speedScale;

            rectStart.localScale = new Vector3(localSacleX, localSacleY, localSacleZ);
            if (localSacleX <= MinScale)
            {
                rectStart.localScale = new Vector3(MinScale, MinScale, MinScale);
            }
        }
    }
}