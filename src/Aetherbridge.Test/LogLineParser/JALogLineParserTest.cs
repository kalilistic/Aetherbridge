using NUnit.Framework;

namespace ACT_FFXIV_Aetherbridge.Test
{
	[TestFixture]
	public class JALogLineParserTest
	{
		private AetherbridgeMock _aetherbridge;
		private ILogLineParser _parser;

		[OneTimeSetUp]
		public void SetUp()
		{
			var language = new Language(4, "Japanese");
			_aetherbridge = (AetherbridgeMock) AetherbridgeMock.GetInstance(language);
			_aetherbridge.LanguageService.UpdateCurrentLanguage(language);
			_aetherbridge.AddLanguage(language);
			_parser = new JALogLineParser(_aetherbridge);
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			_aetherbridge.DeInit();
		}

		[Test]
		public void Parse_LogLine_AddedLoot_Standard()
		{
			const string logLine =
				@"[17:41:23.000] 00:0039:ヨルハ五一式軍装:射が戦利品に追加されました。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0039", logEvent.GameLogCode);
			Assert.AreEqual("17:41:23.000", logEvent.Timestamp);
			Assert.AreEqual("ヨルハ五一式軍装:射が戦利品に追加されました。", logEvent.LogMessage);
			Assert.AreEqual("ヨルハ五一式軍装:射", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_GreedLoot_TheyRoll()
		{
			const string logLine = @"[23:08:44.000] 00:1041:Blue ZooJenovaはヨルハ五一式軍靴:重にGREEDのダイスで64を出した。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("1041", logEvent.GameLogCode);
			Assert.AreEqual("23:08:44.000", logEvent.Timestamp);
			Assert.AreEqual("Blue Zooはヨルハ五一式軍靴:重にGREEDのダイスで64を出した。", logEvent.LogMessage);
			Assert.AreEqual("ヨルハ五一式軍靴:重", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}


		[Test]
		public void Parse_LogLine_GreedLoot_YouRoll()
		{
			const string logLine =
				@"[23:03:55.000] 00:0841:Combatant Oneはカード:９ＳにGREEDのダイスで81を出した。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0841", logEvent.GameLogCode);
			Assert.AreEqual("23:03:55.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant Oneはカード:９ＳにGREEDのダイスで81を出した。",
				logEvent.LogMessage);
			Assert.AreEqual("カード:９Ｓ", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_LostLoot_Standard()
		{
			const string logLine = @"[23:13:29.000] 00:0039:物資コンテナ:二号B型防具を手に入れることができなかった。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0039", logEvent.GameLogCode);
			Assert.AreEqual("23:13:29.000", logEvent.Timestamp);
			Assert.AreEqual("物資コンテナ:二号B型防具を手に入れることができなかった。", logEvent.LogMessage);
			Assert.AreEqual("物資コンテナ:二号B型防具", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_NeedLoot_TheyRoll()
		{
			const string logLine = @"[23:08:44.000] 00:1041:Blue ZooJenovaはヨルハ五一式軍靴:重にNEEDのダイスで64を出した。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("1041", logEvent.GameLogCode);
			Assert.AreEqual("23:08:44.000", logEvent.Timestamp);
			Assert.AreEqual("Blue Zooはヨルハ五一式軍靴:重にNEEDのダイスで64を出した。", logEvent.LogMessage);
			Assert.AreEqual("ヨルハ五一式軍靴:重", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_NeedLoot_YouRoll()
		{
			const string logLine =
				@"[23:03:55.000] 00:0841:Combatant Oneはカード:９ＳにNEEDのダイスで81を出した。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0841", logEvent.GameLogCode);
			Assert.AreEqual("23:03:55.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant Oneはカード:９ＳにNEEDのダイスで81を出した。",
				logEvent.LogMessage);
			Assert.AreEqual("カード:９Ｓ", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_BadItem()
		{
			const string logLine = @"[20:30:25.000] 00:0839:Y1,000ギギギを手に入れた。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNull(logEvent.XIVEvent);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_HQ()
		{
			const string logLine =
				@"[21:56:57.000] 00:083e:Combatant Oneはドードーの笹身を手に入れた。"; //00:083e:Combatant Oneはドードーの笹身を手に入れた。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant Oneはドードーの笹身(HQ)を手に入れた。", logEvent.LogMessage);
			Assert.AreEqual("ドードーの笹身", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(true, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_ItemMostRare()
		{
			const string logLine =
				@"[21:56:57.000] 00:083e:Combatant Oneは希少なほりだしもの博識のメガマテリジャを入手した！";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant Oneは博識のメガマテリジャを手に入れた。", logEvent.LogMessage);
			Assert.AreEqual("博識のメガマテリジャ", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_ItemsMostRare()
		{
			const string logLine =
				@"[16:35:53.000] 00:083e:Combatant Oneは希少なほりだしものカララント:ダークグリーン×2を入手した！";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("16:35:53.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant Oneはカララント:ダークグリーン×2を手に入れた。", logEvent.LogMessage);
			Assert.AreEqual("カララント:ダークグリーン", lootEvent.Item.SingularName);
			Assert.AreEqual(2, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_MultipleWorlds()
		{
			const string logLine =
				@"[21:20:24.000] 00:103e:Flying SirenGilgameshはカード:９Ｓを手に入れた。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("103e", logEvent.GameLogCode);
			Assert.AreEqual("21:20:24.000", logEvent.Timestamp);
			Assert.AreEqual("Flying Sirenはカード:９Ｓを手に入れた。", logEvent.LogMessage);
			Assert.AreEqual("カード:９Ｓ", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Flying Siren", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_Standard()
		{
			const string logLine = @"[21:56:57.000] 00:083e:1,000ギルを手に入れた。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant Oneはギル×1,000を手に入れた。", logEvent.LogMessage);
			Assert.AreEqual("ギル", lootEvent.Item.SingularName);
			Assert.AreEqual(1000, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_TheyObtain()
		{
			const string logLine =
				@"[21:20:24.000] 00:103e:Flying Hippoはカード:９Ｓを手に入れた。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("103e", logEvent.GameLogCode);
			Assert.AreEqual("21:20:24.000", logEvent.Timestamp);
			Assert.AreEqual("Flying Hippoはカード:９Ｓを手に入れた。", logEvent.LogMessage);
			Assert.AreEqual("カード:９Ｓ", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Flying Hippo", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_YouObtain()
		{
			const string logLine = @"[16:35:53.000] 00:083e:Combatant Oneは白銀床板を入手した。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("16:35:53.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant Oneは白銀床板を入手した。", logEvent.LogMessage);
			Assert.AreEqual("白銀床板", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_YouObtain_MultipleItems()
		{
			const string logLine = @"[21:23:42.000] 00:083e:Combatant Oneは発光性アイスクリスタル×3を手に入れた。";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:23:42.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant Oneは発光性アイスクリスタル×3を手に入れた。", logEvent.LogMessage);
			Assert.AreEqual("発光性アイスクリスタル", lootEvent.Item.SingularName);
			Assert.AreEqual(3, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}
	}
}