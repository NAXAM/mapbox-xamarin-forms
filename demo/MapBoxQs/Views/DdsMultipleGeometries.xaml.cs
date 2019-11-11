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
    public partial class DdsMultipleGeometries : ContentPage
    {
        private const string GEOJSON_SOURCE_ID = "GEOJSONFILE";
        public DdsMultipleGeometries()
        {
            InitializeComponent();

            map.Center = new LatLng(65.509486, 16.951005);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 3.296733;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            CreateGeoJsonSource();
            AddPolygonLayer();
            AddPointsLayer();
        }
        private void CreateGeoJsonSource()
        {
            // Load data from GeoJSON file in the assets folder
            map.Functions.AddSource(new GeoJsonSource(GEOJSON_SOURCE_ID,
                "asset://fake_norway_campsites.geojson")
            {
                IsLocal = true
            });
        }

        private void AddPolygonLayer()
        {
            // Create and style a FillLayer that uses the Polygon Feature's coordinates in the GeoJSON data
            FillLayer countryPolygonFillLayer = new FillLayer("polygon", GEOJSON_SOURCE_ID)
            {
                FillColor = Color.Red,
                FillOpacity = .4f,
                Filter = Expression.Eq(Expression.Literal("$type"), Expression.Literal("Polygon")),
            };
            map.Functions.AddLayer(countryPolygonFillLayer);
        }

        private void AddPointsLayer()
        {
            // Create and style a CircleLayer that uses the Point Features' coordinates in the GeoJSON data
            CircleLayer individualCirclesLayer = new CircleLayer("points", GEOJSON_SOURCE_ID)
            {
                CircleColor = Color.Yellow,
                CircleRadius = 3f,
                Filter = Expression.Eq(Expression.Literal("$type"), Expression.Literal("Point"))
            };
            map.Functions.AddLayer(individualCirclesLayer);
        }
    }
}