using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
	internal class JALogLineParserContext : LogLineParserContextBase, ILogLineParserContext
	{
		public JALogLineParserContext(IAetherbridge aetherbridge) : base(aetherbridge)
		{
			ObtainRegex =
				LogLineParserUtil.CreateRegex(@"^(?<ActorNameWithWorldName>.+)(は|に)(?<RawItemName>.+)(?:を入手した|を手に入れた|が分配されました)。");
			ObtainWithMostRareRegex =
				LogLineParserUtil.CreateRegex(@"^(?<ActorNameWithWorldName>.+)は希少なほりだしもの(?<RawItemName>.+)を入手した！");
			UnableToObtainRegex = LogLineParserUtil.CreateRegex(@"^(?<RawItemName>.+)を手に入れることができなかった。");
			ItemNameRegex = null;
			ItemAddedRegex = LogLineParserUtil.CreateRegex(@"^(?<RawItemName>.+)が戦利品に追加されました。");
			GreedRegex =
				LogLineParserUtil.CreateRegex(
					@"^(?<ActorNameWithWorldName>.+)は(?<RawItemName>.+)にGREEDのダイスで(?<Roll>.*)を出した。");
			NeedRegex = LogLineParserUtil.CreateRegex(
				@"^(?<ActorNameWithWorldName>.+)は(?<RawItemName>.+)にNEEDのダイスで(?<Roll>.*)を出した。");
			ActorWithWorldNameRegex =
				LogLineParserUtil.CreateRegex(@"(?<ActorName>[A-Za-z'\-\.]+ [A-Za-z'\-\.]+)(?<WorldName>" + WorldsList +
				                              ")");
			LootFalsePositives = new List<string>();
			NumberDelimiterLocalized = ",";
			ObtainAltRegex = LogLineParserUtil.CreateRegex(@"^(?<RawItemName>.+)(?:を入手した|を手に入れた)。");
			ItemQuantityRegex = LogLineParserUtil.CreateRegex(@"(?<Quantity>[0-9,]+)");
		}
	}
}