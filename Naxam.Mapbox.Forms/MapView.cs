
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Xamarin.Forms;

namespace Naxam.Mapbox.Forms
{
    public class Position
    {
        public double Lat { get; set; }

        public double Long { get; set; }

		public Position()
		{
			
		}

		public Position(double lat, double lon)
		{
			Lat = lat;
			Long = lon;
		}
    }

    public class PositionChangeEventArgs : EventArgs
    {
        private Position _newPosition;

        public PositionChangeEventArgs(Position newPosition)
        {
            NewPosition = newPosition;
        }

        public Position NewPosition
        {
            [CompilerGenerated]
            get;
            [CompilerGenerated]
            private set;
        }
    }

    
    public class MapView : View
    {
        public static readonly BindableProperty IsMarkerClickedProperty = BindableProperty.Create(
          nameof(IsMarkerClicked),
          typeof(bool),
          typeof(MapView),
          default(bool),
       BindingMode.TwoWay
      );

        public bool IsMarkerClicked
        {
            get
            {
                return (bool)GetValue(IsMarkerClickedProperty);
            }
            set { SetValue(IsMarkerClickedProperty, value); }
        }
        public static readonly BindableProperty FocusPositionProperty = BindableProperty.Create(
           nameof(IsTouchInMap),
           typeof(bool),
           typeof(MapView),
           default(bool),
        BindingMode.TwoWay
       //          null,
       //          ((bindable, newValue, oldValue) =>
       //          {
       //              ((MapView)bindable).OnMapCenterChange(bindable,(Position)newValue, (Position)oldValue);
       //          })

       );


        public bool IsTouchInMap
        {
            get
            {
                return (bool)GetValue(FocusPositionProperty);
            }
            set { SetValue(FocusPositionProperty, value); }
        }

		public static readonly BindableProperty CenterProperty = BindableProperty.Create(
			nameof(Center), 
			typeof(Position), 
			typeof(MapView),
			default(Position), 
			BindingMode.TwoWay);

		public Position Center
		{
			get
			{
				return (Position)GetValue(CenterProperty);
			}
			set
			{
				SetValue(CenterProperty, (Position)value);
			}
		}

		public static readonly BindableProperty UserLocationProperty = BindableProperty.Create(
			nameof(UserLocation), 
			typeof(Position), 
			typeof(MapView), 
			default(Position), 
			BindingMode.OneWayToSource);

		public Position UserLocation
		{
			get
			{
				return (Position)GetValue(UserLocationProperty);
			}
			set
			{
				SetValue(UserLocationProperty, (Position)value);
			}
		}

		public double ZoomLevel { get; set; }
    }
}
