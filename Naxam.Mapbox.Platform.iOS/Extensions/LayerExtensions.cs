using Mapbox;
using Naxam.Mapbox.Layers;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class LayerExtensions
    {
        public static MGLStyleLayer ToLayer(this Layer layer, MGLSource source)
        {
            switch (layer)
            {
                case CircleLayer circleLayer:
                    return ToLayer(circleLayer, source);

                case FillExtrusionLayer fillExtrusionLayer:
                    return ToLayer(fillExtrusionLayer, source);

                case FillLayer fillLayer:
                    return ToLayer(fillLayer, source);

                case HillshadeLayer hillshadeLayer:
                    return ToLayer(hillshadeLayer, source);

                case ForegroundLayer foregroundLayer:
                    return ToLayer(foregroundLayer, source);

                case HeatmapLayer heatmapLayer:
                    return ToLayer(heatmapLayer, source);

                case LineLayer lineLayer:
                    return ToLayer(lineLayer, source);

                case RasterLayer rasterLayer:
                    return ToLayer(rasterLayer, source);

                case SymbolLayer symbolLayer:
                    return ToLayer(symbolLayer, source);
            }

            return null;
        }

        public static MGLStyleLayer UpdateLayer(this Layer layer, MGLStyleLayer target)
        {
            switch (layer)
            {
                case CircleLayer circleLayer:
                    return UpdateLayer(circleLayer, target as MGLCircleStyleLayer);

                case FillExtrusionLayer fillExtrusionLayer:
                    return UpdateLayer(fillExtrusionLayer, target as MGLFillExtrusionStyleLayer);

                case FillLayer fillLayer:
                    return UpdateLayer(fillLayer, target as MGLFillStyleLayer);

                case HillshadeLayer hillshadeLayer:
                    return UpdateLayer(hillshadeLayer, target as MGLHillshadeStyleLayer);

                //case ForegroundLayer foregroundLayer:
                //    return UpdateLayer(foregroundLayer, target as MGLForegroundStyleLayer);

                case HeatmapLayer heatmapLayer:
                    return UpdateLayer(heatmapLayer, target as MGLHeatmapStyleLayer);

                case LineLayer lineLayer:
                    return UpdateLayer(lineLayer, target as MGLCircleStyleLayer);

                case RasterLayer rasterLayer:
                    return UpdateLayer(rasterLayer, target as MGLRasterStyleLayer);

                case SymbolLayer symbolLayer:
                    return UpdateLayer(symbolLayer, target as MGLSymbolStyleLayer);
            }

            return null;
        }

        static MGLSymbolStyleLayer ToLayer(SymbolLayer layer, MGLSource source)
        {
            var result = new MGLSymbolStyleLayer(layer.Id, source)
            {
                MaximumZoomLevel = layer.MaxZoom,
                MinimumZoomLevel = layer.MinZoom
            };

            UpdateLayer(layer, result);

            return result;
        }

        static MGLSymbolStyleLayer UpdateLayer(SymbolLayer layer, MGLSymbolStyleLayer result)
        {
            if (layer.IconAllowOverlap != null)
            {
                result.IconAllowsOverlap = layer.IconAllowOverlap.ToExpression();
            }

            if (layer.IconAnchor != null)
            {
                result.IconAnchor = layer.IconAnchor.ToExpression();
            }

            if (layer.IconColor != null)
            {
                result.IconColor = layer.IconColor.ToExpression();
            }

            if (layer.IconColorTransition != null)
            {
                result.IconColorTransition = layer.IconColorTransition.ToTransition();
            }

            if (layer.IconHaloBlur != null)
            {
                result.IconHaloBlur = layer.IconHaloBlur.ToExpression();
            }

            if (layer.IconHaloBlurTransition != null)
            {
                result.IconHaloBlurTransition = layer.IconHaloBlurTransition.ToTransition();
            }

            if (layer.IconHaloColor != null)
            {
                result.IconHaloColor = layer.IconHaloColor.ToExpression();
            }

            if (layer.IconHaloColorTransition != null)
            {
                result.IconHaloColorTransition = layer.IconHaloColorTransition.ToTransition();
            }

            if (layer.IconHaloWidth != null)
            {
                result.IconHaloWidth = layer.IconHaloWidth.ToExpression();
            }

            if (layer.IconHaloWidthTransition != null)
            {
                result.IconHaloWidthTransition = layer.IconHaloWidthTransition.ToTransition();
            }

            if (layer.IconIgnorePlacement != null)
            {
                result.IconIgnoresPlacement = layer.IconIgnorePlacement.ToExpression();
            }

            if (layer.IconImage != null)
            {
                result.IconImageName = layer.IconImage.ToExpression();
            }

            if (layer.IconKeepUpright != null)
            {
                // TODO iOS - No IconKeepUpright
                //result.IconKeepUpright = layer.IconKeepUpright.ToTransition();
            }

            if (layer.IconOffset != null)
            {
                result.IconOffset = layer.IconOffset.ToExpression();
            }

            if (layer.IconOpacity != null)
            {
                result.IconOpacity = layer.IconOpacity.ToExpression();
            }

            if (layer.IconOpacityTransition != null)
            {
                result.IconOpacityTransition = layer.IconOpacityTransition.ToTransition();
            }

            if (layer.IconOptional != null)
            {
                result.IconOptional = layer.IconOptional.ToExpression();
            }

            if (layer.IconPadding != null)
            {
                result.IconPadding = layer.IconPadding.ToExpression();
            }

            if (layer.IconPitchAlignment != null)
            {
                result.IconPitchAlignment = layer.IconPitchAlignment.ToExpression();
            }

            if (layer.IconSize != null)
            {
                // TODO iOS - Need to check if IconSize is IconScale
                result.IconScale = layer.IconSize.ToExpression();
            }

            if (layer.IconTextFit != null)
            {
                result.IconTextFit = layer.IconTextFit.ToExpression();
            }

            if (layer.IconTextFitPadding != null)
            {
                result.IconTextFitPadding = layer.IconTextFitPadding.ToExpression();
            }

            if (layer.IconTranslate != null)
            {
                result.IconTranslation = layer.IconTranslate.ToExpression();
            }

            if (layer.IconTranslateAnchor != null)
            {
                result.IconTranslationAnchor = layer.IconTranslateAnchor.ToExpression();
            }

            if (layer.IconTranslateTransition != null)
            {
                result.IconTranslationTransition = layer.IconTranslateTransition.ToTransition();
            }

            if (layer.SymbolAvoidEdges != null)
            {
                result.SymbolAvoidsEdges = layer.SymbolAvoidEdges.ToExpression();
            }

            if (layer.SymbolPlacement != null)
            {
                result.SymbolPlacement = layer.SymbolPlacement.ToExpression();
            }

            if (layer.SymbolSortKey != null)
            {
                result.SymbolSortKey = layer.SymbolSortKey.ToExpression();
            }

            if (layer.SymbolSpacing != null)
            {
                result.SymbolSpacing = layer.SymbolSpacing.ToExpression();
            }

            if (layer.SymbolZOrder != null)
            {
                // TODO Need to check if ZOrder exists
                // result.SymbolZOrder = layer.SymbolZOrder.ToExpression();
            }

            if (layer.TextAllowOverlap != null)
            {
                result.TextAllowsOverlap = layer.TextAllowOverlap.ToExpression();
            }

            if (layer.TextAnchor != null)
            {
                result.TextAnchor = layer.TextAnchor.ToExpression();
            }

            if (layer.TextColor != null)
            {
                result.TextColor = layer.TextColor.ToExpression();
            }

            if (layer.TextColorTransition != null)
            {
                result.TextColorTransition = layer.TextColorTransition.ToTransition();
            }

            if (layer.TextField != null)
            {
                result.Text = layer.TextField.ToExpression();
            }

            if (layer.TextFont != null)
            {
                result.TextFontNames = layer.TextFont.ToExpression();
            }

            if (layer.TextHaloBlur != null)
            {
                result.TextHaloBlur = layer.TextHaloBlur.ToExpression();
            }

            if (layer.TextHaloBlurTransition != null)
            {
                result.TextHaloBlurTransition = layer.TextHaloBlurTransition.ToTransition();
            }

            if (layer.TextHaloColor != null)
            {
                result.TextHaloColor = layer.TextHaloColor.ToExpression();
            }

            if (layer.TextHaloColorTransition != null)
            {
                result.TextHaloColorTransition = layer.TextHaloColorTransition.ToTransition();
            }

            if (layer.TextHaloWidth != null)
            {
                result.TextHaloWidth = layer.TextHaloWidth.ToExpression();
            }

            if (layer.TextHaloWidthTransition != null)
            {
                result.TextHaloWidthTransition = layer.TextHaloWidthTransition.ToTransition();
            }

            if (layer.TextIgnorePlacement != null)
            {
                result.TextIgnoresPlacement = layer.TextIgnorePlacement.ToExpression();
            }

            if (layer.TextJustify != null)
            {
                result.TextJustification = layer.TextJustify.ToExpression();
            }

            if (layer.TextKeepUpright != null)
            {
                // TODO iOS - Check if TextKeepUpright exists
                //result.TextKeepUpright = layer.TextKeepUpright.ToExpression();
            }

            if (layer.TextLetterSpacing != null)
            {
                result.TextLetterSpacing = layer.TextLetterSpacing.ToExpression();
            }

            if (layer.TextLineHeight != null)
            {
                result.TextLineHeight = layer.TextLineHeight.ToExpression();
            }

            if (layer.TextMaxAngle != null)
            {
                result.MaximumTextAngle = layer.TextMaxAngle.ToExpression();
            }

            if (layer.TextMaxWidth != null)
            {
                result.MaximumTextWidth = layer.TextMaxWidth.ToExpression();
            }

            if (layer.TextOffset != null)
            {
                result.TextOffset = layer.TextOffset.ToExpression();
            }

            if (layer.TextOpacity != null)
            {
                result.TextOpacity = layer.TextOpacity.ToExpression();
            }

            if (layer.TextOpacityTransition != null)
            {
                result.TextOpacityTransition = layer.TextOpacityTransition.ToTransition();
            }

            if (layer.TextOptional != null)
            {
                result.TextOptional = layer.TextOptional.ToExpression();
            }

            if (layer.TextPadding != null)
            {
                result.TextPadding = layer.TextPadding.ToExpression();
            }

            if (layer.TextPitchAlignment != null)
            {
                result.TextPitchAlignment = layer.TextPitchAlignment.ToExpression();
            }

            if (layer.TextRadialOffset != null)
            {
                result.TextRadialOffset = layer.TextRadialOffset.ToExpression();
            }

            if (layer.TextRotate != null)
            {
                result.TextRotation = layer.TextRotate.ToExpression();
            }

            if (layer.TextRotationAlignment != null)
            {
                result.TextRotationAlignment = layer.TextRotationAlignment.ToExpression();
            }

            if (layer.TextSize != null)
            {
                result.TextFontSize = layer.TextSize.ToExpression();
            }

            if (layer.TextTransform != null)
            {
                result.TextTransform = layer.TextTransform.ToExpression();
            }

            if (layer.TextTranslate != null)
            {
                result.TextTranslation = layer.TextTranslate.ToExpression();
            }

            if (layer.TextTranslateAnchor != null)
            {
                result.TextTranslationAnchor = layer.TextTranslateAnchor.ToExpression();
            }

            if (layer.TextTranslateTransition != null)
            {
                result.TextTranslationTransition = layer.TextTranslateTransition.ToTransition();
            }

            if (layer.TextVariableAnchor != null)
            {
                result.TextVariableAnchor = layer.TextVariableAnchor.ToExpression();
            }

            if (layer.TextVariableAnchor != null)
            {
                result.TextVariableAnchor = layer.TextVariableAnchor.ToExpression();
            }

            if (layer.TextWritingMode != null)
            {
                result.TextWritingModes = layer.TextWritingMode.ToExpression();
            }

            if (layer.Filter != null)
            {
                result.Predicate = layer.Filter.ToPredicate();
            }

            return result;
        }

        static MGLRasterStyleLayer ToLayer(RasterLayer layer, MGLSource source)
        {
            var result = new MGLRasterStyleLayer(layer.Id, source)
            {
                MaximumZoomLevel = layer.MaxZoom,
                MinimumZoomLevel = layer.MinZoom
            };

            UpdateLayer(layer, result);

            return result;
        }

        static MGLRasterStyleLayer UpdateLayer(RasterLayer layer, MGLRasterStyleLayer result)
        {

            if (layer.RasterBrightnessMax != null)
            {
                result.MaximumRasterBrightness = layer.RasterBrightnessMax.ToExpression();
            }

            if (layer.RasterBrightnessMaxTransition != null)
            {
                result.MaximumRasterBrightnessTransition = layer.RasterBrightnessMaxTransition.ToTransition();
            }

            if (layer.RasterBrightnessMin != null)
            {
                result.MinimumRasterBrightness = layer.RasterBrightnessMin.ToExpression();
            }

            if (layer.RasterBrightnessMinTransition != null)
            {
                result.MinimumRasterBrightnessTransition = layer.RasterBrightnessMinTransition.ToTransition();
            }

            if (layer.RasterContrast != null)
            {
                result.RasterContrast = layer.RasterContrast.ToExpression();
            }

            if (layer.RasterContrastTransition != null)
            {
                result.RasterContrastTransition = layer.RasterContrastTransition.ToTransition();
            }

            if (layer.RasterFadeDuration != null)
            {
                result.RasterFadeDuration = layer.RasterFadeDuration.ToExpression();
            }

            if (layer.RasterHueRotate != null)
            {
                result.RasterHueRotation = layer.RasterHueRotate.ToExpression();
            }

            if (layer.RasterHueRotateTransition != null)
            {
                result.RasterHueRotationTransition = layer.RasterHueRotateTransition.ToTransition();
            }

            if (layer.RasterOpacity != null)
            {
                result.RasterOpacity = layer.RasterOpacity.ToExpression();
            }

            if (layer.RasterOpacityTransition != null)
            {
                result.RasterOpacityTransition = layer.RasterOpacityTransition.ToTransition();
            }

            if (layer.RasterResampling != null)
            {
                result.RasterResamplingMode = layer.RasterResampling.ToExpression();
            }

            if (layer.RasterSaturation != null)
            {
                result.RasterSaturation = layer.RasterSaturation.ToExpression();
            }

            if (layer.RasterSaturationTransition != null)
            {
                result.RasterSaturationTransition = layer.RasterSaturationTransition.ToTransition();
            }

            //if (layer.Filter != null)
            //{
            //    result.Predicate = layer.Filter.ToPredicate();
            //}

            return result;
        }

        static MGLLineStyleLayer ToLayer(LineLayer layer, MGLSource source)
        {
            var result = new MGLLineStyleLayer(layer.Id, source)
            {
                //MaximumZoomLevel = layer.MaxZoom,
                //MinimumZoomLevel = layer.MinZoom
            };

            UpdateLayer(layer, result);

            return result;
        }

        static MGLLineStyleLayer UpdateLayer(LineLayer layer, MGLLineStyleLayer result)
        {
            if (layer.LineBlur != null)
            {
                result.LineBlur = layer.LineBlur.ToExpression();
            }

            if (layer.LineBlurTransition != null)
            {
                result.LineBlurTransition = layer.LineBlurTransition.ToTransition();
            }

            if (layer.LineCap != null)
            {
                result.LineCap = layer.LineCap.ToExpression();
            }

            if (layer.LineColor != null)
            {
                result.LineColor = layer.LineColor.ToExpression();
            }

            if (layer.LineColorTransition != null)
            {
                result.LineColorTransition = layer.LineColorTransition.ToTransition();
            }

            if (layer.LineDasharray != null)
            {
                result.LineDashPattern = layer.LineDasharray.ToExpression();
            }

            if (layer.LineDasharrayTransition != null)
            {
                result.LineDashPatternTransition = layer.LineDasharrayTransition.ToTransition();
            }

            if (layer.LineGapWidth != null)
            {
                result.LineGapWidth = layer.LineGapWidth.ToExpression();
            }

            if (layer.LineGapWidthTransition != null)
            {
                result.LineGapWidthTransition = layer.LineGapWidthTransition.ToTransition();
            }

            if (layer.LineGradient != null)
            {
                result.LineGradient = layer.LineGradient.ToExpression();
            }

            if (layer.LineJoin != null)
            {
                result.LineJoin = layer.LineJoin.ToExpression();
            }

            if (layer.LineWidth != null)
            {
                result.LineWidth = layer.LineWidth.ToExpression();
            }

            if (layer.LineWidthTransition != null)
            {
                result.LineWidthTransition = layer.LineWidthTransition.ToTransition();
            }

            if (layer.Filter != null)
            {
                result.Predicate = layer.Filter.ToPredicate();
            }

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.SourceLayerIdentifier = layer.SourceLayer;
            }

            return result;
        }

        static MGLHillshadeStyleLayer ToLayer(HillshadeLayer layer, MGLSource source)
        {
            var result = new MGLHillshadeStyleLayer(layer.Id, source)
            {
                //MaximumZoomLevel = layer.MaxZoom,
                //MinimumZoomLevel = layer.MinZoom
            };

            UpdateLayer(layer, result);

            return result;
        }

        static MGLHillshadeStyleLayer UpdateLayer(HillshadeLayer layer, MGLHillshadeStyleLayer result)
        {
            if (layer.HillshadeAccentColor != null)
            {
                result.HillshadeAccentColor = layer.HillshadeAccentColor.ToExpression();
            }

            if (layer.HillshadeAccentColorTransition != null)
            {
                result.HillshadeAccentColorTransition = layer.HillshadeAccentColorTransition.ToTransition();
            }

            if (layer.HillshadeExaggeration != null)
            {
                result.HillshadeExaggeration = layer.HillshadeExaggeration.ToExpression();
            }

            if (layer.HillshadeExaggerationTransition != null)
            {
                result.HillshadeExaggerationTransition = layer.HillshadeExaggerationTransition.ToTransition();
            }

            if (layer.HillshadeHighlightColor != null)
            {
                result.HillshadeHighlightColor = layer.HillshadeHighlightColor.ToExpression();
            }

            if (layer.HillshadeHighlightColorTransition != null)
            {
                result.HillshadeHighlightColorTransition = layer.HillshadeHighlightColorTransition.ToTransition();
            }

            if (layer.HillshadeIlluminationAnchor != null)
            {
                result.HillshadeIlluminationAnchor = layer.HillshadeIlluminationAnchor.ToExpression();
            }

            if (layer.HillshadeIlluminationDirection != null)
            {
                result.HillshadeIlluminationDirection = layer.HillshadeIlluminationDirection.ToExpression();
            }

            if (layer.HillshadeShadowColor != null)
            {
                result.HillshadeShadowColor = layer.HillshadeShadowColor.ToExpression();
            }

            if (layer.HillshadeShadowColorTransition != null)
            {
                result.HillshadeShadowColorTransition = layer.HillshadeShadowColorTransition.ToTransition();
            }

            //if (layer.Filter != null)
            //{
            //    result.Predicate = layer.Filter.ToPredicate();
            //}

            return result;
        }

        static MGLHeatmapStyleLayer ToLayer(HeatmapLayer layer, MGLSource source)
        {
            var result = new MGLHeatmapStyleLayer(layer.Id, source)
            {
                //MaximumZoomLevel = layer.MaxZoom,
                //MinimumZoomLevel = layer.MinZoom
            };

            UpdateLayer(layer, result);

            return result;
        }
        static MGLHeatmapStyleLayer UpdateLayer(HeatmapLayer layer, MGLHeatmapStyleLayer result)
        {
            if (layer.HeatmapColor != null)
            {
                result.HeatmapColor = layer.HeatmapColor.ToExpression();
            }

            if (layer.HeatmapColorTransition != null)
            {
                // TODO iOS - No HeatmapColorTransition
                // result.HeatmapColorTransition = layer.HeatmapColorTransition.ToTransition();
            }

            if (layer.HeatmapIntensity != null)
            {
                result.HeatmapIntensity = layer.HeatmapIntensity.ToExpression();
            }

            if (layer.HeatmapIntensityTransition != null)
            {
                result.HeatmapIntensityTransition = layer.HeatmapIntensityTransition.ToTransition();
            }

            if (layer.HeatmapOpacity != null)
            {
                result.HeatmapOpacity = layer.HeatmapOpacity.ToExpression();
            }

            if (layer.HeatmapOpacityTransition != null)
            {
                result.HeatmapOpacityTransition = layer.HeatmapOpacityTransition.ToTransition();
            }

            if (layer.HeatmapRadius != null)
            {
                result.HeatmapRadius = layer.HeatmapRadius.ToExpression();
            }

            if (layer.HeatmapWeight != null)
            {
                result.HeatmapWeight = layer.HeatmapWeight.ToExpression();
            }

            if (layer.HeatmapRadiusTransition != null)
            {
                result.HeatmapRadiusTransition = layer.HeatmapRadiusTransition.ToTransition();
            }

            if (layer.Filter != null)
            {
                result.Predicate = layer.Filter.ToPredicate();
            }

            return result;
        }

        static MGLForegroundStyleLayer ToLayer(ForegroundLayer layer, MGLSource source)
        {
            //var result = new MGLForegroundStyleLayer(layer.Id, source)
            //{
            //    MaximumZoomLevel = layer.MaxZoom,
            //    MinimumZoomLevel = layer.MinZoom
            //};

            //return result;

            // TODO iOS - MGLForegroundStyleLayer - No info yet.
            return null;
        }

        static MGLFillStyleLayer ToLayer(FillLayer layer, MGLSource source)
        {
            var result = new MGLFillStyleLayer(layer.Id, source)
            {
                MaximumZoomLevel = layer.MaxZoom,
                MinimumZoomLevel = layer.MinZoom
            };

            UpdateLayer(layer, result);

            return result;
        }

        static MGLFillStyleLayer UpdateLayer(FillLayer layer, MGLFillStyleLayer result)
        {

            if (layer.FillAntialiased != null)
            {
                result.FillAntialiased = layer.FillAntialiased.ToExpression();
            }

            if (layer.FillColor != null)
            {
                result.FillColor = layer.FillColor.ToExpression();
            }

            if (layer.FillColorTransition != null)
            {
                result.FillColorTransition = layer.FillColorTransition.ToTransition();
            }

            if (layer.FillOpacity != null)
            {
                result.FillOpacity = layer.FillOpacity.ToExpression();
            }

            if (layer.FillOpacityTransition != null)
            {
                result.FillOpacityTransition = layer.FillOpacityTransition.ToTransition();
            }

            if (layer.FillOutlineColor != null)
            {
                result.FillOutlineColor = layer.FillOutlineColor.ToExpression();
            }

            if (layer.FillOutlineColorTransition != null)
            {
                result.FillOutlineColorTransition = layer.FillOutlineColorTransition.ToTransition();
            }

            if (layer.FillPattern != null)
            {
                result.FillPattern = layer.FillPattern.ToExpression();
            }

            if (layer.FillPatternTransition != null)
            {
                result.FillPatternTransition = layer.FillPatternTransition.ToTransition();
            }

            if (layer.FillTranslate != null)
            {
                result.FillTranslation = layer.FillTranslate.ToExpression();
            }

            if (layer.FillTranslateTransition != null)
            {
                result.FillTranslationTransition = layer.FillTranslateTransition.ToTransition();
            }

            if (layer.FillTranslateAnchor != null)
            {
                result.FillTranslationAnchor = layer.FillTranslateAnchor.ToExpression();
            }

            if (layer.Filter != null)
            {
                result.Predicate = layer.Filter.ToPredicate();
            }

            return result;
        }

        static MGLFillExtrusionStyleLayer ToLayer(FillExtrusionLayer layer, MGLSource source)
        {
            var result = new MGLFillExtrusionStyleLayer(layer.Id, source)
            {
                //MaximumZoomLevel = layer.MaxZoom,
                //MinimumZoomLevel = layer.MinZoom
            };

            UpdateLayer(layer, result);

            return result;
        }

        static MGLFillExtrusionStyleLayer UpdateLayer(FillExtrusionLayer layer, MGLFillExtrusionStyleLayer result)
        {
            if (layer.FillExtrusionBase != null)
            {
                result.FillExtrusionBase = layer.FillExtrusionBase.ToExpression();
            }

            if (layer.FillExtrusionBaseTransition != null)
            {
                result.FillExtrusionBaseTransition = layer.FillExtrusionBaseTransition.ToTransition();
            }

            if (layer.FillExtrusionColor != null)
            {
                result.FillExtrusionColor = layer.FillExtrusionColor.ToExpression();
            }

            if (layer.FillExtrusionColorTransition != null)
            {
                result.FillExtrusionColorTransition = layer.FillExtrusionColorTransition.ToTransition();
            }

            if (layer.FillExtrusionOpacity != null)
            {
                result.FillExtrusionOpacity = layer.FillExtrusionOpacity.ToExpression();
            }

            if (layer.FillExtrusionOpacityTransition != null)
            {
                result.FillExtrusionOpacityTransition = layer.FillExtrusionOpacityTransition.ToTransition();
            }

            if (layer.FillExtrusionHasVerticalGradient != null)
            {
                result.FillExtrusionHasVerticalGradient = layer.FillExtrusionHasVerticalGradient.ToExpression();
            }

            if (layer.FillExtrusionHeight != null)
            {
                result.FillExtrusionHeight = layer.FillExtrusionHeight.ToExpression();
            }

            if (layer.FillExtrusionHeightTransition != null)
            {
                result.FillExtrusionHeightTransition = layer.FillExtrusionHeightTransition.ToTransition();
            }

            if (layer.FillExtrusionPattern != null)
            {
                result.FillExtrusionPattern = layer.FillExtrusionPattern.ToExpression();
            }

            if (layer.FillExtrusionPatternTransition != null)
            {
                result.FillExtrusionPatternTransition = layer.FillExtrusionPatternTransition.ToTransition();
            }

            if (layer.FillExtrusionTranslate != null)
            {
                result.FillExtrusionTranslation = layer.FillExtrusionTranslate.ToExpression();
            }

            if (layer.FillExtrusionTranslateTransition != null)
            {
                result.FillExtrusionTranslationTransition = layer.FillExtrusionTranslateTransition.ToTransition();
            }

            if (layer.FillExtrusionTranslateAnchor != null)
            {
                result.FillExtrusionTranslationAnchor = layer.FillExtrusionTranslateAnchor.ToExpression();
            }

            if (layer.Filter != null)
            {
                result.Predicate = layer.Filter.ToPredicate();
            }

            return result;
        }

        static MGLCircleStyleLayer ToLayer(CircleLayer layer, MGLSource source)
        {
            var result = new MGLCircleStyleLayer(layer.Id, source)
            {
                MaximumZoomLevel = layer.MaxZoom,
                MinimumZoomLevel = layer.MinZoom
            };

            UpdateLayer(layer, result);

            return result;
        }

        static MGLCircleStyleLayer UpdateLayer(CircleLayer layer, MGLCircleStyleLayer result)
        {
            if (layer.CircleBlur != null)
            {
                result.CircleBlur = layer.CircleBlur.ToExpression();
            }

            if (layer.CircleBlurTransition != null)
            {
                result.CircleBlurTransition = layer.CircleBlurTransition.ToTransition();
            }

            if (layer.CircleColor != null)
            {
                result.CircleColor = layer.CircleColor.ToExpression();
            }

            if (layer.CircleColorTransition != null)
            {
                result.CircleColorTransition = layer.CircleColorTransition.ToTransition();
            }

            if (layer.CircleOpacity != null)
            {
                result.CircleOpacity = layer.CircleOpacity.ToExpression();
            }

            if (layer.CircleOpacityTransition != null)
            {
                result.CircleOpacityTransition = layer.CircleOpacityTransition.ToTransition();
            }

            if (layer.CirclePitchAlignment != null)
            {
                result.CirclePitchAlignment = layer.CirclePitchAlignment.ToExpression();
            }

            //if (circleLayer.CirclePitchScale != null)
            //{
            //    // WARN Not available to iOS yet
            //    layer.CirclePitchScale = circleLayer.CirclePitchScale.ToExpression();
            //}

            if (layer.CircleRadius != null)
            {
                result.CircleRadius = layer.CircleRadius.ToExpression();
            }

            if (layer.CircleRadiusTransition != null)
            {
                result.CircleRadiusTransition = layer.CircleRadiusTransition.ToTransition();
            }

            if (layer.CircleStrokeColor != null)
            {
                result.CircleStrokeColor = layer.CircleStrokeColor.ToExpression();
            }

            if (layer.CircleStrokeColorTransition != null)
            {
                result.CircleStrokeColorTransition = layer.CircleStrokeColorTransition.ToTransition();
            }

            if (layer.CircleStrokeOpacity != null)
            {
                result.CircleStrokeOpacity = layer.CircleStrokeOpacity.ToExpression();
            }

            if (layer.CircleStrokeOpacityTransition != null)
            {
                result.CircleStrokeOpacityTransition = layer.CircleStrokeOpacityTransition.ToTransition();
            }

            if (layer.CircleStrokeWidth != null)
            {
                result.CircleStrokeWidth = layer.CircleStrokeWidth.ToExpression();
            }

            if (layer.CircleStrokeWidthTransition != null)
            {
                result.CircleStrokeWidthTransition = layer.CircleStrokeWidthTransition.ToTransition();
            }

            if (layer.CircleTranslate != null)
            {
                result.CircleTranslation = layer.CircleTranslate.ToExpression();
            }

            if (layer.CircleTranslateTransition != null)
            {
                result.CircleTranslationTransition = layer.CircleTranslateTransition.ToTransition();
            }

            if (layer.CircleTranslateAnchor != null)
            {
                result.CircleTranslationAnchor = layer.CircleTranslateAnchor.ToExpression();
            }

            if (layer.Filter != null)
            {
                result.Predicate = layer.Filter.ToPredicate();
            }

            return result;
        }

        static MGLTransition ToTransition(this TransitionOptions options)
        {
            return new MGLTransition
            {
                delay = options.Delay,
                duration = options.Duration
            };
        }
    }
}
