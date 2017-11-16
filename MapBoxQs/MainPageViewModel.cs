using System;
using Naxam.Controls.Mapbox.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapBoxQs
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        bool _IsScaleBarShown = false;
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

            }, (arg) =>  true);
		}

        private MapStyle _CurrentMapStyle = new MapStyle("cj7rtpzfde3oe2sta2xwhdi6l", "El", null, "gevadmin");

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
                System.Diagnostics.Debug.WriteLine($"Zoom level: {ZoomLevel})");
                System.Diagnostics.Debug.WriteLine($"Scale: 1 : {scale}");
            }
        }

		public Action<Position, double?, double?, bool, Action> UpdateViewPortAction
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
    }
}
