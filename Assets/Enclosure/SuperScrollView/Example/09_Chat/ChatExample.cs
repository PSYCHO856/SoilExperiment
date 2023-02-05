using System;
using SuperScrollView;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 

namespace Example_Chat
{
    [DisallowMultipleComponent]
    public sealed class ChatExample : MonoBehaviour
    {
        [SerializeField] private LoopListView2 m_view = null;
        [SerializeField] private ListItemUI m_leftItem = null;
        [SerializeField] private ListItemUI m_rightItem = null;
        private int m_creatCount = 6;

        public List<ListItemData> m_list = new List<ListItemData>(); //只需要更新链表即可

        #region 示例
        #endregion

        public void Awake()
        {
            m_view.InitListView(m_creatCount, OnUpdate);
        }
        /// <summary>
        /// 被动更新消息
        /// </summary>
        /// <param name="strNick"></param>
        /// <param name="str"></param>
        /// <param name="strId 0=右 1=左"></param>
        public void AddItem(string name, string msg, int RLId)
        {
            CharaData item = new CharaData(RLId, "", name);
            var data = new ListItemData(
                charaData: item,
                message: msg,
                imageSpriteName: "null"
            );
            m_list.Insert(0, data);
            m_view.RefreshAllShownItem();
        }
        /// <summary>
        /// 主动更新消息
        /// </summary>
        /// <param name="list"></param>
        // public void AddItemServer(MessageRecordList list)
        // {
        //     CharaData item = new CharaData(list.SendPlayerId == myInfo.PlayerId ? 0 : 1, "", list.SendNickName);
        //     var data = new ListItemData(
        //         charaData: item,
        //         message: list.Content,
        //         imageSpriteName: "null"
        //     );

        //     m_list.Add(data);
        //     m_view.RefreshAllShownItem();
        // }

        /// 刷新长度
        public void SetListItemCount(int count){
            m_view.SetListItemCount(count);//刷新列表个数
            m_view.RefreshAllShownItem();//刷新列表
        }
        private LoopListViewItem2 OnUpdate(LoopListView2 view, int index)
        {
            if (index < 0 || m_list.Count <= index)
                return null;
            var data = m_list[index];
            var prevData = m_list.ElementAtOrDefault(index + 1);
            var charaData = data.CharaData;
            var itemOriginal = charaData.Id == 0 ? m_rightItem : m_leftItem;
            var itemObj = view.NewListViewItem(itemOriginal.name);
            var itemUI = itemObj.GetComponent<ListItemUI>();
            itemUI.SetDisp(data, prevData);

            return itemObj;
        }
    }
}