using System;
using Xamarin.Forms;

namespace Naxam.Mapbox.Layers
{
    public class CircleLayer : StyleLayer
    {
        public Color? CircleColor { get; set; }

        public double? CircleRadius { get; set; }

        public double? CircleBlur { get; set; }

        public double? CircleOpacity { get; set; }

        public Color? StrokeColor { get; set; }

        public double? StrokeWidth { get; set; }

        public double? StrokeOpacity { get; set; }

        public CircleLayer(string id, string sourceId) : base(id, sourceId)
        {
        }
    }
}
