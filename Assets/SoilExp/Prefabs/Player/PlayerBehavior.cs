using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// 视角的类型（天空，地面）
/// </summary>
public enum playerType
{
    groundPlayer,
    skyPlayer,
}
/// <summary>
/// 视角行为
/// </summary>
class PlayerBehavior : MonoBehaviour
{
    public bool isPause;  //是否暂停
    public playerType playerTy= playerType.skyPlayer;      //状态（有无重力）
    public float moveSpeed = 2;        //移动速度
    public float moveSpeedMin = 10;
    public float moveSpeedMax = 100;

    public float jumpSpeed = 20;   //弹跳力
    public float gravity = 100;     //重力


    public float lookSpeed = 15f;//视角旋转速度
    // 垂直方向的 镜头转向 (这里给个限度 最大仰角为45°)
    public float m_minimumY = -45f;
    public float m_maximumY = 45f;
    // 水平方向的 镜头转向
    public float m_minimumX = -360f;
    public float m_maximumX = 360f;


    private Camera myCamera;      //摄像机
    public float whellSpeed = 10;    //滚轮速度
    public float camerFieldOfViewMax = 80;//摄像机的fieldOfView最大值
    public float camerFieldOfViewMin = 20;//摄像机的fieldOfView最小值

    private Vector3 moveDirection = Vector3.zero; //移动方向
    private CharacterController playerController; //玩家控制


    float rotationX = 0;
    float rotationY = 0;

    private void Start()
    {
        playerController = transform.GetComponent<CharacterController>();
        myCamera = transform.Find("Eyes").GetComponent<Camera>();
        //playerTy = playerType.skyPlayer;
    }
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (isPause == false)
        {
            //Debug.Log(transform.localEulerAngles.x+"   "+ rotationY);
            switch (playerTy)
            {
                case playerType.groundPlayer:
                    GroundMove();
                    break;
                case playerType.skyPlayer:
                    SkyMove();
                    break;
            }
            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    if (playerTy == playerType.groundPlayer)
            //    {
            //        playerTy = playerType.skyPlayer;
            //    }
            //    else
            //    {
            //        playerTy = playerType.groundPlayer;
            //    }
            //}

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                moveSpeed = moveSpeed + 2;
            }
            if (Input.GetKeyUp(KeyCode.KeypadMinus))
            {
                moveSpeed = moveSpeed - 2;
            }
            moveSpeed = Mathf.Clamp(moveSpeed, moveSpeedMin, moveSpeedMax);



        }
    }

    //public int translationSpeed = 50;
    public void LateUpdate()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    //鼠标在这一帧移动的水平距离
        //    float x = Input.GetAxis("Mouse X");
        //    //鼠标在这一帧移动的垂直距离
        //    float y = Input.GetAxis("Mouse Y");
        //    //moveDirection = transform.TransformDirection(new Vector3(-x, -y, 0));

        //    transform.Translate(new Vector3(-x * translationSpeed * Time.deltaTime, -y * translationSpeed * Time.deltaTime, 0));
        //    //Debug.Log("a " + moveDirection);
        //    //playerController.Move(moveDirection * Time.deltaTime);
        //}
        //var position = transform.position;
        //position = new Vector3(Mathf.Clamp(position.x, 0, 200), Mathf.Clamp(position.y, 0, 200), Mathf.Clamp(position.z, 0, 200));
        //transform.position = position;

    }

    /// <summary>
    /// 在地面上的移动
    /// </summary>
    private void GroundMove()
    {
        VisualMove();
        if (playerController.isGrounded)
        {
            moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            moveDirection *= moveSpeed;
            // 空格键控制跳跃  
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y += jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        playerController.Move(moveDirection * Time.deltaTime);
    }
    /// <summary>
    /// 空中移动
    /// </summary>
    private void SkyMove()
    {
        VisualMove();
        moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        moveDirection *= moveSpeed;
        if (Input.GetKey(KeyCode.Q))
        {
            moveDirection.y -= moveSpeed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            moveDirection.y += moveSpeed;
        }



        playerController.Move(moveDirection * Time.deltaTime);


        

    }



    /// <summary>
    /// 视角的移动
    /// </summary>
    private void VisualMove()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            myCamera.fieldOfView = myCamera.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * whellSpeed;
            myCamera.fieldOfView = Mathf.Clamp(myCamera.fieldOfView, camerFieldOfViewMin, camerFieldOfViewMax);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (transform.localEulerAngles.y > 0)
            {
                rotationX = transform.localEulerAngles.y;
            }
            else
            {
                rotationX = transform.localEulerAngles.y - 360;
            }
            if (transform.localEulerAngles.x>180)
            {
                rotationY = 360-transform.localEulerAngles.x;
            }
            else
            {
                rotationY = -transform.localEulerAngles.x;
            }
        }
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY += Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, m_minimumY, m_maximumY);

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
        }
    }


    /// <summary>
    /// 镜头移动
    /// </summary>
    /// <param name="targetPosition">目标点</param>
    /// <param name="timeTotal">移动总时间</param>   
    public void Move(Transform target, float timeTotal = 2, Action _action = null)
    {
        Pause_Continue(true);
        transform.DOMove(target.position, timeTotal).OnComplete(delegate {
            //开启会话
            Pause_Continue(false);
            _action?.Invoke();
        });

        transform.DORotate(target.eulerAngles, timeTotal);
    }


    /// <summary>
    /// 镜头移动
    /// </summary>
    /// <param name="targetPosition">目标点</param>
    /// <param name="timeTotal">移动总时间</param>   
    public void Move(Vector3 pos, Vector3 angle, float timeTotal = 2, Action _action = null)
    {
        Pause_Continue(true);
        transform.DOMove(pos, timeTotal).OnComplete(delegate {
            //开启会话
            Pause_Continue(false);
            _action?.Invoke();
        });

        transform.DORotate(angle, timeTotal);
    }


    /// <summary>
    /// 暂停-继续
    /// </summary>
    /// <param name="b"></param>
    public void Pause_Continue(bool b)
    {
        isPause = b;
        playerController.enabled = !b;
    }

}


