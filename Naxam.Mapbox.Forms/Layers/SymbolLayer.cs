using Naxam.Mapbox.Expressions;

namespace Naxam.Mapbox.Layers
{
    public class SymbolLayer : StyleLayer
    {
        public Expression IconAllowOverlap { get; set; }
        public Expression IconAnchor { get; set; }

        public Expression IconColor { get; set; }
        public TransitionOptions IconColorTransition { get; set; }

        public Expression IconHaloBlur { get; set; }
        public TransitionOptions IconHaloBlurTransition { get; set; }

        public Expression IconHaloColor { get; set; }
        public TransitionOptions IconHaloColorTransition { get; set; }

        public Expression IconHaloWidth { get; set; }
        public TransitionOptions IconHaloWidthTransition { get; set; }

        public Expression IconIgnorePlacement { get; set; }
        public Expression IconImage { get; set; }
        public Expression IconKeepUpright { get; set; }
        public Expression IconOffset { get; set; }

        public Expression IconOpacity { get; set; }
        public TransitionOptions IconOpacityTransition { get; set; }

        public Expression IconOptional { get; set; }
        public Expression IconPadding { get; set; }
        public Expression IconPitchAlignment { get; set; }
        public Expression IconRotate { get; set; }
        public Expression IconRotationAlignment { get; set; }
        public Expression IconSize { get; set; }
        public Expression IconTextFit { get; set; }
        public Expression IconTextFitPadding { get; set; }
        public Expression IconTranslate { get; set; }
        public Expression IconTranslateAnchor { get; set; }
        public TransitionOptions IconTranslateTransition { get; set; }

        public Expression SymbolAvoidEdges { get; set; }
        public Expression SymbolPlacement { get; set; }
        public Expression SymbolSortKey { get; set; }
        public Expression SymbolSpacing { get; set; }
        public Expression SymbolZOrder { get; set; }

        public Expression TextAllowOverlap { get; set; }
        public Expression TextAnchor { get; set; }

        public Expression TextColor { get; set; }
        public TransitionOptions TextColorTransition { get; set; }

        public Expression TextField { get; set; }
        public Expression TextFont { get; set; }

        public Expression TextHaloBlur { get; set; }
        public TransitionOptions TextHaloBlurTransition { get; set; }

        public Expression TextHaloColor { get; set; }
        public TransitionOptions TextHaloColorTransition { get; set; }

        public Expression TextHaloWidth { get; set; }
        public TransitionOptions TextHaloWidthTransition { get; set; }

        public Expression TextIgnorePlacement { get; set; }
        public Expression TextJustify { get; set; }
        public Expression TextKeepUpright { get; set; }
        public Expression TextLetterSpacing { get; set; }
        public Expression TextLineHeight { get; set; }
        public Expression TextMaxAngle { get; set; }
        public Expression TextMaxWidth { get; set; }
        public Expression TextOffset { get; set; }
        public Expression TextOpacity { get; set; }
        public TransitionOptions TextOpacityTransition { get; set; }
        public Expression TextOptional { get; set; }
        public Expression TextPadding { get; set; }
        public Expression TextPitchAlignment { get; set; }
        public Expression TextRadialOffset { get; set; }
        public Expression TextRotate { get; set; }
        public Expression TextRotationAlignment { get; set; }
        public Expression TextSize { get; set; }
        public Expression TextTransform { get; set; }
        public Expression TextTranslate { get; set; }
        public Expression TextTranslateAnchor { get; set; }
        public TransitionOptions TextTranslateTransition { get; set; }
        public Expression TextVariableAnchor { get; set; }
        public Expression TextWritingMode { get; set; }

        public SymbolLayer(string id, string sourceId) : base(id, sourceId)
        {
        }
    }
}
