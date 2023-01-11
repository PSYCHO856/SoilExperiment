namespace Example_Chat {
	public sealed class CharaData {
		private readonly int m_id;
		private readonly string m_iconSpriteName;
		private readonly string m_name;


		public int Id {
			get {
				return m_id;
			}
		}
		public string IconSpriteName {
			get {
				return m_iconSpriteName;
			}
		}
		public string name {
			get {
				return m_name;
			}
		}

		public CharaData
			(
				int id,
				string iconSpriteName,
				string name
			) {
				m_id = id;
				m_iconSpriteName = iconSpriteName;
				m_name = name;
			}
	}
}