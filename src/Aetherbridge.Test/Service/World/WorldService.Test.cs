using FFXIV.CrescentCove;
using NUnit.Framework;

namespace ACT_FFXIV_Aetherbridge.Test.Service.World
{
	[TestFixture]
	public class WorldServiceTest
	{
		[SetUp]
		public void TestInitialize()
		{
			var gameDataManager = new GameDataManager();
			var worldRepository = new GameDataRepository<FFXIV.CrescentCove.World>(gameDataManager.World);
			_worldService = new WorldService(worldRepository);
		}

		private WorldService _worldService;

		[Test]
		public void DeInit_SetsNull()
		{
			_worldService.DeInit();
		}


		[Test]
		public void GetWorlds_ReturnsWorlds()
		{
			var worlds = _worldService.GetWorlds();
			Assert.IsTrue(worlds.Count > 0);
		}
	}
}