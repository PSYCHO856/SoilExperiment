using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelDrage : MonoBehaviour
{
    private Camera cam;//发射射线的摄像机
    private GameObject go;//射线碰撞的物体
    public static string btnName;//射线碰撞物体的名字
    private Vector3 screenSpace;
    private Vector3 offset;
    private bool isDrage = false;


    // 缩放限制系数
    public float MinScale = 1;
    public float MaxScale = 5;




    void Start()
    {
        cam = Camera.main;
        Debug.Log(transform.name);
    }


    void Update()
    {

        //整体初始位置 
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //从摄像机发出到点击坐标的射线
        RaycastHit hitInfo;

        if (isDrage == false)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                //划出射线，只有在scene视图中才能看到
                Debug.DrawLine(ray.origin, hitInfo.point);
                go = hitInfo.collider.gameObject;
                //print(btnName);
                screenSpace = cam.WorldToScreenPoint(go.transform.position);
                offset = go.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
                //物体的名字  
                btnName = go.name;
                //组件的名字

            }
            else
            {
                btnName = null;
            }

        }

        if (RotateScale.isChuPing)
        {


            if (Input.touchCount == 0)
            {
                btnName = null;
            }

            if (Input.touchCount == 1)
            {

                if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary)
                {
                    Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
                    Vector3 currentPosition = cam.ScreenToWorldPoint(currentScreenSpace) + offset;

                    if (btnName != null)
                    {
                        go.transform.position = currentPosition;
                    }


                    isDrage = true;
                }
                else
                {
                    isDrage = false;
                }

            }
            else
            {
                isDrage = false;
            }



        }
        else
        {




            if (Input.GetMouseButton(0))
            {


                Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
                Vector3 currentPosition = cam.ScreenToWorldPoint(currentScreenSpace) + offset;

                if (btnName != null)
                {
                    go.transform.position = currentPosition;
                }


                isDrage = true;


            }
            else
            {
                isDrage = false;
            }



            //    if (Input.GetKey(KeyCode.UpArrow))
            //    {
            //        float localSacleX = this.gameObject.transform.localScale.x + 0.01f;
            //        float localSacleY = this.gameObject.transform.localScale.y + 0.01f;
            //        float localSacleZ = this.gameObject.transform.localScale.z + 0.01f;

            //        this.gameObject.transform.localScale = new Vector3(localSacleX, localSacleY, localSacleZ);
            //        if (localSacleX >= MaxScale)
            //        {
            //            this.gameObject.transform.localScale = new Vector3(MaxScale, MaxScale, MaxScale);
            //        }
            //    }
            //    if (Input.GetKey(KeyCode.DownArrow))
            //    {
            //        float localSacleX = this.gameObject.transform.localScale.x - 0.01f;
            //        float localSacleY = this.gameObject.transform.localScale.y - 0.01f;
            //        float localSacleZ = this.gameObject.transform.localScale.z - 0.01f;

            //        this.gameObject.transform.localScale = new Vector3(localSacleX, localSacleY, localSacleZ);
            //        if (localSacleX <= MinScale)
            //        {
            //            this.gameObject.transform.localScale = new Vector3(MinScale, MinScale, MinScale);
            //        }
            //    }



            //    if (Input.GetKey(KeyCode.LeftArrow))
            //    {
            //        this.gameObject.transform.position += new Vector3(-0.01f, 0, 0);
            //    }

            //    if (Input.GetKey(KeyCode.RightArrow))
            //    {
            //        this.gameObject.transform.position += new Vector3(0.01f, 0, 0);
            //    }





            }




        }



}
