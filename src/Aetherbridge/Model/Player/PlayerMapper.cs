using System.Collections.Generic;
using System.Linq;
using FFXIV_ACT_Plugin.Common.Models;

namespace ACT_FFXIV_Aetherbridge
{
	public class PlayerMapper
	{
		private readonly IClassJobService _classJobService;
		private readonly IWorldService _worldService;

		public PlayerMapper(IWorldService worldService, IClassJobService classJobService)
		{
			_worldService = worldService;
			_classJobService = classJobService;
		}

		internal List<IPlayer> MapToPlayers(IEnumerable<Combatant> combatants)
		{
			return combatants?.Select(MapToPlayer).ToList();
		}

		internal IPlayer MapToPlayer(Combatant combatant)
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