using System.Collections.Generic;
using System.Linq;

namespace ACT_FFXIV_Aetherbridge
{
	public static class WorldMapper
	{
		public static List<World> MapToWorlds(List<FFXIV.CrescentCove.World> gameDataWorlds)
		{
			return gameDataWorlds?.Select(MapToWorld).ToList();
		}

		public static World MapToWorld(FFXIV.CrescentCove.World gameDataWorld)
		{
			if (gameDataWorld == null) return null;
			return new World
			{
				Id = gameDataWorld.Id,
				Name = gameDataWorld.Name
			};
		}
	}
}