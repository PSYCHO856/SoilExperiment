using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
/// <summary>
/// 配合UI使用
/// </summary>
public class Eff_UIScale: MonoBehaviour {
    public RectTransform rectStart;
    //的
    public float toScale=1.2f;
    public float duration = 0.3f;
    public bool isStart=false;//是否开始播放
    public bool isEnabelStart=false;//是否激活开始播放1

    public Ease align = Ease.Linear; //默认匀速运动
    public bool isLoop=false;//是否默认循环播放
    private bool isIn = false; //标准位

    Tweener tweener=null;

    private void Awake() {
        if (!rectStart) {
            rectStart = GetComponent < RectTransform > ();
        }
        Init();
    }
  
    private void OnEnable()
    {
        if(isEnabelStart){
            tweener.Restart();
        }
    }
    private void Init() {
        tweener = rectStart.DOScale(Vector3.one*toScale, duration).SetEase(align); //默认动画播放完成会被销毁
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
            isIn = true;
        } else {
            rectStart.DOPlayBackwards(); //倒放
            isIn = false;
        }
    }
    //循环次数-放大缩小动画
    public void LoopPlay(int times=1){
        tweener.Kill();
        rectStart.localScale=Vector3.one;
        tweener = rectStart.DOScale(Vector3.one*toScale, duration).SetLoops(times*2,LoopType.Yoyo);
    }
     //重新播放播放
    public void MoveRestart() {
        tweener.Restart();
    }
    public void MoveLoop() {
        tweener.Play();
        tweener.SetLoops(-1,LoopType.Yoyo);
    }
    private void OnDestroy() {
        rectStart.DOKill();//销毁对象
    }
}