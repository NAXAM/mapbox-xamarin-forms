using Xamarin.Forms;

namespace MapBoxQs
{
    public partial class MapBoxQsPage : ContentPage
    {
        MainPageViewModel viewModel = new MainPageViewModel();

        public MapBoxQsPage()
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}
