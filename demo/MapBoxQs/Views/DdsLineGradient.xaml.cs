using System.Collections.Generic;
using System.IO;
using System.Timers;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
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
    public partial class DdsLineGradient : ContentPage
    {
        private List<Position> routeCoordinates;
        public DdsLineGradient()
        {
            InitializeComponent();

            map.Center = new LatLng(38.875, -77.035);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 12;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            InitCoordinates();

            // Create the LineString from the list of coordinates and then make a GeoJSON
            // FeatureCollection so we can add the line to our map as a layer.
            var lineString = new LineString(routeCoordinates);

            var featureCollection = new FeatureCollection(new List<Feature> { new Feature(lineString) });

            map.Functions.AddSource(new GeoJsonSource("line-source", featureCollection)
            {
                Options = new GeoJsonOptions()
                {
                    LineMetrics = true
                }
            });

            // The layer properties for our line. This is where we set the gradient colors, set the
            // line width, etc
            map.Functions.AddLayer(new LineLayer("linelayer", "line-source")
            {
                LineCap = LayerProperty.LINE_CAP_ROUND,
                LineJoin = LayerProperty.LINE_JOIN_ROUND,
                LineWidth = 14f,
                LineGradient = Expression.Interpolate(
                  Expression.Linear(), Expression.LineProgress(),
                  Expression.CreateStop(0f, Expression.Rgb(6, 1, 255)), // blue
                  Expression.CreateStop(0.1f, Expression.Rgb(59, 118, 227)), // royal blue
                  Expression.CreateStop(0.3f, Expression.Rgb(7, 238, 251)), // cyan
                  Expression.CreateStop(0.5f, Expression.Rgb(0, 255, 42)), // lime
                  Expression.CreateStop(0.7f, Expression.Rgb(255, 252, 0)), // yellow
                  Expression.CreateStop(1f, Expression.Rgb(255, 30, 0)) // red
                )
            });
        }

        private void InitCoordinates()
        {
            routeCoordinates = new List<Position> {
                FromLngLat(-77.044211, 38.852924),
                FromLngLat(-77.045659, 38.860158),
                FromLngLat(-77.044232, 38.862326),
                FromLngLat(-77.040879, 38.865454),
                FromLngLat(-77.039936, 38.867698),
                FromLngLat(-77.040338, 38.86943),
                FromLngLat(-77.04264, 38.872528),
                FromLngLat(-77.03696, 38.878424),
                FromLngLat(-77.032309, 38.87937),
                FromLngLat(-77.030056, 38.880945),
                FromLngLat(-77.027645, 38.881779),
                FromLngLat(-77.026946, 38.882645),
                FromLngLat(-77.026942, 38.885502),
                FromLngLat(-77.028054, 38.887449),
                FromLngLat(-77.02806, 38.892088),
                FromLngLat(-77.03364, 38.892108),
                FromLngLat(-77.033643, 38.899926),
            };
        }

        Position FromLngLat(double lng, double lat)
        {
            return new Position(lat, lng);
        }
    }
}