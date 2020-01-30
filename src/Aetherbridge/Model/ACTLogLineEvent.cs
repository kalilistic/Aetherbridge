using System;

namespace ACT_FFXIV_Aetherbridge
{
	public class ACTLogLineEvent
	{
		public DateTime DetectedTime { get; set; }
		public string DetectedZone { get; set; }
		public bool InCombat { get; set; }
		public bool IsImport { get; set; }
		public string LogLine { get; set; }
	}
}