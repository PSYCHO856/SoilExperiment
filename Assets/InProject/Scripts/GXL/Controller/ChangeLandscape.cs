using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeLandscape: MonoBehaviour {
    public List<Button> btns=new List<Button>();

   
    private void Start() {
        if (btns.Count <= 0)
        {
            Debug.LogError("btns为0");
            return;
        }
        
        //btns[0].onClick.AddListener(OnLandscape);
        //btns[1].onClick.AddListener(OnPortrait);
    }
    public void OnLandscape() {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    public void OnPortrait() {
        Screen.orientation = ScreenOrientation.Portrait;
    }
}