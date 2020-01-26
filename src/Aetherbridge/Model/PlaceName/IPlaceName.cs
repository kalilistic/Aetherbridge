namespace ACT_FFXIV_Aetherbridge
{
	public interface IPlaceName
	{
		int Id { get; set; }
		string Name { get; set; }
		string ToString();
	}
}