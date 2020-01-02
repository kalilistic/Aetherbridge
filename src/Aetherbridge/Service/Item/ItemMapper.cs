using System.Collections.Generic;
using System.Linq;

namespace ACT_FFXIV_Aetherbridge
{
    public static class ItemMapper
    {
        public static List<Item> MapToItems(List<FFXIV.CrescentCove.Item> gameDataItems)
        {
            return gameDataItems?.Select(MapToItem).ToList();
        }

        public static Item MapToItem(FFXIV.CrescentCove.Item gameDataItem)
        {
            if (gameDataItem == null) return null;
            return new Item
            {
                Id = gameDataItem.Id,
                ProperName = gameDataItem.ProperName,
                SingularName = gameDataItem.SingularName,
                PluralName = gameDataItem.PluralName,
                Quantity = gameDataItem.Quantity,
                IsHQ = gameDataItem.IsHQ,
                IsCommon = gameDataItem.IsCommon
            };
        }
    }
}