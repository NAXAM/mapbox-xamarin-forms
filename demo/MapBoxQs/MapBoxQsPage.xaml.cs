using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace MapBoxQs
{
    public partial class MapBoxQsPage : ContentPage
    {
        MainPageViewModel viewModel = new MainPageViewModel();

        public MapBoxQsPage()
        {
            On<iOS>().SetUseSafeArea(true);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}
