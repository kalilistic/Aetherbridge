namespace ACT_FFXIV_Aetherbridge
{
	internal interface ILogLineParser
	{
		LogLineEvent Parse(ACTLogLineEvent actLogLineEvent);
		Item FindItem(string itemName, int quantity);
		void NormalizeObtainWithMostRare();
		void NormalizeObtain();
		void NormalizeRoll();
	}
}