using GeoJSON.Net.Feature;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesSymbolLayerIconSizeChangePage : ContentPage
    {
        public StylesSymbolLayerIconSizeChangePage()
        {
            InitializeComponent();

            map.MapStyle = MapStyle.DARK;
            map.Center = new LatLng(42.353517, -71.078625);
            map.ZoomLevel = 12;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyeLoaded);
            map.DidTapOnMapCommand = new Command<(LatLng, Point)>(HandleMapTapped);
        }

        bool markerSelected;
        private void HandleMapTapped((LatLng geoLocation, Point screenPosition) obj)
        {
            var features = map.Functions.QueryFeatures(obj.screenPosition, "markers.layer");
            var selectedFeatures = map.Functions.QueryFeatures(obj.screenPosition, "selected-markers.layer");

            if (selectedFeatures.Length > 0 && markerSelected) return;

            if (features.Length == 0)
            {
                if (markerSelected)
                {
                    DeselectMarker();
                }

                return;
            }

            var featureCollection = new FeatureCollection(new List<Feature>(features));
            map.Functions.UpdateSource("selected-markers.src", featureCollection);

            if (markerSelected)
            {
                DeselectMarker();
            }

            if (features.Length > 0)
            {
                SelectMarker();
            }
        }

        void DeselectMarker()
        {
            var animation = new Animation(
                callback,
                2, 1
                );

            animation.Commit(this, nameof(DeselectMarker), length: 300);

            void callback(double animatedValue)
            {
                map.Functions.UpdateLayer(new SymbolLayer("selected-markers.layer", "selected-markers.src")
                {
                    IconSize = Expression.Literal(animatedValue)
                });
            }
            markerSelected = false;
        }

        void SelectMarker()
        {
            var animation = new Animation(
                callback,
                1, 2
                );

            animation.Commit(this, nameof(SelectMarker), length: 300);

            void callback(double animatedValue)
            {
                map.Functions.UpdateLayer(new SymbolLayer("selected-markers.layer", "selected-markers.src")
                {
                    IconSize = Expression.Literal(animatedValue)
                });
            }
            markerSelected = true;
        }

        private void HandleStyeLoaded(MapStyle obj)
        {
            var markerCoordinates = new List<Feature> {
                new Feature(new GeoJSON.Net.Geometry.Point(
                    new GeoJSON.Net.Geometry.Position(42.354950, -71.065634))),// Boston Common Park
                new Feature(new GeoJSON.Net.Geometry.Point(
                    new GeoJSON.Net.Geometry.Position(42.346645, -71.097293))),// Fenway Park
                new Feature(new GeoJSON.Net.Geometry.Point(
                    new GeoJSON.Net.Geometry.Position(42.363725, -71.053694))),// The Paul Revere House
            };

            var source = new GeoJsonSource
            {
                Id = "markers.src",
                Data = new FeatureCollection(markerCoordinates)
            };
            map.Functions.AddSource(source);

            IconImageSource icon = (ImageSource)"blue_marker_view.png";
            map.Functions.AddStyleImage(icon);

            // Adding an offset so that the bottom of the blue icon gets fixed to the coordinate, rather than the
            // middle of the icon being fixed to the coordinate point.
            var symbolLayer = new SymbolLayer("markers.layer", source.Id)
            {
                IconImage = Expression.Literal(icon.Id),
                IconAllowOverlap = Expression.Literal(true),
                IconOffset = Expression.Literal(new[] { 0, -9 })
            };
            map.Functions.AddLayer(symbolLayer);

            // Add the selected marker source and layer
            var selectedMarkersSrc = new GeoJsonSource("selected-markers.src");
            map.Functions.AddSource(selectedMarkersSrc);

            // Adding an offset so that the bottom of the blue icon gets fixed to the coordinate, rather than the
            // middle of the icon being fixed to the coordinate point.
            var selectedMarkersLayer = new SymbolLayer("selected-markers.layer", selectedMarkersSrc.Id)
            {
                IconImage = Expression.Literal(icon.Id),
                IconAllowOverlap = Expression.Literal(true),
                IconOffset = Expression.Literal(new[] { 0, -9 })
            };
            map.Functions.AddLayer(selectedMarkersLayer);
        }
    }
}