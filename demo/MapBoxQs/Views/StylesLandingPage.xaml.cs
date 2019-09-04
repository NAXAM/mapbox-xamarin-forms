using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Xamarin.Forms;

namespace MapBoxQs.Views
{
    public partial class StylesLandingPage : ContentPage
    {
        public StylesLandingPage()
        {
            InitializeComponent();

            BindingContext = this;

            Title = "Styles";
            Center = new LatLng(21.028511, 105.804817);
        }

        public LatLng Center { get; set; }
    }
}
