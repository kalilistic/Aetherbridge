using System.Text;
using System.Text.RegularExpressions;

namespace ACT_FFXIV_Aetherbridge
{
    internal sealed class LogLineParser : ILogLineParser
    {
        public static readonly Regex TimestampRegex =
            new Regex(@"^\[(?<Timestamp>[0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]{3})] (?<Residual>.*)", RegexOptions.Compiled);

        public static readonly Regex LogLineCodeRegex =
            new Regex(@"^(?<LogCode>[^:]*):(?<Residual>.*)", RegexOptions.Compiled);

        public static readonly Regex GameLogCodeRegex =
            new Regex(@"^(?<GameLogCode>[^:]*):(?<Residual>.*)", RegexOptions.Compiled);

        internal static readonly Regex ObtainRegex =
            new Regex(@"^(?<ActorNameWithWorldName>[^\s]+( )?[^\s]+).+?(obtain(s)? )(?<RawItemName>.*?)\.",
                RegexOptions.Compiled);

        internal static readonly Regex ObtainWithExpPointsRegex =
            new Regex(@".+?(experience points and )(?<RawItemName>[^\s]+ gil) [^\s]+.", RegexOptions.Compiled);

        internal static readonly Regex ObtainWithMostRareRegex =
            new Regex(
                @"^(?<ActorNameWithWorldName>[^\s]+( )?[^\s]+)( discover and obtain )(?<RawItemName>.*?)(-((items)|(an item)) most rare!)",
                RegexOptions.Compiled);

        internal static readonly Regex UnableToObtainRegex =
            new Regex(@"^Unable to obtain (?<RawItemName>.*?)\.", RegexOptions.Compiled);

        internal static readonly Regex ItemNameRegex =
            new Regex(@"^((the )?(?<Quantity>[\d,\,]+[^\s]?) )?(?<ItemName>.*)", RegexOptions.Compiled);

        internal static readonly Regex ItemAddedRegex =
            new Regex(@"^(?<RawItemName>.*?) has been added to the loot list.", RegexOptions.Compiled);

        internal static readonly Regex GreedRegex =
            new Regex(
                @"^(?<ActorNameWithWorldName>[^\s]+[ ]?[^\s]+) roll[s]? Greed on (?<RawItemName>.*?)\. (?<Roll>.*)!",
                RegexOptions.Compiled);

        internal static readonly Regex NeedRegex = new Regex(
            @"^(?<ActorNameWithWorldName>[^\s]+[ ]?[^\s]+) roll[s]? Need on (?<RawItemName>.*?)\. (?<Roll>.*)!",
            RegexOptions.Compiled);

        private readonly IAetherbridge _aetherbridge;

        public readonly Regex ActorWithWorldNameRegex;

        private LogLineEvent _logLineEvent;
        private string _residualMessage;

        public LogLineParser(IAetherbridge aetherbridge)
        {
            _aetherbridge = aetherbridge;
            var worlds = _aetherbridge.WorldService.GetWorldsAsDelimitedString();
            ActorWithWorldNameRegex =
                new Regex(
                    @"(?<ActorName>[A-Za-z'\-\.]+ [A-Za-z'\-\.]+)(?<WorldName>" + worlds + ")",
                    RegexOptions.Compiled);
        }

        public LogLineEvent Parse(ACTLogLineEvent actLogLineEvent)
        {
            SetupParser(actLogLineEvent);
            ExtractTimestamp();
            ExtractLogCode();
            ExtractGameLogCode();
            ExtractMessage();
            UpdateLogMessage();
            ParseLootEvent();
            return _logLineEvent;
        }

        private void SetupParser(ACTLogLineEvent actLogLineEvent)
        {
            _logLineEvent = new LogLineEvent {ACTLogLineEvent = actLogLineEvent};
            _residualMessage = _logLineEvent.ACTLogLineEvent.LogLine;
        }

        private void ExtractTimestamp()
        {
            var match = MatchResidual(TimestampRegex);
            _logLineEvent.Timestamp = match.Groups["Timestamp"].Value;
        }

        private void ExtractLogCode()
        {
            var match = MatchResidual(LogLineCodeRegex);
            _logLineEvent.LogCode = match.Groups["LogCode"].Value;
        }

        private void ExtractGameLogCode()
        {
            if (!_logLineEvent.LogCode.Equals("00")) return;
            var match = MatchResidual(GameLogCodeRegex);
            _logLineEvent.GameLogCode = match.Groups["GameLogCode"].Value;
        }

        private Match MatchResidual(Regex regex)
        {
            var match = regex.Match(_residualMessage);
            _residualMessage = match.Groups["Residual"].Value;
            return match;
        }

        private void ExtractMessage()
        {
            RemoveSpecialCharacters();
            RemoveWorldName();
            RemoveRedundantNamePrefix();
        }

        private void ParseLootEvent()
        {
            if (_logLineEvent.LogMessage.Contains(" has been added to the loot list.")) ParseAddLoot();
            else if (_logLineEvent.LogMessage.Contains("Unable to obtain ")) ParseLostLoot();
            else if (_logLineEvent.LogMessage.Contains("obtain")) ParseObtainLoot();
            else if (_logLineEvent.LogMessage.Contains(
                " has been awarded for swift first-time completion of duty objectives.")) ParseObtainLoot();
            else if (_logLineEvent.LogMessage.Contains(" Need on ")) ParseNeedLoot();
            else if (_logLineEvent.LogMessage.Contains(" Greed on ")) ParseGreedLoot();
        }

        private void ParseObtainLoot()
        {
            Regex regex;
            if (_logLineEvent.LogMessage.Contains("the company action")) return;
            if (_logLineEvent.LogMessage.Contains("achievement data.")) return;
            if (_logLineEvent.LogMessage.Contains("experience points"))
                regex = ObtainWithExpPointsRegex;
            else if (_logLineEvent.LogMessage.Contains(" discover and obtain "))
                regex = ObtainWithMostRareRegex;
            else
                regex = ObtainRegex;

            var match = regex.Match(_logLineEvent.LogMessage);
            _logLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.ObtainLoot)
            {
                Item = CreateItemFromRawItemName(match.Groups["RawItemName"].Value),
                Actor = CreatePlayerFromActorNameAndFixMessage(match.Groups["ActorNameWithWorldName"].Value)
            };

            if (_logLineEvent.LogMessage.Contains(
                "has been awarded for swift first-time completion of duty objectives."))
                _logLineEvent.LogMessage =
                    _logLineEvent.XIVEvent.Actor.Name + " obtains " + $"{_logLineEvent.XIVEvent.Item.Quantity:n0}" +
                    " gil.";
            _logLineEvent.LogMessage = _logLineEvent.LogMessage.Replace("-an item most rare!", ".");
            _logLineEvent.LogMessage = _logLineEvent.LogMessage.Replace("-items most rare!", ".");
        }

        private void ParseLostLoot()
        {
            var regex = UnableToObtainRegex;
            var match = regex.Match(_logLineEvent.LogMessage);
            _logLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.LostLoot)
            {
                Item = CreateItemFromRawItemName(match.Groups["RawItemName"].Value)
            };
        }

        private Item CreateItemFromRawItemName(string rawItemName)
        {
            var regex = ItemNameRegex;
            var match = regex.Match(rawItemName.Replace(" (HQ)", ""));

            var itemName = match.Groups["ItemName"].Value;
            var quantityStr = match.Groups["Quantity"].Value.Replace(",", "");
            int quantity;
            try
            {
                quantity = int.Parse(quantityStr);
            }
            catch
            {
                quantity = 1;
            }

            var item = quantity > 1
                ? _aetherbridge.ItemService.GetItemByPluralName(itemName)
                : _aetherbridge.ItemService.GetItemBySingularName(itemName);
            item.Quantity = quantity;
            item.IsHQ = _logLineEvent.LogMessage.Contains("(HQ)");
            return item;
        }

        private Player CreatePlayerFromActorNameAndFixMessage(string actorName)
        {
            Player player;
            if (actorName.Equals(string.Empty) || actorName.ToUpper().Equals("YOU"))
            {
                player = (Player) _aetherbridge.GetCurrentPlayer();
                _logLineEvent.LogMessage = _logLineEvent.LogMessage.Remove(0, 3);
                _logLineEvent.LogMessage = player.Name + _logLineEvent.LogMessage;
                _logLineEvent.LogMessage = _logLineEvent.LogMessage.Replace(" discover and obtain ", " obtains ");
                _logLineEvent.LogMessage = _logLineEvent.LogMessage.Replace(" obtain ", " obtains ");
                _logLineEvent.LogMessage = _logLineEvent.LogMessage.Replace(" roll ", " rolls ");
            }
            else
            {
                player = new Player {Name = actorName, IsReporter = false};
            }

            return player;
        }

        private void ParseAddLoot()
        {
            var regex = ItemAddedRegex;
            var match = regex.Match(_logLineEvent.LogMessage);
            _logLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.AddLoot)
            {
                Item = CreateItemFromRawItemName(match.Groups["RawItemName"].Value)
            };
        }

        private void ParseGreedLoot()
        {
            var regex = GreedRegex;
            var match = regex.Match(_logLineEvent.LogMessage);

            _logLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.GreedLoot)
            {
                Item = CreateItemFromRawItemName(match.Groups["RawItemName"].Value),
                Actor = CreatePlayerFromActorNameAndFixMessage(match.Groups["ActorNameWithWorldName"].Value),
                Roll = int.Parse(match.Groups["Roll"].Value)
            };
        }

        private void ParseNeedLoot()
        {
            var regex = NeedRegex;
            var match = regex.Match(_logLineEvent.LogMessage);

            _logLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.NeedLoot)
            {
                Item = CreateItemFromRawItemName(match.Groups["RawItemName"].Value),
                Actor = CreatePlayerFromActorNameAndFixMessage(match.Groups["ActorNameWithWorldName"].Value),
                Roll = int.Parse(match.Groups["Roll"].Value)
            };
        }

        private void UpdateLogMessage()
        {
            _logLineEvent.LogMessage = _residualMessage;
        }

        private void RemoveSpecialCharacters()
        {
            var sb = new StringBuilder(_residualMessage);
            sb.Replace("\uE03C", "(HQ)");
            sb.Replace("\u201C", "\u0022");
            sb.Replace("\u201D", "\u0022");
            sb.Replace("\u2500", "\u002D");
            _residualMessage = Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                    ),
                    Encoding.UTF8.GetBytes(sb.ToString())
                )
            );
            _residualMessage = _residualMessage.Trim();
        }

        private void RemoveWorldName()
        {
            if (!_logLineEvent.LogCode.Equals("00")) return;
            var matches = ActorWithWorldNameRegex.Matches(_residualMessage);

            foreach (Match match in matches)
            {
                var actorName = match.Groups["ActorName"].Value;
                var worldName = match.Groups["WorldName"].Value;
                _residualMessage = _residualMessage.Replace(actorName + worldName, actorName);
            }
        }

        private void RemoveRedundantNamePrefix()
        {
            if (_logLineEvent.GameLogCode == null || !_logLineEvent.GameLogCode.Equals("001d")) return;
            var name = _residualMessage.Split(':')[0];
            _residualMessage = _residualMessage.Remove(0, name.Length + 1);
        }
    }
}