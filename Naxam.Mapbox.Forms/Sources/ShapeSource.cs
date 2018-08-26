using System;
using Xamarin.Forms;

namespace Naxam.Controls.Mapbox.Forms
{
    public class ShapeSource : MapSource
    {
        public ShapeSource()
        {
        }

        public ShapeSource(string id, Annotation shape) : base(id)
        {
            Shape = shape;
        }

        public static readonly BindableProperty ShapeProperty = BindableProperty.Create(
                    nameof(Shape),
                    typeof(Annotation),
                    typeof(ShapeSource),
                    default(Annotation),
                    BindingMode.TwoWay);

        public Annotation Shape
        {
            get => (Annotation)GetValue(ShapeProperty);
            set => SetValue(ShapeProperty, value);
        }
    }
}
