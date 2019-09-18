using Naxam.Mapbox.Expressions;

namespace Naxam.Mapbox.Layers
{
    public class FillExtrusionLayer : StyleLayer
    {
        public Expression FillExtrusionBase { get; set; }

        public TransitionOptions FillExtrusionBaseTransition { get; set; }

        public Expression FillExtrusionColor { get; set; }
        public TransitionOptions FillExtrusionColorTransition { get; set; }

        public Expression FillExtrusionHasVerticalGradient { get; set; }

        public Expression FillExtrusionHeight { get; set; }
        public TransitionOptions FillExtrusionHeightTransition { get; set; }

        public Expression FillExtrusionOpacity { get; set; }
        public TransitionOptions FillExtrusionOpacityTransition { get; set; }

        public Expression FillExtrusionPattern { get; set; }
        public TransitionOptions FillExtrusionPatternTransition { get; set; }

        public Expression FillExtrusionTranslate { get; set; }
        public TransitionOptions FillExtrusionTranslateTransition { get; set; }

        public Expression FillExtrusionTranslateAnchor { get; set; }

        public FillExtrusionLayer(string id, string sourceId) : base(id, sourceId)
        {
        }
    }
}
