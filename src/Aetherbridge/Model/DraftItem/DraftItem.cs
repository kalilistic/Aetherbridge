namespace ACT_FFXIV_Aetherbridge
{
	public class DraftItem : IDraftItem
	{
		public string RawItemName { get; set; }
		public string ItemName { get; set; }
		public int Quantity { get; set; }
	}
}