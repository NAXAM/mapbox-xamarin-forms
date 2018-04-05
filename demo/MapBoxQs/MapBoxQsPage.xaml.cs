using Xamarin.Forms;

namespace MapBoxQs
{
    public partial class MapBoxQsPage : ContentPage
    {

        public MapBoxQsPage()
        {
            var viewModel = new MainPageViewModel(Navigation);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}
