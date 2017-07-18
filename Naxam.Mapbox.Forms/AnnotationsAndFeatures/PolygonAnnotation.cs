using System.Collections.Generic;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
	public class PolygonAnnotation: Annotation
	{
		public PolygonAnnotation()
		{
		}

        public static readonly BindableProperty CoordinatesProperty = BindableProperty.Create (
                    nameof (Coordinates),
                    typeof (IEnumerable<Position>),
                    typeof (PolylineAnnotation),
                    default(IEnumerable<Position>),
                    BindingMode.TwoWay);

        public IEnumerable<Position> Coordinates
        {
            get
            {
                return (IEnumerable<Position>)GetValue(CoordinatesProperty);
            }
            set
            {
                SetValue(CoordinatesProperty, (IEnumerable<Position>)value);
            }
        }
    }
}
