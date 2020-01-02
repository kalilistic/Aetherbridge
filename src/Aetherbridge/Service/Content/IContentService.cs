using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
    public interface IContentService
    {
        List<IContent> GetHighEndContent();
        List<IContent> GetContent();
        IContent GetContentByTerritoryTypeId(int territoryTypeId);
        List<string> GetContentNames();
        List<string> GetHighEndContentNames();
        void DeInit();
    }
}