namespace ACT_FFXIV_Aetherbridge
{
    public interface IItem
    {
        int Id { get; set; }
        string ProperName { get; set; }
        string SingularName { get; set; }
        string PluralName { get; set; }
        int Quantity { get; set; }
        bool IsHQ { get; set; }
        bool IsCommon { get; set; }
        bool IsRetired { get; set; }
    }
}