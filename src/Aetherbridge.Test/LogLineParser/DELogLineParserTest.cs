using NUnit.Framework;

namespace ACT_FFXIV_Aetherbridge.Test
{
	[TestFixture]
	public class DELogLineParserTest
	{
		private AetherbridgeMock _aetherbridge;
		private ILogLineParser _parser;

		[OneTimeSetUp]
		public void SetUp()
		{
			var language = new Language(3, "German", "de");
			_aetherbridge = (AetherbridgeMock) AetherbridgeMock.GetInstance(language);
			_aetherbridge.LanguageService.UpdateCurrentLanguage(language);
			_aetherbridge.AddLanguage(language);
			_parser = new DELogLineParser(new DELogLineParserContext(_aetherbridge));
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
				@"[17:41:23.000] 00:0039:Ihr habt Beutegut (eine  YoRHa-Haube des Spähens Modell Nr. 51) erhalten.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0039", logEvent.GameLogCode);
			Assert.AreEqual("17:41:23.000", logEvent.Timestamp);
			Assert.AreEqual("Ihr habt Beutegut (eine YoRHa-Haube des Spähens Modell Nr. 51) erhalten.",
				logEvent.LogMessage);
			Assert.AreEqual("YoRHa-Haube[p] des Spähens Modell Nr. 51", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_GreedLoot_TheyRoll()
		{
			const string logLine = @"[23:08:44.000] 00:1041:Blue Zoo würfelt mit „Bedarf“ eine 80 auf die  9S-Karte.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("1041", logEvent.GameLogCode);
			Assert.AreEqual("23:08:44.000", logEvent.Timestamp);
			Assert.AreEqual("Blue Zoo würfelt mit „Bedarf“ eine 80 auf die 9S-Karte.", logEvent.LogMessage);
			Assert.AreEqual("9S-Karte", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}


		[Test]
		public void Parse_LogLine_GreedLoot_YouRoll()
		{
			const string logLine =
				@"[23:03:55.000] 00:0841:Du würfelst mit „Gier“ eine 28 auf die Notenrolle von „Alien Manifestation“.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0841", logEvent.GameLogCode);
			Assert.AreEqual("23:03:55.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One würfelt mit „Gier“ eine 28 auf die Notenrolle von „Alien Manifestation“.",
				logEvent.LogMessage);
			Assert.AreEqual("Notenrolle[p] von „Alien Manifestation“", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_LostLoot_Standard()
		{
			const string logLine =
				@"[23:13:29.000] 00:0039:Du konntest die  YoRHa-Jacke des Spähens Modell Nr. 51 nicht erhalten.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0039", logEvent.GameLogCode);
			Assert.AreEqual("23:13:29.000", logEvent.Timestamp);
			Assert.AreEqual("Du konntest die YoRHa-Jacke des Spähens Modell Nr. 51 nicht erhalten.",
				logEvent.LogMessage);
			Assert.AreEqual("YoRHa-Jacke[p] des Spähens Modell Nr. 51", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_NeedLoot_MultipleItems()
		{
			const string logLine =
				@"[22:07:14.000] 00:1041:Blue Zoo würfelt mit „Gier“ eine 80 auf die 2  9S-Karten.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("1041", logEvent.GameLogCode);
			Assert.AreEqual("22:07:14.000", logEvent.Timestamp);
			Assert.AreEqual("Blue Zoo würfelt mit „Gier“ eine 80 auf die 2 9S-Karten.", logEvent.LogMessage);
			Assert.AreEqual("9S-Karte", lootEvent.Item.SingularName);
			Assert.AreEqual(2, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_NeedLoot_TheyRoll()
		{
			const string logLine = @"[23:08:44.000] 00:1041:Blue Zoo würfelt mit „Gier“ eine 80 auf die  9S-Karte.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("1041", logEvent.GameLogCode);
			Assert.AreEqual("23:08:44.000", logEvent.Timestamp);
			Assert.AreEqual("Blue Zoo würfelt mit „Gier“ eine 80 auf die 9S-Karte.", logEvent.LogMessage);
			Assert.AreEqual("9S-Karte", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_NeedLoot_YouRoll()
		{
			const string logLine =
				@"[23:12:36.000] 00:0841:Du würfelst mit „Bedarf“ eine 28 auf die Notenrolle von „Alien Manifestation“.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0841", logEvent.GameLogCode);
			Assert.AreEqual("23:12:36.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One würfelt mit „Bedarf“ eine 28 auf die Notenrolle von „Alien Manifestation“.",
				logEvent.LogMessage);
			Assert.AreEqual("Notenrolle[p] von „Alien Manifestation“", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_BadItem()
		{
			const string logLine = @"[20:30:25.000] 00:0839:Du hast 1.000 gil-fake-item erhalten.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNull(logEvent.XIVEvent);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_HQ()
		{
			const string logLine =
				@"[21:56:57.000] 00:083e:Du hast ein  Megalokrabbenbein  erhalten.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One hat ein Megalokrabbenbein (HQ) erhalten.", logEvent.LogMessage);
			Assert.AreEqual("Megalokrabbenbein", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(true, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_ItemMostRare()
		{
			const string logLine =
				@"[21:56:57.000] 00:083e:Du findest und erhältst eine  Sammlersinn-Materia VII - ein höchst seltener Gegenstand!";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One hat Sammlersinn-Materia VII.", logEvent.LogMessage);
			Assert.AreEqual("Sammlersinn-Materia[p] VII", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_ItemsMostRare()
		{
			const string logLine =
				@"[16:35:53.000] 00:083e:Du findest und erhältst 2  Töpfe dunkelgrünen Farbstoffs - ein höchst seltener Fund!";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("16:35:53.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One hat 2 Töpfe dunkelgrünen Farbstoffs.", logEvent.LogMessage);
			Assert.AreEqual("Topf[p] dunkelgrünen Farbstoffs", lootEvent.Item.SingularName);
			Assert.AreEqual(2, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_MultipleWorlds()
		{
			const string logLine =
				@"[17:37:35.000] 00:103e:John GilgameshSiren hat ein  Paar YoRHa-Handschuhe der Heilung Modell Nr. 51 erhalten.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("103e", logEvent.GameLogCode);
			Assert.AreEqual("17:37:35.000", logEvent.Timestamp);
			Assert.AreEqual("John Gilgamesh hat ein Paar YoRHa-Handschuhe der Heilung Modell Nr. 51 erhalten.",
				logEvent.LogMessage);
			Assert.AreEqual("Paar[p] YoRHa-Handschuhe der Heilung Modell Nr. 51", lootEvent.Item.PluralName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("John Gilgamesh", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_Standard()
		{
			const string logLine = @"[21:56:57.000] 00:083e:Du hast 1.000 Gil erhalten.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One hat 1.000 Gil erhalten.", logEvent.LogMessage);
			Assert.AreEqual("Gil", lootEvent.Item.SingularName);
			Assert.AreEqual(1000, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_TheyObtain()
		{
			const string logLine =
				@"[21:20:24.000] 00:103e:Blue Zoo hat einen  Heiltrank erhalten.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("103e", logEvent.GameLogCode);
			Assert.AreEqual("21:20:24.000", logEvent.Timestamp);
			Assert.AreEqual("Blue Zoo hat einen Heiltrank erhalten.", logEvent.LogMessage);
			Assert.AreEqual("Heiltrank", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Blue Zoo", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_YouObtain()
		{
			const string logLine = @"[16:35:53.000] 00:083e:Du hast  Die Brennende Sonne erhalten.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("16:35:53.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One hat Die Brennende Sonne erhalten.", logEvent.LogMessage);
			Assert.AreEqual("Die Brennende Sonne", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_YouObtain_MultipleItems()
		{
			const string logLine = @"[21:23:42.000] 00:083e:Du hast 20 Allagische Steine der Goëtie erhalten.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:23:42.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One hat 20 Allagische Steine der Goëtie erhalten.", logEvent.LogMessage);
			Assert.AreEqual("Allagisch[a] Steine[p] der Goëtie", lootEvent.Item.PluralName);
			Assert.AreEqual(20, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}
	}
}