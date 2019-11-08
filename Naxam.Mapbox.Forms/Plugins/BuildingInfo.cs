using System;

using Xamarin.Forms;

namespace Naxam.Mapbox
{
    public class BuildingInfo
    {
        public Color Color { get; set; } = Color.LightGray;

        public float Opacity { get; set; } = 0.6f;

        public float MinZoomLevel { get; set; } = 15f;

        public bool IsVisible { get; set; }

        public string AboveLayerId { get; set; }
    }
}

