using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 /// <summary>
 /// 视频播放器-滑动条
 /// </summary>
namespace UI
{
    //挂在Slider组件上
    public class SliderEvent : MonoBehaviour,IEndDragHandler,IBeginDragHandler
    {   
        [Header("视频播放脚本")]
        public PlayVideo toPlayVideo;
        
        private Slider _slider;
        private float _videoTime; //视频时长
        private bool isDrag;
 
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _videoTime = toPlayVideo.GetTime();
        }
 
        private void Start()
        {
            //将滚动条的最大值设置成为视频时长；（也可以保持百分比的方式）
        }
        // Update is called once per frame
        void Update()
        {
            if (!isDrag)
            {
                //滚动条跟着视频播放滑动；（保持百分比方式就不要 *_videoTime）
                var ratio= (float)(toPlayVideo.videoPlayer.frame) /(toPlayVideo.videoPlayer.frameCount);
                 _slider.value=ratio;
                //  Debug.Log("gxl"+ratio+"/"+toPlayVideo.videoPlayer.frameCount);
            }
        }
        
        
        
        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;
            toPlayVideo.SetTime(_slider.value);
        }
 
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDrag = true;
            toPlayVideo.Pause();
            
        }
    }
}