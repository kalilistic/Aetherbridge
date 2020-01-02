using System;
using System.Collections.Generic;
using System.Linq;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
    public class WorldService : IWorldService
    {
        private IGameDataRepository<FFXIV.CrescentCove.World> _repository;

        public WorldService(IGameDataRepository<FFXIV.CrescentCove.World> repository)
        {
            _repository = repository;
        }

        public IWorld GetWorldById(int id)
        {
            return WorldMapper.MapToAetherbridgeWorld(_repository.GetById(id));
        }

        public IWorld GetWorldById(uint id)
        {
            return WorldMapper.MapToAetherbridgeWorld(_repository.GetById(Convert.ToInt32(id)));
        }

        public List<IWorld> GetWorlds()
        {
            return WorldMapper.MapToAetherbridgeWorlds(_repository.GetAll().ToList());
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
    }
}