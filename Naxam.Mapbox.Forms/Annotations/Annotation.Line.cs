using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Naxam.Mapbox.Annotations
{
    public class LineAnnotation : Annotation
    {
        public IList<LatLng> Coordinates { get; private set; } = new List<LatLng>();

        public string LinePattern
        {
            get => Properties.TryGetValue(PROPERTY_LINE_PATTERN, out var val) && val is string ? ((string)val) : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Properties.Remove(PROPERTY_LINE_PATTERN);
                }
                else
                {
                    Properties[PROPERTY_LINE_PATTERN] = value;
                }
            }
        }

        public string LineJoin
        {
            get => Properties.TryGetValue(PROPERTY_LINE_JOIN, out var val) && val is string ? ((string)val) : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Properties.Remove(PROPERTY_LINE_JOIN);
                }
                else
                {
                    Properties[PROPERTY_LINE_JOIN] = value;
                }
            }
        }

        public Color? LineColor
        {
            get => Properties.TryGetValue(PROPERTY_LINE_COLOR, out var val) && val is Color ? ((Color?)val) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_LINE_COLOR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_LINE_COLOR);
                }
            }
        }

        public double? LineOpacity
        {
            get => Properties.TryGetValue(PROPERTY_LINE_OPACITY, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_LINE_OPACITY] = Math.Min(1.0, value.Value);
                }
                else
                {
                    Properties.Remove(PROPERTY_LINE_OPACITY);
                }
            }
        }

        public double? LineWidth
        {
            get => Properties.TryGetValue(PROPERTY_LINE_WIDTH, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_LINE_WIDTH] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_LINE_WIDTH);
                }
            }
        }

        public double? LineGapWidth
        {
            get => Properties.TryGetValue(PROPERTY_LINE_GAP_WIDTH, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_LINE_GAP_WIDTH] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_LINE_GAP_WIDTH);
                }
            }
        }

        public double? LineBlur
        {
            get => Properties.TryGetValue(PROPERTY_LINE_BLUR, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_LINE_BLUR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_LINE_BLUR);
                }
            }
        }

        public double? LineOffset
        {
            get => Properties.TryGetValue(PROPERTY_LINE_OFFSET, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_LINE_OFFSET] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_LINE_OFFSET);
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

        const string PROPERTY_LINE_JOIN = "line-join";
        const string PROPERTY_LINE_OPACITY = "line-opacity";
        const string PROPERTY_LINE_COLOR = "line-color";
        const string PROPERTY_LINE_WIDTH = "line-width";
        const string PROPERTY_LINE_GAP_WIDTH = "line-gap-width";
        const string PROPERTY_LINE_OFFSET = "line-offset";
        const string PROPERTY_LINE_BLUR = "line-blur";
        const string PROPERTY_LINE_PATTERN = "line-pattern";
        const string PROPERTY_IS_DRAGGABLE = "is-draggable";
    }
}
