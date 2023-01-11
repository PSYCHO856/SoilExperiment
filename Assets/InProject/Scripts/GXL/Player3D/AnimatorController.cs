/*
 * @Autor: 郭少侠
 * @Data: 2022-08-02 14:00
 * @Description: 动画控制器
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class AnimatorController : MonoBehaviour
{
     public Animator anim;
     public Slider slide;
     private void Start()
     {    
          anim.speed=0;
          slide.onValueChanged.AddListener(AnimationSlide);
     }
     //进度条控制进度
     private void AnimationSlide(float value){
        anim.Play("run",0,value);
     }
     
}
