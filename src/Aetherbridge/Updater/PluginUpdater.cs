using System;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace ACT_FFXIV.Aetherbridge
{
	public class UpdateService
	{
		public static void UpdatePlugin(PluginUpdaterSettings settings)
		{
			try
			{
				var downloader = new ReleaseDownloader(new ReleaseDownloaderSettings(
					settings.HTTPClient, settings.AuthorName, settings.RepoName, settings.IncludePreRelease,
					settings.PluginPath));
				if (downloader.IsLatestRelease(settings.Version)) return;
				var result = MessageBox.Show(settings.UpdateMessage,
					settings.RepoName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result != DialogResult.Yes) return;
				var status = downloader.DownloadLatestRelease();
				if (status)
				{
					var form = ActGlobals.oFormActMain;
					var method = form.GetType().GetMethod("RestartACT");
					if (method != null) method.Invoke(form, new object[] {true, settings.RestartMessage});
				}
				else
				{
					MessageBox.Show(settings.FailureMessage, settings.RepoName,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch (Exception)
			{
				MessageBox.Show(settings.FailureMessage, settings.RepoName,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}