using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
	public interface IWorldService
	{
		IWorld GetWorldById(int id);
		IWorld GetWorldById(uint id);
		List<IWorld> GetWorlds();
		string GetWorldsAsDelimitedString();
		void DeInit();
	}
}