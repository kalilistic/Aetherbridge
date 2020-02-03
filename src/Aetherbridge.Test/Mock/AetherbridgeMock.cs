using System;
using System.Collections.Generic;
using FFXIV.CrescentCove;

namespace ACT_FFXIV_Aetherbridge.Test
{
	internal class AetherbridgeMock : IAetherbridge
	{
		private static volatile AetherbridgeMock _aetherbridge;
		private static readonly object Lock = new object();
		private static IACTWrapper _actWrapper;
		private static IFFXIVACTPluginWrapper _ffxivACTPluginWrapper;
		internal ILogLineParserFactory LogLineParserFactory;
		public ClassJobService ClassJobService { get; set; }
		public WorldService WorldService { get; set; }
		public LocationService LocationService { get; set; }
		public ContentService ContentService { get; set; }
		public ItemService ItemService { get; set; }
		public LanguageService LanguageService { get; set; }
		public PlayerService PlayerService { get; set; }
		public AetherbridgeConfig AetherbridgeConfig { get; set; }
		#pragma warning disable 67
		public event EventHandler<LogLineEvent> LogLineCaptured;
		#pragma warning restore 67
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

		public void AddLanguage(int languageId)
		{
			var language = LanguageService.GetLanguageById(languageId);
			ClassJobService.AddLanguage(language);
			LocationService.AddLanguage(language);
			ContentService.AddLanguage(language);
			ItemService.AddLanguage(language);
		}

		public void DeInit()
		{
			_aetherbridge = null;
			LogLineParserFactory = null;
			AetherbridgeConfig = null;
		}

		public void EnableLogLineParser()
		{
			if (AetherbridgeConfig.LogLineParserEnabled) return;
			if (LogLineParserFactory == null) InitLogLineParser();
			AetherbridgeConfig.LogLineParserEnabled = true;
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

		private AetherbridgeMock()
		{
			AetherbridgeConfig = new AetherbridgeConfig();
			InitWrappers();
			InitGameData();
		}

		private static void InitWrappers()
		{
			_actWrapper = new ACTWrapperMock();
			_ffxivACTPluginWrapper = new FFXIVACTPluginWrapperMock();
		}

		public static AetherbridgeMock GetInstance()
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