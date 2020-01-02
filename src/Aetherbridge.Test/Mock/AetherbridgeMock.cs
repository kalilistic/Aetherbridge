using System;
using System.Collections.Generic;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge.Test.Mock
{
    internal class AetherbridgeMock : IAetherbridge
    {
        private static volatile IAetherbridge _aetherbridge;
        private static readonly object Lock = new object();
        private static IACTWrapper _actWrapper;
        private static IFFXIVACTPluginWrapper _ffxivACTPluginWrapper;
        internal readonly ILogLineParser LogLineParser;
        public PlayerMapper PlayerMapper;

        private AetherbridgeMock()
        {
            _actWrapper = new ACTWrapperMock();
            _ffxivACTPluginWrapper = new FFXIVACTPluginWrapperMock();
            var gameDataManager = new GameDataManager();
            var worldRepository = new GameDataRepository<FFXIV.CrescentCove.World>(gameDataManager.World);
            WorldService = new WorldService(worldRepository);
            var classJobRepository = new GameDataRepository<FFXIV.CrescentCove.ClassJob>(gameDataManager.ClassJob);
            ClassJobService = new ClassJobService(classJobRepository);
            LocationService = new LocationService(gameDataManager);
            var itemRepository = new GameDataRepository<FFXIV.CrescentCove.Item>(gameDataManager.Item);
            ItemService = new ItemService(itemRepository);
            LogLineParser = new ACT_FFXIV_Aetherbridge.LogLineParser(this);
            PlayerMapper = new PlayerMapper(WorldService, ClassJobService);
        }

        public event EventHandler<LogLineEvent> LogLineCaptured = delegate { };
        public IClassJobService ClassJobService { get; set; }
        public IWorldService WorldService { get; set; }
        public ILocationService LocationService { get; set; }
        public IContentService ContentService { get; set; }
        public IItemService ItemService { get; }
        public IAetherbridgeConfig AetherbridgeConfig { get; set; }

        public void EnableLogLineParser()
        {
            throw new NotImplementedException();
        }

        public void DisableLogLineParser()
        {
            throw new NotImplementedException();
        }

        public void ACTLogLineCaptured(object sender, ACTLogLineEvent actLogLineEvent)
        {
            throw new NotImplementedException();
        }

        public IPlayer GetCurrentPlayer()
        {
            return PlayerMapper.MapToPlayer(_ffxivACTPluginWrapper.GetCurrentCombatant());
        }

        public IPlayer GetCurrentPlayerACT()
        {
            throw new NotImplementedException();
        }

        public List<IPlayer> GetPartyMembers()
        {
            throw new NotImplementedException();
        }

        public List<IPlayer> GetAllianceMembers()
        {
            throw new NotImplementedException();
        }

        public IPlayer GetPlayerByName(string name)
        {
            var player = new Player {Name = name};
            return player;
        }

        public void DeInit()
        {
            throw new NotImplementedException();
        }

        public static IAetherbridge GetInstance()
        {
            if (_aetherbridge != null) return _aetherbridge;

            lock (Lock)
            {
                if (_aetherbridge == null) _aetherbridge = new AetherbridgeMock();
            }

            return _aetherbridge;
        }
    }
}