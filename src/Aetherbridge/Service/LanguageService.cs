using System;
using System.Collections.Generic;
using System.Linq;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
	public class LanguageService
	{
		private readonly IFFXIVACTPluginWrapper _ffxivACTPluginWrapper;
		private readonly List<Language> _languages;
		private IGameDataRepository<FFXIV.CrescentCove.Language> _repository;
		private readonly AetherbridgeConfig _aetherbridgeConfig;

		public LanguageService(IGameDataRepository<FFXIV.CrescentCove.Language> repository,
			IFFXIVACTPluginWrapper ffxivACTPluginWrapper, AetherbridgeConfig aetherbridgeConfig)
		{
			_repository = repository;
			_languages = MapToLanguages(_repository.GetAll().ToList());
			_ffxivACTPluginWrapper = ffxivACTPluginWrapper;
			_aetherbridgeConfig = aetherbridgeConfig;
		}

		public Language GetLanguageById(int id)
		{
			return MapToLanguage(_repository.GetById(id));
		}

		public Language GetLanguageById(uint id)
		{
			return MapToLanguage(_repository.GetById(Convert.ToInt32(id)));
		}

		public List<Language> GetLanguages()
		{
			return _languages;
		}

		public void DeInit()
		{
			_repository = null;
		}

		public static List<Language> MapToLanguages(List<FFXIV.CrescentCove.Language> gameDataLanguages)
		{
			return gameDataLanguages?.Select(MapToLanguage).ToList();
		}

		public static Language MapToLanguage(FFXIV.CrescentCove.Language gameDataLanguage)
		{
			return gameDataLanguage == null ? null : new Language(gameDataLanguage.Id, gameDataLanguage.Name, gameDataLanguage.Abbreviation);
		}

		public Language MapToLanguage(FFXIV_ACT_Plugin.Common.Language pluginLanguage)
		{
			var languageId = (int) pluginLanguage;

			return GetLanguageById(languageId);
		}

		public Language GetCurrentLanguage()
		{
			if (_aetherbridgeConfig != null && IsSupported(_aetherbridgeConfig.GameLanguageId))
			{
				return GetLanguageById(_aetherbridgeConfig.GameLanguageId);
			}
			else if (IsSupported((int) _ffxivACTPluginWrapper.GetSelectedLanguage()))
			{
				return GetLanguageById((int) _ffxivACTPluginWrapper.GetSelectedLanguage());
			}
			else
			{
				return GetLanguageById(1);
			}
		}

		private static bool IsSupported(int languageId)
		{
			return (languageId > 0 && languageId < 5);
		}
	}
}