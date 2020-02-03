using FFXIV.CrescentCove;
using NUnit.Framework;

namespace ACT_FFXIV_Aetherbridge.Test
{
	[TestFixture]
	public class ItemServiceTest
	{
		[SetUp]
		public void TestInitialize()
		{
			var language = new Language(1, "English", "en");
			var gameDataManager = new GameDataManager();
			var languageRepository = new GameDataRepository<FFXIV.CrescentCove.Language>(gameDataManager.Language);
			var languageService = new LanguageService(languageRepository, new FFXIVACTPluginWrapperMock(), new AetherbridgeConfig());
			IGameDataRepository<FFXIV.CrescentCove.Item> itemRepository =
				new GameDataRepository<FFXIV.CrescentCove.Item>(gameDataManager.Item);
			_itemService = new ItemService(languageService, itemRepository);
			_itemService.AddLanguage(language);
		}

		private ItemService _itemService;


		[Test]
		public void DeInit_SetsNull()
		{
			_itemService.DeInit();
		}

		[Test]
		public void GetCommonItemNames_CallTwice_ReturnsItemName()
		{
			_itemService.GetCommonItemNames();
			Assert.AreEqual("Luminous Water Crystal", _itemService.GetCommonItemNames()[0]);
		}

		[Test]
		public void GetCommonItemNames_ReturnsItemName()
		{
			Assert.AreEqual("Luminous Water Crystal", _itemService.GetCommonItemNames()[0]);
		}

		[Test]
		public void GetItemByID_BadID_ReturnsNull()
		{
			var item = _itemService.GetItemById(-1);
			Assert.IsNull(item);
		}

		[Test]
		public void GetItemByID_ReturnsItem()
		{
			var item = _itemService.GetItemById(10);
			Assert.AreEqual("Wind Crystal", item.ProperName);
		}

		[Test]
		public void GetItemByPluralName_ReturnsItem()
		{
			Assert.AreEqual(typeof(ACT_FFXIV_Aetherbridge.Item),
				_itemService.GetItemByPluralName("gil").GetType());
		}

		[Test]
		public void GetItemBySingularName_ReturnsItem()
		{
			Assert.AreEqual(typeof(ACT_FFXIV_Aetherbridge.Item),
				_itemService.GetItemBySingularName("gil").GetType());
		}

		[Test]
		public void GetItemNames_CallTwice_ReturnsItemName()
		{
			_itemService.GetItemNames();
			Assert.AreEqual("Luminous Water Crystal", _itemService.GetItemNames()[0]);
		}

		[Test]
		public void GetItemNames_ReturnsItemName()
		{
			Assert.AreEqual("Luminous Water Crystal", _itemService.GetItemNames()[0]);
		}
	}
}