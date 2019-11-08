using System;
using System.Collections.Specialized;
using System.Linq;
using Com.Mapbox.Mapboxsdk.Plugins.Building;
using Com.Mapbox.Mapboxsdk.Plugins.Traffic;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Xamarin.Forms.Platform.Android;
using Sdk = Com.Mapbox.Mapboxsdk;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
    {
        TrafficPlugin trafficPlugin;

        public void ShowTraffic(bool visible)
        {
            if (map == null || mapStyle == null) return;

            if (trafficPlugin == null) {
                trafficPlugin = new TrafficPlugin(fragment.MapView, map, mapStyle);
            }

            trafficPlugin.SetVisibility(visible);

            // TODO Clean up trafficPlugin
        }
    }
}