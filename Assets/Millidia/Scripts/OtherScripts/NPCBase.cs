using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class NPCBase : MonoBehaviour
{
    protected bool isEnter;
    protected Collider target;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OnPlayerEnter(other);
           
            isEnter = true;
            target = other;
        }
    }

    protected abstract void OnPlayerEnter(Collider other);
 
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnPlayerExit(other);
            isEnter = false;
            target = null;
        }
    }

    protected abstract void OnPlayerExit(Collider other);

    private void Update()
    {
        if (isEnter)
        {
            OnPlayerUpdate();
        }
    }

    protected abstract void OnPlayerUpdate();
    
}
