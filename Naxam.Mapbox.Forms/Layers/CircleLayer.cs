using Naxam.Mapbox.Expressions;

namespace Naxam.Mapbox.Layers
{
    public class CircleLayer : StyleLayer
    {
        public Expression CircleBlur { get; set; }

        public TransitionOptions CircleBlurTransition { get; set; }

        public Expression CircleColor { get; set; }

        public TransitionOptions CircleColorTransition { get; set; }

        public Expression CircleOpacity { get; set; }

        public TransitionOptions CircleOpacityTransition { get; set; }

        public Expression CirclePitchAlignment { get; set; }

        public Expression CirclePitchScale { get; set; }

        public Expression CircleRadius { get; set; }

        public TransitionOptions CircleRadiusTransition { get; set; }

        public Expression CircleStrokeColor { get; set; }

        public TransitionOptions CircleStrokeColorTransition { get; set; }

        public Expression CircleStrokeOpacity { get; set; }

        public TransitionOptions CircleStrokeOpacityTransition { get; set; }
        
        public Expression CircleStrokeWidth { get; set; }

        public TransitionOptions CircleStrokeWidthTransition { get; set; }

        public Expression CircleTranslate { get; set; }

        public TransitionOptions CircleTranslateTransition { get; set; }

        public Expression CircleTranslateAnchor { get; set; }

        public CircleLayer(string id, string sourceId) : base(id, sourceId) { }
    }
}
