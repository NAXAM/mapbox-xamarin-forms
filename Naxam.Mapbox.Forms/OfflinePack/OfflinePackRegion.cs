using System;
namespace Naxam.Controls.Forms
{
    public class OfflinePackRegion
    {
        public string StyleURL { get; set; }

        public CoordinateBounds Bounds { get; set; }

        public double MaximumZoomLevel { get; set; }

        public double MinimumZoomLevel { get; set; }
    }
}