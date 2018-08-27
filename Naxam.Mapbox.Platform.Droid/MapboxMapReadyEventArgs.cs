using System;
using Sdk = Com.Mapbox.Mapboxsdk;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public class MapboxMapReadyEventArgs : EventArgs
    {
        public Sdk.Maps.MapboxMap Map { get; private set; }
        public Sdk.Maps.MapView MapView { get; private set; }
        public MapboxMapReadyEventArgs(Sdk.Maps.MapboxMap map, Sdk.Maps.MapView mapview)
        {
            MapView = mapview;
            Map = map;
        }
    }
}
