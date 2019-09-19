using Acr.UserDialogs;
using MapBoxQs.Services;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using System.Linq;
using Xamarin.Forms;

namespace MapBoxQs.Views
{
    public partial class StylesDefaultPage : ContentPage
    {
        public StylesDefaultPage()
        {
            InitializeComponent();

            Title = "Default Styles";

            map.Center = new LatLng(21.028511, 105.804817);
            map.MapStyle = MapStyle.LIGHT;
        }

        async void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            var buttons = MapBoxService.DefaultStyles.Select((arg) => arg.Name).ToArray();
            var choice = await UserDialogs.Instance.ActionSheetAsync("Change style", "Cancel", "Reload current style", buttons: buttons);
           
            if (buttons.Contains(choice))
            {
                map.MapStyle = MapBoxService.DefaultStyles.FirstOrDefault((arg) => arg.Name == choice);
            }
        }
    }
}
