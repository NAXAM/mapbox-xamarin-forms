using System;
using System.Globalization;
using Xamarin.Forms;

namespace Naxam.Mapbox.Annotations
{
    public class CircleAnnotation : Annotation
    {
        public LatLng Coordinates { get; set; }

        public double? Radius
        {
            get => Properties.TryGetValue(PROPERTY_CIRCLE_RADIUS, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_CIRCLE_RADIUS] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_CIRCLE_RADIUS);
                }
            }
        }

        public Color? Color
        {
            get => Properties.TryGetValue(PROPERTY_CIRCLE_COLOR, out var val) && val is Color ? ((Color?)val) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_CIRCLE_COLOR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_CIRCLE_COLOR);
                }
            }
        }

        public double? Blur
        {
            get => Properties.TryGetValue(PROPERTY_CIRCLE_BLUR, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_CIRCLE_BLUR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_CIRCLE_BLUR);
                }
            }
        }

        public double? Opacity
        {
            get => Properties.TryGetValue(PROPERTY_CIRCLE_OPACITY, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_CIRCLE_OPACITY] = Math.Min(1.0, value.Value);
                }
                else
                {
                    Properties.Remove(PROPERTY_CIRCLE_OPACITY);
                }
            }
        }

        public double? StrokeWidth
        {
            get => Properties.TryGetValue(PROPERTY_CIRCLE_STROKE_WIDTH, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_CIRCLE_STROKE_WIDTH] = Math.Max(0.0, value.Value);
                }
                else
                {
                    Properties.Remove(PROPERTY_CIRCLE_STROKE_WIDTH);
                }
            }
        }

        public Color? StrokeColor
        {
            get => Properties.TryGetValue(PROPERTY_CIRCLE_STROKE_COLOR, out var val) && val is Color ? ((Color?)val) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_CIRCLE_STROKE_COLOR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_CIRCLE_STROKE_COLOR);
                }
            }
        }

        public double? StrokeOpacity
        {
            get => Properties.TryGetValue(PROPERTY_CIRCLE_STROKE_OPACITY, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_CIRCLE_STROKE_OPACITY] = Math.Min(1.0, value.Value);
                }
                else
                {
                    Properties.Remove(PROPERTY_CIRCLE_STROKE_OPACITY);
                }
            }
        }

        public bool? IsDraggable
        {
            get => Properties.TryGetValue(PROPERTY_IS_DRAGGABLE, out var val) && val is bool ? ((bool?)val) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_IS_DRAGGABLE] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_IS_DRAGGABLE);
                }
            }
        }

        const string PROPERTY_CIRCLE_RADIUS = "circle-radius";
        const string PROPERTY_CIRCLE_COLOR = "circle-color";
        const string PROPERTY_CIRCLE_BLUR = "circle-blur";
        const string PROPERTY_CIRCLE_OPACITY = "circle-opacity";
        const string PROPERTY_CIRCLE_STROKE_WIDTH = "circle-stroke-width";
        const string PROPERTY_CIRCLE_STROKE_COLOR = "circle-stroke-color";
        const string PROPERTY_CIRCLE_STROKE_OPACITY = "circle-stroke-opacity";
        const string PROPERTY_IS_DRAGGABLE = "is-draggable";
    }
}