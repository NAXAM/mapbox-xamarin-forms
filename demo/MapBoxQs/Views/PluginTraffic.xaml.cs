using System;
using System.Timers;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PluginTraffic : ContentPage
    {
        private bool visible = true;

        public PluginTraffic()
        {
            InitializeComponent();

            map.Center = new LatLng(1.352506, 103.842874);
            map.MapStyle = MapStyle.DARK;
            map.ZoomLevel = 9.579712;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var courseRouteGeoJson = new GeoJsonSource(
                "coursedata", "asset://marathon_route.geojson")
            {
                IsLocal = true
            };
            map.Functions.ShowTraffic(visible);

            // Add FillExtrusion layer to map using GeoJSON data
            map.Functions.AddLayer(new FillExtrusionLayer("course", "coursedata")
            {
                FillExtrusionColor = Color.Yellow,
                FillExtrusionOpacity = 0.7f,
                FillExtrusionHeight = Expression.Get("e")
            });
        }

        private void HandleTogle(object sender, EventArgs eventArgs)
        {
            map.Functions.ShowTraffic(visible = !visible);
        }
    }
}