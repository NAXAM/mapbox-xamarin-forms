using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Annotations;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
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
        readonly MapBoxQs.Services.IMapBoxService MBService;
        readonly INavigation navigation;
        bool isScaleBarShown = false;
        int i = 1;
        string colorPolyline = "#ff1234";
        OfflinePackRegion forcedRegion;
        IOfflineStorageService offlineService;
        public event PropertyChangedEventHandler PropertyChanged;

        public IMapFunctions MapFunctions { get; set; }

        ObservableCollection<Annotation> _Annotations;
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

        MapStyle _CurrentMapStyle;
        public MapStyle CurrentMapStyle
        {
            get { return _CurrentMapStyle; }
            set
            {
                _CurrentMapStyle = value;
                OnPropertyChanged("CurrentMapStyle");
            }
        }

        ICommand _InfoActionCommand;
        public ICommand InfoActionCommand
        {
            get { return (_InfoActionCommand = _InfoActionCommand ?? new Command<Annotation>(ExecuteInfoActionCommand)); }
        }
        void ExecuteInfoActionCommand(Annotation parameter)
        {
            System.Diagnostics.Debug.WriteLine($"{parameter.Title}\n{parameter.SubTitle}");
        }

        public MainPageViewModel(INavigation navigation)
        {
            MBService = new MapBoxQs.Services.MapBoxService();
            Annotations = new ObservableCollection<Annotation>();

            CurrentMapStyle =
                //new MapStyle("mapbox://styles/hemamaps/cjy9oxfop0v511co4ep2uvt7t");
                MapStyle.LIGHT;

            //CenterLocation =
            //    //new LatLng(-28.4353498348801, 140.31082470261492);
            //    new LatLng(21.004142f, 105.847607f);

            //Annotations = new ObservableCollection<Annotation> {
            //                new SymbolAnnotation {
            //                    Coordinates = new LatLng(21.004142f, 105.847607f),
            //                    Id = Guid.NewGuid().ToString(),
            //                    Title = "Naxam Company Limited",
            //                    SubTitle = "A software development agency from Hanoi, Vietnam",
            //                    IconImage = "harbor-15",
            //                    IconSize = 4,
            //                    IconColor = Color.Green,
            //                    IsDraggable = true
            //,                }
            //            };
            DidFinishRenderingCommand = new Command((obj) =>
            {
                if (isScaleBarShown == false && CenterLocation != LatLng.Zero)
                {
                    isScaleBarShown = ToggleScaleBarFunc?.Invoke(true) ?? false;
                    System.Diagnostics.Debug.WriteLine("Did toggle scale bar");
                }
                if (forcedRegion != null)
                {
                    //UpdateViewPortAction?.Invoke(
                    //    new LatLng
                    //    {
                    //        Lat = forcedRegion.Bounds.SouthWest.Lat / 2 + forcedRegion.Bounds.NorthEast.Lat / 2,
                    //        Long = forcedRegion.Bounds.SouthWest.Long / 2 + forcedRegion.Bounds.NorthEast.Long / 2
                    //    },
                    //    forcedRegion.MaximumZoomLevel / 2 + forcedRegion.MinimumZoomLevel / 2,
                    //    null,
                    //    true,
                    //    null
                    //);
                    //forcedRegion = null;
                }

            }, (arg) => true);

            offlineService = DependencyService.Get<Naxam.Controls.Forms.IOfflineStorageService>();
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

            DidFinishLoadingStyleCommand = new Command<MapStyle>((style) =>
            {
                MapFunctions.AddStyleImage(new IconImageSource()
                {
                    Source = "ic_cross.xml"
                });
                //var source = new GeoJsonSource {
                //    Id = "regions.src",
                //    Url = "https://gist.githubusercontent.com/tobrun/cf0d689c8187d42ebe62757f6d0cf137/raw/4d8ac3c8333f1517df9d303d58f20f4a1d8841e8/regions.geojson"
                //};
                //MapFunctions.AddSource(source);

                //var layer = new FillLayer("regions.layer", "regions.src") {
                //    FillOpacity = 0.7,
                //    FillColor = Color.Green
                //};
                //MapFunctions.AddLayer(layer);

                //var source = new GeoJsonSource
                //{
                //    Id = "ports.src",
                //    Url = "ports.geojson",
                //    IsLocal = true,
                //};
                //MapFunctions.AddSource(source);

                //ImageSource iconSource = "red_marker.png";
                //IconImageSource iconImageSource = iconSource;
                //MapFunctions.AddStyleImage(iconImageSource);

                //var source = new GeoJsonSource
                //{
                //    Id = "us_west_coast.src",
                //    Url = "us_west_coast.geojson",
                //    IsLocal = true,
                //};

                //MapFunctions.AddSource(source);

                //var makersLayer = new SymbolLayer("us_west_coast.markers.layer", source.Id)
                //{
                //    IconImageName = iconImageSource,
                //    IconAllowOverlap = true,
                //    IconOffset = new float[] { 0, -8 }
                //};
                //MapFunctions.AddLayer(makersLayer);

                //var infoWindowLayer = new SymbolLayer("us_west_coast.info-window.layer", source.Id)
                //{
                //    IconImageName = iconImageSource,
                //    IconAllowOverlap = true,
                //    IconOffset = new float[] { -2, -8 }
                //};
                //MapFunctions.AddLayer(makersLayer);

                var geojsonSrc = new GeoJsonSource
                {
                    Id = "earthquakes.src",
                    Url = "https://www.mapbox.com/mapbox-gl-js/assets/earthquakes.geojson",
                    Options = new GeoJsonOptions
                    {
                        Cluster = true,
                        ClusterMaxZoom = 14,
                        ClusterRadius = 50
                    }
                };
                MapFunctions.AddSource(geojsonSrc);

                var unclusteredLayer = new SymbolLayer("unclustered.layer", geojsonSrc.Id)
                {
                    IconImage = Expression.Literal("ic_cross.xml"),
                    IconSize = Expression.Division(Expression.Get("mag"), Expression.Literal(4.0)),
                    IconColor = Expression.Interpolate(
                        Expression.Exponential(1.0),
                        Expression.Get("mag"),
                        Expression.CreateStop(2.0, Expression.Rgb(0, 255, 0)),
                        Expression.CreateStop(4.5, Expression.Rgb(0, 0, 255)),
                        Expression.CreateStop(7.0, Expression.Rgb(255, 0, 0))
                    )
                };
                MapFunctions.AddLayer(unclusteredLayer);

                var layers = new KeyValuePair<int, Color>[] {
                    new KeyValuePair<int, Color>(150, Color.Green),
                    new KeyValuePair<int, Color>(20, Color.Yellow),
                    new KeyValuePair<int, Color>(0, Color.Purple)
                };
                for (int j = 0; j < layers.Length; j++)
                {
                    var item = layers[j];
                    var layer = new CircleLayer($"layer_{item.Key}_{item.Value.ToHex()}", geojsonSrc.Id)
                    {
                        CircleColor = Expression.Color(item.Value),
                        CircleRadius = Expression.Literal(18)
                    };
                    var pointCount = Expression.ToNumber(Expression.Get("point_count"));
                    var filter = j == 0
                        ? Expression.All(
                            Expression.Has("point_count"),
                            Expression.Gte(pointCount, Expression.Literal(item.Key)))
                        : Expression.All(
                            Expression.Has("point_count"),
                            Expression.Gte(pointCount, Expression.Literal(item.Key)),
                            Expression.Lt(pointCount, Expression.Literal(layers[j-1].Key))
                        );
                    layer.Filter = filter;

                    MapFunctions.AddLayer(layer);
                    j++;
                }

                var count = new SymbolLayer("count.layer", geojsonSrc.Id) {
                    TextField = Expression.ToString(Expression.Get("point_count")),
                    TextSize = Expression.Literal(12.0),
                    TextColor = Expression.Color(Color.White),
                    TextIgnorePlacement = Expression.Literal(true),
                    TextAllowOverlap = Expression.Literal(true),
                };
                
                MapFunctions.AddLayer(count);
            });


            DidTapOnCalloutViewCommand = new Command<string>((markerId) =>
            {
                // UserDialogs.Instance.Alert("You just tap on callout view of marker have id: " + markerId);
            });

            DidTapOnMarkerCommand = new Command<string>((markerId) =>
            {
                SelectedAnnotation = Annotations.First(d => d.Id.ToString() == markerId.ToString());
                System.Diagnostics.Debug.WriteLine("You just tap on marker have id: " + markerId);
                UserDialogs.Instance.Alert("You just tap on marker of marker have id: " + SelectedAnnotation.Title);
            });
        }

        private LatLng _UserLocation;

        public LatLng UserLocation
        {
            get { return _UserLocation; }
            set
            {
                _UserLocation = value;
                OnPropertyChanged("UserLocation");
            }
        }


        public ICommand DidFinishLoadingStyleCommand { get; set; }

        public ICommand DidTapOnCalloutViewCommand { get; set; }

        public ICommand DidTapOnMarkerCommand { get; set; }

        Action<Tuple<string, bool>> _SelectAnnotationAction;
        public Action<Tuple<string, bool>> SelectAnnotationAction
        {
            get { return _SelectAnnotationAction; }
            set
            {
                _SelectAnnotationAction = value;
                OnPropertyChanged("SelectAnnotationAction");
            }
        }

        Action<Tuple<string, bool>> _DeselectAnnotationAction;
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

        public Func<double> GetMapScaleReciprocalFunc { get; set; }

        private Func<bool, bool> _ToggleScaleBarFunc;

        public Func<bool, bool> ToggleScaleBarFunc
        {
            get => _ToggleScaleBarFunc;
            set
            {
                _ToggleScaleBarFunc = value;
            }
        }

        private LatLng _centerLocation;

        public LatLng CenterLocation
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
            if (offlineService == null || CenterLocation == LatLng.Zero) return;
            var region = new OfflinePackRegion()
            {
                StyleURL = CurrentMapStyle.UrlString,
                MaximumZoomLevel = 14,
                MinimumZoomLevel = 1,
                Bounds = new LatLngBounds
                {
                    NorthEast = new LatLng
                    {
                        Lat = CenterLocation.Lat - 0.01,
                        Long = CenterLocation.Long + 0.005
                    },
                    SouthWest = new LatLng
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
                    //forcedRegion = chosenPack.Region;
                    //CurrentMapStyle = new MapStyle(chosenPack.Region.StyleURL);
                    //ApplyOfflinePackFunc?.Invoke(chosenPack);
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
                CustomLatitude = positions[0].Lat.ToString();
                CustomLongitude = positions[0].Long.ToString();
            }
            ShowingTool = obj;
        }

        ICommand _FocusUserLocationCommand;
        public ICommand FocusUserLocationCommand
        {
            get { return (_FocusUserLocationCommand = _FocusUserLocationCommand ?? new Command<object>(ExecuteFocusUserLocationCommand, CanExecuteFocusUserLocationCommand)); }
        }
        bool CanExecuteFocusUserLocationCommand(object parameter) { return true; }
        void ExecuteFocusUserLocationCommand(object parameter)
        {
            CenterLocation = UserLocation;
        }

        #region Custom locations
        LatLng[] positions = new[] {
            new LatLng {
                Lat = 21.0333,
                Long = 105.8500
            },
              new LatLng {
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
                && double.TryParse(CustomLongitude, out double lng))
            {
                CenterLocation = new LatLng(lat, lng);
            }
        }

        #endregion

        ICommand _DragFinishedCommand;
        public ICommand DragFinishedCommand
        {
            get { return (_DragFinishedCommand = _DragFinishedCommand ?? new Command<object>(ExecuteDragFinishedCommand, CanExecuteDragFinishedCommand)); }
        }
        bool CanExecuteDragFinishedCommand(object parameter) { return true; }
        void ExecuteDragFinishedCommand(object parameter)
        {

        }

        #region Styles
        public Action ReloadStyleAction { get; set; }

        private MapStyle[] Styles { get; set; }

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

        //PolylineAnnotation polyline;
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
                switch (state)
                {
                    case ActionState.AddPolyline:
                        break;
                }

            }
            else
            {
                switch (state)
                {
                    case ActionState.AddPolyline:
                        //polyline = null;
                        break;
                }
                CurrentAction = ActionState.None;
            }
        }

        ICommand _DidTapOnMapCommand;
        public ICommand DidTapOnMapCommand
        {
            get { return (_DidTapOnMapCommand = _DidTapOnMapCommand ?? new Command<Tuple<LatLng, Point>>(this.ExecuteDidTapOnMapCommand)); }
        }

        ImageSource pinIcon = (ImageSource)"pin.png";

        void ExecuteDidTapOnMapCommand(Tuple<LatLng, Point> currentTap)
        {
            if (Annotations == null || currentTap == null) return;
            var currentPosition = currentTap.Item1;

            if (CurrentAction == ActionState.AddPointAnnotation)
            {
                var annotation = new SymbolAnnotation()
                {
                    Coordinates = currentPosition,
                    IconImage = pinIcon,
                    IconSize = 1,
                    IconColor = Color.Green,
                };
                annotation.Title = "PointAnnot." + i;
                Annotations.Add(annotation);
                i = i + 1;
            }
            if (CurrentAction == ActionState.AddPolyline)
            {
                //if (polyline == null)
                //    polyline = new PolylineAnnotation
                //    {
                //        HexColor = colorPolyline,
                //        Width = 1
                //    };
                //if (polyline.Coordinates == null)
                //{
                //    polyline.Coordinates = new ObservableCollection<LatLng> { currentPosition };
                //    Annotations.Add(polyline);
                //}
                //else
                //    (polyline.Coordinates as ObservableCollection<LatLng>).Add(currentPosition);
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
            i = 1;
        }

        #endregion
        #region Other

        ICommand _QueryFeaturesAtCenterPointCommand;
        public ICommand QueryFeaturesAtCenterPointCommand
        {
            get { return _QueryFeaturesAtCenterPointCommand = _QueryFeaturesAtCenterPointCommand ?? new Command<object>(ExecuteQueryFeaturesAtCenterPointCommand, CanExecuteQueryFeaturesAtCenterPointCommand); }
        }
        bool CanExecuteQueryFeaturesAtCenterPointCommand(object obj) { return true; }
        void ExecuteQueryFeaturesAtCenterPointCommand(object obj)
        {
            if (MapFunctions == null) return;

            var features = MapFunctions.QueryFeatures(CenterLocation);

            System.Diagnostics.Debug.Write(features.Length);
        }

        ICommand _TakeSnapshotCommand;
        public ICommand TakeSnapshotCommand
        {
            get { return _TakeSnapshotCommand = _TakeSnapshotCommand ?? new Command<object>(ExecuteTakeSnapshotCommand, CanExecuteTakeSnapshotCommand); }
        }
        bool CanExecuteTakeSnapshotCommand(object obj) { return true; }
        async void ExecuteTakeSnapshotCommand(object obj)
        {
            if (MapFunctions == null) return;

            var snapshotResult = await MapFunctions.TakeSnapshotAsync();
            await navigation.PushAsync(new Views.ShowPhotoDialog(snapshotResult));
        }

        #endregion
    }
}