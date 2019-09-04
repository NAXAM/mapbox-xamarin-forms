using System;
using Xamarin.Forms;

namespace Naxam.Mapbox.Annotations
{
    public class SymbolAnnotation : Annotation
    {
        public LatLng Coordinates { get; set; }

        public double? IconOpacity
        {
            get => Properties.TryGetValue(PROPERTY_ICON_OPACITY, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_ICON_OPACITY] = Math.Min(1.0, value.Value);
                }
                else
                {
                    Properties.Remove(PROPERTY_ICON_OPACITY);
                }
            }
        }

        public double? IconSize
        {
            get => Properties.TryGetValue(PROPERTY_ICON_SIZE, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_ICON_SIZE] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_ICON_SIZE);
                }
            }
        }

        public double? IconHaloBlur
        {
            get => Properties.TryGetValue(PROPERTY_ICON_HALO_BLUR, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_ICON_HALO_BLUR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_ICON_HALO_BLUR);
                }
            }
        }

        public double? IconHaloWidth
        {
            get => Properties.TryGetValue(PROPERTY_ICON_HALO_WIDTH, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_ICON_HALO_WIDTH] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_ICON_HALO_WIDTH);
                }
            }
        }

        public string IconImage
        {
            get => Properties.TryGetValue(PROPERTY_ICON_IMAGE, out var val) && val is string ? ((string)val) : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Properties.Remove(PROPERTY_ICON_IMAGE);
                }
                else
                {
                    Properties[PROPERTY_ICON_IMAGE] = value;
                }
            }
        }

        public double? IconRotate
        {
            get => Properties.TryGetValue(PROPERTY_ICON_ROTATE, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_ICON_ROTATE] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_ICON_ROTATE);
                }
            }
        }

        public double[] IconOffset
        {
            get => Properties.TryGetValue(PROPERTY_ICON_OFFSET, out var val) && val is double[]? ((double[])val) : null;
            set
            {
                if (value?.Length > 0)
                {
                    Properties[PROPERTY_ICON_OFFSET] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_ICON_OFFSET);
                }
            }
        }

        public string IconAnchor
        {
            get => Properties.TryGetValue(PROPERTY_ICON_ANCHOR, out var val) && val is string ? ((string)val) : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Properties.Remove(PROPERTY_ICON_ANCHOR);
                }
                else
                {
                    Properties[PROPERTY_ICON_ANCHOR] = value;
                }
            }
        }

        public string TextField
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_FIELD, out var val) && val is string ? ((string)val) : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Properties.Remove(PROPERTY_TEXT_FIELD);
                }
                else
                {
                    Properties[PROPERTY_TEXT_FIELD] = value;
                }
            }
        }

        public string TextFont
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_FONT, out var val) && val is string ? ((string)val) : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Properties.Remove(PROPERTY_TEXT_FONT);
                }
                else
                {
                    Properties[PROPERTY_TEXT_FONT] = value;
                }
            }
        }

        public string TextJustify
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_JUSTIFY, out var val) && val is string ? ((string)val) : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Properties.Remove(PROPERTY_TEXT_JUSTIFY);
                }
                else
                {
                    Properties[PROPERTY_TEXT_JUSTIFY] = value;
                }
            }
        }

        public string TextAnchor
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_ANCHOR, out var val) && val is string ? ((string)val) : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Properties.Remove(PROPERTY_TEXT_ANCHOR);
                }
                else
                {
                    Properties[PROPERTY_TEXT_ANCHOR] = value;
                }
            }
        }

        public string TextTransform
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_TRANSFORM, out var val) && val is string ? ((string)val) : null;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Properties.Remove(PROPERTY_TEXT_TRANSFORM);
                }
                else
                {
                    Properties[PROPERTY_TEXT_TRANSFORM] = value;
                }
            }
        }

        public Color? TextColor
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_COLOR, out var val) && val is Color ? ((Color?)val) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_TEXT_COLOR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_COLOR);
                }
            }
        }

        public Color? TextHaloColor
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_HALO_COLOR, out var val) && val is Color ? ((Color?)val) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_TEXT_HALO_COLOR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_HALO_COLOR);
                }
            }
        }

        public Color? IconColor
        {
            get => Properties.TryGetValue(PROPERTY_ICON_COLOR, out var val) && val is Color ? ((Color?)val) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_ICON_COLOR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_ICON_COLOR);
                }
            }
        }

        public Color? IconHaloColor
        {
            get => Properties.TryGetValue(PROPERTY_ICON_HALO_COLOR, out var val) && val is Color ? ((Color?)val) : null;
            set
            {
                if (value.HasValue)
                {
                    Properties[PROPERTY_ICON_HALO_COLOR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_ICON_HALO_COLOR);
                }
            }
        }

        public double? TextSize
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_SIZE, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_TEXT_SIZE] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_SIZE);
                }
            }
        }

        public double? TextMaxWidth
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_MAX_WIDTH, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_TEXT_MAX_WIDTH] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_MAX_WIDTH);
                }
            }
        }

        public double? TextLetterSpacing
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_LETTER_SPACING, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_TEXT_LETTER_SPACING] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_LETTER_SPACING);
                }
            }
        }

        public double? TextRadialOffset
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_RADIAL_OFFSET, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_TEXT_RADIAL_OFFSET] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_RADIAL_OFFSET);
                }
            }
        }

        public double? TextRotate
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_ROTATE, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_TEXT_ROTATE] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_ROTATE);
                }
            }
        }

        public double[] TextOffset
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_OFFSET, out var val) && val is double[]? ((double[])val) : null;
            set
            {
                if (value?.Length > 0)
                {
                    Properties[PROPERTY_TEXT_OFFSET] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_OFFSET);
                }
            }
        }

        public double? TextOpacity
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_OPACITY, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_TEXT_OPACITY] = Math.Min(1.0, value.Value);
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_OPACITY);
                }
            }
        }

        public double? TextHaloBlur
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_HALO_BLUR, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_TEXT_HALO_BLUR] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_HALO_BLUR);
                }
            }
        }

        public double? TextHaloWidth
        {
            get => Properties.TryGetValue(PROPERTY_TEXT_HALO_WIDTH, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_TEXT_HALO_WIDTH] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_TEXT_HALO_WIDTH);
                }
            }
        }

        public double? SymbolSortKey
        {
            get => Properties.TryGetValue(PROPERTY_SYMBOL_SORT_KEY, out var val) && val is double ? ((double?)val) : null;
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    Properties[PROPERTY_SYMBOL_SORT_KEY] = value;
                }
                else
                {
                    Properties.Remove(PROPERTY_SYMBOL_SORT_KEY);
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

        const string PROPERTY_SYMBOL_SORT_KEY = "symbol-sort-key";
        const string PROPERTY_ICON_SIZE = "icon-size";
        const string PROPERTY_ICON_IMAGE = "icon-image";
        const string PROPERTY_ICON_ROTATE = "icon-rotate";
        const string PROPERTY_ICON_OFFSET = "icon-offset";
        const string PROPERTY_ICON_ANCHOR = "icon-anchor";
        const string PROPERTY_TEXT_FIELD = "text-field";
        const string PROPERTY_TEXT_FONT = "text-font";
        const string PROPERTY_TEXT_SIZE = "text-size";
        const string PROPERTY_TEXT_MAX_WIDTH = "text-max-width";
        const string PROPERTY_TEXT_LETTER_SPACING = "text-letter-spacing";
        const string PROPERTY_TEXT_JUSTIFY = "text-justify";
        const string PROPERTY_TEXT_RADIAL_OFFSET = "text-radial-offset";
        const string PROPERTY_TEXT_ANCHOR = "text-anchor";
        const string PROPERTY_TEXT_ROTATE = "text-rotate";
        const string PROPERTY_TEXT_TRANSFORM = "text-transform";
        const string PROPERTY_TEXT_OFFSET = "text-offset";
        const string PROPERTY_ICON_OPACITY = "icon-opacity";
        const string PROPERTY_ICON_COLOR = "icon-color";
        const string PROPERTY_ICON_HALO_COLOR = "icon-halo-color";
        const string PROPERTY_ICON_HALO_WIDTH = "icon-halo-width";
        const string PROPERTY_ICON_HALO_BLUR = "icon-halo-blur";
        const string PROPERTY_TEXT_OPACITY = "text-opacity";
        const string PROPERTY_TEXT_COLOR = "text-color";
        const string PROPERTY_TEXT_HALO_COLOR = "text-halo-color";
        const string PROPERTY_TEXT_HALO_WIDTH = "text-halo-width";
        const string PROPERTY_TEXT_HALO_BLUR = "text-halo-blur";
        const string PROPERTY_IS_DRAGGABLE = "is-draggable";
    }
}
