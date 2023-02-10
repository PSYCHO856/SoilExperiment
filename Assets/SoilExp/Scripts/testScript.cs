using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class testScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Camera uicamera;
    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = uicamera.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     
        //     // 射线碰撞检测
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         // 检测是否碰撞到UI
        //         Debug.Log(hit.collider.gameObject.name);
        //         if (hit.collider.gameObject.GetComponent<CanvasRenderer>() != null)
        //         {
        //             // 在这里添加代码，当鼠标点击到UI上时执行。
        //             Debug.Log("jiance dao ui!!!");
        //         }
        //     }
        // }
        
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("sssss");
            return;
        }
    }
}
