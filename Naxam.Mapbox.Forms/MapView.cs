
using Naxam.Mapbox;
using Naxam.Mapbox.Annotations;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
    public partial class MapView : View
    {
        public static readonly BindableProperty UserLocationProperty = BindableProperty.Create(
            nameof(UserLocation),
            typeof(LatLng),
            typeof(MapView),
            default(LatLng),
            BindingMode.OneWayToSource);
        public LatLng UserLocation
        {
            get => (LatLng)GetValue(UserLocationProperty);
            set => SetValue(UserLocationProperty, value);
        }

        public static BindableProperty ShowUserLocationProperty = BindableProperty.Create(
            nameof(ShowUserLocation),
            typeof(bool),
            typeof(MapView),
            default(bool),
            BindingMode.OneWay
        );
        public bool ShowUserLocation
        {
            get { return (bool)GetValue(ShowUserLocationProperty); }
            set { SetValue(ShowUserLocationProperty, value); }
        }

        
        public static readonly BindableProperty RotatedDegreeProperty = BindableProperty.Create(
            nameof(RotatedDegree),
            typeof(double),
            typeof(MapView),
            0.0,
            BindingMode.TwoWay);
        public double RotatedDegree
        {
            get => (double)GetValue(RotatedDegreeProperty);
            set => SetValue(RotatedDegreeProperty, value);
        }

        public static readonly BindableProperty MapStyleProperty = BindableProperty.Create(
            nameof(MapStyle),
            typeof(MapStyle),
            typeof(MapView),
            default(MapStyle),
            BindingMode.TwoWay);
        public MapStyle MapStyle
        {
            get => (MapStyle)GetValue(MapStyleProperty);
            set => SetValue(MapStyleProperty, value);
        }

        public static BindableProperty VisibleBoundsProperty = BindableProperty.Create(
           nameof(VisibleBounds),
           typeof(LatLngBounds),
           typeof(MapView),
           default,
           BindingMode.TwoWay);
        public LatLngBounds VisibleBounds
        {
            get => (LatLngBounds)GetValue(VisibleBoundsProperty);
            set => SetValue(VisibleBoundsProperty, value);
        }

        public static BindableProperty InfoWindowTemplateProperty = BindableProperty.Create(
            nameof(InfoWindowTemplate),
            typeof(DataTemplate),
            typeof(MapView),
            default(DataTemplate),
            BindingMode.OneWay
        );
        public DataTemplate InfoWindowTemplate
        {
            get { return (DataTemplate)GetValue(InfoWindowTemplateProperty); }
            set { SetValue(InfoWindowTemplateProperty, value); }
        }

        public static readonly BindableProperty FunctionsProperty = BindableProperty.Create(
            nameof(Functions),
            typeof(IMapFunctions),
            typeof(MapView),
            default(IMapFunctions),
            BindingMode.OneWayToSource);
        public IMapFunctions Functions
        {
            get => (IMapFunctions)GetValue(FunctionsProperty);
            set => SetValue(FunctionsProperty, value);
        }
    }

    public class AnnotationChangedEventArgs : EventArgs
    {
        public IEnumerable<Annotation> OldAnnotations { get; set; }
        public IEnumerable<Annotation> NewAnnotations { get; set; }
    }

}
