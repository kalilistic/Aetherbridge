namespace ACT_FFXIV_Aetherbridge
{
	public class Player : IPlayer
	{
		public uint Id { get; set; }
		public IClassJob ClassJob { get; set; }
		public int Level { get; set; }
		public string Name { get; set; }
		public IWorld CurrentWorld { get; set; }
		public IWorld HomeWorld { get; set; }
		public PartyTypeEnum PartyType { get; set; }
		public int Order { get; set; }
		public bool IsReporter { get; set; } = false;
	}
}