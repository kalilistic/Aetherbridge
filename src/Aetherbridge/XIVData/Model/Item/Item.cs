namespace ACT_FFXIV.Aetherbridge.XIVData.Model
{
	public class Item : IGameData
	{
		public ItemLocalized[] Localized { get; set; }
		public bool IsCommon { get; set; }
		public bool IsRetired { get; set; }
		public bool IsUntradable { get; set; }
		public int VendorBuyPrice { get; set; }
		public int ItemAction { get; set; }
		public int Id { get; set; }

		public void SetPropsByStr(string[] propertyStr)
		{
			Id = int.Parse(propertyStr[0]);
			IsUntradable = bool.Parse(propertyStr[1]);
			VendorBuyPrice = int.Parse(propertyStr[2]);
			ItemAction = int.Parse(propertyStr[3]);
			Localized = new[]
			{
				new ItemLocalized
				{
					Language = LanguageEnum.en,
					SingularName = propertyStr[4],
					PluralName = propertyStr[5],
					ProperName = propertyStr[6],
					SingularSearchTerm = propertyStr[7],
					PluralSearchTerm = propertyStr[8],
					SingularREP = propertyStr[9],
					PluralREP = propertyStr[10]
				},
				new ItemLocalized
				{
					Language = LanguageEnum.fr,
					SingularName = propertyStr[11],
					PluralName = propertyStr[12],
					ProperName = propertyStr[13],
					SingularSearchTerm = propertyStr[14],
					PluralSearchTerm = propertyStr[15],
					SingularREP = propertyStr[16],
					PluralREP = propertyStr[17]
				},
				new ItemLocalized
				{
					Language = LanguageEnum.de,
					SingularName = propertyStr[18],
					PluralName = propertyStr[19],
					ProperName = propertyStr[20],
					SingularSearchTerm = propertyStr[21],
					PluralSearchTerm = propertyStr[22],
					SingularREP = propertyStr[23],
					PluralREP = propertyStr[24]
				},
				new ItemLocalized
				{
					Language = LanguageEnum.ja,
					SingularName = propertyStr[25],
					PluralName = propertyStr[26],
					ProperName = propertyStr[27],
					SingularSearchTerm = propertyStr[28],
					PluralSearchTerm = propertyStr[29],
					SingularREP = propertyStr[30],
					PluralREP = propertyStr[31]
				}
			};

			IsCommon = bool.Parse(propertyStr[32]);
			IsRetired = bool.Parse(propertyStr[33]);
		}
	}
}