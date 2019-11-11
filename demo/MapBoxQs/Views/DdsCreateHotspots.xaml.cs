using System.Collections.Generic;
using System.IO;
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
    public partial class DdsCreateHotspots : ContentPage
    {
        public DdsCreateHotspots()
        {
            InitializeComponent();

            map.Center = new LatLng(40.66995747013945, -103.59179687498357);
            map.MapStyle = MapStyle.DARK;
            map.ZoomLevel = 3;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.AddSource(
                // Point to GeoJSON data. This example visualizes all M1.0+ earthquakes from
                // 12/22/15 to 1/21/16 as logged by USGS' Earthquake hazards program.
                new GeoJsonSource("earthquakes",
                  "https://www.mapbox.com/mapbox-gl-js/assets/earthquakes.geojson")
                {
                    Options = new GeoJsonOptions()
                    {
                        Cluster = (true),
                        ClusterMaxZoom = (15), // Max zoom to cluster points on
                        ClusterRadius = (20), // Use small cluster radius for the hotspots look
                    }
                }
              );

            // Use the earthquakes source to create four layers:
            // three for each cluster category, and one for unclustered points

            // Each point range gets a different fill color.
            var layers = new KeyValuePair<int, Color>[] {
              new KeyValuePair<int, Color> (150, Color.FromHex("#E55E5E")),
              new KeyValuePair<int, Color>(20, Color.FromHex("#F9886C")),
              new KeyValuePair<int, Color>(0, Color.FromHex("#FBB03B"))
            };

            CircleLayer unclustered = new CircleLayer("unclustered-points", "earthquakes")
            {
                CircleColor = Color.FromHex("#FBB03B"),
                CircleRadius = 20f,
                CircleBlur = 1f,
                Filter = Expression.Neq(Expression.Get("cluster"), Expression.Literal(true))
            };
            map.Functions.AddLayerBelow(unclustered, "building");

            for (int i = 0; i < layers.Length; i++) {
                var pointCount = Expression.ToNumber(Expression.Get("point_count"));
                var circles = new CircleLayer("cluster-" + i, "earthquakes")
                {
                    CircleColor = layers[i].Value,
                    CircleRadius = 70f,
                    CircleBlur = 1f,
                    Filter = i == 0
                    ? Expression.Gte(pointCount, Expression.Literal(layers[i].Key))
                    : Expression.All(
                      Expression.Gte(pointCount, Expression.Literal(layers[i].Key)),
                      Expression.Lt(pointCount, Expression.Literal(layers[i - 1].Key))
                    )
                };
                map.Functions.AddLayerBelow(circles, "building");
            }
        }
    }
}