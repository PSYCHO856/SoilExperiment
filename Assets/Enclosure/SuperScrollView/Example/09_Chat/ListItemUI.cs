using UnityEngine;
using UnityEngine.UI;

namespace Example_Chat
{
	/// <summary>
	/// 设置单个数据条 的信息和设置宽高
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class ListItemUI : MonoBehaviour
	{
		private static readonly CharaData m_dummyCharaData = new CharaData( -1, string.Empty ,string.Empty);

		[SerializeField] private RectTransform		m_rootUI		= null;
		[SerializeField] private Image				m_iconUI		= null;
		[SerializeField] private RectTransform		m_frameRectUI	= null;
		[SerializeField] private Text				m_textUI		= null;
		[SerializeField] private Text				m_textName		= null;

		[SerializeField] private RectTransform		m_textRectUI	= null;
		[SerializeField] private ContentSizeFitter	m_textFitterUI	= null;
		//[SerializeField] private Image				m_imageUI		= null;
		[SerializeField] private RectTransform		m_imageRectUI	= null;
		public float  h_Offset=100;//高差值
		public float  w_Offset=80;//宽差值

		public void SetDisp( ListItemData data, ListItemData prevData )
		{
			var charaData = data.CharaData;
			var isMessage = data.IsMessage;
			var prevCharaData = prevData != null ? prevData.CharaData : m_dummyCharaData;
			var isSameChara = charaData.Id == prevCharaData.Id;//如果当前人物id==将要的ID 头像不打开。

			m_iconUI.gameObject.SetActive( true );//默认打开吧
			//m_iconUI.sprite = Resources.Load<Sprite>( ResPath.ui_head+data.ImageSpriteName );
			// m_arrowUI.gameObject.SetActive( isMessage );//开启箭头
			m_frameRectUI.gameObject.SetActive( isMessage );
			m_textUI.gameObject.SetActive( isMessage );
			m_textUI.text = data.Message;
			m_textName.text=charaData.name;
			m_textFitterUI.SetLayoutVertical();
			// m_imageUI.gameObject.SetActive( !isMessage );
			// m_imageUI.sprite = Resources.Load<Sprite>( data.ImageSpriteName );

			var contentSize	= isMessage
				? m_textRectUI.sizeDelta
				: m_imageRectUI.sizeDelta
			;

			var frameSize = contentSize + new Vector2(w_Offset, 50+h_Offset);
			var color = charaData.Id == 0
				? new Color32( 255, 255, 255, 255 )
				: new Color32( 160, 231, 90, 255 );
			;

			m_frameRectUI.sizeDelta = new Vector2( frameSize.x,frameSize.y-h_Offset) ;//设置字体框大小
			// m_frameUI.color = color;
			// m_arrowUI.color = color;

			var y = Mathf.Max( frameSize.y, 75);
			
			m_rootUI.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, y );//设置聊天框大小

		}

		public void SetDisp(ListItemData data)
        {
            var isMessage = data.IsMessage;

            m_textUI.gameObject.SetActive(isMessage);
            m_textUI.text = data.Message;
            m_textFitterUI.SetLayoutVertical();

			var contentSize = m_textRectUI.sizeDelta;
			var y = Mathf.Max(contentSize.y + 5, 75);
            m_rootUI.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);//设置聊天框大小
        }
	}
}