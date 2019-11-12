using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
    public partial class DdsAddRainFallStyle : ContentPage
    {
        public const string ID_SOURCE = "source-id";
        public const string ID_LAYER = "layer-id";
        public const string SOURCE_URL = "mapbox://examples.dwtmhwpu";

        private int index = 1;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public DdsAddRainFallStyle()
        {
            InitializeComponent();

            map.Center = new LatLng(36, 106);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 3.6;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            cancellationTokenSource.Cancel();
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            AddRadarData();

            RefreshGeoJsonRunnable().ContinueWith(t => { });
        }

        private async Task RefreshGeoJsonRunnable()
        {
            while (true) {
                if (cancellationTokenSource.IsCancellationRequested) {
                    break;
                }

                await Task.Delay(1000);

                var layer = new FillLayer(ID_LAYER, ID_SOURCE)
                {
                    Filter = Expression.Eq(Expression.Get("idx"), Expression.Literal(index))
                };

                map.Functions.UpdateLayer(layer);

                index++;
                if (index == 37) {
                    index = 0;
                }
            }
        }

        private void AddRadarData()
        {
            var vectorSource = new VectorSource(
              ID_SOURCE,
              SOURCE_URL
            );
            map.Functions.AddSource(vectorSource);

            var layer = new FillLayer(ID_LAYER, ID_SOURCE)
            {
                SourceLayer = "201806261518",
                Filter = Expression.Eq(Expression.Get("idx"), Expression.Literal(0)),
                Visibility = Expression.Visibility(true),
                FillColor = Expression.Interpolate(
                    Expression.Exponential(1f),
                    Expression.Get("value"),
                    Expression.CreateStop(8, Expression.Rgb(20, 160, 240)),
                    Expression.CreateStop(18, Expression.Rgb(20, 190, 240)),
                    Expression.CreateStop(36, Expression.Rgb(20, 220, 240)),
                    Expression.CreateStop(54, Expression.Rgb(20, 250, 240)),
                    Expression.CreateStop(72, Expression.Rgb(20, 250, 160)),
                    Expression.CreateStop(90, Expression.Rgb(135, 250, 80)),
                    Expression.CreateStop(108, Expression.Rgb(250, 250, 0)),
                    Expression.CreateStop(126, Expression.Rgb(250, 180, 0)),
                    Expression.CreateStop(144, Expression.Rgb(250, 110, 0)),
                    Expression.CreateStop(162, Expression.Rgb(250, 40, 0)),
                    Expression.CreateStop(180, Expression.Rgb(180, 40, 40)),
                    Expression.CreateStop(198, Expression.Rgb(110, 40, 80)),
                    Expression.CreateStop(216, Expression.Rgb(80, 40, 110)),
                    Expression.CreateStop(234, Expression.Rgb(50, 40, 140)),
                    Expression.CreateStop(252, Expression.Rgb(20, 40, 170))
                ),
                FillOpacity = (0.7f)
            };
            map.Functions.AddLayer(layer);
        }
    }
}