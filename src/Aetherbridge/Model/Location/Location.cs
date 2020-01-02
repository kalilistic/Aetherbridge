using System;

namespace ACT_FFXIV_Aetherbridge
{
    public class Location : ILocation
    {
        public int TerritoryTypeId { get; set; }
        public IPlaceName Region { get; set; }
        public IPlaceName Zone { get; set; }
        public IPlaceName Territory { get; set; }
        public IPlaceName Map { get; set; }


        public override string ToString()
        {
            return "TerritoryTypeId=" + TerritoryTypeId + Environment.NewLine +
                   "Region=" + Region + Environment.NewLine +
                   "Zone=" + Zone + Environment.NewLine +
                   "Territory=" + Territory + Environment.NewLine +
                   "Map=" + Map + Environment.NewLine;
        }
    }
}