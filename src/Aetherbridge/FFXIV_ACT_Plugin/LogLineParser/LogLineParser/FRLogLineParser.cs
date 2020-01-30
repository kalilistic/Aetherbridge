namespace ACT_FFXIV_Aetherbridge
{
	internal class FRLogLineParser : LogLineParserBase, ILogLineParser
	{
		public FRLogLineParser(ILogLineParserContext context) : base(context)
		{
		}

		public new LogLineEvent Parse(ACTLogLineEvent actLogLineEvent)
		{
			base.Parse(actLogLineEvent);
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(Context.HQString, " " + Context.HQString);
			return LogLineEvent;
		}

		public override Item FindItem(string itemName, int quantity)
		{
			return quantity > 1
				? Context.Aetherbridge.ItemService.GetItemByPluralName(itemName)
				: Context.Aetherbridge.ItemService.GetItemBySingularName(itemName);
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