using System.IO;
using System.Timers;
using GeoJSON.Net.Feature;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DdsDrawGeojsonLine : ContentPage
    {
        public DdsDrawGeojsonLine()
        {
            InitializeComponent();

            map.Center = new LatLng(45.5076, -122.6736);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 11;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream("MapBoxQs.geojson.example.geojson")) {
                using (var textReader = new StreamReader(stream)) {
                    using (var jsonReader = new JsonTextReader(textReader)) {
                        var serializer = new JsonSerializer();
                        var featureCollection = serializer.Deserialize<FeatureCollection>(jsonReader);

                        map.Functions.AddSource(new GeoJsonSource("line-source", featureCollection));

                        // The layer properties for our line. This is where we make the line dotted, set the
                        // color, etc.
                        map.Functions.AddLayer(new LineLayer("linelayer", "line-source")
                        {
                            LineCap = LayerProperty.LINE_CAP_SQUARE,
                            LineJoin = LayerProperty.LINE_JOIN_MITER,
                            LineBlur = .7f,
                            LineWidth = 7f,
                            LineColor = Color.FromHex("#3bb2d0")
                        });
                    }
                }
            }
        }
    }
}