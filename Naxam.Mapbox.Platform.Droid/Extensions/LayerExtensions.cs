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
using NxStyleLayer = Naxam.Mapbox.Layers.StyleLayer;
using NxLineLayer = Naxam.Mapbox.Layers.LineLayer;
using NxBackgroundLayer = Naxam.Mapbox.Layers.BackgroundLayer;
using NxRasterLayer = Naxam.Mapbox.Layers.RasterLayer;
using Naxam.Mapbox.Platform.Droid.Extensions;
using System.Collections.Generic;
using Naxam.Mapbox.Expressions;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    partial class LayerExtensions
    {
        public static NxStyleLayer ToForms(this Layer layer)
        {
            switch (layer)
            {
                case SymbolLayer symbolLayer:
                    return new NxSymbolLayer(layer.Id, symbolLayer.SourceId);
                case RasterLayer rasterLayer:
                    return new NxRasterLayer(layer.Id, rasterLayer.SourceId);
                case FillLayer fillLayer:
                    return new NxFillLayer(layer.Id, fillLayer.SourceId);
                case FillExtrusionLayer fillExtrusionLayer:
                    return new NxFillExtrusionLayer(layer.Id, fillExtrusionLayer.SourceId);
                case HillshadeLayer hillshadeLayer:
                    return new NxHillshadeLayer(layer.Id, hillshadeLayer.SourceId);
                case CircleLayer circleLayer:
                    return new NxCircleLayer(layer.Id, circleLayer.SourceId);
                case HeatmapLayer heatmapLayer:
                    return new NxHeatmapLayer(layer.Id, heatmapLayer.SourceId);
                case LineLayer lineLayer:
                    return new NxLineLayer(layer.Id, lineLayer.SourceId);
            }
            
            return null;
        }
    }
    
    public static partial class LayerExtensions
    {
        public static Layer ToNative(this NxLayer layer)
        {
            switch (layer)
            {
                case NxCircleLayer circleLayer:
                    return ToNative(circleLayer);
                case NxFillExtrusionLayer fillExtrusionLayer:
                    return ToNative(fillExtrusionLayer);
                case NxFillLayer fillLayer:
                    return ToNative(fillLayer);
                case NxHillshadeLayer hillshadeLayer:
                    return ToNative(hillshadeLayer);
                case NxForegroundLayer foregroundLayer:
                    // TODO Android - No ForegroundLayer
                    return null;
                case NxHeatmapLayer heatmapLayer:
                    return ToNative(heatmapLayer);
                case NxLineLayer lineLayer:
                    return ToNative(lineLayer);

                case NxRasterLayer rasterLayer:
                    return ToNative(rasterLayer);

                case NxSymbolLayer symbolLayer:
                    return ToNative(symbolLayer);
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

        static SymbolLayer ToNative(NxSymbolLayer layer)
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

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.WithSourceLayer(layer.SourceLayer);
            }
            
            if (layer.Visibility is ExpressionVisibility visibility)
            {
                properties.Add(PropertyFactory.Visibility((string)visibility.GetValue()));
            }

            if (layer.IconAllowOverlap != null)
            {
                properties.Add(PropertyFactory.IconAllowOverlap(layer.IconAllowOverlap.ToNative()));
            }

            if (layer.IconAnchor != null)
            {
                properties.Add(PropertyFactory.IconAnchor(layer.IconAnchor.ToNative()));
            }

            if (layer.IconColor != null)
            {
                properties.Add(PropertyFactory.IconColor(layer.IconColor.ToNative()));
            }

            if (layer.IconColorTransition != null)
            {
                result.IconColorTransition = layer.IconColorTransition.ToNative();
            }

            if (layer.IconHaloBlur != null)
            {
                properties.Add(PropertyFactory.IconHaloBlur(layer.IconHaloBlur.ToNative()));
            }

            if (layer.IconHaloBlurTransition != null)
            {
                result.IconHaloBlurTransition = layer.IconHaloBlurTransition.ToNative();
            }

            if (layer.IconHaloColor != null)
            {
                properties.Add(PropertyFactory.IconHaloColor(layer.IconHaloColor.ToNative()));
            }

            if (layer.IconHaloColorTransition != null)
            {
                result.IconHaloColorTransition = layer.IconHaloColorTransition.ToNative();
            }

            if (layer.IconHaloWidth != null)
            {
                properties.Add(PropertyFactory.LineDasharray(layer.IconHaloWidth.ToNative()));
            }

            if (layer.IconHaloWidthTransition != null)
            {
                result.IconHaloWidthTransition = layer.IconHaloWidthTransition.ToNative();
            }

            if (layer.IconIgnorePlacement != null)
            {
                properties.Add(PropertyFactory.IconIgnorePlacement(layer.IconIgnorePlacement.ToNative()));
            }

            if (layer.IconImage != null)
            {
                properties.Add(PropertyFactory.IconImage(layer.IconImage.ToNative()));
            }

            if (layer.IconKeepUpright != null)
            {
                properties.Add(PropertyFactory.IconKeepUpright(layer.IconKeepUpright.ToNative()));
            }

            if (layer.IconOffset != null)
            {
                properties.Add(PropertyFactory.IconOffset(layer.IconOffset.ToNative()));
            }

            if (layer.IconOpacity != null)
            {
                properties.Add(PropertyFactory.IconOpacity(layer.IconOpacity.ToNative()));
            }

            if (layer.IconOpacityTransition != null)
            {
                result.IconOpacityTransition = layer.IconOpacityTransition.ToNative();
            }

            if (layer.IconOptional != null)
            {
                properties.Add(PropertyFactory.IconOptional(layer.IconOptional.ToNative()));
            }

            if (layer.IconPadding != null)
            {
                properties.Add(PropertyFactory.IconPadding(layer.IconPadding.ToNative()));
            }

            if (layer.IconPitchAlignment != null)
            {
                properties.Add(PropertyFactory.IconPitchAlignment(layer.IconPitchAlignment.ToNative()));
            }

            if (layer.IconRotate != null)
            {
                properties.Add(PropertyFactory.IconRotate(layer.IconRotate.ToNative()));
            }

            if (layer.IconRotationAlignment != null)
            {
                properties.Add(PropertyFactory.IconRotationAlignment(layer.IconRotationAlignment.ToNative()));
            }

            if (layer.IconSize != null)
            {
                properties.Add(PropertyFactory.IconSize(layer.IconSize.ToNative()));
            }

            if (layer.IconTextFit != null)
            {
                properties.Add(PropertyFactory.IconTextFit(layer.IconTextFit.ToNative()));
            }

            if (layer.IconTextFitPadding != null)
            {
                properties.Add(PropertyFactory.IconTextFitPadding(layer.IconTextFitPadding.ToNative()));
            }

            if (layer.IconTranslate != null)
            {
                properties.Add(PropertyFactory.IconTranslate(layer.IconTranslate.ToNative()));
            }

            if (layer.IconTranslateAnchor != null)
            {
                properties.Add(PropertyFactory.IconTranslateAnchor(layer.IconTranslateAnchor.ToNative()));
            }

            if (layer.IconTranslateTransition != null)
            {
                result.IconTranslateTransition = layer.IconTranslateTransition.ToNative();
            }
            if (layer.SymbolAvoidEdges != null)
            {
                properties.Add(PropertyFactory.SymbolAvoidEdges(layer.SymbolAvoidEdges.ToNative()));
            }

            if (layer.SymbolPlacement != null)
            {
                properties.Add(PropertyFactory.SymbolPlacement(layer.SymbolPlacement.ToNative()));
            }

            if (layer.SymbolSortKey != null)
            {
                properties.Add(PropertyFactory.SymbolSortKey(layer.SymbolSortKey.ToNative()));
            }

            if (layer.SymbolSpacing != null)
            {
                properties.Add(PropertyFactory.SymbolSpacing(layer.SymbolSpacing.ToNative()));
            }

            if (layer.SymbolZOrder != null)
            {
                properties.Add(PropertyFactory.SymbolZOrder(layer.SymbolZOrder.ToNative()));
            }

            if (layer.TextAllowOverlap != null)
            {
                properties.Add(PropertyFactory.TextAllowOverlap(layer.TextAllowOverlap.ToNative()));
            }

            if (layer.TextAnchor != null)
            {
                properties.Add(PropertyFactory.TextAnchor(layer.TextAnchor.ToNative()));
            }

            if (layer.TextColor != null)
            {
                properties.Add(PropertyFactory.TextColor(layer.TextColor.ToNative()));
            }

            if (layer.TextColorTransition != null)
            {
                result.TextColorTransition = layer.TextColorTransition.ToNative();
            }

            if (layer.TextField != null)
            {
                properties.Add(PropertyFactory.TextField(layer.TextField.ToNative()));
            }

            if (layer.TextFont != null)
            {
                properties.Add(PropertyFactory.TextFont(layer.TextFont.ToNative()));
            }

            if (layer.TextHaloColor != null)
            {
                properties.Add(PropertyFactory.TextHaloColor(layer.TextHaloColor.ToNative()));
            }

            if (layer.TextHaloColorTransition != null)
            {
                result.TextHaloColorTransition = layer.TextHaloColorTransition.ToNative();
            }

            if (layer.TextIgnorePlacement != null)
            {
                properties.Add(PropertyFactory.TextIgnorePlacement(layer.TextIgnorePlacement.ToNative()));
            }

            if (layer.TextJustify != null)
            {
                properties.Add(PropertyFactory.TextJustify(layer.TextJustify.ToNative()));
            }

            if (layer.TextKeepUpright != null)
            {
                properties.Add(PropertyFactory.TextKeepUpright(layer.TextKeepUpright.ToNative()));
            }

            if (layer.TextLetterSpacing != null)
            {
                properties.Add(PropertyFactory.TextLetterSpacing(layer.TextLetterSpacing.ToNative()));
            }

            if (layer.TextLineHeight != null)
            {
                properties.Add(PropertyFactory.TextLineHeight(layer.TextLineHeight.ToNative()));
            }

            if (layer.TextMaxAngle != null)
            {
                properties.Add(PropertyFactory.TextMaxAngle(layer.TextMaxAngle.ToNative()));
            }

            if (layer.TextMaxWidth != null)
            {
                properties.Add(PropertyFactory.TextMaxWidth(layer.TextMaxWidth.ToNative()));
            }

            if (layer.TextOffset != null)
            {
                properties.Add(PropertyFactory.TextOffset(layer.TextOffset.ToNative()));
            }

            if (layer.TextOpacity != null)
            {
                properties.Add(PropertyFactory.TextOpacity(layer.TextOpacity.ToNative()));
            }

            if (layer.TextOpacityTransition != null)
            {
                result.TextOpacityTransition = layer.TextOpacityTransition.ToNative();
            }

            if (layer.TextOptional != null)
            {
                properties.Add(PropertyFactory.TextOptional(layer.TextOptional.ToNative()));
            }

            if (layer.TextPadding != null)
            {
                properties.Add(PropertyFactory.TextPadding(layer.TextPadding.ToNative()));
            }

            if (layer.TextPitchAlignment != null)
            {
                properties.Add(PropertyFactory.TextPitchAlignment(layer.TextPitchAlignment.ToNative()));
            }

            if (layer.TextRadialOffset != null)
            {
                properties.Add(PropertyFactory.TextRadialOffset(layer.TextRadialOffset.ToNative()));
            }

            if (layer.TextRotate != null)
            {
                properties.Add(PropertyFactory.TextRotate(layer.TextRotate.ToNative()));
            }

            if (layer.TextRotationAlignment != null)
            {
                properties.Add(PropertyFactory.TextRotationAlignment(layer.TextRotationAlignment.ToNative()));
            }

            if (layer.TextSize != null)
            {
                properties.Add(PropertyFactory.TextSize(layer.TextSize.ToNative()));
            }

            if (layer.TextTransform != null)
            {
                properties.Add(PropertyFactory.TextTransform(layer.TextTransform.ToNative()));
            }

            if (layer.TextTranslate != null)
            {
                properties.Add(PropertyFactory.TextTranslate(layer.TextTranslate.ToNative()));
            }

            if (layer.TextTranslateAnchor != null)
            {
                properties.Add(PropertyFactory.TextTranslateAnchor(layer.TextTranslateAnchor.ToNative()));
            }

            if (layer.TextTranslateTransition != null)
            {
                result.TextTranslateTransition = layer.TextTranslateTransition.ToNative();
            }

            if (layer.TextVariableAnchor != null)
            {
                properties.Add(PropertyFactory.TextVariableAnchor(layer.TextVariableAnchor.ToNative()));
            }

            if (layer.TextWritingMode != null)
            {
                properties.Add(PropertyFactory.TextWritingMode(layer.TextWritingMode.ToNative()));
            }

            result.SetProperties(properties.ToArray());

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToNative());
            }
        }

        static RasterLayer ToNative(NxRasterLayer layer)
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

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.WithSourceLayer(layer.SourceLayer);
            }
            
            if (layer.Visibility is ExpressionVisibility visibility)
            {
                properties.Add(PropertyFactory.Visibility((string)visibility.GetValue()));
            }

            if (layer.RasterBrightnessMax != null)
            {
                properties.Add(PropertyFactory.RasterBrightnessMax(layer.RasterBrightnessMax.ToNative()));
            }

            if (layer.RasterBrightnessMaxTransition != null)
            {
                result.RasterBrightnessMaxTransition = layer.RasterBrightnessMaxTransition.ToNative();
            }

            if (layer.RasterBrightnessMin != null)
            {
                properties.Add(PropertyFactory.RasterBrightnessMin(layer.RasterBrightnessMin.ToNative()));
            }

            if (layer.RasterBrightnessMinTransition != null)
            {
                result.RasterBrightnessMinTransition = layer.RasterBrightnessMinTransition.ToNative();
            }

            if (layer.RasterContrast != null)
            {
                properties.Add(PropertyFactory.RasterContrast(layer.RasterContrast.ToNative()));
            }

            if (layer.RasterContrastTransition != null)
            {
                result.RasterContrastTransition = layer.RasterContrastTransition.ToNative();
            }

            if (layer.RasterFadeDuration != null)
            {
                properties.Add(PropertyFactory.RasterFadeDuration(layer.RasterFadeDuration.ToNative()));
            }

            if (layer.RasterHueRotate != null)
            {
                properties.Add(PropertyFactory.RasterHueRotate(layer.RasterHueRotate.ToNative()));
            }

            if (layer.RasterHueRotateTransition != null)
            {
                result.RasterHueRotateTransition = layer.RasterHueRotateTransition.ToNative();
            }

            if (layer.RasterOpacity != null)
            {
                properties.Add(PropertyFactory.RasterOpacity(layer.RasterOpacity.ToNative()));
            }

            if (layer.RasterOpacityTransition != null)
            {
                result.RasterOpacityTransition = layer.RasterOpacityTransition.ToNative();
            }

            if (layer.RasterResampling != null)
            {
                properties.Add(PropertyFactory.RasterResampling(layer.RasterResampling.ToNative()));
            }

            if (layer.RasterSaturation != null)
            {
                properties.Add(PropertyFactory.RasterSaturation(layer.RasterSaturation.ToNative()));
            }

            if (layer.RasterSaturationTransition != null)
            {
                result.RasterSaturationTransition = layer.RasterSaturationTransition.ToNative();
            }

            result.SetProperties(properties.ToArray());

            //if (layer.Filter != null)
            //{
            //    result.WithFilter(layer.Filter.ToExpression());
            //}
        }

        static LineLayer ToNative(NxLineLayer layer)
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

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.WithSourceLayer(layer.SourceLayer);
            }
            
            if (layer.Visibility is ExpressionVisibility visibility)
            {
                properties.Add(PropertyFactory.Visibility((string)visibility.GetValue()));
            }

            if (layer.LineBlur != null)
            {
                properties.Add(PropertyFactory.LineBlur(layer.LineBlur.ToNative()));
            }

            if (layer.LineBlurTransition != null)
            {
                result.LineBlurTransition = layer.LineBlurTransition.ToNative();
            }

            if (layer.LineCap != null)
            {
                properties.Add(PropertyFactory.LineCap(layer.LineCap.ToNative()));
            }

            if (layer.LineColor != null)
            {
                properties.Add(PropertyFactory.LineColor(layer.LineColor.ToNative()));
            }

            if (layer.LineColorTransition != null)
            {
                result.LineColorTransition = layer.LineColorTransition.ToNative();
            }

            if (layer.LineDasharray != null)
            {
                properties.Add(PropertyFactory.LineDasharray(layer.LineDasharray.ToNative()));
            }

            if (layer.LineDasharrayTransition != null)
            {
                result.LineDasharrayTransition = layer.LineDasharrayTransition.ToNative();
            }

            if (layer.LineGapWidth != null)
            {
                properties.Add(PropertyFactory.LineGapWidth(layer.LineGapWidth.ToNative()));
            }

            if (layer.LineGapWidthTransition != null)
            {
                result.LineGapWidthTransition = layer.LineGapWidthTransition.ToNative();
            }

            if (layer.LineGradient != null)
            {
                properties.Add(PropertyFactory.LineGradient(layer.LineGradient.ToNative()));
            }

            if (layer.LineJoin != null)
            {
                properties.Add(PropertyFactory.LineJoin(layer.LineJoin.ToNative()));
            }

            if (layer.LineWidth != null)
            {
                properties.Add(PropertyFactory.LineWidth(layer.LineWidth.ToNative()));
            }

            if (layer.LineWidthTransition != null)
            {
                result.LineWidthTransition = layer.LineWidthTransition.ToNative();
            }

            result.SetProperties(properties.ToArray());

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToNative());
            }

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.WithSourceLayer(layer.SourceLayer);
            }
        }

        static HillshadeLayer ToNative(NxHillshadeLayer layer)
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

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.WithSourceLayer(layer.SourceLayer);
            }
            
            if (layer.Visibility is ExpressionVisibility visibility)
            {
                properties.Add(PropertyFactory.Visibility((string)visibility.GetValue()));
            }

            if (layer.HillshadeAccentColor != null)
            {
                properties.Add(PropertyFactory.HillshadeAccentColor(layer.HillshadeAccentColor.ToNative()));
            }

            if (layer.HillshadeAccentColorTransition != null)
            {
                result.HillshadeAccentColorTransition = layer.HillshadeAccentColorTransition.ToNative();
            }

            if (layer.HillshadeExaggeration != null)
            {
                properties.Add(PropertyFactory.HillshadeExaggeration(layer.HillshadeExaggeration.ToNative()));
            }

            if (layer.HillshadeExaggerationTransition != null)
            {
                result.HillshadeExaggerationTransition = layer.HillshadeExaggerationTransition.ToNative();
            }

            if (layer.HillshadeHighlightColor != null)
            {
                properties.Add(PropertyFactory.HillshadeHighlightColor(layer.HillshadeHighlightColor.ToNative()));
            }

            if (layer.HillshadeHighlightColorTransition != null)
            {
                result.HillshadeHighlightColorTransition = layer.HillshadeHighlightColorTransition.ToNative();
            }

            if (layer.HillshadeIlluminationAnchor != null)
            {
                properties.Add(PropertyFactory.HillshadeIlluminationAnchor(layer.HillshadeIlluminationAnchor.ToNative()));
            }

            if (layer.HillshadeIlluminationDirection != null)
            {
                properties.Add(PropertyFactory.HillshadeIlluminationDirection(layer.HillshadeIlluminationDirection.ToNative()));
            }

            if (layer.HillshadeShadowColor != null)
            {
                properties.Add(PropertyFactory.HillshadeShadowColor(layer.HillshadeShadowColor.ToNative()));
            }

            if (layer.HillshadeShadowColorTransition != null)
            {
                result.HillshadeShadowColorTransition = layer.HillshadeShadowColorTransition.ToNative();
            }

            result.SetProperties(properties.ToArray());

            // TODO Add other values

            //if (layer.Filter != null)
            //{
            //    result.WithFilter(layer.Filter.ToExpression());
            //}
        }

        static HeatmapLayer ToNative(NxHeatmapLayer layer)
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

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.WithSourceLayer(layer.SourceLayer);
            }
            
            if (layer.Visibility is ExpressionVisibility visibility)
            {
                properties.Add(PropertyFactory.Visibility((string)visibility.GetValue()));
            }

            if (layer.HeatmapColor != null)
            {
                properties.Add(PropertyFactory.HeatmapColor(layer.HeatmapColor.ToNative()));
            }

            if (layer.HeatmapIntensity != null)
            {
                properties.Add(PropertyFactory.HeatmapIntensity(layer.HeatmapIntensity.ToNative()));
            }

            if (layer.HeatmapIntensityTransition != null)
            {
                result.HeatmapIntensityTransition = layer.HeatmapIntensityTransition.ToNative();
            }

            if (layer.HeatmapOpacity != null)
            {
                properties.Add(PropertyFactory.HeatmapOpacity(layer.HeatmapOpacity.ToNative()));
            }

            if (layer.HeatmapOpacityTransition != null)
            {
                result.HeatmapOpacityTransition = layer.HeatmapOpacityTransition.ToNative();
            }

            if (layer.HeatmapRadius != null)
            {
                properties.Add(PropertyFactory.HeatmapRadius(layer.HeatmapRadius.ToNative()));
            }

            if (layer.HeatmapRadiusTransition != null)
            {
                result.HeatmapRadiusTransition = layer.HeatmapRadiusTransition.ToNative();
            }

            if (layer.HeatmapWeight != null)
            {
                properties.Add(PropertyFactory.HeatmapWeight(layer.HeatmapWeight.ToNative()));
            }

            result.SetProperties(properties.ToArray());

            // TODO Add other values

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToNative());
            }
        }

        static FillLayer ToNative(NxFillLayer layer)
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

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.WithSourceLayer(layer.SourceLayer);
            }

            if (layer.Visibility is ExpressionVisibility visibility)
            {
                properties.Add(PropertyFactory.Visibility((string)visibility.GetValue()));
            }

            if (layer.FillAntialiased != null)
            {
                properties.Add(PropertyFactory.FillAntialias(layer.FillAntialiased.ToNative()));
            }

            if (layer.FillColor != null)
            {
                properties.Add(PropertyFactory.FillColor(layer.FillColor.ToNative()));
            }

            if (layer.FillColorTransition != null)
            {
                result.FillColorTransition = layer.FillColorTransition.ToNative();
            }

            if (layer.FillOpacity != null)
            {
                properties.Add(PropertyFactory.FillOpacity(layer.FillOpacity.ToNative()));
            }

            if (layer.FillOpacityTransition != null)
            {
                result.FillOpacityTransition = layer.FillOpacityTransition.ToNative();
            }


            if (layer.FillOutlineColor != null)
            {
                properties.Add(PropertyFactory.FillOutlineColor(layer.FillOutlineColor.ToNative()));
            }

            if (layer.FillOutlineColorTransition != null)
            {
                result.FillOutlineColorTransition = layer.FillOutlineColorTransition.ToNative();
            }

            if (layer.FillPattern != null)
            {
                properties.Add(PropertyFactory.FillPattern(layer.FillPattern.ToNative()));
            }

            if (layer.FillPatternTransition != null)
            {
                result.FillPatternTransition = layer.FillPatternTransition.ToNative();
            }

            if (layer.FillTranslate != null)
            {
                properties.Add(PropertyFactory.FillTranslate(layer.FillTranslate.ToNative()));
            }

            if (layer.FillTranslateTransition != null)
            {
                result.FillTranslateTransition = layer.FillTranslateTransition.ToNative();
            }

            if (layer.FillTranslateAnchor != null)
            {
                properties.Add(PropertyFactory.FillTranslateAnchor(layer.FillTranslateAnchor.ToNative()));
            }

            result.SetProperties(properties.ToArray());

            // TODO Add other values

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToNative());
            }
        }

        static FillExtrusionLayer ToNative(NxFillExtrusionLayer layer)
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

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.WithSourceLayer(layer.SourceLayer);
            }

            if (layer.Visibility is ExpressionVisibility visibility)
            {
                properties.Add(PropertyFactory.Visibility((string)visibility.GetValue()));
            }

            if (layer.FillExtrusionBase != null)
            {
                properties.Add(PropertyFactory.FillExtrusionBase(layer.FillExtrusionBase.ToNative()));
            }

            if (layer.FillExtrusionBaseTransition != null)
            {
                result.FillExtrusionBaseTransition = layer.FillExtrusionBaseTransition.ToNative();
            }

            if (layer.FillExtrusionColor != null)
            {
                properties.Add(PropertyFactory.FillExtrusionColor(layer.FillExtrusionColor.ToNative()));
            }

            if (layer.FillExtrusionColorTransition != null)
            {
                result.FillExtrusionColorTransition = layer.FillExtrusionColorTransition.ToNative();
            }

            if (layer.FillExtrusionOpacity != null)
            {
                properties.Add(PropertyFactory.FillExtrusionOpacity(layer.FillExtrusionOpacity.ToNative()));
            }

            if (layer.FillExtrusionOpacityTransition != null)
            {
                result.FillExtrusionOpacityTransition = layer.FillExtrusionOpacityTransition.ToNative();
            }

            if (layer.FillExtrusionHasVerticalGradient != null)
            {
                properties.Add(PropertyFactory.FillExtrusionVerticalGradient(layer.FillExtrusionHasVerticalGradient.ToNative()));
            }

            if (layer.FillExtrusionHeight != null)
            {
                properties.Add(PropertyFactory.FillExtrusionHeight(layer.FillExtrusionHeight.ToNative()));
            }

            if (layer.FillExtrusionHeightTransition != null)
            {
                result.FillExtrusionHeightTransition = layer.FillExtrusionHeightTransition.ToNative();
            }

            if (layer.FillExtrusionPattern != null)
            {
                properties.Add(PropertyFactory.FillExtrusionPattern(layer.FillExtrusionPattern.ToNative()));
            }

            if (layer.FillExtrusionPatternTransition != null)
            {
                result.FillExtrusionPatternTransition = layer.FillExtrusionPatternTransition.ToNative();
            }

            if (layer.FillExtrusionTranslate != null)
            {
                properties.Add(PropertyFactory.FillExtrusionTranslate(layer.FillExtrusionTranslate.ToNative()));
            }

            if (layer.FillExtrusionTranslateTransition != null)
            {
                result.FillExtrusionTranslateTransition = layer.FillExtrusionTranslateTransition.ToNative();
            }

            if (layer.FillExtrusionTranslateAnchor != null)
            {
                properties.Add(PropertyFactory.FillExtrusionTranslateAnchor(layer.FillExtrusionTranslateAnchor.ToNative()));
            }

            result.SetProperties(properties.ToArray());

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToNative());
            }
        }

        static CircleLayer ToNative(NxCircleLayer layer)
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

            if (string.IsNullOrWhiteSpace(layer.SourceLayer) == false)
            {
                result.WithSourceLayer(layer.SourceLayer);
            }

            if (layer.Visibility is ExpressionVisibility visibility)
            {
                properties.Add(PropertyFactory.Visibility((string)visibility.GetValue()));
            }

            if (layer.CircleBlur != null)
            {
                properties.Add(PropertyFactory.CircleBlur(layer.CircleBlur.ToNative()));
            }

            if (layer.CircleBlurTransition != null)
            {
                result.CircleBlurTransition = layer.CircleBlurTransition.ToNative();
            }

            if (layer.CircleColor != null)
            {
                properties.Add(PropertyFactory.CircleColor(layer.CircleColor.ToNative()));
            }

            if (layer.CircleColorTransition != null)
            {
                result.CircleColorTransition = layer.CircleColorTransition.ToNative();
            }

            if (layer.CircleOpacity != null)
            {
                properties.Add(PropertyFactory.CircleOpacity(layer.CircleOpacity.ToNative()));
            }

            if (layer.CircleOpacityTransition != null)
            {
                result.CircleOpacityTransition = layer.CircleOpacityTransition.ToNative();
            }

            if (layer.CirclePitchAlignment != null)
            {
                properties.Add(PropertyFactory.CirclePitchAlignment(layer.CirclePitchAlignment.ToNative()));
            }

            if (layer.CirclePitchScale != null)
            {
                properties.Add(PropertyFactory.CirclePitchScale(layer.CirclePitchScale.ToNative()));
            }

            if (layer.CircleRadius != null)
            {
                properties.Add(PropertyFactory.CircleRadius(layer.CircleRadius.ToNative()));
            }

            if (layer.CircleRadiusTransition != null)
            {
                result.CircleRadiusTransition = layer.CircleRadiusTransition.ToNative();
            }

            if (layer.CircleStrokeColor != null)
            {
                properties.Add(PropertyFactory.CircleStrokeColor(layer.CircleStrokeColor.ToNative()));
            }

            if (layer.CircleStrokeColorTransition != null)
            {
                result.CircleStrokeColorTransition = layer.CircleStrokeColorTransition.ToNative();
            }

            if (layer.CircleStrokeOpacity != null)
            {
                properties.Add(PropertyFactory.CircleStrokeOpacity(layer.CircleStrokeOpacity.ToNative()));
            }

            if (layer.CircleStrokeOpacityTransition != null)
            {
                result.CircleStrokeOpacityTransition = layer.CircleStrokeOpacityTransition.ToNative();
            }

            if (layer.CircleStrokeWidth != null)
            {
                properties.Add(PropertyFactory.CircleStrokeWidth(layer.CircleStrokeWidth.ToNative()));
            }

            if (layer.CircleStrokeWidthTransition != null)
            {
                result.CircleStrokeWidthTransition = layer.CircleStrokeWidthTransition.ToNative();
            }

            if (layer.CircleTranslate != null)
            {
                properties.Add(PropertyFactory.CircleTranslate(layer.CircleTranslate.ToNative()));
            }

            if (layer.CircleTranslateTransition != null)
            {
                result.CircleTranslateTransition = layer.CircleTranslateTransition.ToNative();
            }

            if (layer.CircleTranslateAnchor != null)
            {
                properties.Add(PropertyFactory.CircleTranslateAnchor(layer.CircleTranslateAnchor.ToNative()));
            }

            result.SetProperties(properties.ToArray());

            // TODO Add other values

            if (layer.Filter != null)
            {
                result.WithFilter(layer.Filter.ToNative());
            }
        }

        public static TransitionOptions ToNative(this Naxam.Mapbox.TransitionOptions options)
        {
            return new TransitionOptions(
                    options.Delay,
                    options.Duration,
                    options.IsEnablePlacementTransitions
                    );
        }
    }
}
