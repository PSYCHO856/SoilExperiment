using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
/// <summary>
/// 绑定到要旋转的物体模型上
/// 后期通过拖拽Image的事件拖拽 旋转
/// </summary>
public class SelectRoleRotate: MonoBehaviour {

    //是否被拖拽
    private bool onDrag = false;
    //旋转速度
    public float speed = 6f;
    public Transform target ;
    private bool isOn=false;

    //阻尼速度
    private float zSpeed;
    //鼠标沿水平方向拖拽的增量
    private float X;
    //鼠标沿竖直方向拖拽的增量     
    //private float Y;
    //鼠标移动的距离
    private float mXY;

    private void Start() {
        //获取旋转的对象=-=
        target=GameObject.FindWithTag("RotateNode").transform;
    }

    //接受鼠标按下的事件
    void OnMouseDown() {
        X = 0f;
    }
    //鼠标拖拽时的操作
    void OnMouseDrag() {
        onDrag = true;
        X = -Input.GetAxis("Mouse X");
    }

    public void OnRole(){
        target.eulerAngles=new Vector3(0,90,0);
        isOn=true;
        MUIMgr.Instance.CloseUI(EMUI.MUI_Main);
        Screen.orientation = ScreenOrientation.Portrait;
    }
    public void OfffRole(){
        target.eulerAngles=new Vector3(0,90,0);
        isOn=false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

    }
    //获取阻尼速度 
    float RiSpeed() {
        if (onDrag) {
            zSpeed = speed;
        } else {
            zSpeed = 0;
        }
        return zSpeed;
    }

    void LateUpdate() {
        if(target){
            target.Rotate(new Vector3(0, X, 0) * RiSpeed(), Space.World);
            if (!Input.GetMouseButtonDown(0)) {
                onDrag = false;
            }
        }
    }
}