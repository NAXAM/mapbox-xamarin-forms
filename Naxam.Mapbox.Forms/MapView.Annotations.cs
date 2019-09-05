using Naxam.Mapbox.Annotations;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
    public partial class MapView : View
    {
        public event EventHandler<AnnotationChangedEventArgs> AnnotationsChanged;

        public static readonly BindableProperty AnnotationsProperty = BindableProperty.Create(
            nameof(Annotations),
            typeof(IEnumerable<Annotation>),
            typeof(MapView),
            default(IEnumerable<Annotation>),
            BindingMode.TwoWay,
            propertyChanged: OnAnnotationsChanged
        );
        static void OnAnnotationsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MapView control)
            {
                control.OnAnnotationsChanged((IEnumerable<Annotation>)oldValue, (IEnumerable<Annotation>)newValue);
            }
        }

        public IEnumerable<Annotation> Annotations
        {
            get => (IEnumerable<Annotation>)GetValue(AnnotationsProperty);
            set => SetValue(AnnotationsProperty, value);
        }

        void OnAnnotationsChanged(IEnumerable<Annotation> oldAnnotation, IEnumerable<Annotation> newAnnotation)
        {
            AnnotationsChanged?.Invoke(this, new AnnotationChangedEventArgs
            {
                OldAnnotations = oldAnnotation,
                NewAnnotations = newAnnotation
            });
        }
    }
}
