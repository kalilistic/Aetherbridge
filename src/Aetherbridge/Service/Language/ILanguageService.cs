using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
	public interface ILanguageService
	{
		ILanguage GetLanguageById(int id);
		List<ILanguage> GetLanguages();
		ILanguage GetCurrentLanguage();
		void DeInit();
	}
}