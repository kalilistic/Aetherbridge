namespace ACT_FFXIV_Aetherbridge
{
	public interface ILocation
	{
		int TerritoryTypeId { get; set; }
		IPlaceName Region { get; set; }
		IPlaceName Zone { get; set; }
		IPlaceName Territory { get; set; }
		IPlaceName Map { get; set; }
	}
}