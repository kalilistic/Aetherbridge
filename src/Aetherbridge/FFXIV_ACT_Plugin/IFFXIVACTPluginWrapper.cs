using System.Collections.Generic;
using FFXIV_ACT_Plugin.Common.Models;

namespace ACT_FFXIV_Aetherbridge
{
    internal interface IFFXIVACTPluginWrapper
    {
        void DeInit();
        FFXIV_ACT_Plugin.Common.Language GetSelectedLanguageId();
        uint GetCurrentTerritoryId();
        Combatant GetCurrentCombatant();
        List<Combatant> GetAllCombatants();
        List<Combatant> GetPartyCombatants();
        List<Combatant> GetAllianceCombatants();
        Combatant GetCombatantByName(string name);
        List<Zone> GetZoneList();
    }
}