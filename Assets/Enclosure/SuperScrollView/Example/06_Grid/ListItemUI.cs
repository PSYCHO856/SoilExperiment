using UnityEngine;
using UnityEngine.UI;

namespace Example_Grid {
	[DisallowMultipleComponent]
	public sealed class ListItemUI: MonoBehaviour {
		[SerializeField] private Image m_icon = null;
		
		private ListItemData m_data;

		private void Awake() {
			// m_buttonUI.onClick.AddListener( () => print( m_data.Name ) );
		}
		//加载图片
		public void SetDisp(ListItemData data) {
			m_data = data;
		}
	}
}