using System;
using System.Collections.Generic;
using System.Linq;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
    public class LocationService : ILocationService
    {
        private List<ILocation> _locations;

        public LocationService(IGameDataManager gameDataManager)
        {
            IGameDataRepository<TerritoryType> territoryTypeRepository =
                new GameDataRepository<TerritoryType>(gameDataManager.TerritoryType);
            IGameDataRepository<FFXIV.CrescentCove.PlaceName> placeNameRepository =
                new GameDataRepository<FFXIV.CrescentCove.PlaceName>(gameDataManager.PlaceName);
            IGameDataRepository<Map> mapRepository = new GameDataRepository<Map>(gameDataManager.Map);

            _locations = new List<ILocation>();

            var territoryList = territoryTypeRepository.GetAll();

            foreach (var territoryType in territoryList)
            {
                PlaceName regionPlaceName;
                try
                {
                    var regionPlaceNameKey = territoryType.RegionPlaceNameId;
                    var regionNameValue = placeNameRepository.Find(pn => pn.Id == territoryType.RegionPlaceNameId)
                        .First()?.Name;
                    regionPlaceName = new PlaceName(regionPlaceNameKey, regionNameValue);
                }
                catch (Exception)
                {
                    regionPlaceName = null;
                }

                PlaceName zonePlaceName;
                try
                {
                    var zonePlaceNameKey = territoryType.ZonePlaceNameId;
                    var zoneNameValue = placeNameRepository.Find(pn => pn.Id == territoryType.ZonePlaceNameId).First()
                        ?.Name;
                    zonePlaceName = new PlaceName(zonePlaceNameKey, zoneNameValue);
                }
                catch (Exception)
                {
                    zonePlaceName = null;
                }


                PlaceName mapPlaceName;
                try
                {
                    var map = mapRepository.Find(m => m.Id == territoryType.MapId).First();
                    var mapPlaceNameKey = map.MapPlaceNameId;
                    var mapPlaceNameValue = placeNameRepository.Find(pn => pn.Id == mapPlaceNameKey).First()?.Name;
                    mapPlaceName = new PlaceName(mapPlaceNameKey, mapPlaceNameValue);
                }
                catch (Exception)
                {
                    mapPlaceName = null;
                }

                PlaceName territoryPlaceName;
                try
                {
                    var territoryPlaceNameKey = territoryType.TerritoryPlaceNameId;
                    var territoryNameValue = placeNameRepository.Find(pn => pn.Id == territoryType.TerritoryPlaceNameId)
                        .First()?.Name;
                    territoryPlaceName = new PlaceName(territoryPlaceNameKey, territoryNameValue);
                }
                catch (Exception)
                {
                    territoryPlaceName = null;
                }

                var location = new Location
                {
                    TerritoryTypeId = territoryType.Id,
                    Region = regionPlaceName,
                    Zone = zonePlaceName,
                    Territory = territoryPlaceName,
                    Map = mapPlaceName
                };

                if (location.Region == null && location.Zone == null) continue;
                _locations.Add(location);
            }
        }

        public List<ILocation> GetLocations()
        {
            return _locations;
        }

        public ILocation GetLocationById(int locationId)
        {
            return _locations.Find(loc => loc.TerritoryTypeId == locationId);
        }

        public List<string> GetZoneNames()
        {
            return _locations.Select(loc => loc.Zone.Name).ToList();
        }

        public void DeInit()
        {
            _locations = null;
        }
    }
}