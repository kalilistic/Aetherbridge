using System.Collections.Generic;
using System.Linq;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
	public class ContentService
	{
		private readonly LanguageService _languageService;
		private readonly IReadOnlyCollection<Zone> _pluginZones;
		private readonly IGameDataRepository<ContentFinderCondition> _repository;
		private List<List<Content>> _content = new List<List<Content>>();
		private List<List<Content>> _highEndContent = new List<List<Content>>();

		public ContentService(LanguageService languageService, IReadOnlyCollection<Zone> pluginZones,
			IGameDataRepository<ContentFinderCondition> repository)
		{
			_languageService = languageService;
			_repository = repository;
			_pluginZones = pluginZones;

			var languages = _languageService.GetLanguages();
			foreach (var _ in languages)
			{
				_content.Add(new List<Content>());
				_highEndContent.Add(new List<Content>());
			}
		}

		public void AddLanguage(Language language)
		{
			var contentFinderConditionList = _repository.GetAll();
			contentFinderConditionList = contentFinderConditionList.OrderBy(content => content.Name);
			foreach (var contentFinderCondition in contentFinderConditionList)
			{
				var inPluginZones = false;
				foreach (var _ in _pluginZones.Where(
					pluginZone => pluginZone.Id == contentFinderCondition.TerritoryType))
					inPluginZones = true;
				if (!inPluginZones) continue;
				var contentName = contentFinderCondition.Localized[language.Index]?.Name;
				if (contentName == null || contentName.Equals(string.Empty)) continue;
				var content = new Content
				{
					Id = contentFinderCondition.Id,
					TerritoryTypeId = contentFinderCondition.TerritoryType,
					IsHighEndDuty = contentFinderCondition.HighEndDuty,
					Name = contentName
				};
				_content[language.Index].Add(content);
				if (content.IsHighEndDuty) _highEndContent[language.Index].Add(content);
			}
		}

		public List<Content> GetHighEndContent()
		{
			return _highEndContent[_languageService.GetCurrentLanguage().Index];
		}

		public List<Content> GetContent()
		{
			return _content[_languageService.GetCurrentLanguage().Index];
		}

		public Content GetContentByTerritoryTypeId(int territoryTypeId)
		{
			return _content[_languageService.GetCurrentLanguage().Index]
				.Find(content => content.TerritoryTypeId == territoryTypeId);
		}

		public List<string> GetContentNames()
		{
			return _content[_languageService.GetCurrentLanguage().Index].Select(content => content.Name).ToList();
		}

		public List<string> GetHighEndContentNames()
		{
			return GetHighEndContent().Select(content => content.Name).ToList();
		}

		public void DeInit()
		{
			_content = null;
			_highEndContent = null;
		}
	}
}