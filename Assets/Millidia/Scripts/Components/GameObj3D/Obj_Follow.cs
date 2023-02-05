using UnityEngine;

public class Obj_Follow : MonoBehaviour {
    [SerializeField]
    private Vector3 offset = new Vector3(0, 16.3f, 0);
    [SerializeField]
    private Transform target;

    private void Awake()
    {
        if(!target){
            target=GameObject.FindWithTag("Player").transform;
            offset=transform.position-target.position;
        }
    }
   private void LateUpdate() {
        //transform.position=offset+target.position;
        Debug.Log(target.position);
   }
}