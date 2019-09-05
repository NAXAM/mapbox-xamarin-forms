using System;
using System.Collections.Generic;
using Naxam.Controls;
using Xamarin.Forms;

namespace Naxam.Mapbox.Annotations
{
    public class FillAnnotation : Annotation
    {
        public IList<IList<LatLng>> Coordinates { get; private set; } = new List<IList<LatLng>>();

        public string FillPattern
        {
            get => Properties.TryGetValue(PROPERTY_FILL_PATTERN, out var val) && val is string ? ((string)val) : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Properties.Remove(PROPERTY_FILL_PATTERN);
                }
                else
                {
                    Properties[PROPERTY_FILL_PATTERN] = value;
                }
            }
        }

        public Color? FillColor
        {
            get => Properties.TryGetValue(PROPERTY_FILL_COLOR, out var val) && val is string sVal ? ((Color?)Color.FromHex(sVal)) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_FILL_COLOR] = value.Value.ToHex();
                }
                else
                {
                    Properties.Remove(PROPERTY_FILL_COLOR);
                }
            }
        }

        public Color? OutlineColor
        {
            get => Properties.TryGetValue(PROPERTY_FILL_OUTLINE_COLOR, out var val) && val is string sVal ? ((Color?)Color.FromHex(sVal)) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_FILL_OUTLINE_COLOR] = value.Value.ToHex();
                }
                else
                {
                    Properties.Remove(PROPERTY_FILL_OUTLINE_COLOR);
                }
            }
        }

        public double? FillOpacity
        {
            get => Properties.TryGetValue(PROPERTY_FILL_OPACITY, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_FILL_OPACITY] = Math.Min(1.0, value.Value);
                }
                else
                {
                    Properties.Remove(PROPERTY_FILL_OPACITY);
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
        const string PROPERTY_FILL_OPACITY = "fill-opacity";
        const string PROPERTY_FILL_COLOR = "fill-color";
        const string PROPERTY_FILL_OUTLINE_COLOR = "fill-outline-color";
        const string PROPERTY_FILL_PATTERN = "fill-pattern";
        const string PROPERTY_IS_DRAGGABLE = "is-draggable";
    }
}
