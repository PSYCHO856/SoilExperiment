using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LookAtBase : MonoBehaviour
{
    public bool isOn=true;
    private void Update()
    {
        OnLookAt();
    }

    protected virtual void OnLookAt(){
        if(isOn)
        transform.rotation =  Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
