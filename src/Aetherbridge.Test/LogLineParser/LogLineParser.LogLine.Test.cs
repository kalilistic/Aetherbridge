using NUnit.Framework;

namespace ACT_FFXIV_Aetherbridge.Test.LogLineParser
{
    [TestFixture]
    public class LogLineParserLogLineTest
    {
        [Test]
        public void Parse_LogLine_00_GameLog_01()
        {
            const string logLine = @"[18:30:00.000] 00:12a9:⇒ Direct hit! The drowned deckhand takes 180 damage.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("12a9", logEvent.GameLogCode);
            Assert.AreEqual("18:30:00.000", logEvent.Timestamp);
            Assert.AreEqual("Direct hit! The drowned deckhand takes 180 damage.", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_00_GameLog_02()
        {
            const string logLine = @"[20:33:25.000] 00:282b:The magitek vangob G-III uses Needle Burst.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.AreEqual("00", logEvent.LogCode);
            Assert.AreEqual("282b", logEvent.GameLogCode);
            Assert.AreEqual("20:33:25.000", logEvent.Timestamp);
            Assert.AreEqual("The magitek vangob G-III uses Needle Burst.", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_01_ChangeZone()
        {
            const string logLine = @"[18:41:52.820] 01:Changed Zone to Limsa Lominsa Lower Decks.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("01", logEvent.LogCode);
            Assert.AreEqual("18:41:52.820", logEvent.Timestamp);
            Assert.AreEqual("Changed Zone to Limsa Lominsa Lower Decks.", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_02_ChangePrimaryPlayer()
        {
            const string logLine = @"[18:41:52.820] 02:Changed primary player to John Smith.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("02", logEvent.LogCode);
            Assert.AreEqual("18:41:52.820", logEvent.Timestamp);
            Assert.AreEqual("Changed primary player to John Smith.", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_03_AddCombatant()
        {
            const string logLine =
                @"[18:30:21.818] 03:52343CD7:Added new combatant Spelaean Slug.  Job: N/A Level: 50 Max HP: 10419 Max MP: 2800 Pos: (198.0219,-67.46501,28.82607) (44440000003245).";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("03", logEvent.LogCode);
            Assert.AreEqual("18:30:21.818", logEvent.Timestamp);
            Assert.AreEqual(
                "52343CD7:Added new combatant Spelaean Slug.  Job: N/A Level: 50 Max HP: 10419 Max MP: 2800 Pos: (198.0219,-67.46501,28.82607) (44440000003245).",
                logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_04_RemoveCombatant()
        {
            const string logLine =
                @"[18:29:44.575] 04:50004CE8:Removing combatant Reaver Parrot.  Max HP: 17301. Pos: (262.1042,-205.7985,45.6841).";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("04", logEvent.LogCode);
            Assert.AreEqual("18:29:44.575", logEvent.Timestamp);
            Assert.AreEqual(
                "50004CE8:Removing combatant Reaver Parrot.  Max HP: 17301. Pos: (262.1042,-205.7985,45.6841).",
                logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_0C_PlayerStats()
        {
            const string logLine =
                @"[18:41:50.373] 0C:Player Stats: 22:323:444:246:364:415:302:240:612:274:164:515:187:141:143:0:441";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("0C", logEvent.LogCode);
            Assert.AreEqual("18:41:50.373", logEvent.Timestamp);
            Assert.AreEqual("Player Stats: 22:323:444:246:364:415:302:240:612:274:164:515:187:141:143:0:441",
                logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_14_NetworkStartsCasting()
        {
            const string logLine =
                @"[18:29:52.089] 14:9B1:Dripping Sallet starts using Bubble Bath on Dripping Sallet.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("14", logEvent.LogCode);
            Assert.AreEqual("18:29:52.089", logEvent.Timestamp);
            Assert.AreEqual("9B1:Dripping Sallet starts using Bubble Bath on Dripping Sallet.", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_15_NetworkAbility()
        {
            const string logLine =
                @"[18:29:40.494] 15:55503CD5:Spelaean Slug:368:Attack:4326DFAA:Hunter Blue:2E:0:430103:A10000:0:0:0:0:0:0:0:0:0:0:0:0:3344:3344:10000:10000:0:1000:318.2886:-197.8173:44.11405:-0.5383875:10164:10419:2800:2800:0:1000:320.2715:-200.3663:44.03292:-2.401525:000015B5";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("15", logEvent.LogCode);
            Assert.AreEqual("18:29:40.494", logEvent.Timestamp);
            Assert.AreEqual(
                "55503CD5:Spelaean Slug:368:Attack:4326DFAA:Hunter Blue:2E:0:430103:A10000:0:0:0:0:0:0:0:0:0:0:0:0:3344:3344:10000:10000:0:1000:318.2886:-197.8173:44.11405:-0.5383875:10164:10419:2800:2800:0:1000:320.2715:-200.3663:44.03292:-2.401525:000015B5",
                logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_16_NetworkAOEAbility()
        {
            const string logLine =
                @"[18:29:49.636] 16:432655BB:Hunter Blue:2C87:The Look:50003CD9:Drowned Steersman:730003:220000:1C:23378000:0:0:0:0:0:0:0:0:0:0:0:0:12744:12744:2800:2800:0:1000:274.9218:-200.6104:45.41054:0.6924591:4250:5344:9800:10000:0:1000:276.7223:-198.4436:45.57396:-2.119371:000015BE";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("16", logEvent.LogCode);
            Assert.AreEqual("18:29:49.636", logEvent.Timestamp);
            Assert.AreEqual(
                "432655BB:Hunter Blue:2C87:The Look:50003CD9:Drowned Steersman:730003:220000:1C:23378000:0:0:0:0:0:0:0:0:0:0:0:0:12744:12744:2800:2800:0:1000:274.9218:-200.6104:45.41054:0.6924591:4250:5344:9800:10000:0:1000:276.7223:-198.4436:45.57396:-2.119371:000015BE",
                logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_17_NetworkCancelAbility()
        {
            const string logLine = @"[18:32:01.964] 17:1066AA22:Hunter Blue:2C87:The Look:Interrupted:";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("17", logEvent.LogCode);
            Assert.AreEqual("18:32:01.964", logEvent.Timestamp);
            Assert.AreEqual("1066AA22:Hunter Blue:2C87:The Look:Interrupted:", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_18_NetworkDoT()
        {
            const string logLine = @"[18:32:04.871] 18:DoT Tick on Blue Zoo for 364 damage.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("18", logEvent.LogCode);
            Assert.AreEqual("18:32:04.871", logEvent.Timestamp);
            Assert.AreEqual("DoT Tick on Blue Zoo for 364 damage.", logEvent.LogMessage);
        }


        [Test]
        public void Parse_LogLine_19_NetworkDeath()
        {
            const string logLine = @"[18:30:57.954] 19:Spelaean Slug was defeated by Blue Zoo.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("19", logEvent.LogCode);
            Assert.AreEqual("18:30:57.954", logEvent.Timestamp);
            Assert.AreEqual("Spelaean Slug was defeated by Blue Zoo.", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_1A_NetworkBuff()
        {
            const string logLine =
                @"[18:29:55.887] 1A:50003CD5:Spelaean Slug gains the effect of Paralysis from Blue Zoo for 6.00 Seconds.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("1A", logEvent.LogCode);
            Assert.AreEqual("18:29:55.887", logEvent.Timestamp);
            Assert.AreEqual("50003CD5:Spelaean Slug gains the effect of Paralysis from Blue Zoo for 6.00 Seconds.",
                logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_1B_NetworkTargetIcon()
        {
            const string logLine = @"[18:34:37.706] 1B:5567BFF2:Blue Zoo:0000:0000:0001:0000:0000:0000:";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("1B", logEvent.LogCode);
            Assert.AreEqual("18:34:37.706", logEvent.Timestamp);
            Assert.AreEqual("5567BFF2:Blue Zoo:0000:0000:0001:0000:0000:0000:", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_1E_NetworkBuffRemove()
        {
            const string logLine =
                @"[18:30:01.136] 1E:50003CDB:Drowned Powder Monkey loses the effect of Paralysis from Blue Zoo.";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("1E", logEvent.LogCode);
            Assert.AreEqual("18:30:01.136", logEvent.Timestamp);
            Assert.AreEqual("50003CDB:Drowned Powder Monkey loses the effect of Paralysis from Blue Zoo.",
                logEvent.LogMessage);
        }


        [Test]
        public void Parse_LogLine_21_Network6D()
        {
            const string logLine = @"[18:39:30.322] 21:4343001C:22000004:12BF:00:00:00";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("21", logEvent.LogCode);
            Assert.AreEqual("18:39:30.322", logEvent.Timestamp);
            Assert.AreEqual("4343001C:22000004:12BF:00:00:00", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_22_NetworkNameToggle()
        {
            const string logLine =
                @"[18:37:38.394] 22:55503DE8:Drowned Powder Monkey:55503DE8:Drowned Powder Monkey:01";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("22", logEvent.LogCode);
            Assert.AreEqual("18:37:38.394", logEvent.Timestamp);
            Assert.AreEqual("55503DE8:Drowned Powder Monkey:55503DE8:Drowned Powder Monkey:01", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_23_NetworkTether()
        {
            const string logLine =
                @"[18:31:54.765] 23:22203D22:Sapphire:2222AAAA:Blue Zoo:035A:0000:0011:5556AAAA:000F:0000:";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("23", logEvent.LogCode);
            Assert.AreEqual("18:31:54.765", logEvent.Timestamp);
            Assert.AreEqual("22203D22:Sapphire:2222AAAA:Blue Zoo:035A:0000:0011:5556AAAA:000F:0000:",
                logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_24_LimitBreak()
        {
            const string logLine = @"[18:29:51.531] 24:Limit Break: 01C2";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("24", logEvent.LogCode);
            Assert.AreEqual("18:29:51.531", logEvent.Timestamp);
            Assert.AreEqual("Limit Break: 01C2", logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_25_NetworkEffectResult()
        {
            const string logLine =
                @"[18:29:39.776] 25:55503CB0:Dripping Sallet:000055B4:15615:12544:2550:2800:0::318.8094:-200.2462:43.95959:2.314356:03F8:02:23FF:0:";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("25", logEvent.LogCode);
            Assert.AreEqual("18:29:39.776", logEvent.Timestamp);
            Assert.AreEqual(
                "55503CB0:Dripping Sallet:000055B4:15615:12544:2550:2800:0::318.8094:-200.2462:43.95959:2.314356:03F8:02:23FF:0:",
                logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_26_NetworkStatusEffects()
        {
            const string logLine =
                @"[18:29:41.043] 26:1066AAAA:Blue Zoo:323CE224:5119:5344:10000:10000:0:0:316.5752:-195.1418:44.66523:-0.5896622:03E8:8E:0:084C:0:1055AAAA:06B7:0:1056AAAA:";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("26", logEvent.LogCode);
            Assert.AreEqual("18:29:41.043", logEvent.Timestamp);
            Assert.AreEqual(
                "1066AAAA:Blue Zoo:323CE224:5119:5344:10000:10000:0:0:316.5752:-195.1418:44.66523:-0.5896622:03E8:8E:0:084C:0:1055AAAA:06B7:0:1056AAAA:",
                logEvent.LogMessage);
        }

        [Test]
        public void Parse_LogLine_27_NetworkUpdateHP()
        {
            const string logLine =
                @"[18:29:42.399] 27:1369AAAA:Blue Zoo:5565:5565:9800:10000:0:0:321.2177:-203.509:44.18475:-0.6153755:";
            var logEvent = LogLineParserTestUtil.ParseEvent(logLine);
            Assert.IsNotNull(logEvent.Id);
            Assert.IsNull(logEvent.XIVEvent);
            Assert.IsNull(logEvent.GameLogCode);
            Assert.AreEqual("27", logEvent.LogCode);
            Assert.AreEqual("18:29:42.399", logEvent.Timestamp);
            Assert.AreEqual("1369AAAA:Blue Zoo:5565:5565:9800:10000:0:0:321.2177:-203.509:44.18475:-0.6153755:",
                logEvent.LogMessage);
        }
    }
}