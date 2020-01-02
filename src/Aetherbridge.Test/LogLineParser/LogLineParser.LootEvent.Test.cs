using NUnit.Framework;

namespace ACT_FFXIV_Aetherbridge.Test.LogLineParser
{
    [TestFixture]
    public class LogLineParserLootEventTest
    {
        [Test]
        public void Parse_LogLine_0039_AddedLoot()
        {
            const string logLine =
                @"[17:41:23.000] 00:0039:A pair of warlock's tights has been added to the loot list.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("0039", logEvent.GameLogCode);
            Assert.AreEqual("17:41:23.000", logEvent.Timestamp);
            Assert.AreEqual("A pair of warlock's tights has been added to the loot list.", logEvent.LogMessage);
            Assert.AreEqual("pair of warlock's tights", lootEvent.Item.SingularName);
            Assert.AreEqual(1, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
        }

        [Test]
        public void Parse_LogLine_0039_LostLoot()
        {
            const string logLine = @"[23:13:29.000] 00:0039:Unable to obtain the lump of dryad sap.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("0039", logEvent.GameLogCode);
            Assert.AreEqual("23:13:29.000", logEvent.Timestamp);
            Assert.AreEqual("Unable to obtain the lump of dryad sap.", logEvent.LogMessage);
            Assert.AreEqual("lump of dryad sap", lootEvent.Item.SingularName);
            Assert.AreEqual(1, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
        }

        [Test]
        public void Parse_LogLine_0839_ObtainsLoot()
        {
            const string logLine = @"[20:30:25.000] 00:0839:You obtain a tiny key.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("0839", logEvent.GameLogCode);
            Assert.AreEqual("20:30:25.000", logEvent.Timestamp);
            Assert.AreEqual("Combatant One obtains a tiny key.", logEvent.LogMessage);
            Assert.AreEqual("tiny key", lootEvent.Item.SingularName);
            Assert.AreEqual(1, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
            Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
        }


        [Test]
        public void Parse_LogLine_083e_ObtainsLoot_01()
        {
            const string logLine =
                @"[21:20:24.000] 00:103e:Blue ZooAdamantoise obtains a pair of Valerian terror knight's sollerets.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("103e", logEvent.GameLogCode);
            Assert.AreEqual("21:20:24.000", logEvent.Timestamp);
            Assert.AreEqual("Blue Zoo obtains a pair of Valerian terror knight's sollerets.", logEvent.LogMessage);
            Assert.AreEqual("pair of Valerian terror knight's sollerets", lootEvent.Item.SingularName);
            Assert.AreEqual(1, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
            Assert.AreEqual("Blue Zoo", lootEvent.Actor.Name);
        }

        //
        [Test]
        public void Parse_LogLine_083e_ObtainsLoot_02()
        {
            const string logLine = @"[16:35:53.000] 00:083e:You obtain the Blazing Sun.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("083e", logEvent.GameLogCode);
            Assert.AreEqual("16:35:53.000", logEvent.Timestamp);
            Assert.AreEqual("Combatant One obtains the Blazing Sun.", logEvent.LogMessage);
            Assert.AreEqual("the Blazing Sun", lootEvent.Item.SingularName);
            Assert.AreEqual(1, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
            Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
        }

        [Test]
        public void Parse_LogLine_083e_ObtainsLoot_03()
        {
            const string logLine =
                @"[16:35:53.000] 00:083e:You discover and obtain 2 pots of general-purpose pastel purple dye─items most rare!";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("083e", logEvent.GameLogCode);
            Assert.AreEqual("16:35:53.000", logEvent.Timestamp);
            Assert.AreEqual("Combatant One obtains 2 pots of general-purpose pastel purple dye.", logEvent.LogMessage);
            Assert.AreEqual("pot of general-purpose pastel purple dye", lootEvent.Item.SingularName);
            Assert.AreEqual(2, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
            Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
        }

        [Test]
        public void Parse_LogLine_083e_ObtainsLoot_04()
        {
            const string logLine = @"[21:56:57.000] 00:083e:You obtain 1,200 gil.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("083e", logEvent.GameLogCode);
            Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
            Assert.AreEqual("Combatant One obtains 1,200 gil.", logEvent.LogMessage);
            Assert.AreEqual("gil", lootEvent.Item.SingularName);
            Assert.AreEqual(1200, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
            Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
        }

        [Test]
        public void Parse_LogLine_083e_ObtainsLoot_05()
        {
            const string logLine =
                @"[21:56:57.000] 00:083e:A bonus of 356,000 experience points and 6,000 gil has been awarded for swift first-time completion of duty objectives.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("083e", logEvent.GameLogCode);
            Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
            Assert.AreEqual("Combatant One obtains 6,000 gil.", logEvent.LogMessage);
            Assert.AreEqual("gil", lootEvent.Item.SingularName);
            Assert.AreEqual(6000, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
            Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
        }

        [Test]
        public void Parse_LogLine_083e_ObtainsLoot_06()
        {
            const string logLine =
                @"[21:56:57.000] 00:083e:You discover and obtain a gatherer's guerdon materia VII─an item most rare!";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("083e", logEvent.GameLogCode);
            Assert.AreEqual("21:56:57.000", logEvent.Timestamp);
            Assert.AreEqual("Combatant One obtains a gatherer's guerdon materia VII.", logEvent.LogMessage);
            Assert.AreEqual("gatherer's guerdon materia VII", lootEvent.Item.SingularName);
            Assert.AreEqual(1, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
            Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
        }

        //

        [Test]
        public void Parse_LogLine_0841_GreedLoot_01()
        {
            const string logLine =
                @"[23:03:55.000] 00:0841:You roll Greed on the pair of Dravanian jackboots of healing. 28!";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("0841", logEvent.GameLogCode);
            Assert.AreEqual("23:03:55.000", logEvent.Timestamp);
            Assert.AreEqual("Combatant One rolls Greed on the pair of Dravanian jackboots of healing. 28!",
                logEvent.LogMessage);
            Assert.AreEqual("pair of Dravanian jackboots of healing", lootEvent.Item.SingularName);
            Assert.AreEqual(1, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
        }

        [Test]
        public void Parse_LogLine_0841_NeedLoot_01()
        {
            const string logLine = @"[23:12:36.000] 00:0841:You roll Need on the Dravanian bracelet of casting. 22!";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("0841", logEvent.GameLogCode);
            Assert.AreEqual("23:12:36.000", logEvent.Timestamp);
            Assert.AreEqual("Combatant One rolls Need on the Dravanian bracelet of casting. 22!", logEvent.LogMessage);
            Assert.AreEqual("Dravanian bracelet of casting", lootEvent.Item.SingularName);
            Assert.AreEqual(1, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
        }

        [Test]
        public void Parse_LogLine_103e_ObtainsLoot_01()
        {
            const string logLine = @"[17:37:35.000] 00:103e:John GilgameshSiren obtains 2 pieces of hippogryph sinew.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("103e", logEvent.GameLogCode);
            Assert.AreEqual("17:37:35.000", logEvent.Timestamp);
            Assert.AreEqual("John Gilgamesh obtains 2 pieces of hippogryph sinew.", logEvent.LogMessage);
            Assert.AreEqual("pieces of hippogryph sinew", lootEvent.Item.PluralName);
            Assert.AreEqual(2, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
            Assert.AreEqual("John Gilgamesh", lootEvent.Actor.Name);
        }

        [Test]
        public void Parse_LogLine_103e_ObtainsLoot_02()
        {
            const string logLine = @"[21:23:42.000] 00:083e:You obtain 60 Allagan tomestones of poetics.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("083e", logEvent.GameLogCode);
            Assert.AreEqual("21:23:42.000", logEvent.Timestamp);
            Assert.AreEqual("Combatant One obtains 60 Allagan tomestones of poetics.", logEvent.LogMessage);
            Assert.AreEqual("Allagan tomestones of poetics", lootEvent.Item.PluralName);
            Assert.AreEqual(60, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
            Assert.AreEqual("Combatant One", lootEvent.Actor.Name);
        }

        [Test]
        public void Parse_LogLine_1041_GreedLoot_01()
        {
            const string logLine = @"[23:08:44.000] 00:1041:Blue Zoo rolls Greed on the Dravanian hat of healing. 92!";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("1041", logEvent.GameLogCode);
            Assert.AreEqual("23:08:44.000", logEvent.Timestamp);
            Assert.AreEqual("Blue Zoo rolls Greed on the Dravanian hat of healing. 92!", logEvent.LogMessage);
            Assert.AreEqual("Dravanian hat of healing", lootEvent.Item.SingularName);
            Assert.AreEqual(1, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
        }

        [Test]
        public void Parse_LogLine_1041_NeedLoot_01()
        {
            const string logLine =
                @"[23:12:36.000] 00:1041:Blue Zoo rolls Need on the Dravanian bracelet of casting. 11!";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("1041", logEvent.GameLogCode);
            Assert.AreEqual("23:12:36.000", logEvent.Timestamp);
            Assert.AreEqual("Blue Zoo rolls Need on the Dravanian bracelet of casting. 11!", logEvent.LogMessage);
            Assert.AreEqual("Dravanian bracelet of casting", lootEvent.Item.SingularName);
            Assert.AreEqual(1, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
        }

        [Test]
        public void Parse_LogLine_1041_NeedLoot_02()
        {
            const string logLine =
                @"[22:07:14.000] 00:1041:Blue ZooMidgardsormr rolls Need on the 5 bottles of lime sulphur. 15!";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            var lootEvent = logEvent.XIVEvent;
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNotNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("1041", logEvent.GameLogCode);
            Assert.AreEqual("22:07:14.000", logEvent.Timestamp);
            Assert.AreEqual("Blue Zoo rolls Need on the 5 bottles of lime sulphur. 15!", logEvent.LogMessage);
            Assert.AreEqual("bottle of lime sulphur", lootEvent.Item.SingularName);
            Assert.AreEqual(5, lootEvent.Item.Quantity);
            Assert.AreEqual(false, lootEvent.Item.IsHQ);
        }
    }
}