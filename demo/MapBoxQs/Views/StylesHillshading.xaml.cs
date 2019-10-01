using System.Timers;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesHillshading : ContentPage
    {
        public StylesHillshading()
        {
            InitializeComponent();

            map.Center = new LatLng(46.133872, 8.5274353171);
            map.MapStyle = MapStyle.OUTDOORS;
            map.ZoomLevel = 7.10806931333;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            string LAYER_ID = "hillshade-layer";
            string SOURCE_ID = "hillshade-source";
            string SOURCE_URL = "mapbox://mapbox.terrain-rgb";
            string HILLSHADE_HIGHLIGHT_COLOR = "#008924";

            var rasterDemSource = new RasterDemSource(SOURCE_ID, SOURCE_URL);
            map.Functions.AddSource(rasterDemSource);

            // Create and style a hillshade layer to add to the map
            var hillshadeLayer = new HillshadeLayer(LAYER_ID, SOURCE_ID) {
                HillshadeHighlightColor = Color.FromHex(HILLSHADE_HIGHLIGHT_COLOR),
                HillshadeShadowColor = Color.Black
            };

            // Add the hillshade layer to the map
            map.Functions.AddLayerBelow(hillshadeLayer, "aerialway");
        }
    }
}