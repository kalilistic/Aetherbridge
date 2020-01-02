using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
    public interface ILocationService
    {
        List<ILocation> GetLocations();
        ILocation GetLocationById(int locationId);
        List<string> GetZoneNames();
        void DeInit();
    }
}