
using System;
using System.Collections;
using System.Collections.Generic;
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

		public static readonly BindableProperty DelegateProperty = BindableProperty.Create(
		nameof(Delegate),
		typeof(MapViewDelegate),
		typeof(MapView),
		default(MapViewDelegate));

		public MapViewDelegate Delegate
		{
			get
			{
				return (MapViewDelegate)GetValue(DelegateProperty);
			}
			set
			{
				SetValue(DelegateProperty, (MapViewDelegate)value);
			}
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
			BindingMode.TwoWay);

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

		public static readonly BindableProperty ZoomLevelProperty = BindableProperty.Create(
			nameof(ZoomLevel),
			typeof(double),
			typeof(MapView),
			10.0);

		public double ZoomLevel
		{
			get
			{
				return (double)GetValue(ZoomLevelProperty);
			}
			set
			{
				SetValue(ZoomLevelProperty, (double)value);
			}
		}

		public static readonly BindableProperty PitchEnabledProperty = BindableProperty.Create(
			nameof(PitchEnabled),
			typeof(bool),
			typeof(MapView),
			default(bool));

		public bool PitchEnabled
		{
			get
			{
				return (bool)GetValue(PitchEnabledProperty);
			}
			set
			{
				SetValue(PitchEnabledProperty, (bool)value);
			}
		}

		public static readonly BindableProperty RotateEnabledProperty = BindableProperty.Create(
			nameof(RotateEnabled),
			typeof(bool),
			typeof(MapView),
			default(bool));

		public bool RotateEnabled
		{
			get
			{
				return (bool)GetValue(RotateEnabledProperty);
			}
			set
			{
				SetValue(RotateEnabledProperty, (bool)value);
			}
		}

		//public static readonly BindableProperty StyleUrlProperty = BindableProperty.Create(
		//	nameof(StyleUrl),
		//	typeof(string),
		//	typeof(MapView),
		//	default(string));

		//public string StyleUrl
		//{
		//	get
		//	{
		//		return (string)GetValue(StyleUrlProperty);
		//	}
		//	set
		//	{
		//		SetValue(StyleUrlProperty, (string)value);
		//	}
		//}

        public static readonly BindableProperty MapStyleProperty = BindableProperty.Create(
        nameof(MapStyle),
        typeof(MapStyle),
        typeof(MapView),
        default(MapStyle));
        public MapStyle MapStyle
        {
            get
            {
                return (MapStyle)GetValue(MapStyleProperty);
            }
            set
            {
                SetValue(MapStyleProperty, (MapStyle)value);
            }
        }

		public static readonly BindableProperty AnnotationsProperty = BindableProperty.Create(							   
			nameof(Annotations),
			typeof(IEnumerable<Annotation>),
			typeof(MapView),								   
			default(IEnumerable<Annotation>),								   
			BindingMode.TwoWay);

		public IEnumerable<Annotation> Annotations
		{
			get
			{
				return (IEnumerable<Annotation>)GetValue(AnnotationsProperty);
			}
			set
			{
				SetValue(AnnotationsProperty, (IEnumerable<Annotation>)value);
			}
		}

		static Func<string, bool> DefaultCanShowCalloutChecker = x => true;
		public static readonly BindableProperty CanShowCalloutCheckerProperty = BindableProperty.Create(                                                                
			nameof(CanShowCalloutChecker),
			typeof(Func<string, bool>),
			typeof(MapView),
			default(Func<string, bool>));

		public Func<string, bool> CanShowCalloutChecker
		{
			get
			{
				return (Func<string, bool>)GetValue(CanShowCalloutCheckerProperty) ?? DefaultCanShowCalloutChecker;
			}
			set
			{
				SetValue(CanShowCalloutCheckerProperty, value);
			}
		}	

		public static readonly BindableProperty TakeSnapshotProperty = BindableProperty.Create(
			nameof(TakeSnapshot),
			typeof(Func<byte[]>),
			typeof(MapView),
			default(Func<byte[]>),
			defaultBindingMode:BindingMode.OneWayToSource);

		public Func<byte[]> TakeSnapshot
		{
			get
			{
				return (Func<byte[]>)GetValue(TakeSnapshotProperty);
			}
			set
			{
				SetValue(TakeSnapshotProperty, value);
			}
		}

		public static readonly BindableProperty DidTapOnMapCommandProperty = BindableProperty.Create(
			nameof(DidTapOnMapCommand),
			typeof(ICommand),
			typeof(MapView),
			default(ICommand));

		public ICommand DidTapOnMapCommand
		{
			get
			{
				return (ICommand)GetValue(DidTapOnMapCommandProperty);
			}
			set
			{
				SetValue(DidTapOnMapCommandProperty, (ICommand)value);
			}
		}
	}
}
