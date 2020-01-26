using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
	internal class ENLogLineParser : LogLineParserBase, ILogLineParser
	{
		public ENLogLineParser(IAetherbridge aetherbridge) : base(aetherbridge)
		{
			ObtainRegex =
				CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+).+?(?:obtain(?:s)? )(?:the |an |a )?(?<RawItemName>.*?)\.");
			ObtainWithMostRareRegex =
				CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+) discover and obtain (?:the |an |a )?(?<RawItemName>.*?)─(?:items|an item) most rare!");
			UnableToObtainRegex = CreateRegex(@"^Unable to obtain (?:the |an |a )?(?<RawItemName>.*?)\.");
			ItemNameRegex = CreateRegex(@"^(?<Quantity>[\d,\,]+[^\s]?)? ?(?<ItemName>.*)");
			ItemAddedRegex = CreateRegex(@"^(?:The |An |A )?(?<RawItemName>.*?) has been added to the loot list.");
			GreedRegex =
				CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+) roll[s]? Greed on (?:the |an |a )?(?<RawItemName>.*?)\. (?<Roll>.*)!");
			NeedRegex = CreateRegex(
				@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+) roll[s]? Need on (?:the |an |a )?(?<RawItemName>.*?)\. (?<Roll>.*)!");
			ActorWithWorldNameRegex =
				CreateRegex(@"(?<ActorName>[A-Za-z'\-\.]+ [A-Za-z'\-\.]+)(?<WorldName>" + WorldsList + ")");
			LootFalsePositives = new List<string> {"the company action", "achievement data."};
			YouLocalized = "YOU";
			NumberDelimiterLocalized = ",";
		}

		public new ILogLineEvent Parse(IACTLogLineEvent actLogLineEvent)
		{
			base.Parse(actLogLineEvent);
			return LogLineEvent;
		}

		public override Item FindItem(string itemName, int quantity)
		{
			return quantity > 1
				? Aetherbridge.ItemService.GetItemByPluralKeyword(itemName)
				: Aetherbridge.ItemService.GetItemBySingularKeyword(itemName);
		}

		public override void NormalizeObtainWithMostRare()
		{
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" discover and obtain ", " obtains ");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace("─an item most rare!", ".");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace("─items most rare!", ".");
			if (LogLineEvent.XIVEvent.Actor.IsReporter)
				LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" obtain ", " obtains ");
		}

		public override void NormalizeObtain()
		{
			if (LogLineEvent.XIVEvent.Actor.IsReporter)
				LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" obtain ", " obtains ");
		}

		public override void NormalizeRoll()
		{
			if (LogLineEvent.XIVEvent.Actor.IsReporter)
				LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" roll ", " rolls ");
		}
	}
}