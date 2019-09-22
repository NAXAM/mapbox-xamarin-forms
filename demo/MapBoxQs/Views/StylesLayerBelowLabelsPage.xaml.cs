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
    public partial class StylesLayerBelowLabelsPage : ContentPage
    {
        public StylesLayerBelowLabelsPage()
        {
            InitializeComponent();

            map.Center = new LatLng(33.749909, -84.381546);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 8.471903;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var source = new GeoJsonSource
            {
                Id = "urban-areas",
                Url = "https://d2ad6b4ur7yvpq.cloudfront.net/naturalearth-3.3.0/ne_50m_urban_areas.geojson"
            };

            map.Functions.AddSource(source);

            var layer = new FillLayer("urban-areas-fill", source.Id) {
              FillColor = Color.FromHex("#ff0088"),
              FillOpacity = 0.4f
            };
            map.Functions.AddLayerBelow(layer, "water");
        }
    }
}