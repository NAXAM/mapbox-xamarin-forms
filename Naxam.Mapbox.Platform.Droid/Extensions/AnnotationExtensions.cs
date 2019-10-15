using Com.Mapbox.Mapboxsdk.Plugins.Annotation;
using Naxam.Mapbox.Annotations;
using Java.Lang;
using System.Linq;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public static class AnnotationExtensions
    {
        public static SymbolOptions ToSymbolOptions(this SymbolAnnotation symbolAnnotation)
        {
            var symbolOptions = new SymbolOptions()
                                .WithLatLng(symbolAnnotation.Coordinates.ToLatLng());
            if (!string.IsNullOrWhiteSpace(symbolAnnotation.IconAnchor))
            {
                symbolOptions.WithIconImage(symbolAnnotation.IconAnchor);
            }
            if (symbolAnnotation.IconColor.HasValue)
            {
                symbolOptions.WithIconColor(symbolAnnotation.IconColor.Value.ToRGBAString());
            }
            if (symbolAnnotation.IconHaloBlur.HasValue)
            {
                symbolOptions.WithIconHaloBlur(new Float(symbolAnnotation.IconHaloBlur.Value));
            }
            if (symbolAnnotation.IconHaloColor.HasValue)
            {
                symbolOptions.WithIconHaloColor(symbolAnnotation.IconHaloColor.Value.ToRGBAString());
            }
            if (symbolAnnotation.IconHaloWidth.HasValue)
            {
                symbolOptions.WithIconHaloWidth(new Float(symbolAnnotation.IconHaloWidth.Value));
            }
            if (symbolAnnotation.IconImage != null)
            {
                symbolOptions.WithIconImage(symbolAnnotation.IconImage.Id);
            }
            if (symbolAnnotation.IconOffset?.Length > 0)
            {
                symbolOptions.WithIconOffset(symbolAnnotation.IconOffset.Select(x => new Float(x)).ToArray());
            }
            if (symbolAnnotation.IconOpacity.HasValue)
            {
                symbolOptions.WithIconOpacity(new Float(symbolAnnotation.IconOpacity.Value));
            }
            if (symbolAnnotation.IconRotate.HasValue)
            {
                symbolOptions.WithIconRotate(new Float(symbolAnnotation.IconRotate.Value));
            }
            if (symbolAnnotation.IconSize.HasValue)
            {
                symbolOptions.WithIconSize(new Float(symbolAnnotation.IconSize.Value));
            }
            if (symbolAnnotation.IsDraggable.HasValue)
            {
                symbolOptions.WithDraggable(symbolAnnotation.IsDraggable.Value);
            }
            if (symbolAnnotation.SymbolSortKey.HasValue)
            {
                symbolOptions.WithSymbolSortKey(new Float(symbolAnnotation.SymbolSortKey.Value));
            }
            if (!string.IsNullOrWhiteSpace(symbolAnnotation.TextAnchor))
            {
                symbolOptions.WithTextAnchor(symbolAnnotation.TextAnchor);
            }
            if (symbolAnnotation.TextColor.HasValue)
            {
                symbolOptions.WithTextColor(symbolAnnotation.TextColor.Value.ToRGBAString());
            }
            if (!string.IsNullOrWhiteSpace(symbolAnnotation.TextField))
            {
                symbolOptions.WithTextField(symbolAnnotation.TextField);
            }
            if (symbolAnnotation.TextFont?.Length > 0)
            {
                symbolOptions.WithTextFont(symbolAnnotation.TextFont);
            }
            if (symbolAnnotation.TextHaloBlur.HasValue)
            {
                symbolOptions.WithTextHaloBlur(new Float(symbolAnnotation.TextHaloBlur.Value));
            }
            if (symbolAnnotation.TextHaloColor.HasValue)
            {
                symbolOptions.WithTextHaloColor(symbolAnnotation.TextHaloColor.Value.ToRGBAString());
            }
            if (symbolAnnotation.TextHaloWidth.HasValue)
            {
                symbolOptions.WithTextHaloWidth(new Float(symbolAnnotation.TextHaloWidth.Value));
            }
            if (!string.IsNullOrWhiteSpace(symbolAnnotation.TextJustify))
            {
                symbolOptions.WithTextJustify(symbolAnnotation.TextJustify);
            }
            if (symbolAnnotation.TextLetterSpacing.HasValue)
            {
                symbolOptions.WithTextLetterSpacing(new Float(symbolAnnotation.TextLetterSpacing.Value));
            }
            if (symbolAnnotation.TextMaxWidth.HasValue)
            {
                symbolOptions.WithTextMaxWidth(new Float(symbolAnnotation.TextMaxWidth.Value));
            }
            if (symbolAnnotation.TextOffset?.Length > 0)
            {
                symbolOptions.WithTextOffset(symbolAnnotation.TextOffset.Select(x => new Float(x)).ToArray());
            }
            if (symbolAnnotation.TextOpacity.HasValue)
            {
                symbolOptions.WithTextOpacity(new Float(symbolAnnotation.TextOpacity.Value));
            }
            if (symbolAnnotation.TextRadialOffset.HasValue)
            {
                symbolOptions.WithTextRadialOffset(new Float(symbolAnnotation.TextRadialOffset.Value));
            }
            if (symbolAnnotation.TextRotate.HasValue)
            {
                symbolOptions.WithTextRotate(new Float(symbolAnnotation.TextRotate.Value));
            }
            if (symbolAnnotation.TextSize.HasValue)
            {
                symbolOptions.WithTextSize(new Float(symbolAnnotation.TextSize.Value));
            }
            if (!string.IsNullOrWhiteSpace(symbolAnnotation.TextTransform))
            {
                symbolOptions.WithTextTransform(symbolAnnotation.TextTransform);
            }

            return symbolOptions;
        }

        public static void Update(this Symbol symbol, SymbolAnnotation symbolAnnotation)
        {
            if (!string.IsNullOrWhiteSpace(symbolAnnotation.IconAnchor)) {
                symbol.IconImage = symbolAnnotation.IconAnchor;
            }
            if (symbolAnnotation.IconColor.HasValue) {
                symbol.IconColor = symbolAnnotation.IconColor.Value.ToRGBAString();
            }
            if (symbolAnnotation.IconHaloBlur.HasValue) {
                symbol.IconHaloBlur = new Float(symbolAnnotation.IconHaloBlur.Value);
            }
            if (symbolAnnotation.IconHaloColor.HasValue) {
                symbol.IconHaloColor = symbolAnnotation.IconHaloColor.Value.ToRGBAString();
            }
            if (symbolAnnotation.IconHaloWidth.HasValue) {
                symbol.IconHaloWidth = new Float(symbolAnnotation.IconHaloWidth.Value);
            }
            if (symbolAnnotation.IconImage != null) {
                symbol.IconImage = (symbolAnnotation.IconImage.Id);
            }
            if (symbolAnnotation.IconOffset?.Length > 0) {
                symbol.IconOffset = new Android.Graphics.PointF(
                    symbolAnnotation.IconOffset[0],
                    symbolAnnotation.IconOffset[1]);
            }
            if (symbolAnnotation.IconOpacity.HasValue) {
                symbol.IconOpacity = (new Float(symbolAnnotation.IconOpacity.Value));
            }
            if (symbolAnnotation.IconRotate.HasValue) {
                symbol.IconRotate = (new Float(symbolAnnotation.IconRotate.Value));
            }
            if (symbolAnnotation.IconSize.HasValue) {
                symbol.IconSize = (new Float(symbolAnnotation.IconSize.Value));
            }
            if (symbolAnnotation.IsDraggable.HasValue) {
                symbol.Draggable = (symbolAnnotation.IsDraggable.Value);
            }
            if (symbolAnnotation.SymbolSortKey.HasValue) {
                symbol.SymbolSortKey = (new Float(symbolAnnotation.SymbolSortKey.Value));
            }
            if (!string.IsNullOrWhiteSpace(symbolAnnotation.TextAnchor)) {
                symbol.TextAnchor = (symbolAnnotation.TextAnchor);
            }
            if (symbolAnnotation.TextColor.HasValue) {
                symbol.TextColor = (symbolAnnotation.TextColor.Value.ToRGBAString());
            }
            if (!string.IsNullOrWhiteSpace(symbolAnnotation.TextField)) {
                symbol.TextField = (symbolAnnotation.TextField);
            }
            if (symbolAnnotation.TextFont?.Length > 0) {
                symbol.SetTextFont(symbolAnnotation.TextFont);
            }
            if (symbolAnnotation.TextHaloBlur.HasValue) {
                symbol.TextHaloBlur = (new Float(symbolAnnotation.TextHaloBlur.Value));
            }
            if (symbolAnnotation.TextHaloColor.HasValue) {
                symbol.TextHaloColor = (symbolAnnotation.TextHaloColor.Value.ToRGBAString());
            }
            if (symbolAnnotation.TextHaloWidth.HasValue) {
                symbol.TextHaloWidth = (new Float(symbolAnnotation.TextHaloWidth.Value));
            }
            if (!string.IsNullOrWhiteSpace(symbolAnnotation.TextJustify)) {
                symbol.TextJustify = (symbolAnnotation.TextJustify);
            }
            if (symbolAnnotation.TextLetterSpacing.HasValue) {
                symbol.TextLetterSpacing = (new Float(symbolAnnotation.TextLetterSpacing.Value));
            }
            if (symbolAnnotation.TextMaxWidth.HasValue) {
                symbol.TextMaxWidth = (new Float(symbolAnnotation.TextMaxWidth.Value));
            }
            if (symbolAnnotation.TextOffset?.Length > 0) {
                symbol.TextOffset = new Android.Graphics.PointF(
                    symbolAnnotation.TextOffset[0],
                    symbolAnnotation.TextOffset[1]);
            }
            if (symbolAnnotation.TextOpacity.HasValue) {
                symbol.TextOpacity = (new Float(symbolAnnotation.TextOpacity.Value));
            }
            if (symbolAnnotation.TextRadialOffset.HasValue) {
                symbol.TextRadialOffset = (new Float(symbolAnnotation.TextRadialOffset.Value));
            }
            if (symbolAnnotation.TextRotate.HasValue) {
                symbol.TextRotate = (new Float(symbolAnnotation.TextRotate.Value));
            }
            if (symbolAnnotation.TextSize.HasValue) {
                symbol.TextSize = new Float(symbolAnnotation.TextSize.Value);
            }
            if (!string.IsNullOrWhiteSpace(symbolAnnotation.TextTransform)) {
                symbol.TextTransform = (symbolAnnotation.TextTransform);
            }
        }
    }

}
