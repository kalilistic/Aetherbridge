namespace ACT_FFXIV_Aetherbridge
{
	public interface IClassJobService
	{
		ClassJob GetClassJobById(int id);
		void DeInit();
		void AddLanguage(ILanguage language);
	}
}