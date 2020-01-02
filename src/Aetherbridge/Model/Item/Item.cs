namespace ACT_FFXIV_Aetherbridge
{
    public class Item : IItem
    {
        public int Id { get; set; }
        public string ProperName { get; set; }
        public string SingularName { get; set; }
        public string PluralName { get; set; }
        public int Quantity { get; set; }
        public bool IsHQ { get; set; }
        public bool IsCommon { get; set; }
        public bool IsRetired { get; set; }
    }
}