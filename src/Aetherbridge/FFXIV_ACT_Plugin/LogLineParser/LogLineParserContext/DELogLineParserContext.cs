using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
	internal class DELogLineParserContext : LogLineParserContextBase, ILogLineParserContext
	{
		public DELogLineParserContext(IAetherbridge aetherbridge) : base(aetherbridge)
		{
			ObtainRegex =
				LogLineParserUtil.CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+).+?(?:hast|hat) (?:einen |eine |ein )?(?<RawItemName>.*?) erhalten\.");
			ObtainWithMostRareRegex =
				LogLineParserUtil.CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+).+?findest und erhältst (?:einen |eine |ein )?(?<RawItemName>.*?) - (?:einen |eine |ein )?höchst seltener (?:Fund|Gegenstand)!");
			UnableToObtainRegex =
				LogLineParserUtil.CreateRegex(@"^Du konntest (?:das |der |die )?(?<RawItemName>.*?) nicht erhalten\.");
			ItemNameRegex = LogLineParserUtil.CreateRegex(@"^((the )?(?<Quantity>[\d,\.]+[^\s]?) )?(?<ItemName>.*)");
			ItemAddedRegex =
				LogLineParserUtil.CreateRegex(
					@"^Ihr habt Beutegut \((?:einen |eine |ein )?(?<RawItemName>.*?)\) erhalten\.");
			GreedRegex =
				LogLineParserUtil.CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+) würfel(s)?t mit „Gier“ (?:einen |eine |ein )?(?<Roll>.*) auf (?:das |der |die )?(?<RawItemName>.+)\.");
			NeedRegex = LogLineParserUtil.CreateRegex(
				@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+) würfel(s)?t mit „Bedarf“ (?:einen |eine |ein )?(?<Roll>.*) auf (?:das |der |die )?(?<RawItemName>.+)\.");
			ActorWithWorldNameRegex =
				LogLineParserUtil.CreateRegex(@"(?<ActorName>[A-Za-z'\-\.]+ [A-Za-z'\-\.]+)(?<WorldName>" + WorldsList +
				                              ")");
			LootFalsePositives = new List<string>();
			YouLocalized = "DU";
			NumberDelimiterLocalized = ".";
		}
	}
}