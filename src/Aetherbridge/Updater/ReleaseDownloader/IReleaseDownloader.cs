namespace ACT_FFXIV.Aetherbridge
{
	public interface IReleaseDownloader
	{
		bool IsLatestRelease(string version);
		bool DownloadLatestRelease();
		void DeInit();
	}
}