using System;

namespace ACT_FFXIV_Aetherbridge
{
	public interface IAetherbridge
	{
		ClassJobService ClassJobService { get; set; }
		WorldService WorldService { get; set; }
		LocationService LocationService { get; set; }
		ContentService ContentService { get; set; }
		ItemService ItemService { get; set; }
		LanguageService LanguageService { get; set; }
		PlayerService PlayerService { get; set; }
		AetherbridgeConfig AetherbridgeConfig { get; set; }
		event EventHandler<LogLineEvent> LogLineCaptured;
		void InitGameData();
		void Initialize();
		void AddLanguage(int languageId);
		void DeInit();
		void EnableLogLineParser();
		void InitLogLineParser();
	}
}