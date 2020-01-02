using System.Collections.Generic;
using System.Linq;
using FFXIV_ACT_Plugin.Common;
using FFXIV_ACT_Plugin.Common.Models;

namespace ACT_FFXIV_Aetherbridge
{
    internal sealed class FFXIVACTPluginWrapper : IFFXIVACTPluginWrapper
    {
        private static volatile IFFXIVACTPluginWrapper _wrapper;
        private static readonly object Lock = new object();
        private readonly IDataRepository _dataRepository;

        private FFXIVACTPluginWrapper(IACTWrapper actWrapper)
        {
            var plugin = actWrapper.GetACTPlugin("FFXIV_ACT_Plugin",
                "FFXIV Plugin Started.");
            _dataRepository = plugin.GetType().GetProperty("DataRepository")
                .GetValue(plugin, null);
        }

        public void DeInit()
        {
        }

        public FFXIV_ACT_Plugin.Common.Language GetSelectedLanguageId()
        {
            return _dataRepository.GetSelectedLanguageID();
        }

        public uint GetCurrentTerritoryId()
        {
            return _dataRepository.GetCurrentTerritoryID();
        }

        public Combatant GetCurrentCombatant()
        {
            return (
                from combatant in _dataRepository?.GetCombatantList()
                where _dataRepository?.GetCurrentPlayerID() == combatant.ID
                select combatant).FirstOrDefault();
        }

        public List<Combatant> GetAllCombatants()
        {
            return new List<Combatant>(_dataRepository.GetCombatantList());
        }

        public List<Combatant> GetPartyCombatants()
        {
            return _dataRepository?.GetCombatantList()?
                    .Where(combatant => combatant.PartyType == FFXIV_ACT_Plugin.Common.Models.PartyTypeEnum.Party) as
                List<Combatant>;
        }

        public List<Combatant> GetAllianceCombatants()
        {
            return _dataRepository?.GetCombatantList()?
                    .Where(combatant => combatant.PartyType == FFXIV_ACT_Plugin.Common.Models.PartyTypeEnum.Alliance) as
                List<Combatant>;
        }

        public Combatant GetCombatantByName(string name)
        {
            return _dataRepository?.GetCombatantList()?.First(combatant => combatant.Name.Equals(name));
        }

        public List<Zone> GetZoneList()
        {
            return _dataRepository.GetResourceDictionary(ResourceType.ZoneList_EN)
                .Select(zone => new Zone(zone.Key, zone.Value)).ToList();
        }

        public static IFFXIVACTPluginWrapper GetInstance()
        {
            return _wrapper;
        }

        public static void Initialize(IACTWrapper actWrapper)
        {
            if (_wrapper != null) return;
            lock (Lock)
            {
                if (_wrapper == null)
                    _wrapper = new FFXIVACTPluginWrapper(actWrapper);
            }
        }
    }
}