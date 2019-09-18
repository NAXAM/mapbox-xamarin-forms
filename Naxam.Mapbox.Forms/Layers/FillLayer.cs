using Naxam.Mapbox.Expressions;

namespace Naxam.Mapbox.Layers
{
    public class FillLayer : StyleLayer
    {
        public Expression FillAntialiased { get; set; }

        public Expression FillColor { get; set; }
        public TransitionOptions FillColorTransition { get; set; }

        public Expression FillOpacity { get; set; }
        public TransitionOptions FillOpacityTransition { get; set; }

        public Expression FillOutlineColor { get; set; }
        public TransitionOptions FillOutlineColorTransition { get; set; }

        public Expression FillPattern { get; set; }
        public TransitionOptions FillPatternTransition { get; set; }

        public Expression FillTranslate { get; set; }
        public TransitionOptions FillTranslateTransition { get; set; }

        public Expression FillTranslateAnchor { get; set; }

        public FillLayer(string id, string sourceId) : base(id, sourceId)
        {
        }
    }
}
