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
    public partial class IconRotateAndAlignment : ContentPage
    {
        private const string GEOJSON_FILE = "MapBoxQs.geojson.icon_rotate_and_alignment.json";

        private const string GEOJSON_SOURCE_ID = "GEOJSON_SOURCE_ID";
        private const string MARKER_IMAGE_ID = "MARKER_IMAGE_ID";
        private const string MARKER_LAYER_ID = "MARKER_LAYER_ID";

        private FeatureCollection featureCollection;
        private GeoJsonSource source;

        public IconRotateAndAlignment()
        {
            InitializeComponent();

            map.Center = new LatLng(10.309840, 123.893147);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 13.5;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream(GEOJSON_FILE)) {
                using (var textReader = new StreamReader(stream)) {
                    using (var jsonReader = new JsonTextReader(textReader)) {
                        var serializer = new JsonSerializer();
                        var data = serializer.Deserialize<FeatureCollection>(jsonReader);

                        SetUpData(data);
                    }
                }
            }
        }

        /**
         * Sets up all of the sources and layers needed for this example
         *
         * @param collection the FeatureCollection to set equal to the globally-declared FeatureCollection
         */
        public void SetUpData(FeatureCollection collection)
        {
            featureCollection = collection;

            SetupSource();
            SetUpImage();
            SetUpMarkerLayer();
        }

        /**
         * Adds the GeoJSON source to the map
         */
        private void SetupSource()
        {
            source = new GeoJsonSource(GEOJSON_SOURCE_ID, featureCollection);
            map.Functions.AddSource(source);
        }

        /**
         * Adds the marker image to the map for use as a SymbolLayer icon
         */
        private void SetUpImage()
        {
            map.Functions.AddStyleImage(new IconImageSource
            {
                Id = MARKER_IMAGE_ID,
                Source = "red_marker.png"
            });
        }

        /**
         * Setup a layer with maki icons, eg. west coast city.
         */
        private void SetUpMarkerLayer()
        {
            map.Functions.AddLayer(new SymbolLayer(MARKER_LAYER_ID, GEOJSON_SOURCE_ID)
            {
                IconImage = MARKER_IMAGE_ID,
                IconAllowOverlap = true,
                IconRotate = Expression.Get("icon_rotate"),
                IconRotationAlignment = Expression.Literal("map")
            });
        }
    }
}
