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
    public partial class DdsSymbolLayerClustering : ContentPage
    {
        public DdsSymbolLayerClustering()
        {
            InitializeComponent();

            map.Center = new LatLng(2.69719, 126.355705);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 4.119118;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            InitLayerIcons();
            AddClusteredGeoJsonSource();
        }


        private void InitLayerIcons()
        {
            map.Functions.AddStyleImage(new IconImageSource
            {
                Id = "single-quake-icon-id",
                Source = "single_quake_icon.png"
            });
            map.Functions.AddStyleImage(new IconImageSource
            {
                Id = "quake-triangle-icon-id",
                Source = "earthquake_triangle.png"
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
                          ClusterRadius = (50)
                      }
                  }
                );

            //Creating a SymbolLayer icon layer for single data/icon points
            map.Functions.AddLayer(new SymbolLayer("unclustered-points", "earthquakes")
            {
                IconImage = ("single-quake-icon-id"),
                IconSize = (
                Expression.Division(
                  Expression.Get("mag"), Expression.Literal(4.0f)
                )
              )
            });

            // Use the earthquakes GeoJSON source to create three point ranges.
            int[] layers = new int[] { 150, 20, 0 };

            for (int i = 0; i < layers.Length; i++) {
                //Add clusters' SymbolLayers images
                SymbolLayer symbolLayer = new SymbolLayer("cluster-" + i, "earthquakes");

                symbolLayer.IconImage = ("quake-triangle-icon-id");
                Expression pointCount = Expression.ToNumber(Expression.Get("point_count"));

                // Add a filter to the cluster layer that hides the icons based on "point_count"
                symbolLayer.Filter = (
                  i == 0
                    ? Expression.All(Expression.Has("point_count"),
                    Expression.Gte(pointCount, Expression.Literal(layers[i]))
                  ) : Expression.All(Expression.Has("point_count"),
                    Expression.Gte(pointCount, Expression.Literal(layers[i])),
                    Expression.Lt(pointCount, Expression.Literal(layers[i - 1]))
                  )
                );
                map.Functions.AddLayer(symbolLayer);
            }

            //Add a SymbolLayer for the cluster data number point count
            map.Functions.AddLayer(new SymbolLayer("count", "earthquakes")
            {
                TextField = (Expression.ToString(Expression.Get("point_count"))),
                TextSize = (12f),
                TextColor = (Color.Black),
                TextIgnorePlacement = (true),
                // The .5f offset moves the data numbers down a little bit so that they're
                // in the middle of the triangle cluster image
                TextOffset = (new float[] { 0f, .5f }),
                TextAllowOverlap = (true)
            });
        }
    }
}