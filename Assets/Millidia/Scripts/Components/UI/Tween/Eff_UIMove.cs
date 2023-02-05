using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
/// <summary>
/// 弹出效果 配合UITweenPoint 预制使用
/// </summary>
public class Eff_UIMove: MonoBehaviour {
    public RectTransform rectStart;
    public RectTransform rectEnd;
    public float duration = 0.8f;

    public GameObject closeImageGb;

    [SerializeField]
    private Ease align = Ease.Linear; //默认匀速运动
    [SerializeField]
    private bool isLoop=false;//是否默认循环播放
    public bool isIn = false; //标准位
    public bool isStart;
    Tweener tweener=null;

    private void Awake() {
        if (!rectStart) {
            rectStart = GetComponent < RectTransform > ();
        }
    }
    private void Start() {
        tweener = rectStart.DOMove(rectEnd.position, duration).SetEase(align); //默认动画播放完成会被销毁
        tweener.SetAutoKill(false); 
        tweener.Pause();
        // tweener.SetLoops(-1);
        if(isLoop){
            MoveLoop();
        }
        if (isStart)
        {
            OnClick();
        }
    }
    public void OnClick() {
        if (isIn == false) {
            rectStart.DOPlayForward(); //前放
            closeImageGb.SetActive(true);
            isIn = true;
        } else {
            //让panel离开屏幕
            rectStart.DOPlayBackwards(); //倒放
            closeImageGb.SetActive(false);
            isIn = false;
        }
    }
    public void MoveOnce() {
        rectStart.DOPlayForward(); //前放
        isIn = true;
    }
    public void MoveLoop() {
        tweener.Play();
        tweener.SetLoops(-1,LoopType.Yoyo);
    }
    private void OnDestroy() {
        rectStart.DOKill();//销毁对象
    }
}