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
    public partial class StylesChangeLayerOpacityPage : ContentPage
    {
        public StylesChangeLayerOpacityPage()
        {
            InitializeComponent();

            map.Center = new LatLng(41.8362, -87.6321);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 9.5;
            map.ZoomMinLevel = 7;
            map.ZoomMaxLevel = 13;
            map.UICompassMarginTop = 75;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var source = new RasterSource("chicago-source", "mapbox://mapbox.u8yyzaor");
            map.Functions.AddSource(source);

            var layer = new RasterLayer("chicago", "chicago-source");
            map.Functions.AddLayer(layer);

            opacity.ValueChanged += SliderValueChanged;
            void SliderValueChanged(object sender, ValueChangedEventArgs e)
            {
                layer.RasterOpacity = e.NewValue;
                map.Functions.UpdateLayer(layer);

                txtOpacity.Text = e.NewValue.ToString("P");
            };
        }
    }
}