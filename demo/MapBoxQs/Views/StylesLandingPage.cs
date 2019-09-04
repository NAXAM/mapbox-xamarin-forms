using Naxam.Controls.Forms;
using Xamarin.Forms;

namespace MapBoxQs.Views
{
    public class StylesLandingPage : ContentPage
    {
        public StylesLandingPage()
        {
            Title = "Get Started";

            var mapView = new MapView()
            {
                Center = new Position(21.028511, 105.804817),
                ZoomLevel = 15
            };

            Content = mapView;
        }
    }
}

