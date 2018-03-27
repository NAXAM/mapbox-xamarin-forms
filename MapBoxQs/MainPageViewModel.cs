using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Naxam.Controls.Mapbox.Forms;
using Xamarin.Forms;

namespace MapBoxQs
{
    public enum MapTools
    {
        None,
        CustomLocation,
        Camera,
        Offline
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        readonly MapBoxQs.Services.IMapBoxService MBService = new MapBoxQs.Services.MapBoxService();

        public event PropertyChangedEventHandler PropertyChanged;
        bool _IsScaleBarShown = false;
        OfflinePackRegion forcedRegion;
        IOfflineStorageService offlineService;

		public MainPageViewModel()
		{
            DidFinishRenderingCommand = new Command((obj) =>
            {
                if (_IsScaleBarShown == false && CenterLocation != null) {
                    _IsScaleBarShown = ToggleScaleBarFunc?.Invoke(true) ?? false;
					System.Diagnostics.Debug.WriteLine("Did toggle scale bar");
					//UpdateViewPortAction?.Invoke(new Position(CenterLocation.Lat + 0.001, CenterLocation.Long + 0.001), 16, null, false, () => {
					//	System.Diagnostics.Debug.WriteLine("Did update center location");
					//});
                }
                if (forcedRegion != null) {
                    UpdateViewPortAction?.Invoke(
                        new Position()
                        {
                            Lat = forcedRegion.Bounds.SouthWest.Lat / 2 + forcedRegion.Bounds.NorthEast.Lat / 2,
                            Long = forcedRegion.Bounds.SouthWest.Long / 2 + forcedRegion.Bounds.NorthEast.Long / 2
                        },
                        forcedRegion.MaximumZoomLevel / 2 + forcedRegion.MinimumZoomLevel / 2,
                        null,
                        true,
                        null
                    );
                    forcedRegion = null;
                }

            }, (arg) =>  true);


            offlineService = DependencyService.Get<Naxam.Controls.Mapbox.Forms.IOfflineStorageService>();
            //offlineService.OfflinePackProgressChanged += (sender, e) => {
            //    var progress = e.OfflinePack.Progress;
            //    float percentage = 0;
            //    if (progress.CountOfResourcesExpected > 0)
            //    {
            //        percentage = (float)progress.CountOfResourcesCompleted / progress.CountOfResourcesExpected;
            //    }
            //    System.Diagnostics.Debug.WriteLine($"Downloaded resources: {progress.CountOfResourcesCompleted} ({percentage * 100} %)");
            //    System.Diagnostics.Debug.WriteLine($"Downloaded tiles: {progress.CountOfTilesCompleted}");
            //    if (progress.CountOfResourcesExpected == progress.CountOfResourcesCompleted) {
            //        System.Diagnostics.Debug.WriteLine("Download completed");
            //    }
            //};
		}

        private MapStyle _CurrentMapStyle;

		public MapStyle CurrentMapStyle
		{
			get { return _CurrentMapStyle; }
			set
			{
				_CurrentMapStyle = value;
				OnPropertyChanged("CurrentMapStyle");
			}
		}

        private double _ZoomLevel = 16;

        public double ZoomLevel
        {
            get { return _ZoomLevel; }
            set {
                _ZoomLevel = value;
                OnPropertyChanged("ZoomLevel");
                var scale = GetMapScaleReciprocalFunc?.Invoke();
                System.Diagnostics.Debug.WriteLine($"Zoom level: {value}");
                System.Diagnostics.Debug.WriteLine($"Scale: 1 : {scale}");
            }
        }

		public Action<Position, double?, double?, bool, Action> UpdateViewPortAction
		{
			get;
			set;
		}

        public Func<StyleLayer, string, bool> InsertLayerBelowLayerFunc
        {
            get;
            set;
        }

		private ICommand _ZoomCommand;

        public ICommand ZoomCommand
        {
            get {
                return _ZoomCommand = _ZoomCommand
                        ?? new Command<int>( (int step) => ZoomLevel += step);
            }
            set {
                _ZoomCommand = value;
                OnPropertyChanged("ZoomLevel");
            }
        }

        private void OnPropertyChanged(string propertyName = null)
        {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		}

        public Func<double> GetMapScaleReciprocalFunc {
            get; set;
        }

        private Func<bool, bool> _ToggleScaleBarFunc;

        public Func<bool, bool> ToggleScaleBarFunc
		{
            get => _ToggleScaleBarFunc;
            set {
                _ToggleScaleBarFunc = value;
            }
		}

		private Position _centerLocation;

		public Position CenterLocation
		{
			get { return _centerLocation; }
			set {
                _centerLocation = value;
                OnPropertyChanged("CenterLocation");
            }
		}

        public ICommand DidFinishRenderingCommand { get; set; }

        private ICommand _DownloadCommand;
        public ICommand DownloadCommand
        {
            get { return _DownloadCommand = _DownloadCommand ?? new Command<object>(DownloadMap); }
            set {
                _DownloadCommand = value;
                OnPropertyChanged("DownloadCommand");
            }
        }

        private void DownloadMap(object obj)
        {
            if (offlineService != null)
            {
                var region = new OfflinePackRegion()
                {
                    StyleURL = CurrentMapStyle.UrlString,
                    MaximumZoomLevel = 14,
                    MinimumZoomLevel = 1,
                    Bounds = new CoordinateBounds()
                    {
                        NorthEast = new Position()
                        {
                            Lat = 55.800,
                            Long = 9.000
                        },
                        SouthWest = new Position()
                        {
                            Lat = 55.650,
                            Long = 8.700
                        }
                    }
                };

                offlineService.DownloadMap(region, new System.Collections.Generic.Dictionary<string, string>() {
                    {"name", "test"}
                });
            }

        }

        private ICommand _ClearOfflinePacksCommand;
        public ICommand ClearOfflinePacksCommand
        {
            get { return _ClearOfflinePacksCommand = _ClearOfflinePacksCommand ?? new Command<object>(ClearOfflinePacks); }
            set { _ClearOfflinePacksCommand = value;
                OnPropertyChanged("ClearOfflinePacksCommand");
            }
        }

        private async void ClearOfflinePacks(object obj)
        {
            var packs = await offlineService.GetPacks();
            if (packs != null)
            {
                foreach (OfflinePack pack in packs)
                {
                    await offlineService.RemovePack(pack);
                }
            }
        }

        private ICommand _LoadOfflinePackCommand;
        public ICommand LoadOfflinePackCommand
        {
            get { return _LoadOfflinePackCommand = _LoadOfflinePackCommand ?? new Command<object>(LoadOfflinePack); }
            set {
                _LoadOfflinePackCommand = value;
                OnPropertyChanged("LoadOfflinePackCommand");
            }
        }

        private async void LoadOfflinePack(object obj)
        {
            var packs = await offlineService.GetPacks();
            if (packs?.FirstOrDefault() is OfflinePack pack)
            {
                forcedRegion = pack.Region;
                CurrentMapStyle = new MapStyle(pack.Region.StyleURL);
            }

        }

       
        public void OfflinePackProgressDidChange(OfflinePack pack)
        {
            var expected = (float)pack.Progress.CountOfResourcesExpected;
            float percentage = 0;
            if (expected > 0)
            {
                percentage = (float)pack.Progress.CountOfResourcesCompleted / expected;
            }
            System.Diagnostics.Debug.WriteLine($"Progress: {percentage}");
            if (pack.State == OfflinePackState.Completed)
            {
                System.Diagnostics.Debug.WriteLine("Download completed");
            }
        }

        private MapTools _ShowingTool = MapTools.None;
        public MapTools ShowingTool
        {
            get { return _ShowingTool; }
            set
            {
                if (_ShowingTool != value)
                {
                    _ShowingTool = value;
                    OnPropertyChanged("ShowingTool");
                }
            }
        }

        ICommand _SwitchToolCommand;
        public ICommand SwitchToolCommand
        {
            get { return (_SwitchToolCommand = _SwitchToolCommand ?? new Command<MapTools>(ExecuteSwitchToolCommand, CanExecuteSwitchToolCommand)); }
        }
        bool CanExecuteSwitchToolCommand(MapTools obj) { return true; }
        void ExecuteSwitchToolCommand(MapTools obj)
        {
            if (ShowingTool == obj)
            {
                ShowingTool = MapTools.None;
                return;
            }
            if (obj == MapTools.CustomLocation)
            {
                if (CenterLocation != null
                    && (Math.Abs(CenterLocation.Lat - positions[0].Lat) > 0.01)
                    || Math.Abs(CenterLocation.Long - positions[0].Long) > 0.01)
                {
                    CustomLatitude = positions[0].Lat.ToString();
                    CustomLongitude = positions[0].Long.ToString();
                }
                else
                {
                    CustomLatitude = positions[1].Lat.ToString();
                    CustomLongitude = positions[1].Long.ToString();
                }
            }
            ShowingTool = obj;
        }

        #region Custom locations
        Position[] positions = new[] {
             new Position {
              Lat = 21.0333,
              Long = 105.8500
                     },
              new Position {
                            Lat = 55.75719563,
                            Long = 8.93032908
              }
         };

        private string _CustomLatitude;
        public string CustomLatitude
        {
            get { return _CustomLatitude; }
            set
            {
                if (_CustomLatitude != value)
                {
                    _CustomLatitude = value;
                    OnPropertyChanged("CustomLatitude");
                }
            }
        }

        private string _CustomLongitude;
        public string CustomLongitude
        {
            get { return _CustomLongitude; }
            set
            {
                if (_CustomLongitude != value)
                {
                    _CustomLongitude = value;
                    OnPropertyChanged("CustomLongitude");
                }
            }
        }

        ICommand _ChangeLocationCommand;
        public ICommand ChangeLocationCommand
        {
            get { return (_ChangeLocationCommand = _ChangeLocationCommand ?? new Command<object>(ExecuteChangeLocationCommand, CanExecuteChangeLocationCommand)); }
        }
        bool CanExecuteChangeLocationCommand(object obj) { return true; }
        void ExecuteChangeLocationCommand(object obj)
        {
            if (double.TryParse(CustomLatitude, out double lat)
                && double.TryParse(CustomLongitude, out double lon))
            {
                UpdateViewPortAction?.Invoke(new Position(lat, lon), null, null, true, null);
            }
        }
        #endregion

        #region Styles
        public Action ReloadStyleAction
        {
            get;
            set;
        }

        private MapStyle[] Styles
        {
            get;
            set;
        }

        ICommand _ShowStylePickerCommand;
        public ICommand ShowStylePickerCommand
        {
            get { return (_ShowStylePickerCommand = _ShowStylePickerCommand ?? new Command<object>(ExecuteShowStylePickerCommand, CanExecuteShowStylePickerCommand)); }
        }
        bool CanExecuteShowStylePickerCommand(object obj) { return true; }
        async void ExecuteShowStylePickerCommand(object obj) {
            if (Styles == null) {
                if (false == await GetAllStyles()) {
                    return;
                }
            }
            var buttons = Styles.Select((arg) => arg.Name).ToArray();
            //var choice = await UserDialogs.Instance.ActionSheetAsync("Change style", "Cancel", "Reload current style", buttons: buttons);
            //if (choice == "Reload current style") {
            //    ReloadStyleAction?.Invoke();
            //}
            //else if (buttons.Contains(choice)) {
            //    CurrentMapStyle = Styles.FirstOrDefault((arg) => arg.Name == choice);
            //}
        }

        private async Task<bool> GetAllStyles() {
            //UserDialogs.Instance.ShowLoading();
            try {
                Styles = await MBService.GetAllStyles();
                //UserDialogs.Instance.HideLoading();
                return Styles != null;
            }
            catch (Exception ex) {
                //UserDialogs.Instance.HideLoading();
                //await UserDialogs.Instance.AlertAsync(ex.Message);
                return false;
            }
        }


        #endregion

    }
}
