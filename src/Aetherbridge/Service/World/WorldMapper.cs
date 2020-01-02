using System.Collections.Generic;
using System.Linq;

namespace ACT_FFXIV_Aetherbridge
{
    public static class WorldMapper
    {
        public static List<IWorld> MapToAetherbridgeWorlds(List<FFXIV.CrescentCove.World> gameDataWorlds)
        {
            return gameDataWorlds?.Select(MapToAetherbridgeWorld).ToList();
        }

        public static IWorld MapToAetherbridgeWorld(FFXIV.CrescentCove.World gameDataWorld)
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