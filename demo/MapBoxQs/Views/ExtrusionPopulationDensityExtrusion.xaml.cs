using System.Timers;
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
    public partial class ExtrusionPopulationDensityExtrusion : ContentPage
    {
        public ExtrusionPopulationDensityExtrusion()
        {
            InitializeComponent();

            map.Center = new LatLng(37.784282779035216, -122.4232292175293);
            map.MapStyle = MapStyle.OUTDOORS;
            map.ZoomLevel = 12;
            map.ZoomMinLevel = 12;
            map.ZoomMaxLevel = 16;
            map.Pitch = 45;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.AddSource(new VectorSource("population", "mapbox://peterqliu.d0vin3el"));

            var fillsLayer = new FillLayer("fills", "population")
            {
                SourceLayer = ("outgeojson"),
                Filter = (Expression.All(Expression.Lt(Expression.Get("pkm2"), 300000))),
                FillColor = Expression.Interpolate(
                    Expression.Exponential(1f),
                    Expression.Get("pkm2"),
                    Expression.CreateStop(0, Expression.Rgb(22, 14, 35)),
                    Expression.CreateStop(14500, Expression.Rgb(0, 97, 127)),
                    Expression.CreateStop(145000, Expression.Rgb(85, 223, 255)))
            };
            map.Functions.AddLayerBelow(fillsLayer, "water");

            var fillExtrusionLayer = new FillExtrusionLayer("extrusions", "population")
            {
                SourceLayer = ("outgeojson"),
                Filter = (Expression.All(Expression.Gt(Expression.Get("p"), 1),
                    Expression.Lt(Expression.Get("pkm2"), 300000))),
                FillExtrusionColor = (Expression.Interpolate(
                    Expression.Exponential(1f),
                    Expression.Get("pkm2"),
                    Expression.CreateStop(0, Expression.Rgb(22, 14, 35)),
                    Expression.CreateStop(14500, Expression.Rgb(0, 97, 127)),
                    Expression.CreateStop(145000, Expression.Rgb(85, 233, 255)))),
                FillExtrusionBase = (0f),
                FillExtrusionHeight = (Expression.Interpolate(
                    Expression.Exponential(1f),
                    Expression.Get("pkm2"),
                    Expression.CreateStop(0, 0f),
                    Expression.CreateStop(1450000, 20000f)))
            };
            map.Functions.AddLayerBelow(fillExtrusionLayer, "airport-label");

            map.Functions.AnimateCamera(new CameraPosition(map.Center, map.ZoomLevel, map.Pitch, 0), 1000);
        }
    }
}