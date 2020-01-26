using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
	public interface IItemService
	{
		Item GetItemById(int id);
		Item GetItemBySingularName(string singularName);
		Item GetItemByPluralName(string pluralName);
		Item GetItemBySingularKeyword(string singularName);
		Item GetItemByPluralKeyword(string pluralName);
		Item GetItemBySingularRegex(string singularName);
		Item GetItemByPluralRegex(string pluralName);
		List<string> GetItemNames();
		List<string> GetCommonItemNames();
		void DeInit();
		void AddLanguage(ILanguage language);
	}
}