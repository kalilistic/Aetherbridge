using System;
using System.Collections.Generic;

namespace ACT_FFXIV_Aetherbridge
{
    public interface IAetherbridge
    {
        IClassJobService ClassJobService { get; }
        IWorldService WorldService { get; }
        ILocationService LocationService { get; }
        IContentService ContentService { get; }
        IItemService ItemService { get; }
        IAetherbridgeConfig AetherbridgeConfig { get; set; }
        event EventHandler<LogLineEvent> LogLineCaptured;
        void EnableLogLineParser();
        void DisableLogLineParser();
        void ACTLogLineCaptured(object sender, ACTLogLineEvent actLogLineEvent);
        IPlayer GetCurrentPlayer();
        IPlayer GetCurrentPlayerACT();
        List<IPlayer> GetPartyMembers();
        List<IPlayer> GetAllianceMembers();
        IPlayer GetPlayerByName(string name);
        void DeInit();
    }
}