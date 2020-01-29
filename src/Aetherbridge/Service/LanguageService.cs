using System;
using System.Collections.Generic;
using System.Linq;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
	public class LanguageService
	{
		private readonly IAetherbridge _aetherbridge;
		private readonly List<Language> _languages;
		private Language _currentLanguage;
		private IGameDataRepository<FFXIV.CrescentCove.Language> _repository;

		public LanguageService(IAetherbridge aetherbridge, IGameDataRepository<FFXIV.CrescentCove.Language> repository)
		{
			_aetherbridge = aetherbridge;
			_repository = repository;
			_languages = MapToLanguages(_repository.GetAll().ToList());
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

		public Language GetCurrentLanguage()
		{
			if (_currentLanguage != null) return _currentLanguage;
			_currentLanguage = _aetherbridge.GetCurrentLanguage();
			return _currentLanguage;
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
	}
}