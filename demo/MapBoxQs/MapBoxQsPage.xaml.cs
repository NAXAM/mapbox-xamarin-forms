using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Naxam.Controls.Forms;

namespace MapBoxQs
{
    public partial class MapBoxQsPage : ContentPage
    {
        void Handle_Tapped(object sender, System.EventArgs e)
        {
            var annotation = (sender as View).BindingContext as Annotation;

            DisplayAlert(annotation?.Title, annotation.SubTitle, "OK");
        }

        public MapBoxQsPage()
        {
            var viewModel = new MainPageViewModel(Navigation);
            On<iOS>().SetUseSafeArea(true);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}
