using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Point = Xamarin.Forms.Point;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesClickToAddImage : ContentPage
    {
        string ID_IMAGE_SOURCE = "source-id";
        string CIRCLE_SOURCE_ID = "circle-source-id";
        string ID_IMAGE_LAYER = "layer-id";

        public StylesClickToAddImage()
        {
            InitializeComponent();

            map.Center = new LatLng(25.7836, -80.11725);
            map.MapStyle = MapStyle.OUTDOORS;
            map.ZoomLevel = 5;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private List<Feature> _features = new List<Feature>();
        private List<LatLng> _latLngs = new List<LatLng>();
        private int imageCountIndex;

        async void HandleMapTapped((LatLng position, Point point) p)
        {
            if (_features.Count == 4)
            {
                _latLngs = new List<LatLng>();
                _features = new List<Feature>();
            }

            _latLngs.Add(p.position);
            _features.Add(new Feature(new GeoJSON.Net.Geometry.Point(new Position(p.position.Lat, p.position.Long))));

            var featureCollection = new FeatureCollection(_features);
            map.Functions?.UpdateSource(CIRCLE_SOURCE_ID, featureCollection);

            if (_features.Count == 4 && map.Functions != null)
            {
                var result = await Plugin.Permissions.CrossPermissions.Current.RequestPermissionsAsync(Permission.Photos);

                if (result[Permission.Photos] != PermissionStatus.Granted) return;
                
                var media = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();
                if (media != null)
                {
                    var quad = new LatLngQuad(_latLngs[0],_latLngs[1],_latLngs[2],_latLngs[3]);
                    map.Functions.AddSource(new MapboxImageSource(ID_IMAGE_SOURCE + imageCountIndex, quad, new StreamImageSource()
                    {
                        Stream = token => Task.FromResult(media.GetStream())
                    }));

                    // Create a raster layer and use the imageSource's ID as the layer's data// Add the layer to the map
                    map.Functions.AddLayer(new RasterLayer(
                        ID_IMAGE_LAYER + imageCountIndex,
                        ID_IMAGE_SOURCE + imageCountIndex));

                    _latLngs = new List<LatLng>();
                    _features = new List<Feature>();
                    featureCollection = new FeatureCollection(_features);
                    map.Functions.UpdateSource(CIRCLE_SOURCE_ID, featureCollection);

                    imageCountIndex++;
                }
            }
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.AddSource(
                new GeoJsonSource(CIRCLE_SOURCE_ID, new FeatureCollection())
            );

            map.Functions.AddLayer(new CircleLayer("circle-layer-bounds-corner-id", CIRCLE_SOURCE_ID)
            {
                CircleRadius = 8f,
                CircleColor = Color.FromHex("#d004d3")
            });

            map.DidTapOnMapCommand = new Command<(LatLng position, Point point)>(HandleMapTapped);
        }
    }
}