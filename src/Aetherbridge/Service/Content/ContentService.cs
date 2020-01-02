using System;
using System.Collections.Generic;
using System.Linq;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
    public class ContentService : IContentService
    {
        private List<IContent> _content = new List<IContent>();
        private List<IContent> _highEndContent = new List<IContent>();

        public ContentService(IReadOnlyCollection<Zone> pluginZones,
            IGameDataRepository<ContentFinderCondition> repository)
        {
            var contentFinderConditionList = repository.GetAll();
            contentFinderConditionList = contentFinderConditionList.OrderBy(content => content.Name);
            foreach (var contentFinderCondition in contentFinderConditionList)
            {
                if (contentFinderCondition.Name.Contains("Special Event")) continue;
                var inPluginZones = false;
                foreach (var _ in pluginZones.Where(
                    pluginZone => pluginZone.Id == contentFinderCondition.TerritoryType &&
                                  pluginZone.Name.Equals(contentFinderCondition.Name, StringComparison.CurrentCulture)))
                    inPluginZones = true;
                if (!inPluginZones) continue;

                var content = new Content
                {
                    Id = contentFinderCondition.Id,
                    TerritoryTypeId = contentFinderCondition.TerritoryType,
                    IsHighEndDuty = contentFinderCondition.HighEndDuty,
                    Name = contentFinderCondition.Name
                };

                _content.Add(content);

                if (content.IsHighEndDuty) _highEndContent.Add(content);
            }
        }

        public List<IContent> GetHighEndContent()
        {
            return _highEndContent;
        }

        public List<IContent> GetContent()
        {
            return _content;
        }

        public IContent GetContentByTerritoryTypeId(int territoryTypeId)
        {
            return _content.Find(content => content.TerritoryTypeId == territoryTypeId);
        }

        public List<string> GetContentNames()
        {
            return _content.Select(content => content.Name).ToList();
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