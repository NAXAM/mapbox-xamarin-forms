using System;
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
    public partial class DdsMultipleHeatmapStyling : ContentPage
    {
        private static readonly string HEATMAP_SOURCE_ID = "HEATMAP_SOURCE_ID";
        private static readonly string HEATMAP_LAYER_ID = "HEATMAP_LAYER_ID";

        private Expression[] listOfHeatmapColors;
        private Expression[] listOfHeatmapRadiusStops;
        private float[] listOfHeatmapIntensityStops;

        private int index;

        public DdsMultipleHeatmapStyling()
        {
            InitializeComponent();

            map.Center = new LatLng(34.056684, -118.254002);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 11.047;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.AddSource(new GeoJsonSource(HEATMAP_SOURCE_ID, "asset://la_heatmap_styling_points.geojson")
            {
                IsLocal = true
            });

            InitHeatmapColors();
            InitHeatmapRadiusStops();
            InitHeatmapIntensityStops();
            AddHeatmapLayer();
        }

        private void HandleTap(object sender, EventArgs eventArgs)
        {
            index++;

            if (index == listOfHeatmapColors.Length) {
                index = 0;
            }

            var layer = new HeatmapLayer(HEATMAP_LAYER_ID, HEATMAP_SOURCE_ID)
            {
                // Color ramp for heatmap.  Domain is 0 (low) to 1 (high).
                // Begin color ramp at 0-stop with a 0-transparency color to create a blur-like effect.
                HeatmapColor = listOfHeatmapColors[index],

                // Increase the heatmap color weight weight by zoom level
                // heatmap-intensity is a multiplier on top of heatmap-weight
                HeatmapIntensity = listOfHeatmapIntensityStops[index],

                // Adjust the heatmap radius by zoom level
                HeatmapRadius = listOfHeatmapRadiusStops[index]
            };

            map.Functions.UpdateLayer(layer);
        }

        private void AddHeatmapLayer()
        {
            // Create the heatmap layer
            var layer = new HeatmapLayer(HEATMAP_LAYER_ID, HEATMAP_SOURCE_ID)
            {
                // Heatmap layer disappears at whatever zoom level is set as the maximum
                MaxZoom = 18,

                // Color ramp for heatmap.  Domain is 0 (low) to 1 (high).
                // Begin color ramp at 0-stop with a 0-transparency color to create a blur-like effect.
                HeatmapColor = listOfHeatmapColors[index],

                // Increase the heatmap color weight weight by zoom level
                // heatmap-intensity is a multiplier on top of heatmap-weight
                HeatmapIntensity = listOfHeatmapIntensityStops[index],

                // Adjust the heatmap radius by zoom level
                HeatmapRadius = listOfHeatmapRadiusStops[index],

                HeatmapOpacity = 1f
            };

            // Add the heatmap layer to the map and above the "water-label" layer
            map.Functions.AddLayerAbove(layer, "waterway-label");
        }

        private void InitHeatmapColors()
        {
            listOfHeatmapColors = new Expression[] {
              // 0
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(0, 0, 0, 0.01),
                Expression.Literal(0.25), Expression.Rgba(224, 176, 63, 0.5),
                Expression.Literal(0.5), Expression.Rgb(247, 252, 84),
                Expression.Literal(0.75), Expression.Rgb(186, 59, 30),
                Expression.Literal(0.9), Expression.Rgb(255, 0, 0)
              ),
              // 1
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(255, 255, 255, 0.4),
                Expression.Literal(0.25), Expression.Rgba(4, 179, 183, 1.0),
                Expression.Literal(0.5), Expression.Rgba(204, 211, 61, 1.0),
                Expression.Literal(0.75), Expression.Rgba(252, 167, 55, 1.0),
                Expression.Literal(1), Expression.Rgba(255, 78, 70, 1.0)
              ),
              // 2
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(12, 182, 253, 0.0),
                Expression.Literal(0.25), Expression.Rgba(87, 17, 229, 0.5),
                Expression.Literal(0.5), Expression.Rgba(255, 0, 0, 1.0),
                Expression.Literal(0.75), Expression.Rgba(229, 134, 15, 0.5),
                Expression.Literal(1), Expression.Rgba(230, 255, 55, 0.6)
              ),
              // 3
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(135, 255, 135, 0.2),
                Expression.Literal(0.5), Expression.Rgba(255, 99, 0, 0.5),
                Expression.Literal(1), Expression.Rgba(47, 21, 197, 0.2)
              ),
              // 4
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(4, 0, 0, 0.2),
                Expression.Literal(0.25), Expression.Rgba(229, 12, 1, 1.0),
                Expression.Literal(0.30), Expression.Rgba(244, 114, 1, 1.0),
                Expression.Literal(0.40), Expression.Rgba(255, 205, 12, 1.0),
                Expression.Literal(0.50), Expression.Rgba(255, 229, 121, 1.0),
                Expression.Literal(1), Expression.Rgba(255, 253, 244, 1.0)
              ),
              // 5
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(0, 0, 0, 0.01),
                Expression.Literal(0.05), Expression.Rgba(0, 0, 0, 0.05),
                Expression.Literal(0.4), Expression.Rgba(254, 142, 2, 0.7),
                Expression.Literal(0.5), Expression.Rgba(255, 165, 5, 0.8),
                Expression.Literal(0.8), Expression.Rgba(255, 187, 4, 0.9),
                Expression.Literal(0.95), Expression.Rgba(255, 228, 173, 0.8),
                Expression.Literal(1), Expression.Rgba(255, 253, 244, .8)
              ),
              //6
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(0, 0, 0, 0.01),
                Expression.Literal(0.3), Expression.Rgba(82, 72, 151, 0.4),
                Expression.Literal(0.4), Expression.Rgba(138, 202, 160, 1.0),
                Expression.Literal(0.5), Expression.Rgba(246, 139, 76, 0.9),
                Expression.Literal(0.9), Expression.Rgba(252, 246, 182, 0.8),
                Expression.Literal(1), Expression.Rgba(255, 255, 255, 0.8)
              ),

              //7
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(0, 0, 0, 0.01),
                Expression.Literal(0.1), Expression.Rgba(0, 2, 114, .1),
                Expression.Literal(0.2), Expression.Rgba(0, 6, 219, .15),
                Expression.Literal(0.3), Expression.Rgba(0, 74, 255, .2),
                Expression.Literal(0.4), Expression.Rgba(0, 202, 255, .25),
                Expression.Literal(0.5), Expression.Rgba(73, 255, 154, .3),
                Expression.Literal(0.6), Expression.Rgba(171, 255, 59, .35),
                Expression.Literal(0.7), Expression.Rgba(255, 197, 3, .4),
                Expression.Literal(0.8), Expression.Rgba(255, 82, 1, 0.7),
                Expression.Literal(0.9), Expression.Rgba(196, 0, 1, 0.8),
                Expression.Literal(0.95), Expression.Rgba(121, 0, 0, 0.8)
              ),
              // 8
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(0, 0, 0, 0.01),
                Expression.Literal(0.1), Expression.Rgba(0, 2, 114, .1),
                Expression.Literal(0.2), Expression.Rgba(0, 6, 219, .15),
                Expression.Literal(0.3), Expression.Rgba(0, 74, 255, .2),
                Expression.Literal(0.4), Expression.Rgba(0, 202, 255, .25),
                Expression.Literal(0.5), Expression.Rgba(73, 255, 154, .3),
                Expression.Literal(0.6), Expression.Rgba(171, 255, 59, .35),
                Expression.Literal(0.7), Expression.Rgba(255, 197, 3, .4),
                Expression.Literal(0.8), Expression.Rgba(255, 82, 1, 0.7),
                Expression.Literal(0.9), Expression.Rgba(196, 0, 1, 0.8),
                Expression.Literal(0.95), Expression.Rgba(121, 0, 0, 0.8)
              ),
              // 9
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(0, 0, 0, 0.01),
                Expression.Literal(0.1), Expression.Rgba(0, 2, 114, .1),
                Expression.Literal(0.2), Expression.Rgba(0, 6, 219, .15),
                Expression.Literal(0.3), Expression.Rgba(0, 74, 255, .2),
                Expression.Literal(0.4), Expression.Rgba(0, 202, 255, .25),
                Expression.Literal(0.5), Expression.Rgba(73, 255, 154, .3),
                Expression.Literal(0.6), Expression.Rgba(171, 255, 59, .35),
                Expression.Literal(0.7), Expression.Rgba(255, 197, 3, .4),
                Expression.Literal(0.8), Expression.Rgba(255, 82, 1, 0.7),
                Expression.Literal(0.9), Expression.Rgba(196, 0, 1, 0.8),
                Expression.Literal(0.95), Expression.Rgba(121, 0, 0, 0.8)
              ),
              // 10
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(0, 0, 0, 0.01),
                Expression.Literal(0.1), Expression.Rgba(0, 2, 114, .1),
                Expression.Literal(0.2), Expression.Rgba(0, 6, 219, .15),
                Expression.Literal(0.3), Expression.Rgba(0, 74, 255, .2),
                Expression.Literal(0.4), Expression.Rgba(0, 202, 255, .25),
                Expression.Literal(0.5), Expression.Rgba(73, 255, 154, .3),
                Expression.Literal(0.6), Expression.Rgba(171, 255, 59, .35),
                Expression.Literal(0.7), Expression.Rgba(255, 197, 3, .4),
                Expression.Literal(0.8), Expression.Rgba(255, 82, 1, 0.7),
                Expression.Literal(0.9), Expression.Rgba(196, 0, 1, 0.8),
                Expression.Literal(0.95), Expression.Rgba(121, 0, 0, 0.8)
              ),
              // 11
              Expression.Interpolate(
                Expression.Linear(), Expression.HeatmapDensity(),
                Expression.Literal(0.01), Expression.Rgba(0, 0, 0, 0.25),
                Expression.Literal(0.25), Expression.Rgba(229, 12, 1, .7),
                Expression.Literal(0.30), Expression.Rgba(244, 114, 1, .7),
                Expression.Literal(0.40), Expression.Rgba(255, 205, 12, .7),
                Expression.Literal(0.50), Expression.Rgba(255, 229, 121, .8),
                Expression.Literal(1), Expression.Rgba(255, 253, 244, .8)
              )
            };
        }

        private void InitHeatmapRadiusStops()
        {
            listOfHeatmapRadiusStops = new Expression[] {
              // 0
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(6), Expression.Literal(50),
                Expression.Literal(20), Expression.Literal(100)
              ),
              // 1
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(12), Expression.Literal(70),
                Expression.Literal(20), Expression.Literal(100)
              ),
              // 2
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(1), Expression.Literal(7),
                Expression.Literal(5), Expression.Literal(50)
              ),
              // 3
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(1), Expression.Literal(7),
                Expression.Literal(5), Expression.Literal(50)
              ),
              // 4
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(1), Expression.Literal(7),
                Expression.Literal(5), Expression.Literal(50)
              ),
              // 5
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(1), Expression.Literal(7),
                Expression.Literal(15), Expression.Literal(200)
              ),
              // 6
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(1), Expression.Literal(10),
                Expression.Literal(8), Expression.Literal(70)
              ),
              // 7
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(1), Expression.Literal(10),
                Expression.Literal(8), Expression.Literal(200)
              ),
              // 8
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(1), Expression.Literal(10),
                Expression.Literal(8), Expression.Literal(200)
              ),
              // 9
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(1), Expression.Literal(10),
                Expression.Literal(8), Expression.Literal(200)
              ),
              // 10
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(1), Expression.Literal(10),
                Expression.Literal(8), Expression.Literal(200)
              ),
              // 11
              Expression.Interpolate(
                Expression.Linear(), Expression.Zoom(),
                Expression.Literal(1), Expression.Literal(10),
                Expression.Literal(8), Expression.Literal(200)
              ),
            };
        }

        private void InitHeatmapIntensityStops()
        {
            listOfHeatmapIntensityStops = new float[] {
                // 0
                0.6f,
                // 1
                0.3f,
                // 2
                1f,
                // 3
                1f,
                // 4
                1f,
                // 5
                1f,
                // 6
                1.5f,
                // 7
                0.8f,
                // 8
                0.25f,
                // 9
                0.8f,
                // 10
                0.25f,
                // 11
                0.5f
            };
        }
    }
}