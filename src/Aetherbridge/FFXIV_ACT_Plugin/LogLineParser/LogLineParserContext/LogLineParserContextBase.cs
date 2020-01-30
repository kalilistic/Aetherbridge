using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ACT_FFXIV_Aetherbridge
{
	public abstract class LogLineParserContextBase
	{
		protected LogLineParserContextBase(IAetherbridge aetherbridge)
		{
			Aetherbridge = aetherbridge;
			WorldsList = Aetherbridge.WorldService.GetWorldsAsDelimitedString();
			TimestampRegex =
				LogLineParserUtil.CreateRegex(@"^\[(?<Timestamp>[0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]{3})] (?<Residual>.*)");
			LogLineCodeRegex = LogLineParserUtil.CreateRegex(@"^(?<LogCode>[^:]*):(?<Residual>.*)");
			GameLogCodeRegex = LogLineParserUtil.CreateRegex(@"^(?<GameLogCode>[^:]*):(?<Residual>.*)");
			HQChar = "\uE03C";
			HQString = "(HQ)";
		}

		public Regex ObtainRegex { get; set; }
		public Regex ObtainWithMostRareRegex { get; set; }
		public Regex UnableToObtainRegex { get; set; }
		public Regex ItemNameRegex { get; set; }
		public Regex ItemAddedRegex { get; set; }
		public Regex GreedRegex { get; set; }
		public Regex NeedRegex { get; set; }
		public Regex ActorWithWorldNameRegex { get; set; }
		public Regex TimestampRegex { get; set; }
		public Regex LogLineCodeRegex { get; set; }
		public Regex GameLogCodeRegex { get; set; }
		public List<string> LootFalsePositives { get; set; }
		public string YouLocalized { get; set; }
		public string NumberDelimiterLocalized { get; set; }
		public Regex ItemQuantityRegex { get; set; }
		public Regex ObtainAltRegex { get; set; }
		public string HQChar { get; set; }
		public string HQString { get; set; }
		public string WorldsList { get; set; }
		public IAetherbridge Aetherbridge { get; set; }
	}
}