using System;
using UnityEngine;

namespace Comp.GameObj3D
{
    /// <summary>
    /// 3d画线自定义摆放 -Start-End
    /// </summary>
    public class LineTipText:MonoBehaviour
    {
       public Transform startP;
       public Transform endP;
       public LineRenderer lineR;
       [SerializeField]
       public EDir eDir;
       public bool isLoop=false;
       protected virtual void Update() {
            if(isLoop){
                lineR.SetPosition(0,startP.position);
                lineR.SetPosition(1,endP.position);
            }
           
       }
       protected virtual void Start() {
            Init();
       }
       public virtual void Init(){
            if(!startP){
                startP=this.transform;
            }
            lineR.positionCount=2;
       }
    }

    public enum EDir{
        DirectX=0,
        DirectY,
        DirectZ
    }
}
