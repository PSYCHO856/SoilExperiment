using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScale : MonoBehaviour
{


    // 缩放限制系数
    public float MinScale = 0.5f;
    public float MaxScale = 2.5f;

    public static bool isChuPing = false;//是否触屏


    private float axisX, axisY;
    private Vector3 nowPos, mousePos, latePos;

    //初始化游戏信息设置
    void Start()
    {

    }

    void Update()
    {


        RotateSmooth();
        ScaleSmooth();
      
    }

    private void RotateSmooth()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
        }

        //物体旋转
        if (Input.GetMouseButton(0))
        {

            nowPos = Input.mousePosition;

            if (nowPos != latePos)
            {
                axisX = -(nowPos.x - mousePos.x) * Time.deltaTime / 5;
                axisY = (nowPos.y - mousePos.y) * Time.deltaTime / 5;
            }
            else
            {
                axisX = 0;
                axisY = 0;
            }

        }
        else
        {
            axisX = 0;
            axisY = 0;
        }
        this.gameObject.transform.Rotate(new Vector3(axisY, axisX, 0), Space.World);
        latePos = nowPos;

    }

    private void ScaleSmooth()
    {
        //鼠标滚轮物体缩放

        if (Input.GetKey(KeyCode.UpArrow))
        {
            float localSacleX = this.gameObject.transform.localScale.x + 0.01f;
            float localSacleY = this.gameObject.transform.localScale.y + 0.01f;
            float localSacleZ = this.gameObject.transform.localScale.z + 0.01f;

            this.gameObject.transform.localScale = new Vector3(localSacleX, localSacleY, localSacleZ);
            if (localSacleX >= MaxScale)
            {
                this.gameObject.transform.localScale = new Vector3(MaxScale, MaxScale, MaxScale);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            float localSacleX = this.gameObject.transform.localScale.x - 0.01f;
            float localSacleY = this.gameObject.transform.localScale.y - 0.01f;
            float localSacleZ = this.gameObject.transform.localScale.z - 0.01f;

            this.gameObject.transform.localScale = new Vector3(localSacleX, localSacleY, localSacleZ);
            if (localSacleX <= MinScale)
            {
                this.gameObject.transform.localScale = new Vector3(MinScale, MinScale, MinScale);
            }
        }
    }
}
