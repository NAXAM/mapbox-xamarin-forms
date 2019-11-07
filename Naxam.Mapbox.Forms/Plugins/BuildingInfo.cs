using System;

using Xamarin.Forms;

namespace Naxam.Mapbox
{
    public class BuildingInfo
    {
        public Color Color { get; set; }

        public float Opacity { get; set; }

        public float MinZoomLevel { get; set; }

        public bool IsVisible { get; set; }

        public string AboveLayerId { get; set; }
    }
}

