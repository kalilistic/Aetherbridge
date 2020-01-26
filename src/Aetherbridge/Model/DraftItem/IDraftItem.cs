namespace ACT_FFXIV_Aetherbridge
{
	public interface IDraftItem
	{
		string RawItemName { get; set; }
		string ItemName { get; set; }
		int Quantity { get; set; }
	}
}