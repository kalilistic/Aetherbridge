using ACT_FFXIV_Aetherbridge.Test.Mock;
using NUnit.Framework;

namespace ACT_FFXIV_Aetherbridge.Test.LogLineParser
{
	[TestFixture]
	public class FRLogLineParserTest
	{
		private AetherbridgeMock _aetherbridge;
		private ILogLineParser _parser;

		[OneTimeSetUp]
		public void SetUp()
		{
			var language = new Language(2, "French");
			_aetherbridge = (AetherbridgeMock) AetherbridgeMock.GetInstance(language);
			_aetherbridge.CurrentLanguage = language;
			_aetherbridge.AddLanguage(language);
			_parser = new FRLogLineParser(_aetherbridge);
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
				@"[17:41:23.000] 00:0039:Une  paire de bottines YoRHa type 51: unité de soin  a été ajoutée au butin.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0039", logEvent.GameLogCode);
			Assert.AreEqual("17:41:23.000", logEvent.Timestamp);
			Assert.AreEqual("Une paire de bottines YoRHa type 51: unité de soin a été ajoutée au butin.",
				logEvent.LogMessage);
			Assert.AreEqual("paire de bottines YoRHa type 51: unité de soin", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_GreedLoot_TheyRoll()
		{
			const string logLine =
				@"[23:08:44.000] 00:1041:Elmo Dy'grunJenova jette les dés (Cupidité) pour le  masque YoRHa type 51: unité de mêlée . 82!";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("1041", logEvent.GameLogCode);
			Assert.AreEqual("23:08:44.000", logEvent.Timestamp);
			Assert.AreEqual("Elmo Dy'grun jette les dés (Cupidité) pour le masque YoRHa type 51: unité de mêlée. 82!",
				logEvent.LogMessage);
			Assert.AreEqual("masque YoRHa type 51: unité de mêlée", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}


		[Test]
		public void Parse_LogLine_GreedLoot_YouRoll()
		{
			const string logLine =
				@"[23:03:55.000] 00:0841:Vous jetez les dés (Cupidité) pour le  rouleau d'orchestrion “Alien Manifestation”. 71!";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0841", logEvent.GameLogCode);
			Assert.AreEqual("23:03:55.000", logEvent.Timestamp);
			Assert.AreEqual(
				"Combatant One jette les dés (Cupidité) pour le rouleau d'orchestrion “Alien Manifestation”. 71!",
				logEvent.LogMessage);
			Assert.AreEqual("rouleau d'orchestrion “Alien Manifestation”", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_LostLoot_Standard()
		{
			const string logLine =
				@"[23:13:29.000] 00:0039:Impossible d'obtenir la  paire de bottines YoRHa type 51: unité paranormale .";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0039", logEvent.GameLogCode);
			Assert.AreEqual("23:13:29.000", logEvent.Timestamp);
			Assert.AreEqual("Impossible d'obtenir la paire de bottines YoRHa type 51: unité paranormale.",
				logEvent.LogMessage);
			Assert.AreEqual("paire de bottines YoRHa type 51: unité paranormale", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_NeedLoot_MultipleItems()
		{
			const string logLine =
				@"[22:07:14.000] 00:1041:Frank Sun-d'azur jette les dés (Besoin) pour la 2  cartes 9S. 67!";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("1041", logEvent.GameLogCode);
			Assert.AreEqual("22:07:14.000", logEvent.Timestamp);
			Assert.AreEqual("Frank Sun-d'azur jette les dés (Besoin) pour la 2 cartes 9S. 67!", logEvent.LogMessage);
			Assert.AreEqual("carte 9S", lootEvent.Item.SingularName);
			Assert.AreEqual(2, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_NeedLoot_TheyRoll()
		{
			const string logLine =
				@"[23:08:44.000] 00:1041:Frank Sun-d'azur jette les dés (Besoin) pour la  carte 9S. 67!";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("1041", logEvent.GameLogCode);
			Assert.AreEqual("23:08:44.000", logEvent.Timestamp);
			Assert.AreEqual("Frank Sun-d'azur jette les dés (Besoin) pour la carte 9S. 67!", logEvent.LogMessage);
			Assert.AreEqual("carte 9S", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_NeedLoot_YouRoll()
		{
			const string logLine =
				@"[23:12:36.000] 00:0841:Vous jetez les dés (Besoin) pour le  rouleau d'orchestrion “Alien Manifestation”. 71!";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("0841", logEvent.GameLogCode);
			Assert.AreEqual("23:12:36.000", logEvent.Timestamp);
			Assert.AreEqual(
				"Combatant One jette les dés (Besoin) pour le rouleau d'orchestrion “Alien Manifestation”. 71!",
				logEvent.LogMessage);
			Assert.AreEqual("rouleau d'orchestrion “Alien Manifestation”", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_BadItem()
		{
			const string logLine = @"[20:30:25.000] 00:0839:Vous obtenez 2057 gills.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNull(logEvent.XIVEvent);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_HQ()
		{
			const string logLine =
				@"[21:56:57.000] 00:083e:Vous obtenez un  tendon flexible.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One obtient un tendon flexible (HQ).", logEvent.LogMessage);
			Assert.AreEqual("tendon flexible", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(true, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_ItemMostRare()
		{
			const string logLine =
				@"[21:56:57.000] 00:083e:Vous découvrez et obtenez une  matéria de la collecte VIII. Un objet des plus rares!";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One obtenez une matéria de la collecte VIII.", logEvent.LogMessage);
			Assert.AreEqual("matéria de la collecte VIII", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_ItemsMostRare()
		{
			const string logLine =
				@"[16:35:53.000] 00:083e:Vous découvrez et obtenez 2  pots de teinture orange métallique. Des objets rares!";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("16:35:53.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One obtenez 2 pots de teinture orange métallique.", logEvent.LogMessage);
			Assert.AreEqual("pot de teinture orange métallique", lootEvent.Item.SingularName);
			Assert.AreEqual(2, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_MultipleWorlds()
		{
			const string logLine =
				@"[17:37:35.000] 00:103e:John GilgameshSiren obtient une  paire de bottines YoRHa type 51: unité de soin .";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("103e", logEvent.GameLogCode);
			Assert.AreEqual("17:37:35.000", logEvent.Timestamp);
			Assert.AreEqual("John Gilgamesh obtient une paire de bottines YoRHa type 51: unité de soin.",
				logEvent.LogMessage);
			Assert.AreEqual("paires de bottines YoRHa type 51: unité de soin", lootEvent.Item.PluralName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("John Gilgamesh", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_Standard()
		{
			const string logLine = @"[21:56:57.000] 00:083e:Vous obtenez 1000 gils.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One obtient 1000 gils.", logEvent.LogMessage);
			Assert.AreEqual("gil", lootEvent.Item.SingularName);
			Assert.AreEqual(1000, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_TheyObtain()
		{
			const string logLine =
				@"[21:20:24.000] 00:103e:Purple Gr'eenJenova obtient une  carte 9S.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("103e", logEvent.GameLogCode);
			Assert.AreEqual("21:20:24.000", logEvent.Timestamp);
			Assert.AreEqual("Purple Gr'een obtient une carte 9S.", logEvent.LogMessage);
			Assert.AreEqual("carte 9S", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Purple Gr'een", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_YouObtain()
		{
			const string logLine = @"[16:35:53.000] 00:083e:Vous obtenez une  matéria du regard divin VII.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("16:35:53.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One obtient une matéria du regard divin VII.", logEvent.LogMessage);
			Assert.AreEqual("matéria du regard divin VII", lootEvent.Item.SingularName);
			Assert.AreEqual(1, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}

		[Test]
		public void Parse_LogLine_ObtainsLoot_YouObtain_MultipleItems()
		{
			const string logLine = @"[21:23:42.000] 00:083e:Vous obtenez 2  peaux souples.";
			var logEvent = _parser.Parse(new ACTLogLineEvent {LogLine = logLine});
			var lootEvent = logEvent.XIVEvent;
			Assert.IsNotNull(logEvent.Id);
			Assert.IsNotNull(logEvent.XIVEvent);
			Assert.AreEqual("00", logEvent.LogCode);
			Assert.AreEqual("083e", logEvent.GameLogCode);
			Assert.AreEqual("21:23:42.000", logEvent.Timestamp);
			Assert.AreEqual("Combatant One obtient 2 peaux souples.", logEvent.LogMessage);
			Assert.AreEqual("peaux souples", lootEvent.Item.PluralName);
			Assert.AreEqual(2, lootEvent.Item.Quantity);
			Assert.AreEqual(false, lootEvent.Item.IsHQ);
			Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
		}
	}
}