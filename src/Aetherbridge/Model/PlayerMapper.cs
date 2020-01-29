using System.Collections.Generic;
using System.Linq;
using FFXIV_ACT_Plugin.Common.Models;

namespace ACT_FFXIV_Aetherbridge
{
	public class PlayerMapper
	{
		private readonly ClassJobService _classJobService;
		private readonly WorldService _worldService;

		public PlayerMapper(WorldService worldService, ClassJobService classJobService)
		{
			_worldService = worldService;
			_classJobService = classJobService;
		}

		internal List<Player> MapToPlayers(IEnumerable<Combatant> combatants)
		{
			return combatants?.Select(MapToPlayer).ToList();
		}

		internal Player MapToPlayer(Combatant combatant)
		{
			if (combatant == null) return null;
			var player = new Player
			{
				Id = combatant.ID,
				Name = combatant.Name,
				Level = combatant.Level,
				PartyType = (PartyTypeEnum) combatant.PartyType,
				Order = combatant.Order,
				CurrentWorld = _worldService.GetWorldById(combatant.CurrentWorldID),
				HomeWorld = _worldService.GetWorldById(combatant.WorldID),
				ClassJob = _classJobService.GetClassJobById(combatant.Job)
			};
			return player;
		}
	}
}