using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapFollowTarget : MonoBehaviour
{
        public Transform target;
        public Transform originCam;
        public RectTransform arrow;
        public List<Button> btns=new List<Button>();
        private Vector3 offset = new Vector3(0, 16.3f, 0);

        private void Awake()
        {
            // offset=originCam.transform.position-target.position;
            // Debug.Log(offset);
        }
        private void Start()
        {
            Camera mapcam=originCam.GetComponent<Camera>();
            btns[0].onClick.AddListener(()=>{
                mapcam.orthographicSize+=1f;
                if(mapcam.orthographicSize>20){
                    mapcam.orthographicSize=20;
                }
            });
            btns[1].onClick.AddListener(()=>{
                mapcam.orthographicSize-=1f;
                if(mapcam.orthographicSize<5){
                    mapcam.orthographicSize=5;
                }
            });
        }
        private void LateUpdate()
        {
            originCam.transform.position = target.position + offset;
            arrow.localEulerAngles=-Vector3.forward*target.localEulerAngles.y;
        }
}
