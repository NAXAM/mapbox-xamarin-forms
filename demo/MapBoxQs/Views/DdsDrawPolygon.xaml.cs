using System.Collections.Generic;
using System.Linq;
using System.Timers;
using GeoJSON.Net.Geometry;
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
    public partial class DdsDrawPolygon : ContentPage
    {
        public DdsDrawPolygon()
        {
            InitializeComponent();

            map.Center = new LatLng(45.520486, -122.673541);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 11;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var polygon = new Polygon(
                POINTS.Select(x => x.Select(y => new[] { y.Longitude, y.Latitude }))
                );

            map.Functions.AddSource(new GeoJsonSource("source-id", polygon));
            map.Functions.AddLayerBelow(new FillLayer("layer-id", "source-id")
            {
                FillColor = Color.FromHex("#3bb2d0")
            }, "settlement-label");
        }

        private static readonly List<List<Position>> POINTS = new List<List<Position>>();
        private static readonly List<Position> OUTER_POINTS = new List<Position>();

        static DdsDrawPolygon()
        {
            OUTER_POINTS.Add(FromLngLat(-122.685699, 45.522585));
            OUTER_POINTS.Add(FromLngLat(-122.708873, 45.534611));
            OUTER_POINTS.Add(FromLngLat(-122.678833, 45.530883));
            OUTER_POINTS.Add(FromLngLat(-122.667503, 45.547115));
            OUTER_POINTS.Add(FromLngLat(-122.660121, 45.530643));
            OUTER_POINTS.Add(FromLngLat(-122.636260, 45.533529));
            OUTER_POINTS.Add(FromLngLat(-122.659091, 45.521743));
            OUTER_POINTS.Add(FromLngLat(-122.648792, 45.510677));
            OUTER_POINTS.Add(FromLngLat(-122.664070, 45.515008));
            OUTER_POINTS.Add(FromLngLat(-122.669048, 45.502496));
            OUTER_POINTS.Add(FromLngLat(-122.678489, 45.515369));
            OUTER_POINTS.Add(FromLngLat(-122.702007, 45.506346));
            OUTER_POINTS.Add(FromLngLat(-122.685699, 45.522585));
            POINTS.Add(OUTER_POINTS);
        }

        static Position FromLngLat(double lng, double lat)
        {
            return new Position(lat, lng);
        }
    }
}