using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
	internal class DELogLineParser : LogLineParserBase, ILogLineParser
	{
		public DELogLineParser(IAetherbridge aetherbridge) : base(aetherbridge)
		{
			ObtainRegex =
				CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+).+?(?:hast|hat) (?:einen |eine |ein )?(?<RawItemName>.*?) erhalten\.");
			ObtainWithMostRareRegex =
				CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+).+?findest und erhältst (?:einen |eine |ein )?(?<RawItemName>.*?) - (?:einen |eine |ein )?höchst seltener (?:Fund|Gegenstand)!");
			UnableToObtainRegex = CreateRegex(@"^Du konntest (?:das |der |die )?(?<RawItemName>.*?) nicht erhalten\.");
			ItemNameRegex = CreateRegex(@"^((the )?(?<Quantity>[\d,\.]+[^\s]?) )?(?<ItemName>.*)");
			ItemAddedRegex =
				CreateRegex(@"^Ihr habt Beutegut \((?:einen |eine |ein )?(?<RawItemName>.*?)\) erhalten\.");
			GreedRegex =
				CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+) würfel(s)?t mit „Gier“ (?:einen |eine |ein )?(?<Roll>.*) auf (?:das |der |die )?(?<RawItemName>.+)\.");
			NeedRegex = CreateRegex(
				@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+) würfel(s)?t mit „Bedarf“ (?:einen |eine |ein )?(?<Roll>.*) auf (?:das |der |die )?(?<RawItemName>.+)\.");
			ActorWithWorldNameRegex =
				CreateRegex(@"(?<ActorName>[A-Za-z'\-\.]+ [A-Za-z'\-\.]+)(?<WorldName>" + WorldsList + ")");
			LootFalsePositives = new List<string>();
			YouLocalized = "DU";
			NumberDelimiterLocalized = ".";
		}

		public new ILogLineEvent Parse(IACTLogLineEvent actLogLineEvent)
		{
			base.Parse(actLogLineEvent);
			return LogLineEvent;
		}

		public override Item FindItem(string itemName, int quantity)
		{
			return quantity > 1
				? Aetherbridge.ItemService.GetItemByPluralRegex(itemName)
				: Aetherbridge.ItemService.GetItemBySingularRegex(itemName);
		}

		public override void NormalizeObtainWithMostRare()
		{
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" - einen höchst seltener Gegenstand!", ".");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" - einen höchst seltener Fund!", ".");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" - eine höchst seltener Gegenstand!", ".");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" - eine höchst seltener Fund!", ".");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" - ein höchst seltener Gegenstand!", ".");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" - ein höchst seltener Fund!", ".");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" findest und erhältst einen ", " hat ");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" findest und erhältst eine ", " hat ");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" findest und erhältst ein ", " hat ");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" findest und erhältst ", " hat ");
		}

		public override void NormalizeObtain()
		{
			if (LogLineEvent.XIVEvent.Actor.IsReporter)
				LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" hast ", " hat ");
		}

		public override void NormalizeRoll()
		{
			if (LogLineEvent.XIVEvent.Actor.IsReporter)
				LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" würfelst ", " würfelt ");
		}
	}
}