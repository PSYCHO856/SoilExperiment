using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPGameDemo;
using Cinemachine;
namespace Player {
    /// <summary>
    /// 第三人称输入按键-控制人物移动是按
    /// </summary>
    public class PlayerInput3rd: PlayerInput {
        public CinemachineBrain CameraBrain;
        public override void Init(){
            base.Init();
        }
        protected override void Update() {
            base.Update();
            Rotate_Update();
        }
        private void LateUpdate() {
            //待机
            if (cc.isGrounded && ETCInput.GetAxis("Vertical")==0 && ETCInput.GetAxis("Horizontal")==0){
                m_Animator.SetBool("idle",true);
                m_Animator.SetFloat("Run",0);
                m_Animator.SetFloat("Turn",0);
                Debug.Log("待机");
            }
            //前跑
            if (cc.isGrounded&&(ETCInput.GetAxis("Vertical")>0)){
                m_Animator.SetBool("idle",false);
                m_Animator.SetFloat("Run",1);
            }
            //后跑
            if (cc.isGrounded&&(ETCInput.GetAxis("Vertical")<0)){
                m_Animator.SetBool("idle",false);
                m_Animator.SetFloat("Run",-1);
            }
            if (ETCInput.GetAxis("Horizontal")>0){
                m_Animator.SetBool("idle",false);
                m_Animator.SetFloat("Turn",1);
            }
            if ((ETCInput.GetAxis("Horizontal")<0)){
                m_Animator.SetBool("idle",false);
                m_Animator.SetFloat("Turn",-1);
            }
        }
        /// <summary>
        /// 旋转使用按键操作
        /// </summary>
        public void Rotate_Update(){
            transform.eulerAngles=new Vector3( transform.eulerAngles.x,CameraBrain.transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}