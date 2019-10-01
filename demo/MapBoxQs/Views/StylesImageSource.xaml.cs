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
    public partial class StylesImageSource : ContentPage
    {
        public StylesImageSource()
        {
            InitializeComponent();

            map.Center = new Naxam.Mapbox.LatLng(25.7845, -80.1263);
            map.MapStyle = MapStyle.DARK;
            map.ZoomLevel = 12;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            string ID_IMAGE_SOURCE = "animated_image_source";
            string ID_IMAGE_LAYER = "animated_image_layer";

            LatLngQuad quad = new LatLngQuad(
                new LatLng(25.7836, -80.11725),
                new LatLng(25.783548, -80.1397431334),
                new LatLng(25.7680, -80.13964),
                new LatLng(25.76795, -80.11725)
            );

            // Add an ImageSource to the map
            var src = new MapboxImageSource(ID_IMAGE_SOURCE, quad, "miami_beach.png");
            map.Functions.AddSource(src);

            // Create a raster layer and use the imageSource's ID as the layer's data. Then add a RasterLayer to the map.
            map.Functions.AddLayer(new RasterLayer(ID_IMAGE_LAYER, ID_IMAGE_SOURCE));
        }
    }
}