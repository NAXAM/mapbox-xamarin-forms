using MapBoxQs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        ExampleItemModel[] examples;

        public HomePage()
        {
            InitializeComponent();

            On<iOS>().SetUseSafeArea(true);

            examples = CreateExamples();

            lstExamples.ItemsSource = examples;
            lstExamples.BindingContext = this;
        }

        private ExampleItemModel[] CreateExamples()
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream("MapBoxQs.examples.json"))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        var serializer = new JsonSerializer();

                        return serializer.Deserialize<ExampleItemModel[]>(jsonReader);
                    }
                }
            }
        }

        public ICommand ViewMapCommand
            => _ViewMapCommand = _ViewMapCommand
            ?? new Command<ExampleItemModel>(ExecuteViewMapCommand);
        ICommand _ViewMapCommand;
        void ExecuteViewMapCommand(ExampleItemModel item)
        {
            switch (item.Title)
            {
                case "Default styles":
                    Navigation.PushAsync(new StylesDefaultPage());
                    break;
                case "Symbol layer icons":
                    Navigation.PushAsync(new StylesSymbolLayerIconsPage());
                    break;
                default:
                    Navigation.PushAsync(new MapBoxQsPage());
                    break;
            }
        }

        public ICommand SearchCommand
            => _SearchCommand = _SearchCommand
            ?? new Command<string>(ExecuteSearchCommand);
        ICommand _SearchCommand;
        void ExecuteSearchCommand(string parameter)
        {
            var words = parameter.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            lstExamples.ItemsSource = words.Length == 0
                ? examples
                : examples.Where(x => words.Any(w => x.Title.Contains(w)));
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == string.Empty)
            {
                ExecuteSearchCommand(string.Empty);
            }
        }
    }
}