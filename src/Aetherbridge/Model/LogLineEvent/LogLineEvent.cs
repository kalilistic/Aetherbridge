using System;

namespace ACT_FFXIV_Aetherbridge
{
    public class LogLineEvent : ILogLineEvent
    {
        public LogLineEvent()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public ACTLogLineEvent ACTLogLineEvent { get; set; }
        public XIVEvent XIVEvent { get; set; }
        public string Timestamp { get; set; }
        public string LogCode { get; set; }
        public string GameLogCode { get; set; }
        public string LogMessage { get; set; }
    }
}