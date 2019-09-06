using Xamarin.Forms;

namespace Naxam.Mapbox.Annotations
{
    public class IconImageSource
    {
        public string Id { set; get; }

        public ImageSource Source { get; set; }

        public static implicit operator IconImageSource(string id)
        {
            return new IconImageSource
            {
                Id = id,
            };
        }

        public static implicit operator IconImageSource(ImageSource source)
        {
            return new IconImageSource
            {
                Source = source,
                Id = source.Id.ToString()
            };
        }
    }

    public class SymbolAnnotation : Annotation
    {
        public LatLng Coordinates { get; set; }

        public double? IconOpacity { get; set; }

        public double? IconSize { get; set; }

        public double? IconHaloBlur { get; set; }

        public double? IconHaloWidth { get; set; }

        public IconImageSource IconImage { get; set; }

        public double? IconRotate { get; set; }

        public double[] IconOffset { get; set; }

        public string IconAnchor { get; set; }

        public string TextField { get; set; }

        public string[] TextFont { get; set; }

        public string TextJustify { get; set; }

        public string TextAnchor { get; set; }

        public string TextTransform { get; set; }

        public Color? TextColor { get; set; }

        public Color? TextHaloColor { get; set; }

        public Color? IconColor { get; set; }

        public Color? IconHaloColor { get; set; }

        public double? TextSize { get; set; }

        public double? TextMaxWidth { get; set; }

        public double? TextLetterSpacing { get; set; }

        public double? TextRadialOffset { get; set; }

        public double? TextRotate { get; set; }

        public double[] TextOffset { get; set; }

        public double? TextOpacity { get; set; }

        public double? TextHaloBlur { get; set; }

        public double? TextHaloWidth { get; set; }

        public double? SymbolSortKey { get; set; }

        public bool? IsDraggable { get; set; }
    }
}
