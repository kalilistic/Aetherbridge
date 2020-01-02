namespace ACT_FFXIV_Aetherbridge
{
    public interface IPlayer
    {
        uint Id { get; set; }
        IClassJob ClassJob { get; set; }
        int Level { get; set; }
        string Name { get; set; }
        IWorld CurrentWorld { get; set; }
        IWorld HomeWorld { get; set; }
        PartyTypeEnum PartyType { get; set; }
        int Order { get; set; }
        bool IsReporter { get; set; }
    }
}