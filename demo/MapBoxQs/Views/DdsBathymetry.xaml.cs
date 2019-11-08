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
    public partial class DdsBathymetry : ContentPage
    {
        private const string GEOJSON_SOURCE_ID = "GEOJSON_SOURCE_ID";
        private static readonly LatLngBounds LAKE_BOUNDS = new LatLngBounds(
            new LatLng(44.936236, -85.673450),
            new LatLng(44.932955, -85.669272));

        public DdsBathymetry()
        {
            InitializeComponent();

            map.Center = new LatLng(44.934448, -85.671378);
            map.MapStyle = MapStyle.OUTDOORS;
            map.ZoomLevel = 15.69;
            map.ZoomMinLevel = 14.9;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.RemoveLayer("water-label");
            map.Functions.AddSource(new GeoJsonSource(GEOJSON_SOURCE_ID, "asset://bathymetry-data.geojson")
            {
                IsLocal = true
            });

            SetUpDepthFillLayers();
            SetUpDepthNumberSymbolLayer();
        }


        /**
         * Adds a FillLayer and uses data-driven styling to display the lake's areas
         */
        private void SetUpDepthFillLayers()
        {
            FillLayer depthPolygonFillLayer = new FillLayer("DEPTH_POLYGON_FILL_LAYER_ID", GEOJSON_SOURCE_ID)
            {
                FillColor = Expression.Interpolate(Expression.Linear(),
                 Expression.Get("depth"),
                 Expression.CreateStop(5, Expression.Rgb(16, 158, 210)),
                 Expression.CreateStop(10, Expression.Rgb(37, 116, 145)),
                 Expression.CreateStop(15, Expression.Rgb(69, 102, 124)),
                 Expression.CreateStop(30, Expression.Rgb(31, 52, 67))),
                FillOpacity = .7f,
                // Only display Polygon Features in this layer
                Filter = Expression.Eq(Expression.GeometryType(), Expression.Literal("Polygon"))
            };
            map.Functions.AddLayer(depthPolygonFillLayer);
        }

        /**
         * Adds a SymbolLayer to display the depth of the lake's areas
         */
        private void SetUpDepthNumberSymbolLayer()
        {
            var depthNumberSymbolLayer = new SymbolLayer("DEPTH_NUMBER_SYMBOL_LAYER_ID",
                GEOJSON_SOURCE_ID)
            {
                TextField = Expression.Get("depth"),
                TextSize = 17f,
                TextColor = Color.White,
                TextAllowOverlap = true
            };
            // Only display Point Features in this layer
            depthNumberSymbolLayer.Filter = Expression.Eq(Expression.GeometryType(), Expression.Literal("Point"));

            map.Functions.AddLayerAbove(depthNumberSymbolLayer, "DEPTH_POLYGON_FILL_LAYER_ID");
        }

    }
}