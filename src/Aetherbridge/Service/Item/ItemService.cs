using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FFXIV.CrescentCove;

// ReSharper disable InvertIf

namespace ACT_FFXIV_Aetherbridge
{
    public class ItemService : IItemService
    {
        private List<string> _commonItemNames;
        private List<string> _itemNames;
        private IGameDataRepository<FFXIV.CrescentCove.Item> _repository;

        public ItemService(IGameDataRepository<FFXIV.CrescentCove.Item> repository)
        {
            _repository = repository;
        }

        public Item GetItemById(int id)
        {
            return ItemMapper.MapToItem(_repository.GetById(id));
        }

        public Item GetItemBySingularName(string singularName)
        {
            var itemNameWithoutPrefix = RemovePrefix(singularName);
            Expression<Func<FFXIV.CrescentCove.Item, bool>> query = item => item.SingularName.Equals(singularName);
            var crescentCoveItem = _repository.Find(query).FirstOrDefault();
            if (crescentCoveItem == null && !singularName.Equals(itemNameWithoutPrefix))
            {
                query = item => item.SingularName.Equals(itemNameWithoutPrefix);
                crescentCoveItem = _repository.Find(query).FirstOrDefault();
            }

            return crescentCoveItem == null
                ? new Item {SingularName = itemNameWithoutPrefix}
                : ItemMapper.MapToItem(crescentCoveItem);
        }

        public Item GetItemByPluralName(string pluralName)
        {
            var itemNameWithoutPrefix = RemovePrefix(pluralName);
            Expression<Func<FFXIV.CrescentCove.Item, bool>> query = item => item.PluralName.Equals(pluralName);
            var crescentCoveItem = _repository.Find(query).FirstOrDefault();
            if (crescentCoveItem == null && !pluralName.Equals(itemNameWithoutPrefix))
            {
                query = item => item.PluralName.Equals(itemNameWithoutPrefix);
                crescentCoveItem = _repository.Find(query).FirstOrDefault();
            }

            return crescentCoveItem == null
                ? new Item {PluralName = itemNameWithoutPrefix}
                : ItemMapper.MapToItem(crescentCoveItem);
        }

        public List<string> GetItemNames()
        {
            if (_itemNames != null) return _itemNames;
            var items = _repository.GetAll().ToList();
            _itemNames = items.Select(str => str.ProperName).OrderBy(x => x).ToList();
            return _itemNames;
        }

        public List<string> GetCommonItemNames()
        {
            if (_commonItemNames != null) return _commonItemNames;
            Expression<Func<FFXIV.CrescentCove.Item, bool>> query = item => item.IsCommon && !item.IsRetired;
            var items = _repository.Find(query);
            _commonItemNames = items.Select(str => str.ProperName).OrderBy(x => x).ToList();
            return _commonItemNames;
        }

        public void DeInit()
        {
            _repository = null;
            _commonItemNames = null;
            _itemNames = null;
        }

        private static string RemovePrefix(string text)
        {
            if (text.Length >= 3 && text.Substring(0, 3).ToLower().Equals("an ")) return text.Remove(0, 3);
            if (text.Length >= 2 && text.Substring(0, 2).ToLower().Equals("a ")) return text.Remove(0, 2);
            return text.Length >= 4 && text.Substring(0, 4).ToLower().Equals("the ") ? text.Remove(0, 4) : text;
        }
    }
}