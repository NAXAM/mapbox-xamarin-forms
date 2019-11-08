using System.Collections;
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
    public partial class DdsCircleLayerClustering : ContentPage
    {
        public DdsCircleLayerClustering()
        {
            InitializeComponent();

            map.Center = new LatLng(12.099, -79.045);
            map.MapStyle = MapStyle.OUTDOORS;
            map.ZoomLevel = 3;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            AddClusteredGeoJsonSource();
            map.Functions.AddStyleImage(new IconImageSource
            {
                Id = "cross-icon-id",
                Source = (ImageSource)"ic_cross.png",
                IsTemplate = true
            });
        }

        private void AddClusteredGeoJsonSource()
        {
            map.Functions.AddSource(
              // Point to GeoJSON data. This example visualizes all M1.0+ earthquakes from
              // 12/22/15 to 1/21/16 as logged by USGS' Earthquake hazards program.
              new GeoJsonSource("earthquakes",
                "https://www.mapbox.com/mapbox-gl-js/assets/earthquakes.geojson")
              {
                  Options = new GeoJsonOptions
                  {
                      Cluster = (true),
                      ClusterMaxZoom = (14),
                      ClusterRadius = (50),
                  }
              }
            );

            // Use the earthquakes GeoJSON source to create three layers: One layer for each cluster category.
            // Each point range gets a different fill color.
            var layers = new KeyValuePair<int, Color>[]{
                new KeyValuePair<int, Color> (150, Color.FromHex("#f74e4e")),
                new KeyValuePair<int, Color>(20, Color.FromHex("#f74e4e")),
                new KeyValuePair<int, Color>(0, Color.FromHex("#f74e4e"))
            };

            //Creating a marker layer for single data points
            SymbolLayer unclustered = new SymbolLayer("unclustered-points", "earthquakes")
            {
                IconImage = "cross-icon-id",
                IconSize = Expression.Division(
                    Expression.Get("mag"), Expression.Literal(4.0f)
                  ),
                IconColor = Expression.Interpolate(
                    Expression.Exponential(1), Expression.Get("mag"),
                    Expression.CreateStop(2.0, Expression.Rgb(0, 255, 0)),
                    Expression.CreateStop(4.5, Expression.Rgb(0, 0, 255)),
                    Expression.CreateStop(7.0, Expression.Rgb(255, 0, 0)))
            };
            map.Functions.AddLayer(unclustered);

            for (int i = 0; i < layers.Length; i++) {
                var pointCount = Expression.ToNumber(Expression.Get("point_count"));

                //Add clusters' circles
                var circles = new CircleLayer("cluster-" + i, "earthquakes")
                {
                    CircleColor = layers[i].Value,
                    CircleRadius = 18f,

                    // Add a filter to the cluster layer that hides the circles based on "point_count"
                    Filter = i == 0
                        ? Expression.All(Expression.Has("point_count"), Expression.Gte(pointCount, Expression.Literal(layers[i].Key)))
                        : Expression.All(Expression.Has("point_count"),
                        Expression.Gte(pointCount, Expression.Literal(layers[i].Key)),
                        Expression.Lt(pointCount, Expression.Literal(layers[i - 1].Key)))
                };
                map.Functions.AddLayer(circles);
            }

            //Add the count labels
            SymbolLayer count = new SymbolLayer("count", "earthquakes")
            {
                TextField = Expression.ToString(Expression.Get("point_count")),
                TextSize = 12f,
                TextColor = Color.White,
                TextIgnorePlacement = (true),
                TextAllowOverlap = (true)
            };
            map.Functions.AddLayer(count);

        }
    }
}