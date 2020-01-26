using System;

namespace ACT_FFXIV_Aetherbridge
{
	public interface IACTWrapper
	{
		bool ACTLogLineParserEnabled { get; set; }
		event EventHandler<ACTLogLineEvent> ACTLogLineCaptured;
		dynamic GetACTPlugin(string pluginFileName, string pluginStatus);
		void DeInit();
		string GetAppDataFolderFullName();
		string GetCharacterName();
	}
}