using FFXIV.CrescentCove;
using NUnit.Framework;

namespace ACT_FFXIV_Aetherbridge.Test
{
	[TestFixture]
	public class ClassJobServiceTest
	{
		[SetUp]
		public void TestInitialize()
		{
			var aetherbridge = (AetherbridgeMock) AetherbridgeMock.GetInstance();
			var language = new Language(1, "English", "en");
			aetherbridge.CurrentLanguage = language;
			var gameDataManager = new GameDataManager();
			var languageRepository = new GameDataRepository<FFXIV.CrescentCove.Language>(gameDataManager.Language);
			var languageService = new LanguageService(languageRepository, new FFXIVACTPluginWrapperMock());
			IGameDataRepository<FFXIV.CrescentCove.ClassJob> classJobRepository =
				new GameDataRepository<FFXIV.CrescentCove.ClassJob>(gameDataManager.ClassJob);
			_classJobService = new ClassJobService(languageService, classJobRepository);
			_classJobService.AddLanguage(language);
		}

		private ClassJobService _classJobService;

		[Test]
		public void DeInit_SetsNull()
		{
			_classJobService.DeInit();
		}

		[Test]
		public void GetClassJobByID_BadID_ReturnsNull()
		{
			var classJob = _classJobService.GetClassJobById(-1);
			Assert.IsNull(classJob);
		}

		[Test]
		public void GetClassJobByID_ReturnsClassJob()
		{
			var classJob = _classJobService.GetClassJobById(2);
			Assert.AreEqual("PGL", classJob.Abbreviation);
		}
	}
}