namespace ACT_FFXIV_Aetherbridge
{
	internal class DELogLineParser : LogLineParserBase, ILogLineParser
	{
		public DELogLineParser(ILogLineParserContext context) : base(context)
		{
		}

		public new LogLineEvent Parse(ACTLogLineEvent actLogLineEvent)
		{
			base.Parse(actLogLineEvent);
			return LogLineEvent;
		}

		public override Item FindItem(string itemName, int quantity)
		{
			return quantity > 1
				? Context.Aetherbridge.ItemService.GetItemByPluralSearchTermDE(itemName)
				: Context.Aetherbridge.ItemService.GetItemBySingularSearchTermDE(itemName);
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