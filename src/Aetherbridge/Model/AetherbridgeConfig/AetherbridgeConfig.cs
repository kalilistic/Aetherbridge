namespace ACT_FFXIV_Aetherbridge
{
	public class AetherbridgeConfig : IAetherbridgeConfig
	{
		public bool LogLineParserEnabled { get; set; } = false;
		public bool ParseGameLogs { get; set; } = true;
		public bool ParseLootEvents { get; set; } = true;
		public Language CurrentLanguage { get; set; }
	}
}