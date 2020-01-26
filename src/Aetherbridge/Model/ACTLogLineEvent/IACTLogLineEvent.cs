using System;

namespace ACT_FFXIV_Aetherbridge
{
	public interface IACTLogLineEvent
	{
		DateTime DetectedTime { get; set; }
		string DetectedZone { get; set; }
		bool InCombat { get; set; }
		bool IsImport { get; set; }
		string LogLine { get; set; }
	}
}