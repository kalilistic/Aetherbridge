using System.Collections.Generic;
using System.Linq;

namespace ACT_FFXIV_Aetherbridge
{
	public static class LanguageMapper
	{
		public static List<Language> MapToLanguages(List<FFXIV.CrescentCove.Language> gameDataLanguages)
		{
			return gameDataLanguages?.Select(MapToLanguage).ToList();
		}

		public static Language MapToLanguage(FFXIV.CrescentCove.Language gameDataLanguage)
		{
			return gameDataLanguage == null ? null : new Language(gameDataLanguage.Id, gameDataLanguage.Name);
		}
	}
}