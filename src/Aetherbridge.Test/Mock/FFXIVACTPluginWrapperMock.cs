using System;
using System.Collections.Generic;
using System.Linq;
using FFXIV_ACT_Plugin.Common.Models;

namespace ACT_FFXIV_Aetherbridge.Test.Mock
{
    public class FFXIVACTPluginWrapperMock : IFFXIVACTPluginWrapper
    {
        private readonly List<Combatant> _combatants;

        public FFXIVACTPluginWrapperMock()
        {
            _combatants = new List<Combatant>
            {
                new Combatant
                {
                    ID = 12345,
                    Name = "Combatant One"
                },
                new Combatant
                {
                    ID = 43432,
                    Name = "Combatant Two"
                },
                new Combatant
                {
                    ID = 23232,
                    Name = "Combatant Three"
                },
                new Combatant
                {
                    ID = 5454345,
                    Name = "Combatant Four"
                },
                new Combatant
                {
                    ID = 767566,
                    Name = "Combatant Five"
                },
                new Combatant
                {
                    ID = 956756,
                    Name = "Combatant Six"
                },
                new Combatant
                {
                    ID = 1213476,
                    Name = "Combatant Seven"
                },
                new Combatant
                {
                    ID = 6575634,
                    Name = "Combatant Eight"
                },
                new Combatant
                {
                    ID = 222426,
                    Name = "Combatant Nine"
                },
                new Combatant
                {
                    ID = 157777,
                    Name = "Combatant Ten"
                },
                new Combatant
                {
                    ID = 5433433,
                    Name = "Combatant Eleven"
                },
                new Combatant
                {
                    ID = 9999887,
                    Name = "John Smith"
                }
            };
        }

        public void DeInit()
        {
            throw new NotImplementedException();
        }

        public FFXIV_ACT_Plugin.Common.Language GetSelectedLanguageId()
        {
            return FFXIV_ACT_Plugin.Common.Language.English;
        }

        public uint GetCurrentTerritoryId()
        {
            return uint.Parse("340");
        }

        public Combatant GetCurrentCombatant()
        {
            return _combatants.ToList()[0];
        }

        public List<Combatant> GetAllCombatants()
        {
            return _combatants;
        }

        public List<Combatant> GetPartyCombatants()
        {
            return new List<Combatant>
            {
                _combatants.ToList()[4],
                _combatants.ToList()[5]
            };
        }

        public List<Combatant> GetAllianceCombatants()
        {
            return new List<Combatant>
            {
                _combatants.ToList()[1],
                _combatants.ToList()[2],
                _combatants.ToList()[3]
            };
        }

        public Combatant GetCombatantByName(string name)
        {
            return _combatants.ToList()[0];
        }

        public List<Zone> GetZoneList()
        {
            throw new NotImplementedException();
        }
    }
}