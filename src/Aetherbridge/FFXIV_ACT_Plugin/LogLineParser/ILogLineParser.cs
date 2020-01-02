namespace ACT_FFXIV_Aetherbridge
{
    public interface ILogLineParser
    {
        LogLineEvent Parse(ACTLogLineEvent actLogLineEvent);
    }
}