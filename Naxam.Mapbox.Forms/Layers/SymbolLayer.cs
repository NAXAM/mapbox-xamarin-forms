using Naxam.Mapbox.Expressions;

namespace Naxam.Mapbox.Layers
{
    public class SymbolLayer : StyleLayer
    {
        public SymbolLayer(string id, string sourceId) : base(id, sourceId)
        {
        }

        public Expression IconImage { get; set; }

        public Expression IconSize { get; set; }

        public Expression IconColor { get; set; }
        public Expression TextField { get; set; }
        public Expression TextSize { get; set; }
        public Expression TextColor { get; set; }
        public Expression TextIgnorePlacement { get; set; }
        public Expression TextAllowOverlap { get; set; }
    }
}
