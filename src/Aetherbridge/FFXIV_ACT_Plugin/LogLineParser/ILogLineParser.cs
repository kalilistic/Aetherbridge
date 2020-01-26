namespace ACT_FFXIV_Aetherbridge
{
	internal interface ILogLineParser
	{
		ILogLineEvent Parse(IACTLogLineEvent actLogLineEvent);
		Item FindItem(string itemName, int quantity);
		void NormalizeObtainWithMostRare();
		void NormalizeObtain();
		void NormalizeRoll();
	}
}