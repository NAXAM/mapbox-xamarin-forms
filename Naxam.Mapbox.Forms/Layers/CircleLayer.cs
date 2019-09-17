using Naxam.Mapbox.Expressions;

namespace Naxam.Mapbox.Layers
{
    public class CircleLayer : StyleLayer
    {
        public Expression CircleColor { get; set; }

        public Expression CircleRadius { get; set; }

        public Expression CircleBlur { get; set; }

        public Expression CircleOpacity { get; set; }

        public Expression CircleStrokeColor { get; set; }

        public Expression CircleStrokeWidth { get; set; }

        public Expression CircleStrokeOpacity { get; set; }

        public CircleLayer(string id, string sourceId) : base(id, sourceId) { }
    }
}
