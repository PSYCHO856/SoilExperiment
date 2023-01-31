using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmBtnClick : MonoBehaviour
{
    public void btnClick()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
