using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPGameDemo
{
    public class PlayerJump : MonoBehaviour
    {
        [Tooltip(" x时间，y重力系数，z方向力系数，w偏移")]
        public Vector4 arg;
        // 上升的曲线
        public AnimationCurve riseCurve = new AnimationCurve(new Keyframe[] {
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    });
        // 水平方向惯性曲线
        public AnimationCurve directionJumpCurve = new AnimationCurve(new Keyframe[] {
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    });

        Animator animator;
        Rigidbody mRigidRB;
        PlayerMove playerMove;

        int mIsJumpAnimatorHash;
        float mGroundedDelay;       // 延迟检测变量
        bool mIsGrounded;             // 是否在地面上
        Coroutine mJumpCoroutine;       // 跳跃协程

        const string JUMP_STR = "Jump";

        void Start()
        {
            InitComponent();
            mIsJumpAnimatorHash = Animator.StringToHash("IsJump");
        }


        void Update()
        {
            var upAxis = -Physics.gravity.normalized;//up轴向
            mIsGrounded = playerMove.IsGroundFlag;
            if (mIsGrounded && mJumpCoroutine != null)//跳跃打断处理
            {
                StopCoroutine(mJumpCoroutine);
                mRigidRB.useGravity = true;
                mJumpCoroutine = null;
            }

            if (Input.GetButtonDown(JUMP_STR) && mJumpCoroutine == null && mIsGrounded)
            {//执行跳跃
                mJumpCoroutine = StartCoroutine(JumpCoroutine(playerMove.MoveDir, upAxis));
                animator.SetBool(mIsJumpAnimatorHash, true);
            }
            else
            {//落地状态逻辑
                if (mIsGrounded)
                    animator.SetBool(mIsJumpAnimatorHash, false);
            }
            mGroundedDelay -= Time.deltaTime;//延迟变量更新

        }

        IEnumerator JumpCoroutine(Vector3 moveDir, Vector3 upAxis)
        {
            mGroundedDelay = Time.maximumDeltaTime * 2f;//两帧延迟
            mRigidRB.useGravity = false;//暂时关闭重力
            var t = arg.w;//时间插值
            do
            {
                var t_riseCurve = riseCurve.Evaluate(t);//上升力曲线采样
                var t_directionJump = directionJumpCurve.Evaluate(t);//方向力曲线采样
                var gravity = Vector3.Lerp(-upAxis, upAxis, t_riseCurve) * arg.y;
                var forward = Vector3.Lerp(moveDir * arg.z * Time.fixedDeltaTime, Vector3.zero, t_directionJump);
                //获得方向并乘以系数
                mRigidRB.velocity = gravity + forward;//更新速率
                t = Mathf.Clamp01(t + Time.deltaTime * arg.x);//更新插值
                yield return null;
            } while (!mIsGrounded);
            mRigidRB.useGravity = true;//恢复重力
        }

        void InitComponent()
        {
            animator = GetComponent<Animator>();
            mRigidRB = GetComponent<Rigidbody>();
            playerMove = GetComponent<PlayerMove>();
        }
    }
}
