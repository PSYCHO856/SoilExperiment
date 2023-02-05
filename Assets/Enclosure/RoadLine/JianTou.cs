using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class JianTou : MonoBehaviour {

    Material mat;
    public float speed=1;
    public Transform nodeP;
    float Index = 0;
    private void Start()
    {
        mat = this.GetComponent<MeshRenderer>().material;
        mat.mainTextureScale=new Vector2(nodeP.localScale.x,1);
        UpdateFun();
        // UtilityTool.Instance.WaitForSecond(0.1f,UpdateFun);
    }

    private void UpdateFun()
    {
        t=transform.DOMoveY(transform.position.y,speed).SetLoops(-1).OnComplete(()=>{
            Debug.Log("22");
        });
        Index -= 1*speed;
        if (Index>=1)
        {
            Index = 0;
        }
        mat.mainTextureOffset=new Vector2(Index,0);
    }
    
    Tweener t;
    private void OnDestroy()
    {
        
    }
}
