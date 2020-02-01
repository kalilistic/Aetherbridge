using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIV.CrescentCove;

// ReSharper disable InvertIf
namespace ACT_FFXIV_Aetherbridge
{
	public class ItemService
	{
		private readonly LanguageService _languageService;
		private readonly IGameDataRepository<FFXIV.CrescentCove.Item> _repository;
		private List<List<string>> _commonItemNames = new List<List<string>>();
		private List<List<string>> _itemNames = new List<List<string>>();
		private List<List<Item>> _items = new List<List<Item>>();

		public ItemService(LanguageService languageService, IGameDataRepository<FFXIV.CrescentCove.Item> repository)
		{
			_languageService = languageService;
			_repository = repository;

			var languages = _languageService.GetLanguages();
			foreach (var _ in languages)
			{
				_commonItemNames.Add(new List<string>());
				_itemNames.Add(new List<string>());
				_items.Add(new List<Item>());
			}
		}

		public void AddLanguage(Language language)
		{
			var crescentItems = _repository.GetAll();
			foreach (var crescentItem in crescentItems)
			{
				var item = MapToItem(crescentItem, language);
				_items[language.Index].Add(item);
				_itemNames[language.Index].Add(item.ProperName);
				if (item.IsCommon) _commonItemNames[language.Index].Add(item.ProperName);
			}

			if (language.Id == 3)
			{
				foreach (var item in _items[language.Index])
				{
					item.SingularRegex = new Regex(item.SingularREP, RegexOptions.Compiled);
					item.PluralRegex = new Regex(item.PluralREP, RegexOptions.Compiled);
				}
			}
		}

		public Item GetItemById(int id)
		{
			return _items[_languageService.GetCurrentLanguage().Index].FirstOrDefault(item => item.Id == id);
		}

		public Item GetItemBySingularName(string singularName)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			var item = _items[languageIndex].FirstOrDefault(i => i.SingularName.Equals(singularName));
			return item;
		}

		public Item GetItemByPluralName(string pluralName)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			var item = _items[languageIndex].FirstOrDefault(i => i.PluralName.Equals(pluralName));
			return item;
		}

		public Item GetItemBySingularSearchTerm(string singularName)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			var item = _items[languageIndex].FirstOrDefault(i => i.SingularSearchTerm.Equals(singularName));
			return item;
		}

		public Item GetItemByPluralSearchTerm(string pluralName)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			var item = _items[languageIndex].FirstOrDefault(i => i.PluralSearchTerm.Equals(pluralName));
			return item;
		}

		public Item GetItemBySingularSearchTermDE(string singularName)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			var items = _items[languageIndex].FindAll(i => singularName.StartsWith(i.SingularSearchTerm));
			switch (items.Count)
			{
				case 0:
					return null;
				case 1:
					return items[0];
			}

			return items.FirstOrDefault(item => item.SingularRegex.Match(singularName).Success);
		}

		public Item GetItemByPluralSearchTermDE(string pluralName)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			var items = _items[languageIndex].FindAll(i => pluralName.StartsWith(i.PluralSearchTerm));
			switch (items.Count)
			{
				case 0:
					return null;
				case 1:
					return items[0];
			}

			return items.FirstOrDefault(item => item.PluralRegex.Match(pluralName).Success);
		}

		public List<string> GetItemNames()
		{
			return _itemNames[_languageService.GetCurrentLanguage().Index];
		}

		public List<string> GetCommonItemNames()
		{
			return _commonItemNames[_languageService.GetCurrentLanguage().Index];
		}

		public void DeInit()
		{
			_items = null;
			_commonItemNames = null;
			_itemNames = null;
		}

		public static List<Item> MapToItems(List<FFXIV.CrescentCove.Item> items, Language language)
		{
			return items.Select(item => MapToItem(item, language)).ToList();
		}

		public static Item MapToItem(FFXIV.CrescentCove.Item item, Language language)
		{
			if (item == null) return null;
			return new Item
			{
				Id = item.Id,
				ProperName = item.Localized[language.Index].ProperName,
				SingularName = item.Localized[language.Index].SingularName,
				PluralName = item.Localized[language.Index].PluralName,
				SingularSearchTerm = item.Localized[language.Index].SingularSearchTerm,
				PluralSearchTerm = item.Localized[language.Index].PluralSearchTerm,
				SingularREP = item.Localized[language.Index].SingularREP,
				PluralREP = item.Localized[language.Index].PluralREP,
				IsCommon = item.IsCommon
			};
		}
	}
}