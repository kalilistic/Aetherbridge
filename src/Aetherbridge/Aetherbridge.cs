using System;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
	public class Aetherbridge : IAetherbridge
	{
		private static volatile Aetherbridge _aetherbridge;
		private static readonly object Lock = new object();
		private static IACTWrapper _actWrapper;
		private static IFFXIVACTPluginWrapper _ffxivACTPluginWrapper;
		internal ILogLineParser LogLineParser;

		private Aetherbridge()
		{
			InitWrappers();
			InitGameData();
			AetherbridgeConfig = new AetherbridgeConfig();
		}

		public ClassJobService ClassJobService { get; set; }
		public WorldService WorldService { get; set; }
		public LocationService LocationService { get; set; }
		public ContentService ContentService { get; set; }
		public ItemService ItemService { get; set; }
		public LanguageService LanguageService { get; set; }
		public PlayerService PlayerService { get; set; }
		public AetherbridgeConfig AetherbridgeConfig { get; set; }

		public void AddLanguage(Language language)
		{
			ClassJobService.AddLanguage(language);
			LocationService.AddLanguage(language);
			ContentService.AddLanguage(language);
			ItemService.AddLanguage(language);
		}

		public event EventHandler<LogLineEvent> LogLineCaptured;


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
			PlayerService = new PlayerService(_actWrapper, _ffxivACTPluginWrapper, WorldService, ClassJobService);
			AddLanguage(GetCurrentLanguage());
		}

		public void Initialize()
		{
			InitGameData();
			EnableLogLineParser();
		}

		public Language GetCurrentLanguage()
		{
			var languageId = (int) _ffxivACTPluginWrapper.GetSelectedLanguage();
			if (languageId == 0 || languageId > 4) languageId = 1;
			return LanguageService.GetLanguageById(languageId);
		}

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

		public Location GetCurrentLocation()
		{
			return LocationService.GetLocationById(Convert.ToInt32(_ffxivACTPluginWrapper.GetCurrentTerritoryId()));
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