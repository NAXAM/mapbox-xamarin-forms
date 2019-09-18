using Naxam.Mapbox.Expressions;

namespace Naxam.Mapbox.Layers
{
    public class HillshadeLayer : ForegroundLayer
    {
        public Expression HillshadeAccentColor { get; set; }
        public TransitionOptions HillshadeAccentColorTransition { get; set; }

        public Expression HillshadeExaggeration { get; set; }
        public TransitionOptions HillshadeExaggerationTransition { get; set; }

        public Expression HillshadeHighlightColor { get; set; }
        public TransitionOptions HillshadeHighlightColorTransition { get; set; }

        public Expression HillshadeIlluminationAnchor { get; set; }

        public Expression HillshadeIlluminationDirection { get; set; }

        public Expression HillshadeShadowColor { get; set; }
        public TransitionOptions HillshadeShadowColorTransition { get; set; }

        public HillshadeLayer(string id, string sourceId) : base(id, sourceId)
        {
        }
    }
}