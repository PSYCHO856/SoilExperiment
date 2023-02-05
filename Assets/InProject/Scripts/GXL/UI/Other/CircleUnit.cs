using System.Net.Mime;
using UnityEngine.UI;
using UnityEngine;
//道具元件
public class CircleUnit : UnitBase
{
   [SerializeField] Text t_sign;
   [SerializeField] AudioUIBind audioUIBind;
   public TaskEmit taskUnit;
   /// <summary>
   /// 初始化音效 文本
   /// </summary>
   /// <param name="guideID"></param>
   public void Init(string guideID){
      audioUIBind.clickSound=$"se_{guideID}";
      taskUnit.taskID=guideID;
      t_sign.text=OnVertical(GuideConfigInfo.Datas[guideID].Describe);
      this.name=$"cicleUnit_{guideID}";
   }
   string OnVertical(string str){
      var Count=str.Length;
      for (int i = 1; i < Count; i++)
      {
         str=str.Insert(2*i-1,"\n");
      }
      return str;
   }
}
