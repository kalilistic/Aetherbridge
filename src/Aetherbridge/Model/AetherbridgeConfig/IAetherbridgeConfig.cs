namespace ACT_FFXIV_Aetherbridge
{
	public interface IAetherbridgeConfig
	{
		bool LogLineParserEnabled { get; set; }
		bool ParseGameLogs { get; set; }
		bool ParseLootEvents { get; set; }
		Language CurrentLanguage { get; set; }
	}
}