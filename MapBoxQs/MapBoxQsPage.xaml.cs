using System;
using Naxam.Controls.Mapbox.Forms;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace MapBoxQs
{
    public partial class MapBoxQsPage : ContentPage
    {

        public MapBoxQs.Services.IMapBoxService _MBService = new MapBoxQs.Services.MapBoxService();
        MainPageViewModel _ViewModel = new MainPageViewModel();
        public MapBoxQsPage()
        {
            InitializeComponent();

            var positions = new[] { 
                new Position {
					Lat = 21.0333,
					Long = 105.8500
                },
				new Position {
					Lat = 31.0333,
					Long = 105.8500
				}
            };

            var random = new Random();

            btnChangeLocation.Clicked += delegate {
                map.Center = positions[random.Next(2)%2];
            };


            map.DidTapOnMapCommand = new Command<Tuple<Position, Point>>((Tuple<Position, Point> obj) =>
            {
                var features = map.GetFeaturesAroundPoint.Invoke(obj.Item2, 6, null);
                var filtered = features.Where((arg) => arg.Attributes != null);
                foreach (IFeature feat in filtered) {
                    var str = JsonConvert.SerializeObject(feat);
        			System.Diagnostics.Debug.WriteLine(str);
                }

            });
            map.DidFinishLoadingStyleCommand = new Command<MapStyle>((obj) =>
            {
                map.ResetPositionFunc.Execute(null);
                foreach (Layer layer in obj.OriginalLayers)
				{
                    System.Diagnostics.Debug.WriteLine(layer.Id);
                    var styleLayer = map.GetStyleLayerFunc(layer.Id, false);
                    if (styleLayer != null) {
                        System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(styleLayer));
                    }
				}

			});
			map.ZoomLevel = Device.RuntimePlatform == Device.Android ? 4 : 10;


			BindingContext = _ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetAllStyles();
        }

        async void ShowStylesPicker(object sender, EventArgs args)
        {
            if (_Styles == null || _Styles.Length == 0) {
                await DisplayAlert("Error", "No style available", "Dismiss");
                return;
            }
            var buttons = _Styles.Select((arg) => arg.Name).ToArray();

            var result = await DisplayActionSheet("Change style", "Cancel", null, buttons);
            if (!string.IsNullOrEmpty(result) && buttons.Contains(result)) {
                _ViewModel.CurrentMapStyle = _Styles.FirstOrDefault((arg) => arg.Name == result);
            }
        }

        private async void GetAllStyles()
        {
            IsBusy = true;
            _Styles = await _MBService.GetAllStyles();
            IsBusy = false;

        }

        public MapStyle[] _Styles
        {
            get;
            set;
        }

        void ReloadStyle(object sender, EventArgs args)
        {
            map.ReloadStyleFunc?.Execute(sender);
        }

        void ZoomIn(object sender, EventArgs args)
        {
            map.ZoomLevel += 1.0f;
        }

		void ZoomOut(object sender, EventArgs args)
		{
			map.ZoomLevel -= 1.0f;
		}

    }
}
