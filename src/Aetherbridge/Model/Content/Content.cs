namespace ACT_FFXIV_Aetherbridge
{
    public class Content : IContent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsHighEndDuty { get; set; }
        public int TerritoryTypeId { get; set; }

        public override string ToString()
        {
            return Id + ":" + Name;
        }
    }
}