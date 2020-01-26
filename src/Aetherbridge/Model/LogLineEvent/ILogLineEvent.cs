namespace ACT_FFXIV_Aetherbridge
{
	public interface ILogLineEvent
	{
		string Id { get; set; }
		IACTLogLineEvent ACTLogLineEvent { get; set; }
		XIVEvent XIVEvent { get; set; }
		string Timestamp { get; set; }
		string LogCode { get; set; }
		string GameLogCode { get; set; }
		string LogMessage { get; set; }
	}
}