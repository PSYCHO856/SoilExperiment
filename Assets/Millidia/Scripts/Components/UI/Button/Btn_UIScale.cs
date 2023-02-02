using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// 配合Button使用
/// </summary>
public class Btn_UIScale: MonoBehaviour,
IPointerEnterHandler,
IPointerExitHandler
{
    public RectTransform rectStart;
    public float toScale=1.2f;
    public float duration = 0.3f;

    [SerializeField]
    private Ease align = Ease.Linear; //默认匀速运动
    [SerializeField]
    private bool isLoop=false;//是否默认循环播放
    public bool isIn = false; //标准位
    public bool isStart;//是否开始播放
    Tweener tweener=null;

    private void Awake() {
        if (!rectStart) {
            rectStart = GetComponent < RectTransform > ();
        }
    }
    private void Start() {
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
            //让panel离开屏幕
            rectStart.DOPlayBackwards(); //倒放
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

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        rectStart.DOPlayForward(); //前放
        isIn = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        rectStart.DOPlayBackwards(); //倒放
        isIn = false;
    }
}