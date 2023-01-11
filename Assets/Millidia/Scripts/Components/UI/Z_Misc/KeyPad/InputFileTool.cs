using App.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFileTool:MonoBehaviour,ISelectHandler, IDeselectHandler
{
    public InputField inputField;
    public class OnSelectEvent : UnityEvent
    {

    }
    public void OnSelect(BaseEventData eventData)
    {
        //MinimizeTool.Instance.OpenKeyBoard();
        KeyPadTool.Instance.Show(inputField);
        KeyPadTool.Instance.SetPostion(transform.position+new Vector3(-100,-420));
    }
    private void OnDisable()
    {
        KeyPadTool.Instance.DestroyKeyPad();
    }
    public void OnDeselect(BaseEventData eventData)
    {
       // MinimizeTool.Instance.CloseKeyBoard();
    }
}
