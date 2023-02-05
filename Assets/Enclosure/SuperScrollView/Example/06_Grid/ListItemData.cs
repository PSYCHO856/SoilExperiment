namespace Example_Grid
{
	public sealed class ListItemData
	{
		public string Name;
    	public string Icon;
		public string SkillIndex;

        public ListItemData(string name, string icon, string skillIndex)
        {
            Name = name;
            Icon = icon;
            SkillIndex = skillIndex;
        }
    }
}