using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ACT_FFXIV_Aetherbridge
{
	public interface ILogLineParserContext
	{
		Regex ObtainRegex { get; set; }
		Regex ObtainWithMostRareRegex { get; set; }
		Regex UnableToObtainRegex { get; set; }
		Regex ItemNameRegex { get; set; }
		Regex ItemAddedRegex { get; set; }
		Regex GreedRegex { get; set; }
		Regex NeedRegex { get; set; }
		Regex ActorWithWorldNameRegex { get; set; }
		Regex TimestampRegex { get; set; }
		Regex LogLineCodeRegex { get; set; }
		Regex GameLogCodeRegex { get; set; }
		List<string> LootFalsePositives { get; set; }
		string YouLocalized { get; set; }
		string NumberDelimiterLocalized { get; set; }
		Regex ItemQuantityRegex { get; set; }
		Regex ObtainAltRegex { get; set; }
		string HQChar { get; set; }
		string HQString { get; set; }
		string WorldsList { get; set; }
		IAetherbridge Aetherbridge { get; set; }
	}
}