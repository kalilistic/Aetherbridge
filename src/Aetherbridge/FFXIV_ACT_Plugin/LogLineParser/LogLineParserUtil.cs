using System.Text.RegularExpressions;

namespace ACT_FFXIV_Aetherbridge
{
	internal static class LogLineParserUtil
	{
		public static Regex CreateRegex(string pattern)
		{
			return new Regex(pattern, RegexOptions.Compiled);
		}
	}
}