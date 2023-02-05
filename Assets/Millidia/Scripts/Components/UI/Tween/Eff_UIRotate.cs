using System.Net.Sockets;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 旋转效果 
/// </summary>
public class Eff_UIRotate: MonoBehaviour {
        public RectTransform rectStart;
        public RectTransform rectEnd;
        public float duration=0.8f;
        
        [SerializeField]
        public Ease align = Ease.Linear;//默认匀速运动
        private bool isIn = false; //标准位

        private void Awake() {
            if (!rectStart) {
                rectStart = GetComponent < RectTransform > ();
            }
        }
        private void Start() {
            Tweener tweener = rectStart.DOMove (rectEnd.position, duration).SetEase(align); //默认动画播放完成会被销毁
            //Tweener对象保存这个动画的信息 每次调用do类型的方法都会创建一个tweener对象，这个对象是dotween来管理
            tweener.SetAutoKill(false); // 把autokill 自动销毁设置为false
            tweener.Pause(); //暂停动画,使其一开始不播放
        }
        public void OnClick() {
            if (isIn == false) {
                rectStart.DOPlayForward(); //前放
                isIn = true;
            } else {
                //让panel离开屏幕
                rectStart.DOPlayBackwards(); //倒放
                isIn = false;
            }
        }
        public void MoveOnce() {
             rectStart.DOPlayForward(); //前放
             isIn = true;
        }
        private void OnDestroy() {
        }
}