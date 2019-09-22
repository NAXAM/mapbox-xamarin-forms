using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesVectorLayerPage : ContentPage
    {
        public StylesVectorLayerPage()
        {
            InitializeComponent();

            map.Center = new LatLng(37.753574, -122.447303);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 13;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var source = new VectorSource("terrain-data", "mapbox://mapbox.mapbox-terrain-v2");
            map.Functions.AddSource(source);

            var layer = new LineLayer("terrain-data", source.Id) {
                SourceLayer = "contour",
                LineJoin = "round",
                LineCap = "round",
                LineColor = Color.FromHex("#ff69b4"),
                LineWidth = 1f
            };
            map.Functions.AddLayer(layer);
        }
    }
}