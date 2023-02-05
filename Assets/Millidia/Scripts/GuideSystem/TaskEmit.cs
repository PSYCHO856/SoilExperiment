using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 任务触发器
/// </summary>
public abstract class TaskEmit : MonoBehaviour
{
    public int index=0;//主任务索引
    public string taskID="20102";
    public abstract  void OnEmit();
}
