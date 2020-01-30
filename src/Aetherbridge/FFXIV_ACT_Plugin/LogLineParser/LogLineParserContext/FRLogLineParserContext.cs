using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
	internal class FRLogLineParserContext : LogLineParserContextBase, ILogLineParserContext
	{
		public FRLogLineParserContext(IAetherbridge aetherbridge) : base(aetherbridge)
		{
			ObtainRegex =
				LogLineParserUtil.CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+).+?(?:obtenez|obtient) (?:une |un )?(?<RawItemName>.*?)\.");
			ObtainWithMostRareRegex =
				LogLineParserUtil.CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+ ?[^\s]+).+?(?:découvrez et obtenez) (?:une |un )?(?<RawItemName>.*?)\.");
			UnableToObtainRegex =
				LogLineParserUtil.CreateRegex(@"^Impossible d'obtenir (?:les |le |la |)?(?<RawItemName>.*?)\.");
			ItemNameRegex =
				LogLineParserUtil.CreateRegex(@"^(?:(?:the )?(?<Quantity>[\d,\.]+[^\s]?) )?(?<ItemName>.*)");
			ItemAddedRegex =
				LogLineParserUtil.CreateRegex(@"^(?:Une |Un )(?<RawItemName>.*?) a été ajoutée au butin\.");
			GreedRegex =
				LogLineParserUtil.CreateRegex(
					@"^(?<ActorNameWithWorldName>[^\s]+(?: )?[^\s]+) (?:jette |jetez )les dés \(Cupidité\) pour (?:les |le |la |)(?<RawItemName>.*?)\. (?<Roll>.*)!");
			NeedRegex = LogLineParserUtil.CreateRegex(
				@"^(?<ActorNameWithWorldName>[^\s]+(?: )?[^\s]+) (?:jette |jetez )les dés \(Besoin\) pour (?:les |le |la |)(?<RawItemName>.*?)\. (?<Roll>.*)!");
			ActorWithWorldNameRegex =
				LogLineParserUtil.CreateRegex(@"(?<ActorName>[A-Za-z'\-\.]+ [A-Za-z'\-\.]+)(?<WorldName>" + WorldsList +
				                              ")");
			LootFalsePositives = new List<string>();
			YouLocalized = "VOUS";
			NumberDelimiterLocalized = ",";
		}
	}
}