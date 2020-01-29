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

		private AetherbridgeMock(Language language)
		{
			_actWrapper = new ACTWrapperMock();
			_ffxivACTPluginWrapper = new FFXIVACTPluginWrapperMock();
			CurrentLanguage = language ?? new Language(1, "English");
			var gameDataManager = new GameDataManager();
			var languageRepository = new GameDataRepository<FFXIV.CrescentCove.Language>(gameDataManager.Language);
			LanguageService = new LanguageService(this, languageRepository);
			var worldRepository = new GameDataRepository<FFXIV.CrescentCove.World>(gameDataManager.World);
			WorldService = new WorldService(worldRepository);
			var classJobRepository = new GameDataRepository<FFXIV.CrescentCove.ClassJob>(gameDataManager.ClassJob);
			ClassJobService = new ClassJobService(LanguageService, classJobRepository);
			LocationService = new LocationService(LanguageService, gameDataManager);
			var contentRepository =
				new GameDataRepository<ContentFinderCondition>(gameDataManager.ContentFinderCondition);
			ContentService =
				new ContentService(LanguageService, _ffxivACTPluginWrapper.GetZoneList(), contentRepository);
			var itemRepository = new GameDataRepository<FFXIV.CrescentCove.Item>(gameDataManager.Item);
			ItemService = new ItemService(LanguageService, itemRepository);
			LogLineParser = new ENLogLineParser(this);
			PlayerMapper = new PlayerMapper(WorldService, ClassJobService);
		}

		public Language CurrentLanguage { get; set; }

		public event EventHandler<LogLineEvent> LogLineCaptured = delegate { };
		public ClassJobService ClassJobService { get; set; }
		public WorldService WorldService { get; set; }
		public LocationService LocationService { get; set; }
		public ContentService ContentService { get; set; }
		public ItemService ItemService { get; }
		public LanguageService LanguageService { get; set; }
		public AetherbridgeConfig AetherbridgeConfig { get; set; }

		public Player GetCurrentPlayer()
		{
			var player = PlayerMapper.MapToPlayer(_ffxivACTPluginWrapper.GetCurrentCombatant());
			player.IsReporter = true;
			return player;
		}

		public Language GetCurrentLanguage()
		{
			return CurrentLanguage;
		}

		public void InitGameData()
		{
			throw new NotImplementedException();
		}

		public void Initialize()
		{
			throw new NotImplementedException();
		}

		public void AddLanguage(Language language)
		{
			ClassJobService.AddLanguage(language);
			LocationService.AddLanguage(language);
			ContentService.AddLanguage(language);
			ItemService.AddLanguage(language);
		}


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

		public Player GetCurrentPlayerACT()
		{
			throw new NotImplementedException();
		}

		public List<Player> GetPartyMembers()
		{
			throw new NotImplementedException();
		}

		public List<Player> GetAllianceMembers()
		{
			throw new NotImplementedException();
		}

		public Player GetPlayerByName(string name)
		{
			var player = new Player {Name = name};
			return player;
		}

		public void DeInit()
		{
			_aetherbridge = null;
			CurrentLanguage = null;
		}

		public static IAetherbridge GetInstance()
		{
			if (_aetherbridge != null) return _aetherbridge;

			lock (Lock)
			{
				if (_aetherbridge == null) _aetherbridge = new AetherbridgeMock(null);
			}

			return _aetherbridge;
		}

		public static IAetherbridge GetInstance(Language language)
		{
			if (_aetherbridge != null) return _aetherbridge;

			lock (Lock)
			{
				if (_aetherbridge == null) _aetherbridge = new AetherbridgeMock(language);
			}

			return _aetherbridge;
		}
	}
}