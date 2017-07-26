using System;
using Naxam.Controls.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapBoxQs
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		public MainPageViewModel()
		{
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

        private double _ZoomLevel;

        public double ZoomLevel
        {
            get { return _ZoomLevel; }
            set {
                _ZoomLevel = value;
                OnPropertyChanged("ZoomLevel");
            }
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
    }
}
