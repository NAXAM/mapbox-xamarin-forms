using System;
namespace Naxam.Controls.Mapbox.Forms
{
    public class OfflinePackProgress
    {
        public ulong CountOfResourcesCompleted { get; set; }

        public ulong CountOfBytesCompleted { get; set; }

        public ulong CountOfTilesCompleted { get; set; }

        public ulong CountOfResourcesExpected { get; set; }

        public ulong MaximumResourcesExpected { get; set; }

        public ulong CountOfTileBytesCompleted { get; set; }
    }
}
