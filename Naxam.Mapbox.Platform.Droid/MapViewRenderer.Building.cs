using System;
using System.Collections.Specialized;
using System.Linq;
using Com.Mapbox.Mapboxsdk.Plugins.Building;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Xamarin.Forms.Platform.Android;
using Sdk = Com.Mapbox.Mapboxsdk;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
    {
        public void ShowBuilding(BuildingInfo buildingInfo)
        {
            if (map == null || mapStyle == null) return;

            var buildingPlugin = new BuildingPlugin(fragment.MapView, map, mapStyle, buildingInfo.AboveLayerId);
            buildingPlugin.SetColor(buildingInfo.Color.ToAndroid());
            buildingPlugin.SetOpacity(buildingInfo.Opacity);
            buildingPlugin.SetMinZoomLevel(buildingInfo.MinZoomLevel);
            buildingPlugin.SetVisibility(buildingInfo.IsVisible);

            // TODO Clean up buildingPlugin
        }
    }
}