using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public ISceneState SceneSate;
    private Slider loadingSlider;
    private Text Jindu;

    private void Awake()
    {
        loadingSlider = transform.GetComponentInChildren<Slider>();
        loadingSlider.value = 0;
        Jindu = transform.Find("Loading/Jindu").GetComponent<Text>();
    }

    private void Start()
    {  
        StartCoroutine(LoadScene());    
    }

    IEnumerator LoadScene()
    {

      SceneStateController.Instance.asyncOperation = SceneManager.LoadSceneAsync(SceneSate.sceneName);
        SceneStateController.Instance.asyncOperation.allowSceneActivation = false;  //先不切换场景
        // 生成loading界面
        loadingSlider = transform.GetComponentInChildren<Slider>();
        loadingSlider.value = 0;

        while (SceneStateController.Instance.asyncOperation.progress < 0.89)
        {
            while (SceneStateController.Instance.asyncOperation.progress > loadingSlider.value)
            {
                loadingSlider.value += 0.01f;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
        while (loadingSlider.value < 1)
        {
            loadingSlider.value += 0.01f;
            yield return new WaitForEndOfFrame();
        }

        SceneStateController.Instance.asyncOperation.allowSceneActivation = true;  //切换场景
        yield return new WaitForEndOfFrame();
    }

    private void Update()
    {
        Jindu.text = (loadingSlider.value * 100).ToString("f0") + "%";
    }

}

