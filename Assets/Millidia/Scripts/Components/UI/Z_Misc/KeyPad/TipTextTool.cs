using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Comp.UI
{
    public class TipTextTool : MonoBehaviour
    {
        public class SureClick : UnityEvent { }
        public SureClick sureClick { get; set; } = new SureClick();
        public static TipTextTool keyPadTool;
        public CanvasGroup canvasGroup;
       public  static TipTextTool Instance
        {
            get
            {
                
                if(keyPadTool==null)
                {
                    GameObject gameObject=Resources.Load<GameObject>("GUIs/Tool/TipTextTool");
                    GameObject obj=Instantiate(gameObject, GameObject.Find("Canvas").transform);
                    keyPadTool = obj.GetComponent<TipTextTool>();
                }
                return keyPadTool;
            }
        }
        /// <summary>
        /// 设置坐标位置
        /// </summary>
        /// <param name="pos"></param>
        public void Open(Vector3 pos,string name)
        {
            canvasGroup.alpha=1;
            gameObject.SetActive(true);
            transform.position=pos;
            transform.GetComponentInChildren<Text>().text=name;
        }
       
        public void Close(){
            gameObject.SetActive(false);
            canvasGroup.DOFade(0, 0.3f);
        }
    }
}