using System.Text.RegularExpressions;

namespace ACT_FFXIV_Aetherbridge
{
	internal class JALogLineParser : LogLineParserBase, ILogLineParser
	{
		public JALogLineParser(ILogLineParserContext context) : base(context)
		{
		}

		public new LogLineEvent Parse(ACTLogLineEvent actLogLineEvent)
		{
			base.Parse(actLogLineEvent);
			return LogLineEvent;
		}

		public override Item FindItem(string itemName, int quantity)
		{
			return Context.Aetherbridge.ItemService.GetItemBySingularName(itemName);
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
			var match = Context.ObtainRegex.Match(LogLineEvent.LogMessage);
			if (match.Success)
			{
				LogLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.ObtainLoot)
				{
					Item = CreateItem(match),
					Actor = CreateActor(match)
				};
				return match.Success;
			}

			match = Context.ObtainAltRegex.Match(LogLineEvent.LogMessage);
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
			const string gil = @"ギル";
			const char currencyIndicator = '×';
			string quantityStr;
			if (draftItem.RawItemName.EndsWith(gil))
			{
				quantityStr = draftItem.RawItemName.Replace(gil, string.Empty);
			}
			else
			{
				var match = Context.ItemQuantityRegex.Match(draftItem.RawItemName);
				quantityStr = match.Groups["Quantity"].Value;
			}
			try
			{
				draftItem.Quantity = int.Parse(quantityStr.Replace(Context.NumberDelimiterLocalized, string.Empty));
				draftItem.ItemName = draftItem.RawItemName.Replace(quantityStr, string.Empty);
				if (draftItem.ItemName[draftItem.ItemName.Length - 1] == currencyIndicator)
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
			if (LogLineEvent.XIVEvent.Item.IsHQ) itemName += Context.HQString;

			if (LogLineEvent.XIVEvent.Item.Quantity > 1) itemName += "×" + $"{LogLineEvent.XIVEvent.Item.Quantity:n0}";
			LogLineEvent.LogMessage =
				LogLineEvent.XIVEvent.Actor.Name + "は" + itemName + "を手に入れた。";
		}
	}
}