using Xamarin.Forms.Platform.Android;

using Com.Mapbox.Mapboxsdk.Style.Layers;
using NxLayer = Naxam.Mapbox.Layers.Layer;
using NxCircleLayer = Naxam.Mapbox.Layers.CircleLayer;
using NxFillExtrusionLayer = Naxam.Mapbox.Layers.FillExtrusionLayer;
using NxFillLayer = Naxam.Mapbox.Layers.FillLayer;
using NxForegroundLayer = Naxam.Mapbox.Layers.ForegroundLayer;
using NxHeatmapLayer = Naxam.Mapbox.Layers.HeatmapLayer;
using NxHillshadeLayer = Naxam.Mapbox.Layers.HillshadeLayer;
using NxSymbolLayer = Naxam.Mapbox.Layers.SymbolLayer;
using NxLineLayer = Naxam.Mapbox.Layers.LineLayer;
using NxBackgroundLayer = Naxam.Mapbox.Layers.BackgroundLayer;
using NxRasterLayer = Naxam.Mapbox.Layers.RasterLayer;
using Naxam.Mapbox.Platform.Droid.Extensions;
using System.Collections.Generic;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public static class LayerExtensions
    {
        public static Layer ToLayer(this NxLayer layer)
        {
            switch (layer)
            {
                case NxCircleLayer circleLayer:
                    return ToLayer(circleLayer);
                case NxFillExtrusionLayer fillExtrusionLayer:
                    return ToLayer(fillExtrusionLayer);
                case NxFillLayer fillLayer:
                    return ToLayer(fillLayer);
                case NxHillshadeLayer hillshadeLayer:
                    return ToLayer(hillshadeLayer);
                case NxForegroundLayer foregroundLayer:
                    // TODO Android - No ForegroundLayer
                    return null;
                case NxHeatmapLayer heatmapLayer:
                    return ToLayer(heatmapLayer);
                case NxLineLayer lineLayer:
                    return ToLayer(lineLayer);

                case NxRasterLayer rasterLayer:
                    return ToLayer(rasterLayer);

                case NxSymbolLayer symbolLayer:
                    return ToLayer(symbolLayer);
                    //case NxBackgroundLayer backgroundLayer:
                    //    result = backgroundLayer.ToNative();
                    //    break;
            }

            return null;
        }

        public static void UpdateLayer(this NxLayer layer, Layer target)
        {
            if (layer == null || target == null) { return; }

            switch (layer)
            {
                case NxCircleLayer circleLayer:
                    UpdateLayer(target as CircleLayer, circleLayer);
                    break;
                case NxFillExtrusionLayer fillExtrusionLayer:
                    UpdateLayer(target as FillExtrusionLayer, fillExtrusionLayer);
                    break;
                case NxFillLayer fillLayer:
                    UpdateLayer(target as FillLayer, fillLayer);
                    break;
                case NxHillshadeLayer hillshadeLayer:
                    UpdateLayer(target as HillshadeLayer, hillshadeLayer);
                    break;
                case NxForegroundLayer foregroundLayer:
                    // TODO Android - No ForegroundLayer
                    break;
                case NxHeatmapLayer heatmapLayer:
                    UpdateLayer(target as HeatmapLayer, heatmapLayer);
                    break;
                case NxLineLayer lineLayer:
                    UpdateLayer(target as LineLayer, lineLayer);
                    break;

                case NxRasterLayer rasterLayer:
                    UpdateLayer(target as RasterLayer, rasterLayer);
                    break;

                case NxSymbolLayer symbolLayer:
                    UpdateLayer(target as SymbolLayer, symbolLayer);
                    break;
                    //case NxBackgroundLayer backgroundLayer:
                    //    result = backgroundLayer.ToNative();
                    //    break;
            }
        }

        static SymbolLayer ToLayer(NxSymbolLayer layer)
        {
            if (layer == null) { return null; }

            var result = new SymbolLayer(layer.Id, layer.SourceId)
            {
                //MinZoom = layer.MinZoom,
                //MaxZoom = layer.MaxZoom
            };

            UpdateLayer(result, layer);

            return result;
        }

        static void UpdateLayer(SymbolLayer result, NxSymbolLayer layer)
        {
            var properties = new List<PropertyValue>();

            if (layer.IconAllowOverlap != null)
            {
                properties.Add(PropertyFactory.IconAllowOverlap(layer.IconAllowOverlap.ToExpression()));
            }

            if (layer.IconAnchor != null)
            {
                properties.Add(PropertyFactory.IconAnchor(layer.IconAnchor.ToExpression()));
            }

            if (layer.IconColor != null)
            {
                properties.Add(PropertyFactory.IconColor(layer.IconColor.ToExpression()));
            }

            if (layer.IconColorTransition != null)
            {
                result.IconColorTransition = layer.IconColorTransition.ToTransition();
            }

            if (layer.IconHaloBlur != null)
            {
                properties.Add(PropertyFactory.IconHaloBlur(layer.IconHaloBlur.ToExpression()));
            }

            if (layer.IconHaloBlurTransition != null)
            {
                result.IconHaloBlurTransition = layer.IconHaloBlurTransition.ToTransition();
            }

            if (layer.IconHaloColor != null)
            {
                properties.Add(PropertyFactory.IconHaloColor(layer.IconHaloColor.ToExpression()));
            }

            if (layer.IconHaloColorTransition != null)
            {
                result.IconHaloColorTransition = layer.IconHaloColorTransition.ToTransition();
            }

            if (layer.IconHaloWidth != null)
            {
                properties.Add(PropertyFactory.LineDasharray(layer.IconHaloWidth.ToExpression()));
            }

            if (layer.IconHaloWidthTransition != null)
            {
                result.IconHaloWidthTransition = layer.IconHaloWidthTransition.ToTransition();
            }

            if (layer.IconIgnorePlacement != null)
            {
                properties.Add(PropertyFactory.IconIgnorePlacement(layer.IconIgnorePlacement.ToExpression()));
            }

            if (layer.IconImage != null)
            {
                properties.Add(PropertyFactory.IconImage(layer.IconImage.ToExpression()));
            }

            if (layer.IconKeepUpright != null)
            {
                properties.Add(PropertyFactory.IconKeepUpright(layer.IconKeepUpright.ToExpression()));
            }

            if (layer.IconOffset != null)
            {
                properties.Add(PropertyFactory.IconOffset(layer.IconOffset.ToExpression()));
            }

            if (layer.IconOpacity != null)
            {
                properties.Add(PropertyFactory.IconOpacity(layer.IconOpacity.ToExpression()));
            }

            if (layer.IconOpacityTransition != null)
            {
                result.IconOpacityTransition = layer.IconOpacityTransition.ToTransition();
            }

            if (layer.IconOptional != null)
            {
                properties.Add(PropertyFactory.IconOptional(layer.IconOptional.ToExpression()));
            }

            if (layer.IconPadding != null)
            {
                properties.Add(PropertyFactory.IconPadding(layer.IconPadding.ToExpression()));
            }

            if (layer.IconPitchAlignment != null)
            {
                properties.Add(PropertyFactory.IconPitchAlignment(layer.IconPitchAlignment.ToExpression()));
            }

            if (layer.IconRotate != null)
            {
                properties.Add(PropertyFactory.IconRotate(layer.IconRotate.ToExpression()));
            }

            if (layer.IconRotationAlignment != null)
            {
                properties.Add(PropertyFactory.IconRotationAlignment(layer.IconRotationAlignment.ToExpression()));
            }

            if (layer.IconSize != null)
            {
                properties.Add(PropertyFactory.IconSize(layer.IconSize.ToExpression()));
            }

            if (layer.IconTextFit != null)
            {
                properties.Add(PropertyFactory.IconTextFit(layer.IconTextFit.ToExpression()));
            }

            if (layer.IconTextFitPadding != null)
            {
                properties.Add(PropertyFactory.IconTextFitPadding(layer.IconTextFitPadding.ToExpression()));
            }

            if (layer.IconTranslate != null)
            {
                properties.Add(PropertyFactory.IconTranslate(layer.IconTranslate.ToExpression()));
            }

            if (layer.IconTranslateAnchor != null)
            {
                properties.Add(PropertyFactory.IconTranslateAnchor(layer.IconTranslateAnchor.ToExpression()));
            }

            if (layer.IconTranslateTransition != null)
            {
                result.IconTranslateTransition = layer.IconTranslateTransition.ToTransition();
            }
            if (layer.SymbolAvoidEdges != null)
            {
                properties.Add(PropertyFactory.SymbolAvoidEdges(layer.SymbolAvoidEdges.ToExpression()));
            }

            if (layer.SymbolPlacement != null)
            {
                properties.Add(PropertyFactory.SymbolPlacement(layer.SymbolPlacement.ToExpression()));
            }

            if (layer.SymbolSortKey != null)
            {
                properties.Add(PropertyFactory.SymbolSortKey(layer.SymbolSortKey.ToExpression()));
            }

            if (layer.SymbolSpacing != null)
            {
                properties.Add(PropertyFactory.SymbolSpacing(layer.SymbolSpacing.ToExpression()));
            }

            if (layer.SymbolZOrder != null)
            {
                properties.Add(PropertyFactory.SymbolZOrder(layer.SymbolZOrder.ToExpression()));
            }

            if (layer.TextAllowOverlap != null)
            {
                properties.Add(PropertyFactory.TextAllowOverlap(layer.TextAllowOverlap.ToExpression()));
            }

            if (layer.TextAnchor != null)
            {
                properties.Add(PropertyFactory.TextAnchor(layer.TextAnchor.ToExpression()));
            }

            if (layer.TextColor != null)
            {
                properties.Add(PropertyFactory.TextColor(layer.TextColor.ToExpression()));
            }

            if (layer.TextColorTransition != null)
            {
                result.TextColorTransition = layer.TextColorTransition.ToTransition();
            }

            if (layer.TextField != null)
            {
                properties.Add(PropertyFactory.TextField(layer.TextField.ToExpression()));
            }

            if (layer.TextFont != null)
            {
                properties.Add(PropertyFactory.TextFont(layer.TextFont.ToExpression()));
            }

            if (layer.TextHaloColor != null)
            {
                properties.Add(PropertyFactory.TextHaloColor(layer.TextHaloColor.ToExpression()));
            }

            if (layer.TextHaloColorTransition != null)
            {
                result.TextHaloColorTransition = layer.TextHaloColorTransition.ToTransition();
            }

            if (layer.TextIgnorePlacement != null)
            {
                properties.Add(PropertyFactory.TextIgnorePlacement(layer.TextIgnorePlacement.ToExpression()));
            }

            if (layer.TextJustify != null)
            {
                properties.Add(PropertyFactory.TextJustify(layer.TextJustify.ToExpression()));
            }

            if (layer.TextKeepUpright != null)
            {
                properties.Add(PropertyFactory.TextKeepUpright(layer.TextKeepUpright.ToExpression()));
            }

            if (layer.TextLetterSpacing != null)
            {
                properties.Add(PropertyFactory.TextLetterSpacing(layer.TextLetterSpacing.ToExpression()));
            }

            if (layer.TextLineHeight != null)
            {
                properties.Add(PropertyFactory.TextLineHeight(layer.TextLineHeight.ToExpression()));
            }

            if (layer.TextMaxAngle != null)
            {
                properties.Add(PropertyFactory.TextMaxAngle(layer.TextMaxAngle.ToExpression()));
            }

            if (layer.TextMaxWidth != null)
            {
                properties.Add(PropertyFactory.TextMaxWidth(layer.TextMaxWidth.ToExpression()));
            }

            if (layer.TextOffset != null)
            {
                properties.Add(PropertyFactory.TextOffset(layer.TextOffset.ToExpression()));
            }

            if (layer.TextOpacity != null)
            {
                properties.Add(PropertyFactory.TextOpacity(layer.TextOpacity.ToExpression()));
            }

            if (layer.TextOpacityTransition != null)
            {
                result.TextOpacityTransition = layer.TextOpacityTransition.ToTransition();
            }

            if (layer.TextOptional != null)
            {
                properties.Add(PropertyFactory.TextOptional(layer.TextOptional.ToExpression()));
            }

            if (layer.TextPadding != null)
            {
                properties.Add(PropertyFactory.TextPadding(layer.TextPadding.ToExpression()));
            }

            if (layer.TextPitchAlignment != null)
            {
                properties.Add(PropertyFactory.TextPitchAlignment(layer.TextPitchAlignment.ToExpression()));
            }

            if (layer.TextRadialOffset != null)
            {
                properties.Add(PropertyFactory.TextRadialOffset(layer.TextRadialOffset.ToExpression()));
            }

            if (layer.TextRotate != null)
            {
                properties.Add(PropertyFactory.TextRotate(layer.TextRotate.ToExpression()));
            }

            if (layer.TextRotationAlignment != null)
            {
                properties.Add(PropertyFactory.TextRotationAlignment(layer.TextRotationAlignment.ToExpression()));
            }

            if (layer.TextSize != null)
            {
                properties.Add(PropertyFactory.TextSize(layer.TextSize.ToExpression()));
            }

            if (layer.TextTransform != null)
            {
                properties.Add(PropertyFactory.TextTransform(layer.TextTransform.ToExpression()));
            }

            if (layer.TextTranslate != null)
            {
                properties.Add(PropertyFactory.TextTranslate(layer.TextTranslate.ToExpression()));
            }

            if (layer.TextTranslateAnchor != null)
            {
                properties.Add(PropertyFactory.TextTranslateAnchor(layer.TextTranslateAnchor.ToExpression()));
            }

            if (layer.TextTranslateTransition != null)
            {
                result.TextTranslateTransition = layer.TextTranslateTransition.ToTransition();
            }

            if (layer.TextVariableAnchor != null)
            {
                properties.Add(PropertyFactory.TextVariableAnchor(layer.TextVariableAnchor.ToExpression()));
            }

            if (layer.TextWritingMode != null)
            {
                properties.Add(PropertyFactory.TextWritingMode(layer.TextWritingMode.ToExpression()));
            }


            result.SetProperties(properties.ToArray());

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToExpression());
            }
        }

        static RasterLayer ToLayer(NxRasterLayer layer)
        {
            if (layer == null) { return null; }

            var result = new RasterLayer(layer.Id, layer.SourceId)
            {
                //MinZoom = layer.MinZoom,
                //MaxZoom = layer.MaxZoom
            };

            UpdateLayer(result, layer);

            return result;
        }

        static void UpdateLayer(RasterLayer result, NxRasterLayer layer)
        {
            var properties = new List<PropertyValue>();

            if (layer.RasterBrightnessMax != null)
            {
                properties.Add(PropertyFactory.LineBlur(layer.RasterBrightnessMax.ToExpression()));
            }

            if (layer.RasterBrightnessMaxTransition != null)
            {
                result.RasterBrightnessMaxTransition = layer.RasterBrightnessMaxTransition.ToTransition();
            }

            if (layer.RasterBrightnessMin != null)
            {
                properties.Add(PropertyFactory.RasterBrightnessMin(layer.RasterBrightnessMin.ToExpression()));
            }

            if (layer.RasterBrightnessMinTransition != null)
            {
                result.RasterBrightnessMinTransition = layer.RasterBrightnessMinTransition.ToTransition();
            }

            if (layer.RasterContrast != null)
            {
                properties.Add(PropertyFactory.LineDasharray(layer.RasterContrast.ToExpression()));
            }

            if (layer.RasterContrastTransition != null)
            {
                result.RasterContrastTransition = layer.RasterContrastTransition.ToTransition();
            }

            if (layer.RasterFadeDuration != null)
            {
                properties.Add(PropertyFactory.RasterFadeDuration(layer.RasterFadeDuration.ToExpression()));
            }

            if (layer.RasterHueRotate != null)
            {
                properties.Add(PropertyFactory.RasterHueRotate(layer.RasterHueRotate.ToExpression()));
            }

            if (layer.RasterHueRotateTransition != null)
            {
                result.RasterHueRotateTransition = layer.RasterHueRotateTransition.ToTransition();
            }

            if (layer.RasterOpacity != null)
            {
                properties.Add(PropertyFactory.LineGradient(layer.RasterOpacity.ToExpression()));
            }

            if (layer.RasterOpacityTransition != null)
            {
                result.RasterOpacityTransition = layer.RasterOpacityTransition.ToTransition();
            }

            if (layer.RasterResampling != null)
            {
                properties.Add(PropertyFactory.RasterResampling(layer.RasterResampling.ToExpression()));
            }

            if (layer.RasterSaturation != null)
            {
                properties.Add(PropertyFactory.RasterSaturation(layer.RasterSaturation.ToExpression()));
            }

            if (layer.RasterSaturationTransition != null)
            {
                result.RasterSaturationTransition = layer.RasterSaturationTransition.ToTransition();
            }

            result.SetProperties(properties.ToArray());

            //if (layer.Filter != null)
            //{
            //    result.WithFilter(layer.Filter.ToExpression());
            //}
        }

        static LineLayer ToLayer(NxLineLayer layer)
        {
            if (layer == null) { return null; }

            var result = new LineLayer(layer.Id, layer.SourceId)
            {
                //MinZoom = layer.MinZoom,
                //MaxZoom = layer.MaxZoom
            };

            UpdateLayer(result, layer);

            return result;
        }

        static void UpdateLayer(LineLayer result, NxLineLayer layer)
        {
            var properties = new List<PropertyValue>();

            if (layer.LineBlur != null)
            {
                properties.Add(PropertyFactory.LineBlur(layer.LineBlur.ToExpression()));
            }

            if (layer.LineBlurTransition != null)
            {
                result.LineBlurTransition = layer.LineBlurTransition.ToTransition();
            }

            if (layer.LineCap != null)
            {
                properties.Add(PropertyFactory.LineCap(layer.LineCap.ToExpression()));
            }

            if (layer.LineColor != null)
            {
                properties.Add(PropertyFactory.LineColor(layer.LineColor.ToExpression()));
            }

            if (layer.LineColorTransition != null)
            {
                result.LineColorTransition = layer.LineColorTransition.ToTransition();
            }

            if (layer.LineDasharray != null)
            {
                properties.Add(PropertyFactory.LineDasharray(layer.LineDasharray.ToExpression()));
            }

            if (layer.LineDasharrayTransition != null)
            {
                result.LineDasharrayTransition = layer.LineDasharrayTransition.ToTransition();
            }

            if (layer.LineGapWidth != null)
            {
                properties.Add(PropertyFactory.LineGapWidth(layer.LineGapWidth.ToExpression()));
            }

            if (layer.LineGapWidthTransition != null)
            {
                result.LineGapWidthTransition = layer.LineGapWidthTransition.ToTransition();
            }

            if (layer.LineGradient != null)
            {
                properties.Add(PropertyFactory.LineGradient(layer.LineGradient.ToExpression()));
            }

            if (layer.LineJoin != null)
            {
                properties.Add(PropertyFactory.LineJoin(layer.LineJoin.ToExpression()));
            }

            if (layer.LineWidth != null)
            {
                properties.Add(PropertyFactory.LineWidth(layer.LineWidth.ToExpression()));
            }

            if (layer.LineWidthTransition != null)
            {
                result.LineWidthTransition = layer.LineWidthTransition.ToTransition();
            }

            result.SetProperties(properties.ToArray());

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToExpression());
            }

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.WithSourceLayer(layer.SourceLayer);
            }
        }

        static HillshadeLayer ToLayer(NxHillshadeLayer layer)
        {
            if (layer == null) { return null; }

            var result = new HillshadeLayer(layer.Id, layer.SourceId)
            {
                //MinZoom = layer.MinZoom,
                //MaxZoom = layer.MaxZoom
            };

            UpdateLayer(result, layer);

            return result;
        }

        static void UpdateLayer(HillshadeLayer result, NxHillshadeLayer layer)
        {
            var properties = new List<PropertyValue>();

            if (layer.HillshadeAccentColor != null)
            {
                properties.Add(PropertyFactory.HillshadeAccentColor(layer.HillshadeAccentColor.ToExpression()));
            }

            if (layer.HillshadeAccentColorTransition != null)
            {
                result.HillshadeAccentColorTransition = layer.HillshadeAccentColorTransition.ToTransition();
            }

            if (layer.HillshadeExaggeration != null)
            {
                properties.Add(PropertyFactory.HillshadeExaggeration(layer.HillshadeExaggeration.ToExpression()));
            }

            if (layer.HillshadeExaggerationTransition != null)
            {
                result.HillshadeExaggerationTransition = layer.HillshadeExaggerationTransition.ToTransition();
            }

            if (layer.HillshadeHighlightColor != null)
            {
                properties.Add(PropertyFactory.HillshadeHighlightColor(layer.HillshadeHighlightColor.ToExpression()));
            }

            if (layer.HillshadeHighlightColorTransition != null)
            {
                result.HillshadeHighlightColorTransition = layer.HillshadeHighlightColorTransition.ToTransition();
            }

            if (layer.HillshadeIlluminationAnchor != null)
            {
                properties.Add(PropertyFactory.HillshadeIlluminationAnchor(layer.HillshadeIlluminationAnchor.ToExpression()));
            }

            if (layer.HillshadeIlluminationDirection != null)
            {
                properties.Add(PropertyFactory.HillshadeIlluminationDirection(layer.HillshadeIlluminationDirection.ToExpression()));
            }

            if (layer.HillshadeShadowColor != null)
            {
                properties.Add(PropertyFactory.HillshadeShadowColor(layer.HillshadeShadowColor.ToExpression()));
            }

            if (layer.HillshadeShadowColorTransition != null)
            {
                result.HillshadeShadowColorTransition = layer.HillshadeShadowColorTransition.ToTransition();
            }

            result.SetProperties(properties.ToArray());

            // TODO Add other values

            //if (layer.Filter != null)
            //{
            //    result.WithFilter(layer.Filter.ToExpression());
            //}
        }

        static HeatmapLayer ToLayer(NxHeatmapLayer layer)
        {
            if (layer == null) { return null; }

            var result = new HeatmapLayer(layer.Id, layer.SourceId)
            {
                //MinZoom = layer.MinZoom,
                //MaxZoom = layer.MaxZoom
            };

            UpdateLayer(result, layer);

            return result;
        }

        static void UpdateLayer(HeatmapLayer result, NxHeatmapLayer layer)
        {
            var properties = new List<PropertyValue>();

            if (layer.HeatmapColor != null)
            {
                properties.Add(PropertyFactory.HeatmapColor(layer.HeatmapColor.ToExpression()));
            }

            if (layer.HeatmapIntensity != null)
            {
                properties.Add(PropertyFactory.HeatmapIntensity(layer.HeatmapIntensity.ToExpression()));
            }

            if (layer.HeatmapIntensityTransition != null)
            {
                result.HeatmapIntensityTransition = layer.HeatmapIntensityTransition.ToTransition();
            }

            if (layer.HeatmapOpacity != null)
            {
                properties.Add(PropertyFactory.HeatmapOpacity(layer.HeatmapOpacity.ToExpression()));
            }

            if (layer.HeatmapOpacityTransition != null)
            {
                result.HeatmapOpacityTransition = layer.HeatmapOpacityTransition.ToTransition();
            }

            if (layer.HeatmapRadius != null)
            {
                properties.Add(PropertyFactory.HeatmapRadius(layer.HeatmapRadius.ToExpression()));
            }

            if (layer.HeatmapRadiusTransition != null)
            {
                result.HeatmapRadiusTransition = layer.HeatmapRadiusTransition.ToTransition();
            }

            if (layer.HeatmapWeight != null)
            {
                properties.Add(PropertyFactory.HeatmapWeight(layer.HeatmapWeight.ToExpression()));
            }

            result.SetProperties(properties.ToArray());

            // TODO Add other values

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToExpression());
            }
        }

        static FillLayer ToLayer(NxFillLayer layer)
        {
            if (layer == null) { return null; }

            var result = new FillLayer(layer.Id, layer.SourceId)
            {
                //MinZoom = layer.MinZoom,
                //MaxZoom = layer.MaxZoom
            };

            UpdateLayer(result, layer);

            return result;
        }

        static void UpdateLayer(FillLayer result, NxFillLayer layer)
        {
            var properties = new List<PropertyValue>();

            if (layer.FillAntialiased != null)
            {
                properties.Add(PropertyFactory.FillAntialias(layer.FillAntialiased.ToExpression()));
            }

            if (layer.FillColor != null)
            {
                properties.Add(PropertyFactory.FillColor(layer.FillColor.ToExpression()));
            }

            if (layer.FillColorTransition != null)
            {
                result.FillColorTransition = layer.FillColorTransition.ToTransition();
            }

            if (layer.FillOpacity != null)
            {
                properties.Add(PropertyFactory.FillOpacity(layer.FillOpacity.ToExpression()));
            }

            if (layer.FillOpacityTransition != null)
            {
                result.FillOpacityTransition = layer.FillOpacityTransition.ToTransition();
            }


            if (layer.FillOutlineColor != null)
            {
                properties.Add(PropertyFactory.FillOutlineColor(layer.FillOutlineColor.ToExpression()));
            }

            if (layer.FillOutlineColorTransition != null)
            {
                result.FillOutlineColorTransition = layer.FillOutlineColorTransition.ToTransition();
            }

            if (layer.FillPattern != null)
            {
                properties.Add(PropertyFactory.FillPattern(layer.FillPattern.ToExpression()));
            }

            if (layer.FillPatternTransition != null)
            {
                result.FillPatternTransition = layer.FillPatternTransition.ToTransition();
            }

            if (layer.FillTranslate != null)
            {
                properties.Add(PropertyFactory.FillTranslate(layer.FillTranslate.ToExpression()));
            }

            if (layer.FillTranslateTransition != null)
            {
                result.FillTranslateTransition = layer.FillTranslateTransition.ToTransition();
            }

            if (layer.FillTranslateAnchor != null)
            {
                properties.Add(PropertyFactory.FillTranslateAnchor(layer.FillTranslateAnchor.ToExpression()));
            }

            result.SetProperties(properties.ToArray());

            // TODO Add other values

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToExpression());
            }
        }

        static FillExtrusionLayer ToLayer(NxFillExtrusionLayer layer)
        {
            if (layer == null) { return null; }

            var result = new FillExtrusionLayer(layer.Id, layer.SourceId)
            {
                //MinZoom = layer.MinZoom,
                //MaxZoom = layer.MaxZoom
            };

            UpdateLayer(result, layer);

            return result;
        }

        static void UpdateLayer(FillExtrusionLayer result, NxFillExtrusionLayer layer)
        {
            var properties = new List<PropertyValue>();

            if (layer.FillExtrusionBase != null)
            {
                properties.Add(PropertyFactory.FillExtrusionBase(layer.FillExtrusionBase.ToExpression()));
            }

            if (layer.FillExtrusionBaseTransition != null)
            {
                result.FillExtrusionBaseTransition = layer.FillExtrusionBaseTransition.ToTransition();
            }

            if (layer.FillExtrusionColor != null)
            {
                properties.Add(PropertyFactory.FillExtrusionColor(layer.FillExtrusionColor.ToExpression()));
            }

            if (layer.FillExtrusionColorTransition != null)
            {
                result.FillExtrusionColorTransition = layer.FillExtrusionColorTransition.ToTransition();
            }

            if (layer.FillExtrusionOpacity != null)
            {
                properties.Add(PropertyFactory.FillExtrusionOpacity(layer.FillExtrusionOpacity.ToExpression()));
            }

            if (layer.FillExtrusionOpacityTransition != null)
            {
                result.FillExtrusionOpacityTransition = layer.FillExtrusionOpacityTransition.ToTransition();
            }

            if (layer.FillExtrusionHasVerticalGradient != null)
            {
                properties.Add(PropertyFactory.FillExtrusionVerticalGradient(layer.FillExtrusionHasVerticalGradient.ToExpression()));
            }

            if (layer.FillExtrusionHeight != null)
            {
                properties.Add(PropertyFactory.FillExtrusionHeight(layer.FillExtrusionHeight.ToExpression()));
            }

            if (layer.FillExtrusionHeightTransition != null)
            {
                result.FillExtrusionHeightTransition = layer.FillExtrusionHeightTransition.ToTransition();
            }

            if (layer.FillExtrusionPattern != null)
            {
                properties.Add(PropertyFactory.FillExtrusionPattern(layer.FillExtrusionPattern.ToExpression()));
            }

            if (layer.FillExtrusionPatternTransition != null)
            {
                result.FillExtrusionPatternTransition = layer.FillExtrusionPatternTransition.ToTransition();
            }

            if (layer.FillExtrusionTranslate != null)
            {
                properties.Add(PropertyFactory.FillExtrusionTranslate(layer.FillExtrusionTranslate.ToExpression()));
            }

            if (layer.FillExtrusionTranslateTransition != null)
            {
                result.FillExtrusionTranslateTransition = layer.FillExtrusionTranslateTransition.ToTransition();
            }

            if (layer.FillExtrusionTranslateAnchor != null)
            {
                properties.Add(PropertyFactory.FillExtrusionTranslateAnchor(layer.FillExtrusionTranslateAnchor.ToExpression()));
            }

            result.SetProperties(properties.ToArray());

            // TODO Add other values

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToExpression());
            }
        }

        static CircleLayer ToLayer(NxCircleLayer layer)
        {
            if (layer == null) { return null; }

            var result = new CircleLayer(layer.Id, layer.SourceId)
            {
                //MinZoom = layer.MinZoom,
                //MaxZoom = layer.MaxZoom
            };

            UpdateLayer(result, layer);

            return result;
        }

        static void UpdateLayer(CircleLayer result, NxCircleLayer layer)
        {
            var properties = new List<PropertyValue>();

            if (layer.CircleBlur != null)
            {
                properties.Add(PropertyFactory.CircleBlur(layer.CircleBlur.ToExpression()));
            }

            if (layer.CircleBlurTransition != null)
            {
                result.CircleBlurTransition = layer.CircleBlurTransition.ToTransition();
            }

            if (layer.CircleColor != null)
            {
                properties.Add(PropertyFactory.CircleColor(layer.CircleColor.ToExpression()));
            }

            if (layer.CircleColorTransition != null)
            {
                result.CircleColorTransition = layer.CircleColorTransition.ToTransition();
            }

            if (layer.CircleOpacity != null)
            {
                properties.Add(PropertyFactory.CircleOpacity(layer.CircleOpacity.ToExpression()));
            }

            if (layer.CircleOpacityTransition != null)
            {
                result.CircleOpacityTransition = layer.CircleOpacityTransition.ToTransition();
            }

            if (layer.CirclePitchAlignment != null)
            {
                properties.Add(PropertyFactory.CirclePitchAlignment(layer.CirclePitchAlignment.ToExpression()));
            }

            if (layer.CirclePitchScale != null)
            {
                properties.Add(PropertyFactory.CirclePitchScale(layer.CirclePitchScale.ToExpression()));
            }

            if (layer.CircleRadius != null)
            {
                properties.Add(PropertyFactory.CircleRadius(layer.CircleRadius.ToExpression()));
            }

            if (layer.CircleRadiusTransition != null)
            {
                result.CircleRadiusTransition = layer.CircleRadiusTransition.ToTransition();
            }

            if (layer.CircleStrokeColor != null)
            {
                properties.Add(PropertyFactory.CircleStrokeColor(layer.CircleStrokeColor.ToExpression()));
            }

            if (layer.CircleStrokeColorTransition != null)
            {
                result.CircleStrokeColorTransition = layer.CircleStrokeColorTransition.ToTransition();
            }

            if (layer.CircleStrokeOpacity != null)
            {
                properties.Add(PropertyFactory.CircleStrokeOpacity(layer.CircleStrokeOpacity.ToExpression()));
            }

            if (layer.CircleStrokeOpacityTransition != null)
            {
                result.CircleStrokeOpacityTransition = layer.CircleStrokeOpacityTransition.ToTransition();
            }

            if (layer.CircleStrokeWidth != null)
            {
                properties.Add(PropertyFactory.CircleStrokeWidth(layer.CircleStrokeWidth.ToExpression()));
            }

            if (layer.CircleStrokeWidthTransition != null)
            {
                result.CircleStrokeWidthTransition = layer.CircleStrokeWidthTransition.ToTransition();
            }

            if (layer.CircleTranslate != null)
            {
                properties.Add(PropertyFactory.CircleTranslate(layer.CircleTranslate.ToExpression()));
            }

            if (layer.CircleTranslateTransition != null)
            {
                result.CircleTranslateTransition = layer.CircleTranslateTransition.ToTransition();
            }

            if (layer.CircleTranslateAnchor != null)
            {
                properties.Add(PropertyFactory.CircleTranslateAnchor(layer.CircleTranslateAnchor.ToExpression()));
            }

            result.SetProperties(properties.ToArray());

            // TODO Add other values

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToExpression());
            }
        }

        static TransitionOptions ToTransition(this Naxam.Mapbox.TransitionOptions options)
        {
            return new TransitionOptions(
                    options.Delay,
                    options.Duration,
                    options.IsEnablePlacementTransitions
                    );
        }
    }
}
