namespace ACT_FFXIV_Aetherbridge
{
	internal class ENLogLineParser : LogLineParserBase, ILogLineParser
	{
		public ENLogLineParser(ILogLineParserContext context) : base(context)
		{
			Context = context;
		}

		public new LogLineEvent Parse(ACTLogLineEvent actLogLineEvent)
		{
			base.Parse(actLogLineEvent);
			return LogLineEvent;
		}

		public override Item FindItem(string itemName, int quantity)
		{
			return quantity > 1
				? Context.Aetherbridge.ItemService.GetItemByPluralKeyword(itemName)
				: Context.Aetherbridge.ItemService.GetItemBySingularKeyword(itemName);
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