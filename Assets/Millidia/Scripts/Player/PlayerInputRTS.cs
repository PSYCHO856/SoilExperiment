using System;
using UnityEngine;
using HedgehogTeam.EasyTouch;

namespace Player {
    /// <summary>
    /// RTS玩家基类
    /// </summary>
    public class PlayerInputRTS: MonoBehaviour {

        //移动速度
        public float moveSpeed = 1.0f;
        public float rotateSpeed = 12f;
        float R_x =0;
        protected Vector3 velocity;

        private void Start() {

            Init();
        }
        public virtual void Init() {}

        protected virtual void Update() {

           OnRoate();
           OnETC();
        }

        protected virtual void OnRoate(){
            if(Input.GetMouseButton(1)){
                R_x -= Input.GetAxis("Mouse X") * rotateSpeed*Time.deltaTime*10;
                //欧拉角转化为四元数
                Quaternion rotation = Quaternion.Euler(0, R_x, 0);
                transform.rotation = rotation;
            }
        }

        private void OnETC() {
            Gesture current = EasyTouch.current;
            if (current == null) {
                return;
            }
            // Swipe 拖动手势
            if (current.type == EasyTouch.EvtType.On_Swipe && current.touchCount == 1) {
                transform.Translate(moveSpeed * Vector3.left * current.deltaPosition.x / Screen.width);
                transform.Translate(moveSpeed * Vector3.back * current.deltaPosition.y / Screen.height);
                Debug.Log("Swipe");
            }
             // Twist
            // if (current.type == EasyTouch.EvtType.On_Twist ){
            //     transform.Rotate( Vector3.up * current.twistAngle);
            //     Debug.Log("On_Twist");
            // }
        }

    }

}