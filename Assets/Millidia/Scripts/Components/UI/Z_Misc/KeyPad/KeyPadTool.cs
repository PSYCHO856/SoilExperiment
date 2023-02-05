using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Tool
{
    public class KeyPadTool : MonoBehaviour
    {
        public class SureClick : UnityEvent { }
        public SureClick sureClick { get; set; } = new SureClick();
        public static KeyPadTool keyPadTool;
        public CanvasGroup canvasGroup;
       public  static KeyPadTool Instance
        {
            get
            {
                
                if(keyPadTool==null)
                {
                    GameObject gameObject=Resources.Load<GameObject>("GUIs/Tools/KeyPad");
                    GameObject obj=Instantiate(gameObject, GameObject.Find("Canvas").transform);
                    keyPadTool = obj.GetComponent<KeyPadTool>();
                }
                return keyPadTool;
            }
        }
        [SerializeField]
        KeyCaculate caculate;

        [SerializeField]
        Text MyInput;
        [SerializeField]
        Text MyResult;
        InputField inputField;
        public void Show(InputField tEXInput)
        {
            gameObject.transform.SetAsLastSibling();
            InputTextAll = tEXInput.text;
            ShowKeyPad();
        }
      
        public void SetPostion(Vector3 vector3)
        {

            gameObject.transform.position = vector3;
        }
        public void DestroyKeyPad()
        {
            Destroy(gameObject);
        }
        public void ShowKeyPad()
        {
            gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.3f);
        }
        /// <summary>
        /// 输入框的文本集合
        /// </summary>
        string InputTextAll = "";
        /// <summary>
        /// 当前输入的文本
        /// </summary>
        string currentInputText = "";
        public void SetText(string text)
        {
            currentInputText = text;
            InputTextAll += text;
            setInputText();
        }
        public void Revoke()
        {
            if (InputTextAll.Length > 0)
            {
                InputTextAll=InputTextAll.Remove(InputTextAll.Length - 1);
                setInputText();
            }
        }
        void setInputText()
        {
            if(MyInput!=null){
                MyInput.text = InputTextAll;
            }
        //    if(inputField!=null)
        //         inputField.text = InputTextAll;
        }
        public void Sure()
        {
            sureClick?.Invoke();
            GetResult();
        }
        public void Clear(){
            InputTextAll=string.Empty;
            setInputText();
        }
        public void Close(){
            canvasGroup.DOFade(0, 0.3f).OnComplete(()=> {
            gameObject.SetActive(false);
            });
        }
        private void GetResult(){
            MyResult.text=""+caculate.GetFloatNum(InputTextAll);
        }
    }
}