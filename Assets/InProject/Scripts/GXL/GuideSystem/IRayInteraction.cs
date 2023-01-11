using System;
using UnityEngine;
/// <summary>
/// 射线交互接口
/// </summary>
public interface IRayInteraction 
{
    void OnHover(RaycastHit hitInfo);
}
