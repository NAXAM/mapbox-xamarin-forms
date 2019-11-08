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
    public partial class PluginPlaces : ContentPage
    {
        public PluginPlaces()
        {
            InitializeComponent();

            map.Center = new LatLng(37.7912561, -122.3964485);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 0.346515;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            // TODO Need to work special way
        }
    }
}