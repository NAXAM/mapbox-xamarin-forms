using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
	public class PolygonAnnotation: PolylineAnnotation
	{
		public PolygonAnnotation()
		{
		}

        public static readonly BindableProperty FillColorProperty = BindableProperty.Create(
            nameof(FillColor),
            typeof(Color),
            typeof(PolylineAnnotation),
            default(Color),
            BindingMode.TwoWay);

        public Color FillColor
        {
            get
            {
                return (Color)GetValue(FillColorProperty);
            }
            set
            {
                SetValue(FillColorProperty, (Color)value);
            }
        }
    }
}
