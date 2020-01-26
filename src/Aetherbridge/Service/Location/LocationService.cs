using System;
using System.Collections.Generic;
using System.Linq;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
	public class LocationService : ILocationService
	{
		private readonly ILanguageService _languageService;
		private readonly IGameDataRepository<Map> _mapRepository;
		private readonly IGameDataRepository<FFXIV.CrescentCove.PlaceName> _placeNameRepository;
		private readonly IGameDataRepository<TerritoryType> _territoryTypeRepository;
		private List<List<Location>> _locations = new List<List<Location>>();

		public LocationService(ILanguageService languageService, IGameDataManager gameDataManager)
		{
			_languageService = languageService;

			_territoryTypeRepository = new GameDataRepository<TerritoryType>(gameDataManager.TerritoryType);
			_placeNameRepository = new GameDataRepository<FFXIV.CrescentCove.PlaceName>(gameDataManager.PlaceName);
			_mapRepository = new GameDataRepository<Map>(gameDataManager.Map);

			var languages = _languageService.GetLanguages();
			foreach (var _ in languages) _locations.Add(new List<Location>());
		}

		public void AddLanguage(ILanguage language)
		{
			var territoryList = _territoryTypeRepository.GetAll();
			foreach (var territoryType in territoryList)
			{
				PlaceName regionPlaceName;
				try
				{
					var regionPlaceNameKey = territoryType.RegionPlaceNameId;
					var regionNameValue = _placeNameRepository.Find(pn => pn.Id == territoryType.RegionPlaceNameId)
						.First()?.Localized[_languageService.GetCurrentLanguage().Index].Name;
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
					var zoneNameValue = _placeNameRepository.Find(pn => pn.Id == territoryType.ZonePlaceNameId).First()
						?.Localized[_languageService.GetCurrentLanguage().Index].Name;
					zonePlaceName = new PlaceName(zonePlaceNameKey, zoneNameValue);
				}
				catch (Exception)
				{
					zonePlaceName = null;
				}


				PlaceName mapPlaceName;
				try
				{
					var map = _mapRepository.Find(m => m.Id == territoryType.MapId).First();
					var mapPlaceNameKey = map.MapPlaceNameId;
					var mapPlaceNameValue = _placeNameRepository.Find(pn => pn.Id == mapPlaceNameKey).First()
						?.Localized[_languageService.GetCurrentLanguage().Index].Name;
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
					var territoryNameValue = _placeNameRepository
						.Find(pn => pn.Id == territoryType.TerritoryPlaceNameId)
						.First()?.Localized[_languageService.GetCurrentLanguage().Index].Name;
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
				_locations[language.Index].Add(location);
			}
		}

		public List<ILocation> GetLocations()
		{
			return new List<ILocation>(_locations[_languageService.GetCurrentLanguage().Index]);
		}

		public ILocation GetLocationById(int locationId)
		{
			return _locations[_languageService.GetCurrentLanguage().Index]
				.Find(loc => loc.TerritoryTypeId == locationId);
		}

		public List<string> GetZoneNames()
		{
			var locations = _locations[_languageService.GetCurrentLanguage().Index];
			return (from location in locations where location.Zone != null select location.Zone.Name).ToList();
		}

		public void DeInit()
		{
			_locations = null;
		}
	}
}