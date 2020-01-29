namespace ACT_FFXIV_Aetherbridge
{
	public class Player
	{
		public uint Id { get; set; }
		public ClassJob ClassJob { get; set; }
		public int Level { get; set; }
		public string Name { get; set; }
		public World CurrentWorld { get; set; }
		public World HomeWorld { get; set; }
		public PartyTypeEnum PartyType { get; set; }
		public int Order { get; set; }
		public bool IsReporter { get; set; } = false;
	}
}