
using System;
using UnityEngine;
namespace Player 
{
    /// <summary>
    /// 玩家基类
    /// </summary>
    public abstract class PlayerInput : MonoBehaviour {
	    public Animator m_Animator;
	    protected CharacterController cc;

        //移动速度
        public float moveSpeed = 8.0f;
        //重力
        public float GRAVITY = -9.8f;

        protected Vector3 velocity;

        private void Start() {
            if(!m_Animator){
                m_Animator = GetComponent<Animator>();
            }
            Init();
        }
        public virtual void  Init(){
            ETCInput.SetAxisSpeed("Horizontal",moveSpeed);
            ETCInput.SetAxisSpeed("Vertical",moveSpeed);
        }

        protected virtual void Update() {
            OnGravity();
        }
        /// <summary>
        /// 计算重力
        /// </summary>
        protected virtual void OnGravity(){
            velocity=GRAVITY*Vector3.up;
            cc= GetComponentInChildren<CharacterController>();
            cc.Move(velocity*Time.deltaTime);
        }
    }
    
}
