using System;
using System.Collections.Generic;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
	public class Aetherbridge : IAetherbridge
	{
		private static volatile Aetherbridge _aetherbridge;
		private static readonly object Lock = new object();
		private static IACTWrapper _actWrapper;
		private static IFFXIVACTPluginWrapper _ffxivACTPluginWrapper;
		private static PlayerMapper _playerMapper;
		internal ILogLineParser LogLineParser;

		private Aetherbridge()
		{
			InitWrappers();
			InitGameData();
			AetherbridgeConfig = new AetherbridgeConfig();
		}

		public IClassJobService ClassJobService { get; set; }
		public IWorldService WorldService { get; set; }
		public ILocationService LocationService { get; set; }
		public IContentService ContentService { get; set; }
		public IItemService ItemService { get; set; }
		public ILanguageService LanguageService { get; set; }
		public IAetherbridgeConfig AetherbridgeConfig { get; set; }

		public void InitLogLineParser()
		{
			var lang = GetCurrentLanguage();
			switch (lang.Id)
			{
				case 1:
					LogLineParser = new ENLogLineParser(this);
					break;
				case 2:
					LogLineParser = new FRLogLineParser(this);
					break;
				case 3:
					LogLineParser = new DELogLineParser(this);
					break;
				case 4:
					LogLineParser = new JALogLineParser(this);
					break;
			}
		}

		public void AddLanguage(ILanguage language)
		{
			ClassJobService.AddLanguage(language);
			LocationService.AddLanguage(language);
			ContentService.AddLanguage(language);
			ItemService.AddLanguage(language);
		}

		public event EventHandler<ILogLineEvent> LogLineCaptured;

		public void EnableLogLineParser()
		{
			if (AetherbridgeConfig.LogLineParserEnabled) return;
			if (LogLineParser == null) InitLogLineParser();
			AetherbridgeConfig.LogLineParserEnabled = true;
			_actWrapper.ACTLogLineParserEnabled = true;
			_actWrapper.ACTLogLineCaptured += ACTLogLineCaptured;
		}

		public void DisableLogLineParser()
		{
			if (!AetherbridgeConfig.LogLineParserEnabled) return;
			AetherbridgeConfig.LogLineParserEnabled = false;
			_actWrapper.ACTLogLineParserEnabled = false;
			_actWrapper.ACTLogLineCaptured -= ACTLogLineCaptured;
		}

		public void ACTLogLineCaptured(object sender, ACTLogLineEvent actLogLineEvent)
		{
			var logLineEvent = LogLineParser.Parse(actLogLineEvent);
			if (!actLogLineEvent.IsImport && logLineEvent?.XIVEvent != null)
				logLineEvent.XIVEvent.Location = GetCurrentLocation();
			LogLineCaptured?.Invoke(this, logLineEvent);
		}

		public IPlayer GetCurrentPlayerACT()
		{
			return new Player {Name = _actWrapper.GetCharacterName(), IsReporter = true};
		}

		public IPlayer GetCurrentPlayer()
		{
			var combatant = _ffxivACTPluginWrapper.GetCurrentCombatant();
			IPlayer player;
			if (combatant == null)
			{
				var importName = ACTWrapper.GetInstance().GetCharacterName();
				player = importName.Equals(string.Empty) || !importName.Contains(" ")
					? new Player {Name = "Your Character"}
					: new Player {Name = ACTWrapper.GetInstance().GetCharacterName()};
			}
			else
			{
				player = _playerMapper.MapToPlayer(combatant);
			}

			player.IsReporter = true;
			return player;
		}

		public List<IPlayer> GetPartyMembers()
		{
			return _playerMapper.MapToPlayers(_ffxivACTPluginWrapper.GetPartyCombatants());
		}

		public List<IPlayer> GetAllianceMembers()
		{
			return _playerMapper.MapToPlayers(_ffxivACTPluginWrapper.GetAllianceCombatants());
		}

		public IPlayer GetPlayerByName(string name)
		{
			try
			{
				var combatant = _ffxivACTPluginWrapper.GetCombatantByName(name);
				return _playerMapper.MapToPlayer(combatant);
			}
			catch (Exception)
			{
				return new Player {Name = ACTWrapper.GetInstance().GetCharacterName()};
			}
		}

		public void DeInit()
		{
			DisableLogLineParser();
		}

		private static void InitWrappers()
		{
			_actWrapper = ACTWrapper.GetInstance();
			FFXIVACTPluginWrapper.Initialize(_actWrapper);
			_ffxivACTPluginWrapper = FFXIVACTPluginWrapper.GetInstance();
		}

		public void InitGameData()
		{
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
			_playerMapper = new PlayerMapper(WorldService, ClassJobService);
			AddLanguage(GetCurrentLanguage());
		}

		public void Initialize()
		{
			InitGameData();
			EnableLogLineParser();
		}

		public ILocation GetCurrentLocation()
		{
			return LocationService.GetLocationById(Convert.ToInt32(_ffxivACTPluginWrapper.GetCurrentTerritoryId()));
		}

		public ILanguage GetCurrentLanguage()
		{
			var languageId = (int) _ffxivACTPluginWrapper.GetSelectedLanguage();
			if (languageId == 0 || languageId > 4) languageId = 1;
			return LanguageService.GetLanguageById(languageId);
		}

		public string GetAppDirectory()
		{
			return _actWrapper.GetAppDataFolderFullName();
		}

		public static Aetherbridge GetInstance()
		{
			if (_aetherbridge != null) return _aetherbridge;

			lock (Lock)
			{
				if (_aetherbridge == null) _aetherbridge = new Aetherbridge();
			}

			return _aetherbridge;
		}
	}
}