
using Naxam.Mapbox.Forms.AnnotationsAndFeatures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naxam.Controls.Mapbox.Forms
{
    public class PositionChangeEventArgs : EventArgs
    {
        //private Position _newPosition;

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

    public partial class MapView : View
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

        public static readonly BindableProperty DragFinishedCommandProperty = BindableProperty.Create(
            nameof(DragFinishedCommand),
            typeof(ICommand),
            typeof(MapView),
            default(ICommand),
            BindingMode.OneWay);
        public ICommand DragFinishedCommand
        {
            get { return (ICommand)GetValue(DragFinishedCommandProperty); }
            set { SetValue(DragFinishedCommandProperty, value); }
        }

        public static readonly BindableProperty FocusPositionProperty = BindableProperty.Create(
            nameof(FocusPosition),
           typeof(bool),
           typeof(MapView),
           default(bool),
            BindingMode.OneWay);


        public bool FocusPosition
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

        public static BindableProperty ShowUserLocationProperty = BindableProperty.Create(
            propertyName: nameof(ShowUserLocation),
            returnType: typeof(bool),
            declaringType: typeof(MapView),
            defaultValue: default(bool),
            defaultBindingMode: BindingMode.OneWay
        );
        
        public bool ShowUserLocation
        {
            get { return (bool)GetValue(ShowUserLocationProperty); }
            set { SetValue(ShowUserLocationProperty, value); }
        }
        
        public static readonly BindableProperty ZoomLevelProperty = BindableProperty.Create(
            nameof(ZoomLevel),
            typeof(double),
            typeof(MapView),
            10.0,
            BindingMode.TwoWay);

        public double ZoomLevel
        {
            get
            {
                return (double)GetValue(ZoomLevelProperty);
            }
            set
            {
                if (Math.Abs(value - ZoomLevel) > 0.01)
                {
                    SetValue(ZoomLevelProperty, (double)value);
                }
            }
        }

        public static readonly BindableProperty PitchProperty = BindableProperty.Create(
            nameof(Pitch),
            typeof(double),
            typeof(MapView),
            0.0,
            BindingMode.TwoWay);

        public double Pitch
        {
            get
            {
                return (double)GetValue(PitchProperty);
            }
            set
            {
                SetValue(PitchProperty, (double)value);
            }
        }

        public static readonly BindableProperty PitchEnabledProperty = BindableProperty.Create(
            nameof(PitchEnabled),
            typeof(bool),
            typeof(MapView),
            default(bool),
            BindingMode.TwoWay);

        public bool PitchEnabled
        {
            get
            {
                return (bool)GetValue(PitchEnabledProperty);
            }
            set
            {
                SetValue(PitchEnabledProperty, value);
            }
        }

        public static readonly BindableProperty ScrollEnabledProperty = BindableProperty.Create(
            nameof(ScrollEnabled),
            typeof(bool),
            typeof(MapView),
            default(bool),
            BindingMode.OneWay);
        public bool ScrollEnabled
        {
            get { return (bool)GetValue(ScrollEnabledProperty); }
            set { SetValue(ScrollEnabledProperty, value); }
        }

        public static readonly BindableProperty RotateEnabledProperty = BindableProperty.Create(
            nameof(RotateEnabled),
            typeof(bool),
            typeof(MapView),
            default(bool),
            BindingMode.TwoWay);

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

        public static readonly BindableProperty RotatedDegreeProperty = BindableProperty.Create(
            nameof(RotatedDegree),
            typeof(double),
            typeof(MapView),
            0.0,
            BindingMode.TwoWay);

        public double RotatedDegree
        {
            get
            {
                return (double)GetValue(RotatedDegreeProperty);
            }
            set
            {
                SetValue(RotatedDegreeProperty, (double)value);
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
        default(MapStyle),
            defaultBindingMode: BindingMode.TwoWay);
        public MapStyle MapStyle
        {
            get
            {
                return (MapStyle)GetValue(MapStyleProperty);
            }
            set
            {
                SetValue(MapStyleProperty, value);
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

        public static BindableProperty MarkersProperty = BindableProperty.Create(
           propertyName: nameof(Markers),
           returnType: typeof(IEnumerable<PointAnnotation>),
           declaringType: typeof(MapView),
           defaultValue: default(IEnumerable<PointAnnotation>),
           defaultBindingMode: BindingMode.OneWay
           );
        public IEnumerable<PointAnnotation> Markers
        {
            get => (IEnumerable<PointAnnotation>)GetValue(MarkersProperty);
            set => SetValue(MarkersProperty, value);
        }

        public static BindableProperty PolylinesProperty = BindableProperty.Create(
           propertyName: nameof(Polylines),
           returnType: typeof(IEnumerable<PolylineAnnotation>),
           declaringType: typeof(MapView),
           defaultValue: default(IEnumerable<PolylineAnnotation>),
           defaultBindingMode: BindingMode.OneWay
           );
        public IEnumerable<PolylineAnnotation> Polylines
        {
            get => (IEnumerable<PolylineAnnotation>)GetValue(PolylinesProperty);
            set => SetValue(PolylinesProperty, value);
        }


        public static BindableProperty RegionProperty = BindableProperty.Create(
           propertyName: nameof(Region),
           returnType: typeof(MapRegion),
           declaringType: typeof(MapView),
           defaultValue: MapRegion.Empty,
           defaultBindingMode: BindingMode.TwoWay
           );
        public MapRegion Region
        {
            get => (MapRegion)GetValue(RegionProperty);
            set => SetValue(RegionProperty, value);
        }

        public static BindableProperty InfoWindowTemplateProperty = BindableProperty.Create(
            propertyName: nameof(InfoWindowTemplate),
            returnType: typeof(DataTemplate),
            declaringType: typeof(MapView),
            defaultValue: default(DataTemplate),
            defaultBindingMode: BindingMode.OneWay
        );
        public DataTemplate InfoWindowTemplate
        {
            get { return (DataTemplate)GetValue(InfoWindowTemplateProperty); }
            set { SetValue(InfoWindowTemplateProperty, value); }
        }
        
        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(
            propertyName: nameof(ItemsSource),
            returnType: typeof(IEnumerable),
            declaringType: typeof(MapView),
            defaultValue: default(IEnumerable),
            defaultBindingMode: BindingMode.OneWay
        );

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
    }
}
