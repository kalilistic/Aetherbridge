using FFXIV.CrescentCove;
using NUnit.Framework;

namespace ACT_FFXIV_Aetherbridge.Test.Service.Item
{
    [TestFixture]
    public class ItemServiceTest
    {
        [SetUp]
        public void TestInitialize()
        {
            const string itemsStr = "10wind crystalwind crystalsWind CrystalTrueFalse";
            IGameDataRepository<FFXIV.CrescentCove.Item> itemRepository = new GameDataRepository<FFXIV.CrescentCove.Item>(itemsStr);
            _itemService = new ItemService(itemRepository);
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
            Assert.AreEqual("Wind Crystal", _itemService.GetCommonItemNames()[0]);
        }

        [Test]
        public void GetCommonItemNames_ReturnsItemName()
        {
            Assert.AreEqual("Wind Crystal", _itemService.GetCommonItemNames()[0]);
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
                _itemService.GetItemByPluralName("wind crystals").GetType());
        }

        [Test]
        public void GetItemBySingularName_ReturnsItem()
        {
            Assert.AreEqual(typeof(ACT_FFXIV_Aetherbridge.Item),
                _itemService.GetItemBySingularName("wind crystal").GetType());
        }

        [Test]
        public void GetItemNames_CallTwice_ReturnsItemName()
        {
            _itemService.GetItemNames();
            Assert.AreEqual("Wind Crystal", _itemService.GetItemNames()[0]);
        }

        [Test]
        public void GetItemNames_ReturnsItemName()
        {
            Assert.AreEqual("Wind Crystal", _itemService.GetItemNames()[0]);
        }
    }
}