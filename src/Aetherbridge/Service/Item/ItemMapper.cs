using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ACT_FFXIV_Aetherbridge
{
	public static class ItemMapper
	{
		public static List<Item> MapToItems(List<FFXIV.CrescentCove.Item> items, ILanguage language)
		{
			return items.Select(item => MapToItem(item, language)).ToList();
		}

		public static Item MapToItem(FFXIV.CrescentCove.Item item, ILanguage language)
		{
			if (item == null) return null;
			return new Item
			{
				Id = item.Id,
				ProperName = item.Localized[language.Index].ProperName,
				SingularName = item.Localized[language.Index].SingularName,
				SingularNameKeyword = item.Localized[language.Index].SingularNameKeyword,
				SingularNameRegex = new Regex(item.Localized[language.Index].SingularNameREP, RegexOptions.Compiled),
				PluralName = item.Localized[language.Index].PluralName,
				PluralNameKeyword = item.Localized[language.Index].PluralNameKeyword,
				PluralNameRegex = new Regex(item.Localized[language.Index].PluralNameREP, RegexOptions.Compiled),
				IsCommon = item.IsCommon
			};
		}
	}
}