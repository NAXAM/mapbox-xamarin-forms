using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace MapBoxQs
{
    public partial class MapBoxQsPage : ContentPage
    {

        public MapBoxQsPage()
        {
            var viewModel = new MainPageViewModel(Navigation);
            On<iOS>().SetUseSafeArea(true);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}
