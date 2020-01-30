namespace ACT_FFXIV_Aetherbridge
{
	internal class JALogLineParserFactory : ILogLineParserFactory
	{
		public JALogLineParserFactory(IAetherbridge aetherbridge)
		{
			Context = new JALogLineParserContext(aetherbridge);
		}

		public ILogLineParserContext Context { get; set; }

		public ILogLineParser CreateParser()
		{
			return new JALogLineParser(Context);
		}
	}
}