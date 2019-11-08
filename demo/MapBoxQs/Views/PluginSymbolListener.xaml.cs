using System;
using System.Linq;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Annotations;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PluginSymbolListener : ContentPage
    {
        const string MAKI_ICON_CAFE = "cafe-15";
        const string MAKI_ICON_HARBOR = "harbor-15";
        const string MAKI_ICON_AIRPORT = "airport-15";
            
        public PluginSymbolListener()
        {
            InitializeComponent();

            map.Center = new LatLng(40.7135, -74.0066);
            map.MapStyle = MapStyle.DARK;
            //map.ZoomLevel = 16;
            //map.Pitch = 45;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
            map.DidTapOnMarkerCommand = new Command<string>(HandleMarkerClicked);
        }

        private void HandleMarkerClicked(string id)
        {
            var symbol = map.Annotations.FirstOrDefault(x => x.Id == id) as SymbolAnnotation;

            if (symbol != null)
            {
                symbol.IconImage = MAKI_ICON_CAFE;
                map.Functions.UpdateAnnotation(symbol);
            }
        }


        private void HandleStyleLoaded(MapStyle obj)
        {
            var symbol = new SymbolAnnotation {
                Coordinates = new LatLng(60.169091, 24.939876),
                IconImage = MAKI_ICON_HARBOR,
                IconSize = 2.0f,
                IsDraggable = true,
                Id = Guid.NewGuid().ToString()
            };

            map.Annotations = new[] {symbol};
        }

    }
}