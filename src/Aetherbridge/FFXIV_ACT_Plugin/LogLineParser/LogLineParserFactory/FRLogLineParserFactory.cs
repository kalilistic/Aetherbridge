namespace ACT_FFXIV_Aetherbridge
{
	internal class FRLogLineParserFactory : ILogLineParserFactory
	{
		public FRLogLineParserFactory(IAetherbridge aetherbridge)
		{
			Context = new FRLogLineParserContext(aetherbridge);
		}

		public ILogLineParserContext Context { get; set; }

		public ILogLineParser CreateParser()
		{
			return new FRLogLineParser(Context);
		}
	}
}