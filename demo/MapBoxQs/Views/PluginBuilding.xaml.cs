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
    public partial class PluginBuilding : ContentPage
    {
        public PluginBuilding()
        {
            InitializeComponent();

            map.Center = new LatLng(41.87827, -87.62877);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 16;
            map.Pitch = 60;
            map.RotatedDegree = 300;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.ShowBuilding(new BuildingInfo() {
                MinZoomLevel = 15,
                IsVisible = true
            });

            map.Functions.AnimateCamera(new CameraPosition(map.Center, map.ZoomLevel, map.Pitch, map.RotatedDegree), 1000);
        }
    }
}