using Foundation;
using Mapbox;
using Naxam.Mapbox.Layers;
using Xamarin.Forms.Platform.iOS;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class LayerExtensions
    {
        public static MGLStyleLayer ToLayer(this Layer layer, MGLSource source)
        {
            MGLStyleLayer result = null;
            var id = layer.Id;

            switch (layer)
            {
                case CircleLayer circleLayer:
                    return ToLayer(circleLayer, source);

                case LineLayer lineLayer:
                    {
                        var newLayer = new MGLLineStyleLayer(id, source)
                        {
                            LineWidth = NSExpression.FromConstant(NSNumber.FromDouble(lineLayer.LineWidth)),
                            LineColor = NSExpression.FromConstant(lineLayer.LineColor.ToUIColor())
                        };
                        if (lineLayer.Dashes != null && lineLayer.Dashes.Length != 0)
                        {
                            var arr = new NSMutableArray<NSNumber>();
                            foreach (double dash in lineLayer.Dashes)
                            {
                                arr.Add(NSNumber.FromDouble(dash));
                            }
                            newLayer.LineDashPattern = NSExpression.FromConstant(arr);
                        }
                        //TODO lineCap
                        return newLayer;
                    }

                case FillLayer fl:
                    {
                        var newLayer = new MGLFillStyleLayer(id, source)
                        {
                            FillColor = NSExpression.FromConstant(fl.FillColor.ToUIColor()),
                            FillOpacity = NSExpression.FromConstant(NSNumber.FromDouble(fl.FillOpacity))
                        };
                        return newLayer;
                    }

                case SymbolLayer symbolLayer:
                    {
                        var newLayer = new MGLSymbolStyleLayer(id, source);

                        if (symbolLayer.IconImage != null)
                        {
                            // TODO Need to ensure ID if it's a local resource
                            newLayer.IconImageName = symbolLayer.IconImage.ToExpression();
                        }

                        if (symbolLayer.IconSize != null)
                        {
                            // TODO iOS - Name mitmatched: IconSize vs. IconScale
                            newLayer.IconScale = symbolLayer.IconSize.ToExpression();
                        }

                        if (symbolLayer.IconColor != null)
                        {
                            var color = symbolLayer.IconColor.ToExpression();
                            newLayer.IconColor = color;
                        }

                        if (symbolLayer.IconColor != null)
                        {
                            var color = symbolLayer.IconColor.ToExpression();
                            newLayer.IconColor = color;
                        }

                        if (symbolLayer.TextAllowOverlap != null)
                        {
                            newLayer.TextAllowsOverlap = symbolLayer.TextAllowOverlap.ToExpression();
                        }

                        if (symbolLayer.TextColor != null)
                        {
                            newLayer.TextColor = symbolLayer.TextColor.ToExpression();
                        }

                        if (symbolLayer.TextField != null)
                        {
                            newLayer.Text = symbolLayer.TextField.ToExpression();
                        }

                        if (symbolLayer.TextIgnorePlacement != null)
                        {
                            newLayer.TextIgnoresPlacement = symbolLayer.TextIgnorePlacement.ToExpression();
                        }

                        if (symbolLayer.TextSize != null)
                        {
                            newLayer.TextFontSize = symbolLayer.TextSize.ToExpression();
                        }

                        // TODO Add other properties

                        if (symbolLayer.Filter != null)
                        {
                            newLayer.Predicate = symbolLayer.Filter.ToPredicate();
                        }

                        return newLayer;
                    }

                case RasterLayer rl:
                    {

                        var newLayer = new MGLRasterStyleLayer(id, source);
                        return newLayer;
                    }
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
