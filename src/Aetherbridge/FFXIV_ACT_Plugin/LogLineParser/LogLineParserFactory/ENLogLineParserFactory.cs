namespace ACT_FFXIV_Aetherbridge
{
	internal class ENLogLineParserFactory : ILogLineParserFactory
	{
		public ENLogLineParserFactory(IAetherbridge aetherbridge)
		{
			Context = new ENLogLineParserContext(aetherbridge);
		}

		public ILogLineParserContext Context { get; set; }

		public ILogLineParser CreateParser()
		{
			return new ENLogLineParser(Context);
		}
	}
}