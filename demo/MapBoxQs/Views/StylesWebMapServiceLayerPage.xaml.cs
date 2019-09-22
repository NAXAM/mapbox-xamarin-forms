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
    public partial class StylesWebMapServiceLayerPage : ContentPage
    {
        public StylesWebMapServiceLayerPage()
        {
            InitializeComponent();

            map.Center = new LatLng(40.6892, -74.5447);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 8;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var source = new RasterSource(
                "web-map-source", 
                new TileSet("tileset", "https://img.nj.gov/imagerywms/Natural2015?bbox={bbox-epsg-3857}&format=image/png&service=WMS&version=1.1.1&request=GetMap&srs=EPSG:3857&transparent=true&width=256&height=256&layers=Natural2015"), 
                256);
            map.Functions.AddSource(source);

            var layer = new RasterLayer("web-map-layer", source.Id);
            map.Functions.AddLayerBelow(layer, "aeroway-line");
        }
    }
}