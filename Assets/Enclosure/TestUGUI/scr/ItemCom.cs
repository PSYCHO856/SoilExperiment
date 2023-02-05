using UnityEngine;
public class ItemCom: MonoBehaviour {
    public RectTransform target = null;
    public RectTransform origin = null;
    public int itemStyle = -1; //物品分类

    public bool isUp = false;//是否装备了
    /// <summary>
    /// 0 返回 1装备
    /// </summary>
    /// <param name="i"></param>
    public void ResetPos(int i) {
        if (i == 0) {
            transform.SetParent(origin.transform);
            isUp=false;
        } else {
            transform.SetParent(target.transform);
            isUp=true;
        }
        transform.localPosition = Vector2.zero;
    }
}