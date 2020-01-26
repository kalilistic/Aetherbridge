using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
	internal class FRLogLineParser : LogLineParserBase, ILogLineParser
	{
		public FRLogLineParser(IAetherbridge aetherbridge) : base(aetherbridge)
		{
			ObtainRegex =
				CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+).+?(?:obtenez|obtient) (?:une |un )?(?<RawItemName>.*?)\.");
			ObtainWithMostRareRegex =
				CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+).+?(?:découvrez et obtenez) (?:une |un )?(?<RawItemName>.*?)\.");
			UnableToObtainRegex = CreateRegex(@"^Impossible d'obtenir (?:les |le |la |)?(?<RawItemName>.*?)\.");
			ItemNameRegex = CreateRegex(@"^(?:(?:the )?(?<Quantity>[\d,\.]+[^\s]?) )?(?<ItemName>.*)");
			ItemAddedRegex = CreateRegex(@"^(?:Une |Un )(?<RawItemName>.*?) a été ajoutée au butin\.");
			GreedRegex =
				CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+(?: )?[^\s]+) (?:jette |jetez )les dés \(Cupidité\) pour (?:les |le |la |)(?<RawItemName>.*?)\. (?<Roll>.*)!");
			NeedRegex = CreateRegex(
				@"^(?<ActorNameWithWorldName>[^\s]+(?: )?[^\s]+) (?:jette |jetez )les dés \(Besoin\) pour (?:les |le |la |)(?<RawItemName>.*?)\. (?<Roll>.*)!");
			ActorWithWorldNameRegex =
				CreateRegex(@"(?<ActorName>[A-Za-z'\-\.]+ [A-Za-z'\-\.]+)(?<WorldName>" + WorldsList + ")");
			LootFalsePositives = new List<string>();
			YouLocalized = "VOUS";
			NumberDelimiterLocalized = ",";
		}

		public new ILogLineEvent Parse(IACTLogLineEvent actLogLineEvent)
		{
			base.Parse(actLogLineEvent);
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(HQString, " " + HQString);
			return LogLineEvent;
		}

		public override Item FindItem(string itemName, int quantity)
		{
			return quantity > 1
				? Aetherbridge.ItemService.GetItemByPluralName(itemName)
				: Aetherbridge.ItemService.GetItemBySingularName(itemName);
		}

		public override void NormalizeObtainWithMostRare()
		{
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace("découvrez et obtenez", "obtenez");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" Des objets rares!", string.Empty);
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" Un objet des plus rares!", string.Empty);
		}

		public override void NormalizeObtain()
		{
			if (LogLineEvent.XIVEvent.Actor.IsReporter)
				LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" obtenez ", " obtient ");
		}

		public override void NormalizeRoll()
		{
			if (LogLineEvent.XIVEvent.Actor.IsReporter)
				LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" jetez ", " jette ");
		}
	}
}