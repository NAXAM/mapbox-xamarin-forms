using System;
using Naxam.Mapbox.Platform.Droid;
using Sdk = Mapbox.MapboxSdk;

namespace Naxam.Mapbox.Platform.Droid
{
    public class MapboxMapReadyEventArgs : EventArgs
    {
        public Sdk.Maps.MapboxMap Map { get; private set; }
        public Sdk.Maps.MapView MapView { get; private set; }
        public MapboxMapReadyEventArgs (Sdk.Maps.MapboxMap map, Sdk.Maps.MapView mapview)
        {
            MapView = mapview;
            Map = map;
        }
    }
}
