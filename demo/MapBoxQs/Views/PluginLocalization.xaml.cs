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
    public partial class PluginLocalization : ContentPage
    {
        public PluginLocalization()
        {
            InitializeComponent();

            map.Center = new LatLng(35.830744, 136.711369);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 4.5257;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            // TODO This could be done without LocalizationPlugin
        }
    }
}