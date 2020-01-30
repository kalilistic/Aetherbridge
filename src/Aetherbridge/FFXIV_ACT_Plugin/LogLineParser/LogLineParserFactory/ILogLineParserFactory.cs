namespace ACT_FFXIV_Aetherbridge
{
	internal interface ILogLineParserFactory
	{
		ILogLineParserContext Context { get; set; }
		ILogLineParser CreateParser();
	}
}