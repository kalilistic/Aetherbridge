using System;
using System.Collections.Generic;
using System.Linq;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
	public class WorldService
	{
		private IGameDataRepository<FFXIV.CrescentCove.World> _repository;

		public WorldService(IGameDataRepository<FFXIV.CrescentCove.World> repository)
		{
			_repository = repository;
		}

		public World GetWorldById(int id)
		{
			return MapToWorld(_repository.GetById(id));
		}

		public World GetWorldById(uint id)
		{
			return MapToWorld(_repository.GetById(Convert.ToInt32(id)));
		}

		public List<World> GetWorlds()
		{
			return MapToWorlds(_repository.GetAll().ToList());
		}

		public string GetWorldsAsDelimitedString()
		{
			var worldsStr = string.Empty;
			var worlds = GetWorlds();
			worldsStr = worlds.Aggregate(worldsStr, (current, world) => current + world.Name + "|");
			worldsStr = worldsStr.TrimEnd('|');
			return worldsStr;
		}

		public void DeInit()
		{
			_repository = null;
		}

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