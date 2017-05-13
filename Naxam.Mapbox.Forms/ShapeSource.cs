using System;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
    public class ShapeSource: BindableObject
    {
        public ShapeSource ()
        {
        }

        public ShapeSource (string id, Annotation shape)
        {
            Id = id;
            Shape = shape;
        }

        public string Id {
            get;
            set;
        }

        public static readonly BindableProperty ShapeProperty = BindableProperty.Create (
                    nameof (Shape),
                    typeof (Annotation),
                    typeof (ShapeSource),
                    default(Annotation),
                    BindingMode.TwoWay);

        public Annotation Shape {
            get {
                return (Annotation)GetValue (ShapeProperty);
            }
            set {
                SetValue (ShapeProperty, (Annotation)value);
            }
        }
    }
}
