using Naxam.Mapbox.Expressions;

namespace Naxam.Mapbox.Layers
{
    public class SymbolLayer : StyleLayer
    {
        public SymbolLayer(string id, string sourceId) : base(id, sourceId)
        {
        }

        public Expression FilterExpression { get; set; }

        public IconImageSource IconImageName { get; set; }

        public bool? IconAllowOverlap { get; set; }

        public float[] IconOffset { get; set; }

        public double? IconOpacity { get; set; }
    }
}
