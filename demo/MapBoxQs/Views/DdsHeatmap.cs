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
    public partial class DdsHeatmap : ContentPage
    {
        private static readonly string EARTHQUAKE_SOURCE_URL = "https://www.mapbox.com/mapbox-gl-js/assets/earthquakes.geojson";
        private static readonly string EARTHQUAKE_SOURCE_ID = "earthquakes";
        private static readonly string HEATMAP_LAYER_ID = "earthquakes-heat";
        private static readonly string HEATMAP_LAYER_SOURCE = "earthquakes";
        private static readonly string CIRCLE_LAYER_ID = "earthquakes-circle";

        public DdsHeatmap()
        {
            InitializeComponent();

            map.Center = new LatLng(52.276656, -153.147986);
            map.MapStyle = MapStyle.DARK;
            map.ZoomLevel = 2.040885;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            AddEarthquakeSource();
            AddHeatmapLayer();
            AddCircleLayer();
        }

        private void AddEarthquakeSource()
        {
            map.Functions.AddSource(new GeoJsonSource(EARTHQUAKE_SOURCE_ID, EARTHQUAKE_SOURCE_URL));
        }

        private void AddHeatmapLayer()
        {
            HeatmapLayer layer = new HeatmapLayer(HEATMAP_LAYER_ID, EARTHQUAKE_SOURCE_ID)
            {
                MaxZoom = 9,
                SourceLayer = HEATMAP_LAYER_SOURCE,

                // Color ramp for heatmap.  Domain is 0 (low) to 1 (high).
                // Begin color ramp at 0-stop with a 0-transparency color
                // to create a blur-like effect.
                HeatmapColor = Expression.Interpolate(
                  Expression.Linear(), Expression.HeatmapDensity(),
                  Expression.Literal(0), Expression.Rgba(33, 102, 172, 0),
                  Expression.Literal(0.2), Expression.Rgb(103, 169, 207),
                  Expression.Literal(0.4), Expression.Rgb(209, 229, 240),
                  Expression.Literal(0.6), Expression.Rgb(253, 219, 199),
                  Expression.Literal(0.8), Expression.Rgb(239, 138, 98),
                  Expression.Literal(1), Expression.Rgb(178, 24, 43)
                ),

                // Increase the heatmap weight based on frequency and property magnitude
                HeatmapWeight =(
                Expression.Interpolate(
                  Expression.Linear(), Expression.Get("mag"),
                  Expression.CreateStop(0, 0),
                  Expression.CreateStop(6, 1)
                )
              ),

                // Increase the heatmap color weight weight by zoom level
                // heatmap-intensity is a multiplier on top of heatmap-weight
                HeatmapIntensity = (
                Expression.Interpolate(
                  Expression.Linear(), Expression.Zoom(),
                  Expression.CreateStop(0, 1),
                  Expression.CreateStop(9, 3)
                )
              ),

                // Adjust the heatmap radius by zoom level
                HeatmapRadius = (
                Expression.Interpolate(
                  Expression.Linear(), Expression.Zoom(),
                  Expression.CreateStop(0, 2),
                  Expression.CreateStop(9, 20)
                )
              ),

                // Transition from heatmap to circle layer by zoom level
                HeatmapOpacity = (
                Expression.Interpolate(
                  Expression.Linear(), Expression.Zoom(),
                  Expression.CreateStop(7, 1),
                  Expression.CreateStop(9, 0)
                )
              )
            };

            map.Functions.AddLayerAbove(layer, "waterway-label");
        }

        private void AddCircleLayer()
        {
            CircleLayer circleLayer = new CircleLayer(CIRCLE_LAYER_ID, EARTHQUAKE_SOURCE_ID) { 

              // Size circle radius by earthquake magnitude and zoom level
              CircleRadius = 
                Expression.Interpolate(
                  Expression.Linear(), Expression.Zoom(),
                  Expression.Literal(7), Expression.Interpolate(
                    Expression.Linear(), Expression.Get("mag"),
                    Expression.CreateStop(1, 1),
                    Expression.CreateStop(6, 4)
                  ),
                  Expression.Literal(16), Expression.Interpolate(
                    Expression.Linear(), Expression.Get("mag"),
                    Expression.CreateStop(1, 5),
                    Expression.CreateStop(6, 50)
                  )
                )
              ,

              // Color circle by earthquake magnitude
              CircleColor = (
                Expression.Interpolate(
                  Expression.Linear(), Expression.Get("mag"),
                  Expression.Literal(1), Expression.Rgba(33, 102, 172, 0),
                  Expression.Literal(2), Expression.Rgb(103, 169, 207),
                  Expression.Literal(3), Expression.Rgb(209, 229, 240),
                  Expression.Literal(4), Expression.Rgb(253, 219, 199),
                  Expression.Literal(5), Expression.Rgb(239, 138, 98),
                  Expression.Literal(6), Expression.Rgb(178, 24, 43)
                )
              ),

              // Transition from heatmap to circle layer by zoom level
              CircleOpacity = 
                Expression.Interpolate(
                  Expression.Linear(), Expression.Zoom(),
                  Expression.CreateStop(7, 0),
                  Expression.CreateStop(8, 1)
                )
              ,
              CircleStrokeColor = "white",
              CircleStrokeWidth = 1.0f
            };

            map.Functions.AddLayerBelow(circleLayer, HEATMAP_LAYER_ID);
        }
    }
}