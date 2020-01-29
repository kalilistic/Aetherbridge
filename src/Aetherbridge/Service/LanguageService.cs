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
		private Language _currentLanguage;
		private IGameDataRepository<FFXIV.CrescentCove.Language> _repository;

		public LanguageService(IGameDataRepository<FFXIV.CrescentCove.Language> repository,
			IFFXIVACTPluginWrapper ffxivACTPluginWrapper)
		{
			_repository = repository;
			_languages = MapToLanguages(_repository.GetAll().ToList());
			_ffxivACTPluginWrapper = ffxivACTPluginWrapper;
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
			return gameDataLanguage == null ? null : new Language(gameDataLanguage.Id, gameDataLanguage.Name);
		}

		public Language MapToLanguage(FFXIV_ACT_Plugin.Common.Language pluginLanguage)
		{
			var languageId = (int) pluginLanguage;
			if (languageId == 0 || languageId > 4) languageId = 1;
			return new Language(languageId, pluginLanguage.ToString());
		}

		public Language GetCurrentLanguage()
		{
			if (_currentLanguage != null) return _currentLanguage;
			_currentLanguage = MapToLanguage(_ffxivACTPluginWrapper.GetSelectedLanguage());

			return _currentLanguage;
		}

		public void UpdateCurrentLanguage(Language language)
		{
			_currentLanguage = language;
		}
	}
}