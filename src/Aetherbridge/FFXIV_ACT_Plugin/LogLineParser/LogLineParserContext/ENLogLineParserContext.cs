using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
	internal class ENLogLineParserContext : LogLineParserContextBase, ILogLineParserContext
	{
		public ENLogLineParserContext(IAetherbridge aetherbridge) : base(aetherbridge)
		{
			ObtainRegex =
				LogLineParserUtil.CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+).+?(?:(obtain|claim)(?:s)? )(?:the |an |a )?(?<RawItemName>.*?)\.");
			ObtainWithMostRareRegex =
				LogLineParserUtil.CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+) discover and obtain (?:the |an |a )?(?<RawItemName>.*?)─(?:items|an item) most rare!");
			UnableToObtainRegex =
				LogLineParserUtil.CreateRegex(@"^Unable to obtain (?:the |an |a )?(?<RawItemName>.*?)\.");
			ItemNameRegex = LogLineParserUtil.CreateRegex(@"^(?<Quantity>[\d,\,]+[^\s]?)? ?(?<ItemName>.*)");
			ItemAddedRegex =
				LogLineParserUtil.CreateRegex(@"^(?:The |An |A )?(?<RawItemName>.*?) has been added to the loot list.");
			GreedRegex =
				LogLineParserUtil.CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+) roll[s]? Greed on (?:the |an |a )?(?<RawItemName>.*?)\. (?<Roll>.*)!");
			NeedRegex = LogLineParserUtil.CreateRegex(
				@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+) roll[s]? Need on (?:the |an |a )?(?<RawItemName>.*?)\. (?<Roll>.*)!");
			ActorWithWorldNameRegex =
				LogLineParserUtil.CreateRegex(@"(?<ActorName>[A-Za-z'\-\.]+ [A-Za-z'\-\.]+)(?<WorldName>" + WorldsList +
				                              ")");
			LootFalsePositives = new List<string> {"the company action", "achievement data."};
			YouLocalized = "YOU";
			NumberDelimiterLocalized = ",";
		}
	}
}