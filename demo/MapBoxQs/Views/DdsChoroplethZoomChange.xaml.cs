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
    public partial class DdsChoroplethZoomChange : ContentPage
    {
        private const int ZOOM_THRESHOLD = 4;

        public DdsChoroplethZoomChange()
        {
            InitializeComponent();

            map.Center = new LatLng(38.88, -98);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 3;
            map.ZoomMinLevel = 3;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var vectorSource = new VectorSource(
              "population",
              "http://api.mapbox.com/v4/mapbox.660ui7x6.json?access_token=" + MapBoxService.AccessToken
            );
            map.Functions.AddSource(vectorSource);

            var statePopulationLayer = new FillLayer("state-population", "population")
            {
                SourceLayer = "state_county_population_2014_cen",
                Filter = Expression.Eq(Expression.Get("isState"), Expression.Literal(true)),
                FillColor = Expression.Step(
                    Expression.Get("population"), Expression.Rgb(0, 0, 0),
                    Expression.CreateStop(0, Expression.Rgb(242, 241, 45)),
                    Expression.CreateStop(750000, Expression.Rgb(238, 211, 34)),
                    Expression.CreateStop(1000000, Expression.Rgb(218, 156, 32)),
                    Expression.CreateStop(2500000, Expression.Rgb(202, 131, 35)),
                    Expression.CreateStop(5000000, Expression.Rgb(184, 107, 37)),
                    Expression.CreateStop(7500000, Expression.Rgb(162, 86, 38)),
                    Expression.CreateStop(10000000, Expression.Rgb(139, 66, 37)),
                    Expression.CreateStop(25000000, Expression.Rgb(114, 49, 34))),
                FillOpacity = (0.75f)
            };

            map.Functions.AddLayerBelow(statePopulationLayer, "waterway-label");

            var countyPopulationLayer = new FillLayer("county-population", "population")
            {
                SourceLayer = ("state_county_population_2014_cen"),
                Filter = Expression.Eq(Expression.Get("isCounty"), Expression.Literal(true)),
                FillColor = Expression.Step(Expression.Get("population"), Expression.Rgb(0, 0, 0),
                Expression.CreateStop(0, Expression.Rgb(242, 241, 45)),
                Expression.CreateStop(100, Expression.Rgb(238, 211, 34)),
                Expression.CreateStop(1000, Expression.Rgb(230, 183, 30)),
                Expression.CreateStop(5000, Expression.Rgb(218, 156, 32)),
                Expression.CreateStop(10000, Expression.Rgb(202, 131, 35)),
                Expression.CreateStop(50000, Expression.Rgb(184, 107, 37)),
                Expression.CreateStop(100000, Expression.Rgb(162, 86, 38)),
                Expression.CreateStop(500000, Expression.Rgb(139, 66, 37)),
                Expression.CreateStop(1000000, Expression.Rgb(114, 49, 34))),
                FillOpacity = 0.75f,
                Visibility = Expression.Visibility(false)
            };

            map.Functions.AddLayerBelow(countyPopulationLayer, "waterway-label");

            var updateLayer = new FillLayer("county-population", "population");
            map.CameraMovedCommand = new Command<CameraPosition>((cameraPosition) =>
            {
                var visible = cameraPosition.Zoom > ZOOM_THRESHOLD;
                updateLayer.Visibility = Expression.Visibility(visible);
                map.Functions.UpdateLayer(updateLayer);
            });
        }
    }
}