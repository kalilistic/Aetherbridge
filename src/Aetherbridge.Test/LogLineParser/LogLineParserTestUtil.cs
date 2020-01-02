using ACT_FFXIV_Aetherbridge.Test.Mock;

namespace ACT_FFXIV_Aetherbridge.Test.LogLineParser
{
    public class LogLineParserTestUtil
    {
        internal static LogLineEvent ParseEvent(string logLine)
        {
            var actLogLineEvent = new ACTLogLineEvent {LogLine = logLine};
            var aetherbridge = AetherbridgeMock.GetInstance();
            var logLineEvent = new ACT_FFXIV_Aetherbridge.LogLineParser(aetherbridge).Parse(actLogLineEvent);
            return logLineEvent;
        }
    }
}