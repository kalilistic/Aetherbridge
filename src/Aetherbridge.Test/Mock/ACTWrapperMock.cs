using System;

namespace ACT_FFXIV_Aetherbridge.Test.Mock
{
	public class ACTWrapperMock : IACTWrapper
	{
		public event EventHandler<ACTLogLineEvent> ACTLogLineCaptured;

		public dynamic GetACTPlugin(string pluginFileName, string pluginStatus)
		{
			return new ACTPluginMock();
		}

		public void DeInit()
		{
		}

		public string GetAppDataFolderFullName()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}

		public string GetCharacterName()
		{
			return "John Smith";
		}

		public bool ACTLogLineParserEnabled { get; set; }

		protected virtual void OnACTLogLineCaptured(ACTLogLineEvent e)
		{
			ACTLogLineCaptured?.Invoke(this, e);
		}
	}
}