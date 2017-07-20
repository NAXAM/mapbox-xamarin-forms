using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
	public class PolylineAnnotation: Annotation
	{
		public PolylineAnnotation()
		{
		}

        public static readonly BindableProperty CoordinatesProperty = BindableProperty.Create (
                    nameof (Coordinates),
                    typeof (ObservableCollection<Position>),
                    typeof (PolylineAnnotation),
                    new ObservableCollection<Position>(),
                    BindingMode.TwoWay);

        public ObservableCollection<Position> Coordinates
        {
            get
            {
                return (ObservableCollection<Position>)GetValue(CoordinatesProperty);
            }
            set
            {
                SetValue(CoordinatesProperty, (ObservableCollection<Position>)value);
            }
        }

        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
            nameof(StrokeWidth),
            typeof(double),
            typeof(PolylineAnnotation),
            1.0,
            BindingMode.TwoWay);

        public double StrokeWidth
        {
            get
            {
                return (double)GetValue(StrokeWidthProperty);
            }
            set
            {
                SetValue(StrokeWidthProperty, (double)value);
            }
        }

        public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(
            nameof(StrokeColor),
            typeof(Color),
            typeof(PolylineAnnotation),
            default(Color),
            BindingMode.TwoWay);

        public Color StrokeColor
        {
            get
            {
                return (Color)GetValue(StrokeColorProperty);
            }
            set
            {
                SetValue(StrokeColorProperty, (Color)value);
            }
        }

        public static readonly BindableProperty AlphaProperty = BindableProperty.Create(
            nameof(Alpha),
            typeof(double),
            typeof(PolylineAnnotation),
            default(double),
            BindingMode.TwoWay);

        public double Alpha
        {
            get
            {
                return (double)GetValue(AlphaProperty);
            }
            set
            {
                SetValue(AlphaProperty, (double)value);
            }
        }
    }
}
