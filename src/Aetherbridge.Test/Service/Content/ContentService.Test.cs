using System.Collections.Generic;
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
            const string contentsStr = "1169the Thousand Maws of Toto–RakTrue";
            IGameDataRepository<ContentFinderCondition> contentRepository =
                new GameDataRepository<ContentFinderCondition>(contentsStr);
            var pluginZones = new List<Zone> {new Zone(169, "the Thousand Maws of Toto–Rak")};
            _contentService = new ContentService(pluginZones, contentRepository);
        }

        private IContentService _contentService;

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
        public void GetZoneNames_ReturnsZoneNames()
        {
            var zones = _contentService.GetHighEndContentNames();
            Assert.IsTrue(zones.Count > 0);
            Assert.IsTrue(zones[0] is string);
        }
    }
}