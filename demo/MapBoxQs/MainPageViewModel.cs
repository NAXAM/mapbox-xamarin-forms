using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Naxam.Controls.Mapbox.Forms;
using Xamarin.Forms;

namespace MapBoxQs
{
    public enum MapTools
    {
        None,
        CustomLocation,
        Camera,
        Offline,
        Annotations,
        Misc
    }

    public enum ActionState
    {
        None,
        AddPointAnnotation,
        AddPolyline
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        readonly MapBoxQs.Services.IMapBoxService MBService = new MapBoxQs.Services.MapBoxService();
        private readonly INavigation navigation;

        public event PropertyChangedEventHandler PropertyChanged;
        bool _IsScaleBarShown = false;
        OfflinePackRegion forcedRegion;
        IOfflineStorageService offlineService;

        private ObservableCollection<Annotation> _Annotations = new ObservableCollection<Annotation>();

        public ObservableCollection<Annotation> Annotations
        {
            get { return _Annotations; }
            set
            {
                _Annotations = value;
                OnPropertyChanged("Annotations");
            }
        }


        Annotation _SelectedAnnotation;
        public Annotation SelectedAnnotation
        {
            get => _SelectedAnnotation;
            set
            {
                _SelectedAnnotation = value;
                OnPropertyChanged("SelectedAnnotation");
            }
        }

        #region MapStyle Layer
        private ObservableCollection<Layer> _ListLayers;
        public ObservableCollection<Layer> ListLayers
        {
            get { return _ListLayers; }
            set
            {
                _ListLayers = value;
                OnPropertyChanged("ListLayers");
            }
        }

        private Layer _SelectedLayer;
        public Layer SelectedLayer
        {
            get { return _SelectedLayer; }
            set
            {
                _SelectedLayer = value;
                OnPropertyChanged("SelectedLayer");
            }
        }
        #endregion
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

        public MainPageViewModel(INavigation navigation)
        {

            DidFinishRenderingCommand = new Command((obj) =>
            {
                if (_IsScaleBarShown == false && CenterLocation != null)
                {
                    _IsScaleBarShown = ToggleScaleBarFunc?.Invoke(true) ?? false;
                    System.Diagnostics.Debug.WriteLine("Did toggle scale bar");
                }
                if (forcedRegion != null)
                {
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
                foreach (Layer layer in CurrentMapStyle.OriginalLayers)
                {
                    System.Diagnostics.Debug.WriteLine(layer.Id);
                }

            }, (arg) => true);

            offlineService = DependencyService.Get<Naxam.Controls.Mapbox.Forms.IOfflineStorageService>();
            offlineService.OfflinePackProgressChanged += (sender, e) =>
            {
                var progress = e.OfflinePack.Progress;
                float percentage = 0;
                if (progress.CountOfResourcesExpected > 0)
                {
                    percentage = (float)progress.CountOfResourcesCompleted / progress.CountOfResourcesExpected;
                }
                System.Diagnostics.Debug.WriteLine($"Downloaded resources: {progress.CountOfResourcesCompleted} ({percentage * 100} %)");
                System.Diagnostics.Debug.WriteLine($"Downloaded tiles: {progress.CountOfTilesCompleted}");
                if (progress.CountOfResourcesExpected == progress.CountOfResourcesCompleted)
                {
                    System.Diagnostics.Debug.WriteLine("Download completed");
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();
                    });
                }
            };
            this.navigation = navigation;

            ListLayers = new ObservableCollection<Layer>();

            DidFinishLoadingStyleCommand = new Command<MapStyle>((style) =>
           {
               ListLayers = new ObservableCollection<Layer>(style.OriginalLayers);
           });


            DidTapOnCalloutViewCommand = new Command<string>((markerId) =>
            {
                UserDialogs.Instance.Alert("You just tap on callout view of marker have id: " + markerId);
            });

            DidTapOnMarkerCommand = new Command<string>((markerId) =>
            {
                SelectedAnnotation = Annotations.First(d => d.Id.ToString() == markerId.ToString());
                System.Diagnostics.Debug.WriteLine("You just tap on marker have id: " + markerId);
            });
        }

        public ICommand DidFinishLoadingStyleCommand
        {
            get;
            set;
        }

        public ICommand DidTapOnCalloutViewCommand
        {
            get;
            set;
        }

        public ICommand DidTapOnMarkerCommand
        {
            get;
            set;
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

        private Action<Tuple<string, bool>> _SelectAnnotationAction;
        public Action<Tuple<string, bool>> SelectAnnotationAction
        {
            get { return _SelectAnnotationAction; }
            set
            {
                _SelectAnnotationAction = value;
                OnPropertyChanged("SelectAnnotationAction");
            }
        }

        private Action<Tuple<string, bool>> _DeselectAnnotationAction;
        public Action<Tuple<string, bool>> DeselectAnnotationAction
        {
            get { return _DeselectAnnotationAction; }
            set
            {
                _DeselectAnnotationAction = value;
                OnPropertyChanged("DeselectAnnotationAction");
            }
        }



        #region Zoom
        private ICommand _ZoomCommand;

        public ICommand ZoomCommand
        {
            get
            {
                return _ZoomCommand = _ZoomCommand
                        ?? new Command<int>((int step) => ZoomLevel += step);
            }
            set
            {
                _ZoomCommand = value;
                OnPropertyChanged("ZoomLevel");
            }
        }

        private double _ZoomLevel = 16;

        public double ZoomLevel
        {
            get { return _ZoomLevel; }
            set
            {
                _ZoomLevel = value;
                OnPropertyChanged("ZoomLevel");
                var scale = GetMapScaleReciprocalFunc?.Invoke();
                System.Diagnostics.Debug.WriteLine($"Zoom level: {value}");
                System.Diagnostics.Debug.WriteLine($"Scale: 1 : {scale}");
            }
        }
        #endregion
        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public Func<double> GetMapScaleReciprocalFunc
        {
            get; set;
        }

        private Func<bool, bool> _ToggleScaleBarFunc;

        public Func<bool, bool> ToggleScaleBarFunc
        {
            get => _ToggleScaleBarFunc;
            set
            {
                _ToggleScaleBarFunc = value;
            }
        }

        public Func<Task<byte[]>> TakeSnapshotFunc { get; set; }
        public Func<string, bool, StyleLayer> GetStyleLayerFunc { get; set; }
        public Func<string, Tuple<string, string>> GetImageForAnnotationFunc
        {
            get => GetImageForAnnotation;
        }

        Tuple<string, string> GetImageForAnnotation(string annotationId)
        {
            return new Tuple<string, string>("default_pin", "pin");
        }

        public Func<string, Byte[]> GetStyleImageFunc { get; set; }

        private Position _centerLocation;

        public Position CenterLocation
        {
            get { return _centerLocation; }
            set
            {
                _centerLocation = value;
                OnPropertyChanged("CenterLocation");
            }
        }

        public ICommand DidFinishRenderingCommand { get; set; }


        private ICommand _DownloadCommand;
        public ICommand DownloadCommand
        {
            get { return _DownloadCommand = _DownloadCommand ?? new Command<object>(DownloadMap); }
            set
            {
                _DownloadCommand = value;
                OnPropertyChanged("DownloadCommand");
            }
        }
        private async void DownloadMap(object obj)
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
                            Lat = CenterLocation.Lat - 0.01,
                            Long = CenterLocation.Long + 0.005
                        },
                        SouthWest = new Position()
                        {
                            Lat = CenterLocation.Lat + 0.01,
                            Long = CenterLocation.Long - 0.005
                        }
                    }
                };
                UserDialogs.Instance.ShowLoading();
                var pack = await offlineService.DownloadMap(region, new System.Collections.Generic.Dictionary<string, string>() {
                    {"name", "test"},
                    {"started_at", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy")}
                });
                if (pack != null)
                {
                    offlineService.RequestPackProgress(pack);
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
        }

        private ICommand _ClearOfflinePacksCommand;
        public ICommand ClearOfflinePacksCommand
        {
            get { return _ClearOfflinePacksCommand = _ClearOfflinePacksCommand ?? new Command<object>(ClearOfflinePacks); }
            set
            {
                _ClearOfflinePacksCommand = value;
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
            set
            {
                _LoadOfflinePackCommand = value;
                OnPropertyChanged("LoadOfflinePackCommand");
            }
        }

        private async void LoadOfflinePack(object obj)
        {
            var packs = await offlineService.GetPacks();
            if (packs != null && packs.Length != 0)
            {
                var buttons = new List<string>();
                foreach (OfflinePack pack in packs)
                {
                    if (pack.Info != null
                        && pack.Info.TryGetValue("name", out string name)
                        && pack.Info.TryGetValue("started_at", out string startTime))
                    {
                        buttons.Add(name + " - " + startTime);
                    }
                }
                var chosen = await UserDialogs.Instance.ActionSheetAsync("Load offline pack", "Cancel", null, null, buttons.ToArray());
                var chosenIndex = buttons.IndexOf(chosen);
                if (chosenIndex >= 0 && chosenIndex < packs.Length)
                {
                    var chosenPack = packs[chosenIndex];
                    forcedRegion = chosenPack.Region;
                    CurrentMapStyle = new MapStyle(chosenPack.Region.StyleURL);

                }
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("There's no offline pack to load");
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
        async void ExecuteShowStylePickerCommand(object obj)
        {
            if (Styles == null)
            {
                if (false == await GetAllStyles())
                {
                    return;
                }
            }
            var buttons = Styles.Select((arg) => arg.Name).ToArray();
            var choice = await UserDialogs.Instance.ActionSheetAsync("Change style", "Cancel", "Reload current style", buttons: buttons);
            if (choice == "Reload current style")
            {
                ReloadStyleAction?.Invoke();
            }
            else if (buttons.Contains(choice))
            {
                CurrentMapStyle = Styles.FirstOrDefault((arg) => arg.Name == choice);
            }
        }

        private async Task<bool> GetAllStyles()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                Styles = await MBService.GetAllStyles();
                UserDialogs.Instance.HideLoading();
                return Styles != null;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(ex.Message);
                return false;
            }
        }

        #endregion
        #region Rotation
        private double _RotatedDegree;
        public double RotatedDegree
        {
            get { return _RotatedDegree; }
            set
            {
                if (Math.Abs(_RotatedDegree - value) > 0.001)
                {
                    _RotatedDegree = value;
                    OnPropertyChanged("RotatedDegree");
                }
            }
        }
        private ICommand _RotateCommand;
        public ICommand RotateCommand
        {
            get
            {
                return _RotateCommand = _RotateCommand
                        ?? new Command<int>(RotateMap);
            }
        }

        private void RotateMap(int obj)
        {
            if (obj == 0)
            {
                RotatedDegree = 0;
            }
            else
            {
                RotatedDegree += obj;
            }
        }
        #endregion
        #region Pitch
        private double _Pitch;
        public double Pitch
        {
            get { return _Pitch; }
            set
            {
                if (Math.Abs(_Pitch - value) > 0.001)
                {
                    _Pitch = value;
                    OnPropertyChanged("Pitch");
                }
            }
        }

        private ICommand _TiltCommand;
        public ICommand TiltCommand
        {
            get
            {
                return _TiltCommand = _TiltCommand
                        ?? new Command<int>(TiltMap);
            }
        }


        private void TiltMap(int obj)
        {
            if (obj == 0)
            {
                Pitch = 0.0;
            }
            else
            {
                var newValue = Pitch + obj;
                Pitch = Math.Max(0, newValue);
            }
        }
        #endregion
        #region Annotations
        private ActionState _CurrentAction;
        public ActionState CurrentAction
        {
            get { return _CurrentAction; }
            set
            {
                if (_CurrentAction != value)
                {
                    _CurrentAction = value;
                    OnPropertyChanged("CurrentAction");
                }
            }
        }

        ICommand _ToggleCurrentActionCommand;
        public ICommand ToggleCurrentActionCommand
        {
            get { return (_ToggleCurrentActionCommand = _ToggleCurrentActionCommand ?? new Command<ActionState>(ExecuteToggleCurrentActionCommand, CanExecuteToggleCurrentActionCommand)); }
        }
        bool CanExecuteToggleCurrentActionCommand(ActionState state) { return true; }
        void ExecuteToggleCurrentActionCommand(ActionState state)
        {
            if (CurrentAction != state)
            {
                CurrentAction = state;
                Annotations.Clear();
            }
            else
            {
                CurrentAction = ActionState.None;
            }
        }

        ICommand _DidTapOnMapCommand;
        public ICommand DidTapOnMapCommand
        {
            get { return (_DidTapOnMapCommand = _DidTapOnMapCommand ?? new Command<Tuple<Position, Point>>(ExecuteDidTapOnMapCommand, CanExecuteDidTapOnMapCommand)); }
        }
        bool CanExecuteDidTapOnMapCommand(Tuple<Position, Point> obj) { return true; }
        void ExecuteDidTapOnMapCommand(Tuple<Position, Point> obj)
        {
            Annotations = Annotations ?? new ObservableCollection<Annotation>();
            if (CurrentAction == ActionState.AddPointAnnotation)
            {
                var annot = new PointAnnotation()
                {
                    Id = Annotations.Count.ToString(),
                    Coordinate = obj.Item1
                };
                annot.Title = "PointAnnot." + annot.Id;
                Annotations.Add(annot);
                OnPropertyChanged("Annotations");
            }
        }

        ICommand _ClearAllAnnotation;
        public ICommand ClearAllAnnotation
        {
            get { return _ClearAllAnnotation = _ClearAllAnnotation ?? new Command<object>(ExecuteClearAllAnnotation, CanExecuteClearAllAnnotation); }
        }
        bool CanExecuteClearAllAnnotation(object obj) { return true; }
        void ExecuteClearAllAnnotation(object obj)
        {
            Annotations.Clear();
        }

        #endregion
        #region Other

        ICommand _GetStyleImageCommand;
        public ICommand GetStyleImageCommand
        {
            get { return _GetStyleImageCommand = _GetStyleImageCommand ?? new Command<object>(ExecuteGetStyleImageCommand, CanExecuteGetStyleImageCommand); }
        }
        bool CanExecuteGetStyleImageCommand(object obj) { return true; }
        async void ExecuteGetStyleImageCommand(object obj)
        {
            var configs = new PromptConfig()
            {
                Title = "Get an image of the current style",
                Message = "Please input image name",
                CancelText = "Cancel",
                OkText = "Get image",
                Text = "city-small"
            };

            var result = await UserDialogs.Instance.PromptAsync(configs);
            if (result.Ok && false == string.IsNullOrEmpty(result.Text))
            {
                var styleImageResult = GetStyleImageFunc?.Invoke(result.Text);
                if (styleImageResult == null)
                {
                    await UserDialogs.Instance.AlertAsync("Image not found!");
                }
                else
                {
                    await navigation.PushAsync(new Views.ShowPhotoDialog(styleImageResult));
                }
            }
        }

        ICommand _TakeSnapshotCommand;
        public ICommand TakeSnapshotCommand
        {
            get { return _TakeSnapshotCommand = _TakeSnapshotCommand ?? new Command<object>(ExecuteTakeSnapshotCommand, CanExecuteTakeSnapshotCommand); }
        }
        bool CanExecuteTakeSnapshotCommand(object obj) { return true; }
        async void ExecuteTakeSnapshotCommand(object obj)
        {
            var snapshotResult = await TakeSnapshotFunc?.Invoke();
            await navigation.PushAsync(new Views.ShowPhotoDialog(snapshotResult));
        }

        ICommand _GetStyleLayerCommand;
        public ICommand GetStyleLayerCommand
        {
            get { return _GetStyleLayerCommand = _GetStyleLayerCommand ?? new Command<object>(ExecuteGetStyleLayerCommand, CanExecuteGetStyleLayerCommand); }
        }
        bool CanExecuteGetStyleLayerCommand(object obj) { return true; }
        async void ExecuteGetStyleLayerCommand(object obj)
        {
            var buttons = ListLayers.Select((arg) => arg.Id).ToArray();
            var choice = await UserDialogs.Instance.ActionSheetAsync("Choose Layer", "", "OK", buttons: buttons);
            if (choice == "OK")
            {

            }
            else if (buttons.Contains(choice))
            {
                var layer = GetStyleLayerFunc(choice, false);
                if (layer is BackgroundLayer background)
                {
                    UserDialogs.Instance.Alert("You choose layer: " + background.Id + "\nType: " + nameof(BackgroundLayer) + "\n" + background.BackgroundColor.ToString(), "Layer Detail");
                    return;
                }
                if (layer is LineLayer line)
                {
                    UserDialogs.Instance.Alert("You choose layer: " + line.Id + "\nType: " + nameof(LineLayer) + "\n" + line.LineColor.ToString(), "Layer Detail");
                    return;
                }
                if (layer is CircleLayer circle)
                {
                    UserDialogs.Instance.Alert("You choose layer: " + circle.Id + "\nType: " + nameof(CircleLayer) + "\n" + circle.CircleColor.ToString(), "Layer Detail");
                    return;
                }
                if (layer is FillLayer fill)
                {
                    UserDialogs.Instance.Alert("You choose layer: " + fill.Id + "\nType: " + nameof(FillLayer) + "\n" + fill.FillColor.ToString(), "Layer Detail");
                    return;
                }
                if (layer is RasterStyleLayer raster)
                {
                    UserDialogs.Instance.Alert("You choose layer: " + raster.Id + "\nType: " + nameof(RasterStyleLayer) + "\n" + raster.SourceId.ToString(), "Layer Detail");
                    return;
                }
                if (layer is SymbolLayer symbol)
                {
                    UserDialogs.Instance.Alert("You choose layer: " + symbol.Id + "\nType: " + nameof(SymbolLayer) + "\n" + symbol.IconImageName.ToString(), "Layer Detail");
                    return;
                }
                UserDialogs.Instance.Alert("Can not find informations of layer : " + choice, "Layer Detail");
            }
        }

        ICommand _AddSatelliteLayerCommand;
        public ICommand AddSatelliteLayerCommand
        {
            get { return _AddSatelliteLayerCommand = _AddSatelliteLayerCommand ?? new Command<object>(ExecuteAddSatelliteLayerCommand, CanExecuteAddSatelliteLayerCommand); }
        }
        bool CanExecuteAddSatelliteLayerCommand(object obj) { return true; }
        void ExecuteAddSatelliteLayerCommand(object obj)
        {
            List<MapSource> listCustomLayer = new List<MapSource>();
            listCustomLayer.Add(new RasterSource("my-raster-source", "mapbox://mapbox.satellite", 512));
            CurrentMapStyle.CustomSources = listCustomLayer;
        }


        ICommand _SelectAnnotationCommand;
        public ICommand SelectAnnotationCommand
        {
            get { return _SelectAnnotationCommand = _SelectAnnotationCommand ?? new Command<object>(ExecuteSelectAnnotationCommand, CanExecuteSelectAnnotationCommand); }
        }
        bool CanExecuteSelectAnnotationCommand(object obj) { return true; }
        async void ExecuteSelectAnnotationCommand(object obj)
        {
            var buttons = Annotations.Select((arg) => arg.Id).ToArray();
            var choice = await UserDialogs.Instance.ActionSheetAsync("Choose Layer", "Cancel", "OK", buttons: buttons);
            if (buttons.Contains(choice))
            {
                SelectAnnotationAction?.Invoke(new Tuple<string, bool>(choice, false));
                if (Annotations.First(d => d.Id == choice) is PointAnnotation point)
                {
                    UserDialogs.Instance.Alert("You just select marker:\nId: " + point.Id + "\nLat: " + point.Coordinate.Lat + "\nLng: " + point.Coordinate.Long, "Selected Annotation");
                    SelectedAnnotation = point;
                }
            }
        }

        ICommand _DeselectAnnotationCommand;
        public ICommand DeselectAnnotationCommand
        {
            get { return _DeselectAnnotationCommand = _DeselectAnnotationCommand ?? new Command<object>(ExecuteDeselectAnnotationCommand, CanExecuteDeselectAnnotationCommand); }
        }
        bool CanExecuteDeselectAnnotationCommand(object obj) { return true; }
        void ExecuteDeselectAnnotationCommand(object obj)
        {
            if (SelectedAnnotation != null)
                DeselectAnnotationAction?.Invoke(new Tuple<string, bool>(SelectedAnnotation.Id, false));
        }
        #endregion
    }
}
