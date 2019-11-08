using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
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
    public partial class DdsPolygonHoles : ContentPage
    {
        public DdsPolygonHoles()
        {
            InitializeComponent();

            map.Center = new LatLng(25.255377, 55.3089185);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 13;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var outerLineString = new LineString(Config.POLYGON_COORDINATES);
            var innerLineString = new LineString(Config.HOLE_COORDINATES[0]);
            var secondInnerLineString = new LineString(Config.HOLE_COORDINATES[1]);

            List<LineString> innerList = new List<LineString> {
                innerLineString,
                secondInnerLineString
            };

            map.Functions.AddSource(new GeoJsonSource("source-id",
              new Feature(new Polygon(new[] { outerLineString, innerLineString, secondInnerLineString }))));

            var polygonFillLayer = new FillLayer("layer-id", "source-id")
            {
                FillColor = Config.BLUE_COLOR
            };

            if (map.Functions.GetLayers().Any(x => x.Id == "road-number-shield")) {
                map.Functions.AddLayerBelow(polygonFillLayer, "road-number-shield");
            }
            else {
                map.Functions.AddLayer(polygonFillLayer);
            }
        }

        static class Config
        {
            public static readonly Color BLUE_COLOR = Color.FromHex("#3bb2d0");
            public static readonly Color RED_COLOR = Color.FromHex("#AF0000");

            public static readonly List<Position> POLYGON_COORDINATES = new List<Position>() {
                FromLngLat(55.30122473231012, 25.26476622289597 ),
                FromLngLat(55.29743486255916, 25.25827212207261),
                FromLngLat(55.28978863411328, 25.251356725509737),
                FromLngLat(55.300027931336984, 25.246425506635504),
                FromLngLat(55.307474692951274, 25.244200378933655),
                FromLngLat(55.31212891895635, 25.256408010450187),
                FromLngLat(55.30774064871093, 25.26266169122738),
                FromLngLat(55.301357710197806, 25.264946609615492),
                FromLngLat(55.30122473231012, 25.26476622289597 ),
            };

            public static readonly List<List<Position>> HOLE_COORDINATES = new List<List<Position>>() {
                new List<Position>()
                  {
                    FromLngLat(55.30084858315658, 25.256531695820797),
                    FromLngLat(55.298280197635705, 25.252243254705405),
                    FromLngLat(55.30163885563897, 25.250501032248863),
                    FromLngLat(55.304059065092645, 25.254700192612702),
                    FromLngLat(55.30084858315658, 25.256531695820797),
                  },
                new List<Position>() {
                    FromLngLat(55.30173763969924, 25.262517391695198),
                    FromLngLat(55.301095543307355, 25.26122200491396),
                    FromLngLat(55.30396028103232, 25.259479911263526),
                    FromLngLat(55.30489872958182, 25.261132667394975),
                    FromLngLat(55.30173763969924, 25.262517391695198),
                  }
              };

            static Position FromLngLat(double lng, double lat)
            {
                return new Position(lat, lng);
            }
        }
    }
}