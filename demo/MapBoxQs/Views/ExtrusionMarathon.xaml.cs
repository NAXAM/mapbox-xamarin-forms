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
    public partial class ExtrusionMarathon : ContentPage
    {
        public ExtrusionMarathon()
        {
            InitializeComponent();

            map.Center = new LatLng(33.334915, -118.335074);
            map.MapStyle = MapStyle.OUTDOORS;
            map.ZoomLevel = 12.692151;
            map.ZoomMinLevel = 11;
            map.Pitch = 55.8873;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var courseRouteGeoJson = new GeoJsonSource(
                "coursedata", "asset://marathon_route.geojson")
            {
                IsLocal =  true
            };
            map.Functions.AddSource(courseRouteGeoJson);

            // Add FillExtrusion layer to map using GeoJSON data
            map.Functions.AddLayer(new FillExtrusionLayer("course", "coursedata"){
                FillExtrusionColor = Color.Yellow,
                FillExtrusionOpacity = 0.7f,
                FillExtrusionHeight = Expression.Get("e")
            });
        }
    }
}