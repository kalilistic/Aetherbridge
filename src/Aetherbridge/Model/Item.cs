using System.Text.RegularExpressions;

namespace ACT_FFXIV_Aetherbridge
{
	public class Item
	{
		public int Id { get; set; }
		public string ProperName { get; set; }
		public string SingularName { get; set; }
		public string PluralName { get; set; }
		public int Quantity { get; set; }
		public bool IsHQ { get; set; }
		public bool IsCommon { get; set; }
		public bool IsRetired { get; set; }
		internal string SingularNameKeyword { get; set; }
		internal Regex SingularNameRegex { get; set; }
		internal string PluralNameKeyword { get; set; }
		internal Regex PluralNameRegex { get; set; }
	}
}