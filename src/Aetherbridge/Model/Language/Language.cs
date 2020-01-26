namespace ACT_FFXIV_Aetherbridge
{
	public class Language : ILanguage
	{
		public Language(int id, string name)
		{
			Id = id;
			Index = id - 1;
			Name = name;
		}

		public int Id { get; set; }
		public int Index { get; set; }
		public string Name { get; set; }

		public override string ToString()
		{
			return Name + "(" + Id + ")";
		}
	}
}