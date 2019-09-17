using Naxam.Mapbox.Expressions;
using System;
using Xamarin.Forms;

namespace Naxam.Mapbox.Layers
{
    public class CircleLayer : StyleLayer
    {
        public Expression CircleColor { get; set; }

        public Expression CircleRadius { get; set; }

        public Expression CircleBlur { get; set; }

        public Expression CircleOpacity { get; set; }

        public Expression StrokeColor { get; set; }

        public Expression StrokeWidth { get; set; }

        public Expression StrokeOpacity { get; set; }

        public CircleLayer(string id, string sourceId) : base(id, sourceId)
        {
        }
    }
}
