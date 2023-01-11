using System;
using UnityEngine.UI;
using UnityEngine;
using Comp.UI;
//3d道具插槽1
public class ToolSlotUnit: UnitBase {
   public bool isOn = false; //是否进入开启任务
   Transform otherT = null;
   public string taskBigID = "260000";
   public string signName = "";
   private void Start() {
      if (!String.IsNullOrEmpty(signName)) {
         signName = StringEx.SplitToIndex(signName, "_", 1);
      }
   }
   private void OnTriggerEnter(Collider other) {
      UnitBase unitBase;
      if (other.TryGetComponent < UnitBase > (out unitBase)) {
         if (unitBase.UID.Equals(UID)) {
            otherT = other.transform;
            if (isOn) {
               MessageCenter.Instance.BoradCastMessage(EMsg.GuideBig_Emit, taskBigID);
               transform.position = otherT.position;
               otherT.gameObject.SetActive(false); //关闭其他提示框
               Debug.Log("关闭其他" + otherT.name);
               this.GetComponent < BoxCollider > ().enabled = false; //关闭当前物体碰撞器
               transform.parent = transform.parent.parent;
            }
         }
      }

   }
   private void OnTriggerExit(Collider other) {
      UnitBase unitBase;
      if (other.TryGetComponent < UnitBase > (out unitBase)) {
         if (unitBase.UID.Equals(UID)) {
            otherT = null;
         }
      }
   }

   private void OnMouseDrag() {
      if (!String.IsNullOrEmpty(signName)) {
         TipTextTool.Instance.Open(Input.mousePosition, signName);
      }
   }

   private void OnMouseUp() {
      Debug.Log("OnMouseUp");
      if (otherT != null) {
         Debug.Log($"进入插槽-{UID}");
         MessageCenter.Instance.BoradCastMessage(EMsg.GuideBig_Emit, taskBigID);
         transform.position = otherT.position;
         otherT.gameObject.SetActive(false); //关闭其他提示框
         this.GetComponent < BoxCollider > ().enabled = false; //关闭当前物体碰撞器
         transform.parent = transform.parent.parent;
      }
      TipTextTool.Instance.Close();
   }
}