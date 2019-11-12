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
    public partial class DdsInfoWindowSymbol : ContentPage
    {
        private const string GEOJSON_FILE = "MapBoxQs.geojson.us_west_coast.geojson";

        private const string GEOJSON_SOURCE_ID = "GEOJSON_SOURCE_ID";
        private const string MARKER_IMAGE_ID = "MARKER_IMAGE_ID";
        private const string MARKER_LAYER_ID = "MARKER_LAYER_ID";
        private const string CALLOUT_LAYER_ID = "CALLOUT_LAYER_ID";
        private const string PROPERTY_SELECTED = "selected";
        private const string PROPERTY_NAME = "name";
        private const string PROPERTY_CAPITAL = "capital";


        private FeatureCollection featureCollection;
        private GeoJsonSource source;

        public DdsInfoWindowSymbol()
        {
            InitializeComponent();

            map.Center = new LatLng(42.149683, -119.155770);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 3.853171;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);

            map.DidTapOnMapCommand = new Command<(LatLng, Point)>(HandleTap);
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
            SetUpInfoWindowLayer();
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
                Source = "red_marker.png",
            });
        }

        /**
         * Updates the display of data on the map after the FeatureCollection has been modified
         */
        private void RefreshSource()
        {
            map.Functions.UpdateSource(source.Id, featureCollection);
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
                IconOffset = (new float[] { 0f, -8f })
            });
        }

        /**
         * Setup a layer with Android SDK call-outs
         * <p>
         * name of the feature is used as key for the iconImage
         * </p>
         */
        private void SetUpInfoWindowLayer()
        {
            map.Functions.AddLayer(new SymbolLayer(CALLOUT_LAYER_ID, GEOJSON_SOURCE_ID)
            {
                /* show image with id title based on the value of the name feature property */
                IconImage = Expression.Get("name"),

                /* set anchor of icon to bottom-left */
                IconAnchor = LayerProperty.ICON_ANCHOR_BOTTOM,

                /* all info window and marker image to appear at the same time*/
                IconAllowOverlap = true,

                /* offset the info window to be above the marker */
                IconOffset = (new float[] { -2f, -28f }),

                /* add a filter to show only when selected feature property is true */
                Filter = Expression.Eq(Expression.Get(PROPERTY_SELECTED), Expression.Literal(true)),
            });
        }

        private void HandleTap((LatLng, Point) obj)
        {
            var features = map.Functions.QueryFeatures(obj.Item1, MARKER_LAYER_ID);
            if (features.Length > 0) {
                var name = features[0].Properties[PROPERTY_NAME];
                var featureList = featureCollection.Features;
                if (featureList != null) {
                    for (int i = 0; i < featureList.Count; i++) {
                        if (featureList[i].Properties[PROPERTY_NAME] == name) {
                            if (FeatureSelectStatus(i)) {
                                SetFeatureSelectState(featureList[i], false);
                            }
                            else {
                                SetSelected(i);
                            }
                        }
                    }
                }
            }
        }

        /**
         * Set a feature selected state.
         *
         * @param index the index of selected feature
         */
        private void SetSelected(int index)
        {
            if (featureCollection.Features != null) {
                Feature feature = featureCollection.Features[index];
                SetFeatureSelectState(feature, true);
                RefreshSource();
            }
        }

        /**
         * Selects the state of a feature
         *
         * @param feature the feature to be selected.
         */
        private void SetFeatureSelectState(Feature feature, bool selectedState)
        {
            if (feature.Properties != null) {
                feature.Properties[PROPERTY_SELECTED] = selectedState;
                RefreshSource();
            }
        }

        /**
         * Checks whether a Feature's boolean "selected" property is true or false
         *
         * @param index the specific Feature's index position in the FeatureCollection's list of Features.
         * @return true if "selected" is true. False if the boolean property is false.
         */
        private bool FeatureSelectStatus(int index)
        {
            if (featureCollection == null) {
                return false;
            }
            return featureCollection.Features[index].Properties[PROPERTY_SELECTED] is bool b && b;
        }

        ///**
        // * Invoked when the bitmaps have been generated from a view.
        // */
        //public void setImageGenResults(HashMap<String, Bitmap> imageMap)
        //{
        // TODO Generate image from XF View
        //    if (mapboxMap != null) {
        //        mapboxMap.getStyle(style-> {
        //            // calling addImages is faster as separate addImage calls for each bitmap.
        //            style.addImages(imageMap);
        //        });
        //    }
        //}

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

    }
}