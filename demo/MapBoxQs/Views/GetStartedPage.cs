using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Xamarin.Forms;

namespace MapBoxQs.Views
{
    public class GetStartedPage : ContentPage
    {
        public GetStartedPage()
        {
            Title = "Get Started";

            var mapView = new MapView
            {
                Center = new LatLng(21.028511, 105.804817),
                ZoomLevel = 15
            };

            Content = mapView;
        }
    }
}

