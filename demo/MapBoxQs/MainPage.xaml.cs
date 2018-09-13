using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Prism.Navigation;

namespace MapBoxQs
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }

    public class MainPageViewModel
    {
        readonly INavigationService navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        ICommand _StartCommand;

        public ICommand StartCommand
        {
            get { return (_StartCommand = _StartCommand ?? new Command<Type>(ExecuteStartCommand)); }
        }
        void ExecuteStartCommand(Type parameter)
        {
            navigationService.NavigateAsync("MapBoxQsPage")
                             .ContinueWith(x =>
                             {
                                 System.Diagnostics.Debug.WriteLine("Navigated");
                             });
        }
    }
}
