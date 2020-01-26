namespace ACT_FFXIV_Aetherbridge
{
	public interface IContent
	{
		int Id { get; set; }
		string Name { get; set; }
		bool IsHighEndDuty { get; set; }
		int TerritoryTypeId { get; set; }
		string ToString();
	}
}