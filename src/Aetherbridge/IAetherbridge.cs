using System;

namespace ACT_FFXIV_Aetherbridge
{
	public interface IAetherbridge
	{
		ClassJobService ClassJobService { get; }
		WorldService WorldService { get; }
		LocationService LocationService { get; }
		ContentService ContentService { get; }
		ItemService ItemService { get; }
		LanguageService LanguageService { get; set; }
		AetherbridgeConfig AetherbridgeConfig { get; set; }
		event EventHandler<LogLineEvent> LogLineCaptured;
		Player GetCurrentPlayer();
		Language GetCurrentLanguage();
		void InitGameData();
		void Initialize();
		void AddLanguage(Language language);
	}
}