namespace ACT_FFXIV_Aetherbridge
{
	internal class DELogLineParserFactory : ILogLineParserFactory
	{
		public DELogLineParserFactory(IAetherbridge aetherbridge)
		{
			Context = new DELogLineParserContext(aetherbridge);
		}

		public ILogLineParserContext Context { get; set; }

		public ILogLineParser CreateParser()
		{
			return new DELogLineParser(Context);
		}
	}
}