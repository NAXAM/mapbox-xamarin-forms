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
    public partial class ExtrusionRotation : ContentPage
    {
        public ExtrusionRotation()
        {
            InitializeComponent();

            map.Center = new LatLng(40.706, -74.011);
            map.MapStyle = MapStyle.DARK;
            map.ZoomLevel = 16;
            map.Pitch = 45;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var courseRouteGeoJson = new GeoJsonSource(
                "coursedata", "asset://marathon_route.geojson")
            {
                IsLocal =  true
            };
            map.Functions.ShowBuilding(new BuildingInfo() {
                Color = Color.LightGray,
                Opacity = 0.6f,
                MinZoomLevel = 15,
                IsVisible = true
            });

            // Add FillExtrusion layer to map using GeoJSON data
            map.Functions.AddLayer(new FillExtrusionLayer("course", "coursedata"){
                FillExtrusionColor = Color.Yellow,
                FillExtrusionOpacity = 0.7f,
                FillExtrusionHeight = Expression.Get("e")
            });
        }
    }
}