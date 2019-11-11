using System.IO;
using System.Timers;
using GeoJSON.Net.Feature;
using MapBoxQs.Services;
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
    public partial class DdsStyleLineIdentityProperty : ContentPage
    {
        public DdsStyleLineIdentityProperty()
        {
            InitializeComponent();

            map.Center = new LatLng(37.82882682974591, -122.48383155304096);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 15.3;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.AddSource(new GeoJsonSource("lines", "asset://golden_gate_lines.geojson") {
                IsLocal = true
            });

            var layer = new LineLayer("finalLines", "lines")
            {
                LineColor = 
                Expression.Match(Expression.Get("color"), Expression.Rgb(0, 0, 0),
                  Expression.CreateStop("red", Expression.Rgb(247, 69, 93)),
                  Expression.CreateStop("blue", Expression.Rgb(51, 201, 235))),
                Visibility = Expression.Visibility(true),
                LineWidth = 3f
            };
            map.Functions.AddLayer(layer);
        }
    }
}