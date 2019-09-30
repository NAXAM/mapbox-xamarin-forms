
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Feature;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MapBoxQs.Views
{
    public partial class LabLineBehindMovingIconPage : ContentPage
    {
        const string lineSourceId = "trip.src";
        const string dotSourceId = "dot.src";

        public LabLineBehindMovingIconPage()
        {
            InitializeComponent();

            Title = "Line behind moving icon";

            lineString = LoadLine();

            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 13;
            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        GeoJSON.Net.Geometry.LineString lineString;

        void HandleStyleLoaded(MapStyle obj)
        {
            var fisrtPoint = lineString.Coordinates[0];
            map.Center = new LatLng(fisrtPoint.Latitude, fisrtPoint.Longitude);

            IconImageSource icon = (ImageSource)"pink_dot.png";
            map.Functions.AddStyleImage(icon);

            map.Functions.AddSource(new GeoJsonSource()
            {
                Id = lineSourceId,
                Data = new FeatureCollection()
            });
            var routeLayer = new LineLayer("trip.layer", lineSourceId)
            {
                LineColor = Expression.Color(Color.FromHex("#F13C6E")),
                LineCap = Expression.Literal("round"),
                LineJoin = Expression.Literal("round"),
                LineWidth = Expression.Literal(4.0f)
            };
            map.Functions.AddLayerBelow(routeLayer, "road-label");

            map.Functions.AddSource(new GeoJsonSource()
            {
                Id = dotSourceId,
                Data = new Feature(new GeoJSON.Net.Geometry.Point(fisrtPoint))
            });

            var dotLayer = new SymbolLayer("dot.layer", dotSourceId)
            {
                IconImage = Expression.Literal(icon.Id),
                IconSize = Expression.Literal(0.5f),
                IconOffset = Expression.Literal(new[] { 5, 0 }),
                IconIgnorePlacement = Expression.Literal(true),
                IconAllowOverlap = Expression.Literal(true)
            };
            map.Functions.AddLayer(dotLayer);

            Animate();
        }

        FeatureCollection trip = new FeatureCollection();

        async void Animate()
        {
            await Task.Delay(3000);

            for (int i = 1; i < lineString.Coordinates.Count; i++)
            {
                var position = lineString.Coordinates[i];

                map.Functions.UpdateSource(
                    dotSourceId,
                    new Feature(new GeoJSON.Net.Geometry.Point(position), null)
                );

                await Task.Delay(100);
                
                var lineFeature = new Feature(new GeoJSON.Net.Geometry.LineString(lineString.Coordinates.Take(i+1)), null);
                trip.Features.Add(lineFeature);
                map.Functions.UpdateSource(lineSourceId, trip);

                await Task.Delay(1000);
            }
        }

        GeoJSON.Net.Geometry.LineString LoadLine()
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream("MapBoxQs.sample_route_trip.geojson"))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        var serializer = new JsonSerializer();

                        return serializer.Deserialize<FeatureCollection>(jsonReader).Features[0].Geometry as GeoJSON.Net.Geometry.LineString;
                    }
                }
            }
        }
    }
}
