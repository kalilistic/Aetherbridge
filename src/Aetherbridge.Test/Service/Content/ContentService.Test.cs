using ACT_FFXIV_Aetherbridge.Test.Mock;
using FFXIV.CrescentCove;
using NUnit.Framework;

// ReSharper disable IsExpressionAlwaysTrue

namespace ACT_FFXIV_Aetherbridge.Test.Service.Content
{
	[TestFixture]
	public class ContentServiceTest
	{
		[SetUp]
		public void TestInitialize()
		{
			var aetherbridge = (AetherbridgeMock) AetherbridgeMock.GetInstance();
			var language = new Language(1, "English");
			aetherbridge.CurrentLanguage = language;
			var gameDataManager = new GameDataManager();
			var languageRepository = new GameDataRepository<FFXIV.CrescentCove.Language>(gameDataManager.Language);
			var languageService = new LanguageService(languageRepository, new FFXIVACTPluginWrapperMock());
			IGameDataRepository<ContentFinderCondition> contentRepository =
				new GameDataRepository<ContentFinderCondition>(gameDataManager.ContentFinderCondition);
			var pluginZones = new FFXIVACTPluginWrapperMock().GetZoneList();
			_contentService = new ContentService(languageService, pluginZones, contentRepository);
			_contentService.AddLanguage(language);
		}

		private ContentService _contentService;

		[Test]
		public void DeInit_SetsNull()
		{
			_contentService.DeInit();
		}

		[Test]
		public void GetContent_ReturnsContent()
		{
			var contents = _contentService.GetContent();
			Assert.IsTrue(contents.Count > 0);
		}

		[Test]
		public void GetContentNames_ReturnsContentNames()
		{
			var zones = _contentService.GetContentNames();
			Assert.IsTrue(zones.Count > 0);
		}

		[Test]
		public void GetHighEndContentNames_ReturnsHighEndContentNames()
		{
			var zones = _contentService.GetHighEndContentNames();
			Assert.IsTrue(zones.Count > 0);
		}
	}
}