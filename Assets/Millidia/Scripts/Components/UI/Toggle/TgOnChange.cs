using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// Toggle 切换状态
/// </summary>
public class TgOnChange: MonoBehaviour {
    private Toggle tg;

    public GameObject onObj;
    public GameObject offObj;
    private void Awake() {
        tg=transform.GetComponent<Toggle>();
    }
    private void Start() {
        init();
        if(tg){
            tg.onValueChanged.AddListener((isValue)=>{
                onObj.SetActive(isValue);
                offObj.SetActive(!isValue);
            });
        }
    }

    public void init(){
        onObj.SetActive(tg.isOn);
        offObj.SetActive(!tg.isOn);
    }
    private void OnDestroy() {
    }
}