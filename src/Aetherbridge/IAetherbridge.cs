using System;

namespace ACT_FFXIV_Aetherbridge
{
	public interface IAetherbridge
	{
		IClassJobService ClassJobService { get; }
		IWorldService WorldService { get; }
		ILocationService LocationService { get; }
		IContentService ContentService { get; }
		IItemService ItemService { get; }
		ILanguageService LanguageService { get; set; }
		IAetherbridgeConfig AetherbridgeConfig { get; set; }
		event EventHandler<ILogLineEvent> LogLineCaptured;
		IPlayer GetCurrentPlayer();
		ILanguage GetCurrentLanguage();
		void InitGameData();
		void Initialize();
		void AddLanguage(ILanguage language);
	}
}