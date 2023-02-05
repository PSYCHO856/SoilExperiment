using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImputFileChangeCursor : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private EventSystem system;                                 //事件系统
    private bool isSelect = false;                              //光标是否在当前输入框标志
    public Direction direction = Direction.vertical;            //垂直切换输入框的光标


    //枚举光标切换的方向
    public enum Direction
    {
        //垂直切换
        vertical = 0,
        //水平切换
        horizontal = 1
    }
    void Start()
    {
        system = EventSystem.current;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isSelect)
        {
            TabDownClick();
        }
    }
    public void TabDownClick()
    {
        Selectable next = null;
        //currentSelectedGameObject  代表点击到的游戏对象
        var current = system.currentSelectedGameObject.GetComponent<Selectable>();

        int mark = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 1 : -1;
        Vector3 dir = direction == Direction.horizontal ? Vector3.left * mark : Vector3.up * mark;
        next = GetNextSelectable(current, dir);

        if (next != null)
        {
            var inputField = next.GetComponent<InputField>();
            if (inputField == null) return;
            StartCoroutine(Wait(next));
        }
    }

    private static Selectable GetNextSelectable(Selectable current, Vector3 dir)
    {
        //通过向量(向量代表方向  垂直还是水平)去查找此对象旁边的可选对象 返回Selectable
        Selectable next = current.FindSelectable(dir);
        if (next == null)
            next = current.FindLoopSelectable(-dir);
        return next;
    }

    IEnumerator Wait(Selectable next)
    {
        yield return new WaitForEndOfFrame();
        //将游戏对象设置为选中 
        system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelect = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelect = false;
    }
}
public static class Develop
{
    //当输入框光标走到了最后一个输入框时   将光标移到第一个输入框去
    public static Selectable FindLoopSelectable(this Selectable current, Vector3 dir)
    {
        Selectable first = current.FindSelectable(dir);
        if (first != null)
        {
            current = first.FindLoopSelectable(dir);
        }
        return current;
    }
}
 
 
