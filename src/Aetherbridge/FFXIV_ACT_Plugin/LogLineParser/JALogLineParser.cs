using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ACT_FFXIV_Aetherbridge
{
	internal class JALogLineParser : LogLineParserBase, ILogLineParser
	{
		private readonly Regex _itemQuantityRegex;
		private readonly char _numberDelimiterLocalizedChar;
		private readonly Regex _obtainAltRegex;

		public JALogLineParser(IAetherbridge aetherbridge) : base(aetherbridge)
		{
			ObtainRegex = CreateRegex(@"^(?<ActorNameWithWorldName>.+)は(?<RawItemName>.+)(?:を入手した|を手に入れた)。");
			ObtainWithMostRareRegex = CreateRegex(@"^(?<ActorNameWithWorldName>.+)は希少なほりだしもの(?<RawItemName>.+)を入手した！");
			UnableToObtainRegex = CreateRegex(@"^(?<RawItemName>.+)を手に入れることができなかった。");
			ItemNameRegex = null;
			ItemAddedRegex = CreateRegex(@"^(?<RawItemName>.+)が戦利品に追加されました。");
			GreedRegex = CreateRegex(@"^(?<ActorNameWithWorldName>.+)は(?<RawItemName>.+)にGREEDのダイスで(?<Roll>.*)を出した。");
			NeedRegex = CreateRegex(@"^(?<ActorNameWithWorldName>.+)は(?<RawItemName>.+)にNEEDのダイスで(?<Roll>.*)を出した。");
			ActorWithWorldNameRegex =
				CreateRegex(@"(?<ActorName>[A-Za-z'\-\.]+ [A-Za-z'\-\.]+)(?<WorldName>" + WorldsList + ")");
			LootFalsePositives = new List<string>();
			NumberDelimiterLocalized = ",";

			_obtainAltRegex = CreateRegex(@"^(?<RawItemName>.+)(?:を入手した|を手に入れた)。");
			_itemQuantityRegex = CreateRegex(@"(?<Quantity>[0-9,]+)");
			_numberDelimiterLocalizedChar = NumberDelimiterLocalized.ToCharArray()[0];
		}

		public new LogLineEvent Parse(ACTLogLineEvent actLogLineEvent)
		{
			base.Parse(actLogLineEvent);
			return LogLineEvent;
		}

		public override Item FindItem(string itemName, int quantity)
		{
			return Aetherbridge.ItemService.GetItemBySingularName(itemName);
		}

		public override void NormalizeObtainWithMostRare()
		{
			NormalizeObtainCommon();
		}

		public override void NormalizeObtain()
		{
			NormalizeObtainCommon();
		}

		public override void NormalizeRoll()
		{
		}

		protected override bool IsObtainLoot()
		{
			var match = ObtainRegex.Match(LogLineEvent.LogMessage);
			if (match.Success)
			{
				LogLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.ObtainLoot)
				{
					Item = CreateItem(match),
					Actor = CreateActor(match)
				};
				return match.Success;
			}

			match = _obtainAltRegex.Match(LogLineEvent.LogMessage);
			if (!match.Success) return match.Success;
			LogLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.ObtainLoot)
			{
				Item = CreateItem(match),
				Actor = CreateActor(match)
			};
			NormalizeObtain();
			return match.Success;
		}

		protected override void ParseItemNameAndQuantity(DraftItem draftItem)
		{
			var match = _itemQuantityRegex.Match(draftItem.RawItemName);
			var quantityStr = match.Groups["Quantity"].Value.Replace(NumberDelimiterLocalized, string.Empty);
			try
			{
				draftItem.Quantity = int.Parse(quantityStr);
				draftItem.ItemName = draftItem.RawItemName.Replace(match.Groups["Quantity"].Value, string.Empty);
				if (draftItem.ItemName[draftItem.ItemName.Length - 1] == '×')
					draftItem.ItemName = draftItem.ItemName.Remove(draftItem.ItemName.Length - 1);
			}
			catch
			{
				draftItem.Quantity = 1;
				draftItem.ItemName = draftItem.RawItemName;
			}
		}

		private void NormalizeObtainCommon()
		{
			if (LogLineEvent.XIVEvent.Item == null) return;
			var itemName = LogLineEvent.XIVEvent.Item.ProperName;
			if (LogLineEvent.XIVEvent.Item.IsHQ) itemName += HQString;

			if (LogLineEvent.XIVEvent.Item.Quantity > 1) itemName += "×" + $"{LogLineEvent.XIVEvent.Item.Quantity:n0}";
			LogLineEvent.LogMessage =
				LogLineEvent.XIVEvent.Actor.Name + "は" + itemName + "を手に入れた。";
		}
	}
}