using System;
using System.Windows.Forms;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge
{
	public class Aetherbridge : IAetherbridge
	{
		private static volatile Aetherbridge _aetherbridge;
		private static readonly object Lock = new object();
		private static IACTWrapper _actWrapper;
		private static IFFXIVACTPluginWrapper _ffxivACTPluginWrapper;
		internal ILogLineParserFactory LogLineParserFactory;

		private Aetherbridge()
		{
			AetherbridgeConfig = new AetherbridgeConfig();
			InitWrappers();
			InitGameData();
		}

		public ClassJobService ClassJobService { get; set; }
		public WorldService WorldService { get; set; }
		public LocationService LocationService { get; set; }
		public ContentService ContentService { get; set; }
		public ItemService ItemService { get; set; }
		public LanguageService LanguageService { get; set; }
		public PlayerService PlayerService { get; set; }
		public AetherbridgeConfig AetherbridgeConfig { get; set; }

		public void AddLanguage(int languageId)
		{
			var language = LanguageService.GetLanguageById(languageId);
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
			LanguageService = new LanguageService(languageRepository, _ffxivACTPluginWrapper, AetherbridgeConfig);
			var worldRepository = new GameDataRepository<FFXIV.CrescentCove.World>(gameDataManager.World);
			WorldService = new WorldService(worldRepository);
			var classJobRepository = new GameDataRepository<FFXIV.CrescentCove.ClassJob>(gameDataManager.ClassJob);
			ClassJobService = new ClassJobService(LanguageService, classJobRepository);
			LocationService = new LocationService(LanguageService, gameDataManager, _ffxivACTPluginWrapper);
			var contentRepository =
				new GameDataRepository<ContentFinderCondition>(gameDataManager.ContentFinderCondition);
			ContentService =
				new ContentService(LanguageService, _ffxivACTPluginWrapper.GetZoneList(), contentRepository);
			var itemRepository = new GameDataRepository<FFXIV.CrescentCove.Item>(gameDataManager.Item);
			ItemService = new ItemService(LanguageService, itemRepository);
			PlayerService = new PlayerService(_actWrapper, _ffxivACTPluginWrapper, WorldService, ClassJobService);
		}

		public void Initialize()
		{
			InitGameData();
			EnableLogLineParser();
		}

		public void InitLogLineParser()
		{
			var lang = LanguageService.GetCurrentLanguage();
			switch (lang.Id)
			{
				case 1:
					LogLineParserFactory = new ENLogLineParserFactory(this);
					break;
				case 2:
					LogLineParserFactory = new FRLogLineParserFactory(this);
					break;
				case 3:
					LogLineParserFactory = new DELogLineParserFactory(this);
					break;
				case 4:
					LogLineParserFactory = new JALogLineParserFactory(this);
					break;
			}
		}

		public void EnableLogLineParser()
		{
			if (AetherbridgeConfig.LogLineParserEnabled) return;
			if (LogLineParserFactory == null) InitLogLineParser();
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
			var logLineEvent = LogLineParserFactory.CreateParser().Parse(actLogLineEvent);
			if (!actLogLineEvent.IsImport && logLineEvent?.XIVEvent != null) {
				logLineEvent.XIVEvent.Location = LocationService.GetCurrentLocation();
				if (logLineEvent.XIVEvent.Actor != null)
				{
					var actor = PlayerService.GetPlayerByName(logLineEvent.XIVEvent.Actor.Name);
					if (actor != null)
					{
						actor.IsReporter = logLineEvent.XIVEvent.Actor.IsReporter;
						logLineEvent.XIVEvent.Actor = actor;
					}
				}
			}
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