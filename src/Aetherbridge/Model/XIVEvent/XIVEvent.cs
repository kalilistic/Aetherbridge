namespace ACT_FFXIV_Aetherbridge
{
    public class XIVEvent
    {
        private XIVEvent()
        {
        }

        public XIVEvent(XIVEventTypeEnum eventType, XIVEventSubTypeEnum subType)
        {
            XIVEventType = eventType;
            XIVEventSubType = subType;
        }

        public XIVEventTypeEnum XIVEventType { get; set; }
        public XIVEventSubTypeEnum XIVEventSubType { get; set; }
        public Item Item { get; set; }
        public Player Actor { get; set; }
        public int Roll { get; set; }
        public ILocation Location { get; set; }
    }
}