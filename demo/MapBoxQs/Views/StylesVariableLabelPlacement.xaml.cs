using System.Collections.Generic;
using System.Timers;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Point = Xamarin.Forms.Point;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesVariableLabelPlacement : ContentPage
    {
        const string GEOJSON_SRC_ID = "poi_source_id";
        const string POI_LABELS_LAYER_ID = "poi_labels_layer_id";

        public StylesVariableLabelPlacement()
        {
            InitializeComponent();

            map.Center = new LatLng(38.907, -77.04);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 11.15;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var source = new GeoJsonSource(GEOJSON_SRC_ID, "asset://poi_places.geojson")
            {
                IsLocal = true
            };
            map.Functions.AddSource(source);

            var layer = new SymbolLayer(POI_LABELS_LAYER_ID, GEOJSON_SRC_ID)
                {
                    TextField = Expression.Get("description"),
                    TextSize = 17f,
                    TextColor = Color.Red,
                    TextVariableAnchor = new []
                    {
                        LayerProperty.TEXT_ANCHOR_TOP, 
                        LayerProperty.TEXT_ANCHOR_BOTTOM, 
                        LayerProperty.TEXT_ANCHOR_LEFT, 
                        LayerProperty.TEXT_ANCHOR_RIGHT
                    },
                    TextJustify = LayerProperty.TEXT_JUSTIFY_AUTO,
                    TextRadialOffset = 0.5f
                };

            map.Functions.AddLayer(layer);
        }
    }
}