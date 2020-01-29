using System.Collections.Generic;
using System.Linq;
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
				var item = ItemMapper.MapToItem(crescentItem, language);
				_items[language.Index].Add(item);
				_itemNames[language.Index].Add(item.ProperName);
				if (item.IsCommon) _commonItemNames[language.Index].Add(item.ProperName);
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

		public Item GetItemBySingularKeyword(string singularName)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			var item = _items[languageIndex].FirstOrDefault(i => i.SingularNameKeyword.Equals(singularName));
			return item;
		}

		public Item GetItemByPluralKeyword(string pluralName)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			var item = _items[languageIndex].FirstOrDefault(i => i.PluralNameKeyword.Equals(pluralName));
			return item;
		}

		public Item GetItemBySingularRegex(string singularName)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			var item = _items[languageIndex].FirstOrDefault(i => i.SingularNameRegex.Match(singularName).Success);
			return item;
		}

		public Item GetItemByPluralRegex(string pluralName)
		{
			var languageIndex = _languageService.GetCurrentLanguage().Index;
			var item = _items[languageIndex].FirstOrDefault(i => i.PluralNameRegex.Match(pluralName).Success);
			return item;
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
	}
}