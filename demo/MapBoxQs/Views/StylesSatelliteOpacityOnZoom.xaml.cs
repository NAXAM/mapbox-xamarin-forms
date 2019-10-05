using System.Timers;
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
    public partial class StylesSatelliteOpacityOnZoom : ContentPage
    {
        public StylesSatelliteOpacityOnZoom()
        {
            InitializeComponent();

            map.Center = new LatLng(30.044, 31.235);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 10;
            map.ZoomMinLevel = 6;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.AddSource(new RasterSource(
                "SATELLITE_RASTER_SOURCE_ID",
                "mapbox://mapbox.satellite", 
                512));

            // Create a new map layer for the satellite raster images and add the satellite layer to the map.
            // Use runtime styling to adjust the satellite layer's opacity based on the map camera's zoom level
            map.Functions.AddLayer(
                new RasterLayer(
                    "SATELLITE_RASTER_LAYER_ID", 
                    "SATELLITE_RASTER_SOURCE_ID"){
                    RasterOpacity = Expression.Interpolate(
                        Expression.Linear(), 
                        Expression.Zoom(),
                        Expression.CreateStop(15, 0),
                        Expression.CreateStop(18, 1)
                    )
                });

            // Create a new camera position and animate the map camera to show the fade in/out UI of the satellite layer
            map.Functions.AnimateCamera(new CameraPosition(map.Center, 19, null, null), 9000);
        }
    }
}