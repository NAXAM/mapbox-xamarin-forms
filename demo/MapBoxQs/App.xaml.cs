using Xamarin.Forms;
using Prism.Autofac;
using Prism.Ioc;

[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
namespace MapBoxQs
{
    public partial class App : PrismApplication
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnInitialized()
        {
            NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MapBoxQsPage, MapboxQsPageViewModel>();
        }


        //protected override void OnStart()
        //{
        //    // Handle when your app starts
        //}

        //protected override void OnSleep()
        //{
        //    // Handle when your app sleeps
        //}

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
