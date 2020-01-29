using ACT_FFXIV_Aetherbridge.Test.Mock;
using FFXIV.CrescentCove;
using NUnit.Framework;

// ReSharper disable IsExpressionAlwaysTrue

namespace ACT_FFXIV_Aetherbridge.Test.Service.Location
{
	[TestFixture]
	public class LocationServiceTest
	{
		[SetUp]
		public void TestInitialize()
		{
			var aetherbridge = AetherbridgeMock.GetInstance();
			var language = new Language(1, "English");
			var gameDataManager = new GameDataManager();
			var languageRepository = new GameDataRepository<FFXIV.CrescentCove.Language>(gameDataManager.Language);
			var languageService = new LanguageService(aetherbridge, languageRepository);
			_locationService = new LocationService(languageService, gameDataManager);
			_locationService.AddLanguage(language);
		}

		private LocationService _locationService;

		[Test]
		public void DeInit_SetsNull()
		{
			_locationService.DeInit();
		}

		[Test]
		public void GetLocationById_ReturnsCorrectLocation()
		{
			var location = _locationService.GetLocationById(340);
			Assert.AreEqual(340, location.TerritoryTypeId);
			Assert.AreEqual(1165, location.Map.Id);
			Assert.AreEqual("The Lavender Beds", location.Map.Name);
			Assert.AreEqual(426, location.Territory.Id);
			Assert.AreEqual("The Lavender Beds", location.Territory.Name);
			Assert.AreEqual(23, location.Region.Id);
			Assert.AreEqual("The Black Shroud", location.Region.Name);
			Assert.AreEqual(507, location.Zone.Id);
			Assert.AreEqual("The Black Shroud", location.Zone.Name);
		}

		[Test]
		public void GetLocations_ReturnsLocations()
		{
			var locations = _locationService.GetLocations();
			Assert.IsTrue(locations.Count > 0);
		}

		[Test]
		public void GetZoneNames_ReturnsZoneNames()
		{
			var zones = _locationService.GetZoneNames();
			Assert.IsTrue(zones.Count > 0);
			Assert.IsTrue(zones[0] is string);
		}
	}
}