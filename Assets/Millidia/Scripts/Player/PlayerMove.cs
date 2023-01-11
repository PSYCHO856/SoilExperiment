using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPGameDemo
{
    public class PlayerMove : MonoBehaviour
    {

        public float speed = 17f;//移动速度
        public float rotSpeed = 17f;//旋转速度
        public Transform[] groundPoints;//地面检测点
        public LayerMask groundLayerMask = ~0;//地面LayerMask
        public LayerMask wallLayerMask = ~0;//墙壁LayerMask
        public bool IsGroundFlag { get; private set; }        // 是否在地面上
        public bool IsMoving { get; private set; }
        public Vector3 MoveDir { get; private set; }

        Animator animator;
        Rigidbody selfRigidbody;//自身刚体组件
        Camera mainCamera;

        int mIsMoveAnimatorHash;//移动Animator变量哈希

        const float INPUT_EPS = 0.2f;           // 输入最小值
        const float GROUND_RAYCAST_LEN = 0.2f;//地面检测射线长度
        const float HEIGHT = 1.2f;//玩家高度估算值
        const float OBLIQUE_P0 = 0.3f, OBLIQUE_P1 = 0.6f;//斜方向射线偏移
        const float RAYCAST_LEN = 0.37f;//射线长度
        const float DOT_RANGE = 0.86f;//点乘范围约束
        const float ANGLE_STEP = 30f;//检测角度间距
        const float CLIFF_RAYCAST_LEN = 0.4f;//悬崖地面检测射线长度

        void Start()
        {
            InitComponent();
            mIsMoveAnimatorHash = Animator.StringToHash("IsMove");
        }

        void Update()
        {

            var h = Input.GetAxis("Horizontal");//横向轴的值
            var v = Input.GetAxis("Vertical");//纵向轴的值
            var inputDir = new Vector3(h, 0f, v);//输入向量
            var upAxis = -Physics.gravity.normalized;//up轴向
            var moveDir = CameraDirProcess(inputDir, upAxis);//相机输入方向修正
            MoveDir = moveDir;
            if (inputDir.magnitude > INPUT_EPS)//是否有输入方向
            {
                var raycastHit = default(RaycastHit);
                var groundNormal = Vector3.zero;
                IsGroundFlag = GroundProcess(ref raycastHit, ref moveDir, out groundNormal, upAxis);//地面检测

                if (IsGroundFlag)
                {
                    var cacheMoveDir = moveDir;
                    var wallFlag = WallProcess(ref raycastHit, ref moveDir, groundNormal, upAxis);//墙壁检测
                    var cliffFlag = false;
                    if (!wallFlag)
                        cliffFlag = CliffProcess(ref raycastHit, ref moveDir, groundNormal, upAxis);//悬崖检测
                    if (!cliffFlag)
                        selfRigidbody.velocity = moveDir * speed * Time.fixedDeltaTime;//更新位置
                    UpdateGroundDetectPoints();//打乱地面检测点顺序
                    RotateProcess(cacheMoveDir, upAxis);//更新旋转
                }
                animator.SetBool(mIsMoveAnimatorHash, true);//更新Animator变量
                IsMoving = true;
            }
            else//没有移动
            {
                MoveDir = Vector3.zero;
                animator.SetBool(mIsMoveAnimatorHash, false);//更新Animator变量
                IsMoving = false;
            }
        }

        public void FootR()
        {

        }

        public void FootL()
        {

        }

        void InitComponent()
        {
            animator = GetComponent<Animator>();
            selfRigidbody = GetComponent<Rigidbody>();
            mainCamera = Camera.main;
        }
        Vector3 CameraDirProcess(Vector3 inputDirection, Vector3 upAxis)
        {
            // 由相机y轴旋转到upaxis的一个旋转四元数
            var quat = Quaternion.FromToRotation(mainCamera.transform.up, upAxis);
            // 作用：四元数和向量相乘表示这个向量按照这个四元数进行旋转之后得到的新的向量
            // 复合旋转就是四元数依次相乘，最后乘以向量
            var cameraForwardDir = quat * mainCamera.transform.forward;//转换forward方向
            var moveDir = Quaternion.LookRotation(cameraForwardDir, upAxis) * inputDirection.normalized;//转换输入向量方向
            return moveDir;
        }

        bool GroundProcess(ref RaycastHit raycastHit, ref Vector3 moveDirection, out Vector3 groundNormal, Vector3 upAxis)
        {

            var result = false;
            for (int i = 0; i < groundPoints.Length; i++)
            {
                var tempRaycastHit = default(RaycastHit);
                if (Physics.Raycast(new Ray(groundPoints[i].position, -upAxis), out tempRaycastHit, GROUND_RAYCAST_LEN, groundLayerMask))//投射地面射线
                {
                    if (raycastHit.transform == null || Vector3.Distance(transform.position, tempRaycastHit.point) < Vector3.Distance(transform.position, raycastHit.point))
                        raycastHit = tempRaycastHit;//选取最近的地面点
                    result = true;
                    break;
                }
            }
            groundNormal = raycastHit.normal;//返回地面法线
            var upQuat = Quaternion.FromToRotation(upAxis, groundNormal);
            moveDirection = upQuat * moveDirection;//根据地面法线修正移动位置
            return result;
        }

        bool WallProcess(ref RaycastHit raycastHit, ref Vector3 moveDir, Vector3 groundNormal, Vector3 upAxis)
        {
            var result = false;
            var ray = new Ray(transform.position + upAxis * HEIGHT, moveDir);
            for (float angle = -90f; angle <= 90f; angle += ANGLE_STEP)//180度内每隔一定角度进行射线检测
            {
                var quat = Quaternion.AngleAxis(angle, upAxis);//得到当前角度
                ray = new Ray(transform.position, quat * moveDir);
                var p0 = ray.origin + ray.direction * OBLIQUE_P0;
                var p1 = ray.origin + upAxis * HEIGHT + ray.direction * OBLIQUE_P1;
                if (Physics.Linecast(p0, p1, out raycastHit, wallLayerMask))//是否碰到墙壁
                {
                    var newRay = new Ray(Vector3.Project(raycastHit.point, upAxis) + Vector3.ProjectOnPlane(ray.origin, upAxis), ray.direction);
                    if (Physics.Raycast(newRay, out raycastHit, RAYCAST_LEN, wallLayerMask))//重新得到射线位置并投射
                    {
                        if (Vector3.Dot(moveDir, -raycastHit.normal) < DOT_RANGE)//点乘约束
                        {
                            var cross = Vector3.Cross(raycastHit.normal, upAxis).normalized;
                            var cross2 = -cross;
                            if (Vector3.Dot(cross, moveDir) > Vector3.Dot(cross2, moveDir))//获得最接近方向
                                moveDir = cross;
                            else
                                moveDir = cross2;
                            break;//若已确定修正方向则跳出循环
                        }
                    }
                    result = true;//确定碰到了墙壁
                }
            }
            return result;
        }

        bool CliffProcess(ref RaycastHit raycastHit, ref Vector3 moveDirection, Vector3 groundNormal, Vector3 upAxis)
        {
            var result = false;
            for (int i = 0; i < groundPoints.Length; i++)//遍历地面检测点
            {
                var relative = groundPoints[i].position - transform.position;//取相对位置
                var quat = Quaternion.FromToRotation(upAxis, groundNormal);//映射到地面法线方向四元数
                var newPoint = transform.position + moveDirection + quat * relative;
                var ray = new Ray(newPoint, -upAxis);//取移动后的位置投射射线
                if (!Physics.Raycast(ray, out raycastHit, CLIFF_RAYCAST_LEN, groundLayerMask))//只要有一个未检测到地面则为悬崖
                {
                    result = true;//返回true
                    break;
                }
            }
            return result;
        }

        void RotateProcess(Vector3 moveDirection, Vector3 upAxis)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, upAxis);//投影到up平面上
            var playerLookAtQuat = Quaternion.LookRotation(moveDirection, upAxis);//得到移动方向代表的旋转
            transform.rotation = Quaternion.Lerp(transform.rotation, playerLookAtQuat, rotSpeed * Time.deltaTime);//更新插值
        }

        void UpdateGroundDetectPoints()
        {
            var groupPointsIndex_n = Random.Range(0, groundPoints.Length);//随机一个索引
            var temp = groundPoints[groupPointsIndex_n];
            groundPoints[groupPointsIndex_n] = groundPoints[groundPoints.Length - 1];
            groundPoints[groundPoints.Length - 1] = temp;//交换地面检测点，防止每次顺序都一样。
        }
    }
}
