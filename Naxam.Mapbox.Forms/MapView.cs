
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
    public class PositionChangeEventArgs : EventArgs
    {
        private Position _newPosition;

        public PositionChangeEventArgs (Position newPosition)
        {
            NewPosition = newPosition;
        }

        public Position NewPosition {
            [CompilerGenerated]
            get;
            [CompilerGenerated]
            private set;
        }
    }

    public partial class MapView : View
    {
        public static readonly BindableProperty IsMarkerClickedProperty = BindableProperty.Create (
          nameof (IsMarkerClicked),
          typeof (bool),
          typeof (MapView),
          default (bool),
       BindingMode.TwoWay
      );

        public bool IsMarkerClicked {
            get {
                return (bool)GetValue (IsMarkerClickedProperty);
            }
            set { SetValue (IsMarkerClickedProperty, value); }
        }

        public static readonly BindableProperty FocusPositionProperty = BindableProperty.Create (
           nameof (IsTouchInMap),
           typeof (bool),
           typeof (MapView),
           default (bool),
            BindingMode.TwoWay);


        public bool IsTouchInMap {
            get {
                return (bool)GetValue (FocusPositionProperty);
            }
            set { SetValue (FocusPositionProperty, value); }
        }

        public static readonly BindableProperty CenterProperty = BindableProperty.Create (
            nameof (Center),
            typeof (Position),
            typeof (MapView),
            default (Position),
            BindingMode.TwoWay);

        public Position Center {
            get {
                return (Position)GetValue (CenterProperty);
            }
            set {
                SetValue (CenterProperty, (Position)value);
            }
        }

        public static readonly BindableProperty UserLocationProperty = BindableProperty.Create (
            nameof (UserLocation),
            typeof (Position),
            typeof (MapView),
            default (Position),
            BindingMode.TwoWay);

        public Position UserLocation {
            get {
                return (Position)GetValue (UserLocationProperty);
            }
            set {
                SetValue (UserLocationProperty, (Position)value);
            }
        }

        public static readonly BindableProperty ZoomLevelProperty = BindableProperty.Create (
            nameof (ZoomLevel),
            typeof (double),
            typeof (MapView),
            10.0,
            BindingMode.TwoWay);

        public double ZoomLevel {
            get {
                return (double)GetValue (ZoomLevelProperty);
            }
            set {
                if (Math.Abs(value - ZoomLevel) > 0.01) {
                    SetValue(ZoomLevelProperty, (double)value);
                }
            }
        }

        public static readonly BindableProperty PitchProperty = BindableProperty.Create (
            nameof (Pitch),
            typeof (double),
            typeof (MapView),
            0.0,
            BindingMode.TwoWay);

        public double Pitch {
            get {
                return (double)GetValue (PitchProperty);
            }
            set {
                SetValue (PitchProperty, (double)value);
            }
        }

        public static readonly BindableProperty PitchEnabledProperty = BindableProperty.Create (
            nameof (PitchEnabled),
            typeof (bool),
            typeof (MapView),
            default (bool),
            BindingMode.TwoWay);

        public bool PitchEnabled {
            get {
                return (bool)GetValue (PitchEnabledProperty);
            }
            set {
                SetValue (PitchEnabledProperty, value);
            }
        }

        public static readonly BindableProperty RotateEnabledProperty = BindableProperty.Create (
            nameof (RotateEnabled),
            typeof (bool),
            typeof (MapView),
            default (bool),
            BindingMode.TwoWay);

        public bool RotateEnabled {
            get {
                return (bool)GetValue (RotateEnabledProperty);
            }
            set {
                SetValue (RotateEnabledProperty, (bool)value);
            }
        }

        public static readonly BindableProperty RotatedDegreeProperty = BindableProperty.Create (
            nameof (RotatedDegree),
            typeof (double),
            typeof (MapView),
            0.0,
            BindingMode.TwoWay);

        public double RotatedDegree {
            get {
                return (double)GetValue (RotatedDegreeProperty);
            }
            set {
                SetValue (RotatedDegreeProperty, (double)value);
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

        public static readonly BindableProperty MapStyleProperty = BindableProperty.Create (
        nameof (MapStyle),
        typeof (MapStyle),
        typeof (MapView),
        default (MapStyle));
        public MapStyle MapStyle {
            get {
                return (MapStyle)GetValue (MapStyleProperty);
            }
            set {
                SetValue (MapStyleProperty, (MapStyle)value);
            }
        }

        public static readonly BindableProperty AnnotationsProperty = BindableProperty.Create (
            nameof (Annotations),
            typeof (IEnumerable<Annotation>),
            typeof (MapView),
            default (IEnumerable<Annotation>),
            BindingMode.TwoWay);

        public IEnumerable<Annotation> Annotations {
            get {
                return (IEnumerable<Annotation>)GetValue (AnnotationsProperty);
            }
            set {
                SetValue (AnnotationsProperty, (IEnumerable<Annotation>)value);
            }
        }

        static Func<string, bool> DefaultCanShowCalloutChecker = x => true;
        public static readonly BindableProperty CanShowCalloutCheckerProperty = BindableProperty.Create (
            nameof (CanShowCalloutChecker),
            typeof (Func<string, bool>),
            typeof (MapView),
            default (Func<string, bool>),
            BindingMode.OneWay);

        public Func<string, bool> CanShowCalloutChecker {
            get {
                return (Func<string, bool>)GetValue (CanShowCalloutCheckerProperty) ?? DefaultCanShowCalloutChecker;
            }
            set {
                SetValue (CanShowCalloutCheckerProperty, value);
            }
        }

        public static readonly BindableProperty TakeSnapshotProperty = BindableProperty.Create (
            nameof (TakeSnapshot),
            typeof (Func<byte []>),
            typeof (MapView),
            default (Func<byte []>),
            defaultBindingMode: BindingMode.OneWayToSource);

        public Func<byte []> TakeSnapshot {
            get {
                return (Func<byte []>)GetValue (TakeSnapshotProperty);
            }
            set {
                SetValue (TakeSnapshotProperty, value);
            }
        }

        public static readonly BindableProperty GetFeaturesAroundPointProperty = BindableProperty.Create (
            nameof (GetFeaturesAroundPoint),
            typeof (Func<Point, double, string [], IFeature []>),
            typeof (MapView),
            default (Func<Point, double, string [], IFeature []>),
            BindingMode.OneWayToSource);

        public Func<Point, double, string [], IFeature []> GetFeaturesAroundPoint {
            get {
                return ((Func<Point, double, string [], IFeature []>)GetValue (GetFeaturesAroundPointProperty));
            }
            set {
                SetValue (GetFeaturesAroundPointProperty, value);
            }
        }

        public static readonly BindableProperty ResetPositionFuncProperty = BindableProperty.Create (
            nameof (ResetPositionFunc),
            typeof (ICommand),
            typeof (MapView),
            default (ICommand),
            BindingMode.OneWayToSource);

        public ICommand ResetPositionFunc {
            get {
                return (ICommand)GetValue (ResetPositionFuncProperty);
            }
            set {
                SetValue (ResetPositionFuncProperty, value);
            }
        }

		public static readonly BindableProperty ReloadStyleFuncProperty = BindableProperty.Create(
			nameof(ReloadStyleFunc),
			typeof(ICommand),
			typeof(MapView),
			default(ICommand),
			BindingMode.OneWayToSource);

		public ICommand ReloadStyleFunc
		{
			get
			{
				return (ICommand)GetValue(ReloadStyleFuncProperty);
			}
			set
			{
				SetValue(ReloadStyleFuncProperty, value);
			}
		}


        public static readonly BindableProperty UpdateShapeOfSourceFuncProperty = BindableProperty.Create (
            nameof (UpdateShapeOfSourceFunc),
            typeof (Func<Annotation, string, bool>),
            typeof (MapView),
            default (Func<Annotation, string, bool>),
             BindingMode.OneWayToSource);

        public Func<Annotation, string, bool> UpdateShapeOfSourceFunc {
            get {
                return ((Func<Annotation, string, bool>)GetValue (UpdateShapeOfSourceFuncProperty));
            }
            set {
                SetValue (UpdateShapeOfSourceFuncProperty, value);
            }
        }

		/// <summary>
		/// The update layer func property.
        /// (layer Id, is visible, is custom layer)
		/// </summary>
		public static readonly BindableProperty UpdateLayerFuncProperty = BindableProperty.Create (
            nameof (UpdateLayerFunc),
            typeof (Func<string, bool, bool, bool>),
            typeof (MapView),
            default (Func<string, bool, bool, bool>),
             BindingMode.OneWayToSource);

        public Func<string, bool, bool, bool> UpdateLayerFunc {
            get {
                return ((Func<string, bool, bool, bool>)GetValue (UpdateLayerFuncProperty));
            }
            set {
                SetValue (UpdateLayerFuncProperty, value);
            }
        }
    }
}
