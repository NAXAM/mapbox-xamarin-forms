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
            Xamarin.Forms.Page page = null;
            switch (item.Title)
            {
                case "Default styles":
                    page = new StylesDefaultPage();
                    break;
                case "Symbol layer icons":
                    page = new StylesSymbolLayerIconsPage();
                    break;
                case "Symbol layer icon size change":
                    page = new StylesSymbolLayerIconSizeChangePage() ;
                    break;
                case "Create a line layer":
                    page = new StylesLineLayerPage();
                    break;
                case "Change a layer's color":
                    page = new StylesChangeLayerColorPage();
                    break;
                case "Add a vector tile source":
                    page = new StylesVectorLayerPage();
                    break;
                case "Add a WMS source":
                    page = new StylesWebMapServiceLayerPage();
                    break;
                case "Add a new layer below labels":
                    page = new StylesLayerBelowLabelsPage();
                    break;
                case "Adjust a layer's opacity":
                    page = new StylesChangeLayerOpacityPage();
                    break;
                case "Color dependent on zoom level":
                    page = new StylesColorDependentOnRoomLevelPage();
                    break;
                case "Change a map's language":
                    page = new StylesLanguageSwitchPage();
                    break;
                case "Show and hide layers":
                    page = new StylesShowHideLayersPage();
                    break;
                case "Mapbox Studio style":
                    page = new StylesMapboxStudio();
                    break;
                case "Local style or custom raster style":
                    page = new StylesLocalStyleSource();
                    break;
                case "Use an image source":
                    page = new StylesImageSource();
                    break;
                case "Show time lapse":
                    page = new StylesShowTimeLapse();
                    break;
                case "Hillshading":
                    page = new StylesHillshading();
                    break;
                case "Multiple text formats":
                    page = new StylesTextFieldMultipleFormats();
                    break;
                case "Transparent render surface":
                    page = new StylesTransparentRenderSurface();
                    break;
                case "Click to add photo":
                    page = new StylesClickToAddImage();
                    break;
                case "Text anchor position":
                    page = new StylesRotatingTextAnchorPosition();
                    break;
                case "Line behind moving icon":
                    page = new LabLineBehindMovingIconPage();
                    break;
                case "A simple offline map":
                    page = new OfflineSimpleOfflineMapPage();
                    break;
                case "Sideload offline map":
                    page = new OfflineSideloadOfflineMapPage();
                    break;
                default:
                    page = new MapBoxQsPage();
                    break;
            }

            if (page != null)
            {
                page.Title = item.Title;

                Navigation.PushAsync(page);
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
                : examples.Where(x => words.Any(w => x.Title.ToUpper().Contains(w.ToUpper())));
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