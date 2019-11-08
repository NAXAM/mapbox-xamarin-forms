using System.Timers;
using MapBoxQs.Services;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DdsStyleCirclesCategorically : ContentPage
    {
        public DdsStyleCirclesCategorically()
        {
            InitializeComponent();

            map.Center = new LatLng(37.753574, -122.447303);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 12;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.AddSource(new VectorSource(
                "ethnicity-source",
                "http://api.mapbox.com/v4/examples.8fgz4egr.json?access_token=" + MapBoxService.AccessToken
            ));

            var circleLayer = new CircleLayer("population", "ethnicity-source")
            {
                SourceLayer = "sf2010",
                CircleRadius =
                Expression.Interpolate(
                  Expression.Exponential(1.75f),
                  Expression.Zoom(),
                  Expression.CreateStop(12f, 2f),
                  Expression.CreateStop(22f, 180f)
                ),
                CircleColor =
                Expression.Match(
                    Expression.Get("ethnicity"), Expression.Rgb(0f, 0f, 0f),
                  Expression.CreateStop("white", Expression.Rgb(251f, 176f, 59f)),
                  Expression.CreateStop("Black", Expression.Rgb(34f, 59f, 83f)),
                  Expression.CreateStop("Hispanic", Expression.Rgb(229f, 94f, 94f)),
                  Expression.CreateStop("Asian", Expression.Rgb(59f, 178f, 208f)),
                  Expression.CreateStop("Other", Expression.Rgb(204f, 204f, 204f)))
            };

            map.Functions.AddLayer(circleLayer);
        }
    }
}