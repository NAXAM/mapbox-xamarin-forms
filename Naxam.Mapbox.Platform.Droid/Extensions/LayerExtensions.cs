using Sdk = Com.Mapbox.Mapboxsdk;
using Xamarin.Forms.Platform.Android;

using Com.Mapbox.Mapboxsdk.Style.Layers;
using NxLayer = Naxam.Mapbox.Layers.Layer;
using NxCircleLayer = Naxam.Mapbox.Layers.CircleLayer;
using NxSymbolLayer = Naxam.Mapbox.Layers.SymbolLayer;
using NxLineLayer = Naxam.Mapbox.Layers.LineLayer;
using NxBackgroundLayer = Naxam.Mapbox.Layers.BackgroundLayer;
using NxFillLayer = Naxam.Mapbox.Layers.FillLayer;
using NxRasterLayer = Naxam.Mapbox.Layers.RasterLayer;
using System.Linq;
using Naxam.Mapbox.Platform.Droid.Extensions;
using System.Collections.Generic;
using Com.Mapbox.Mapboxsdk.Style.Expressions;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public static class LayerExtensions
    {
        public static Sdk.Style.Layers.Layer ToLayer(this NxLayer layer)
        {
            Layer result = null;
            switch (layer)
            {
                case NxCircleLayer circleLayer:
                    result = circleLayer.ToNative();
                    break;
                case NxLineLayer lineLayer:
                    result = lineLayer.ToNative();
                    break;
                case NxBackgroundLayer backgroundLayer:
                    result = backgroundLayer.ToNative();
                    break;
                case NxFillLayer fillLayer:
                    result = fillLayer.ToNative();
                    break;
                case NxSymbolLayer symbolLayer:
                    result = symbolLayer.ToNative();
                    break;
                case NxRasterLayer circleLayer:
                    result = circleLayer.ToNative();
                    break;
            }

            return result;
        }

        public static NxLayer ToLayer(this Layer layer)
        {
            switch (layer)
            {
                case CircleLayer circleLayer:
                    return circleLayer.ToForms();
                case LineLayer lineLayer:
                    return lineLayer.ToForms();
                case BackgroundLayer backgroundLayer:
                    return backgroundLayer.ToForms();
                case FillLayer fillLayer:
                    return fillLayer.ToForms();
                case SymbolLayer symbolLayer:
                    return symbolLayer.ToForms();
                case RasterLayer circleLayer:
                    return circleLayer.ToForms();
            }

            return null;
        }

        public static CircleLayer ToNative(this NxCircleLayer layer)
        {
            if (layer == null) { return null; }

            var native = new CircleLayer(layer.Id, layer.SourceId);
            var properties = new List<PropertyValue>();
            if (layer.CircleBlur != null)
            {
                properties.Add(
                    PropertyFactory.CircleBlur(layer.CircleBlur.ToExpression())
                    );
            }

            if (layer.CircleOpacity != null)
            {
                properties.Add(
                    PropertyFactory.CircleOpacity(layer.CircleOpacity.ToExpression())
                    );
            }

            if (layer.CircleRadius != null)
            {
                properties.Add(
                    PropertyFactory.CircleRadius(layer.CircleRadius.ToExpression())
                    );
            }

            if (layer.CircleColor != null)
            {
                properties.Add(PropertyFactory.CircleColor(layer.CircleColor.ToExpression()));
            }

            native.SetProperties(properties.ToArray());

            // TODO Add other values

            if (layer.Filter != null)
            {
                native.WithFilter(layer.Filter.ToExpression());
            }

            return native;
        }
        public static LineLayer ToNative(this NxLineLayer layer)
        {
            if (layer == null) { return null; }

            var native = new LineLayer(layer.Id, layer.SourceId);
            native.SetProperties(
                PropertyFactory.LineWidth(new Java.Lang.Float(layer.LineWidth)),
                PropertyFactory.LineColor(layer.LineColor.ToAndroid())
            );

            return native;
        }
        public static BackgroundLayer ToNative(this NxBackgroundLayer background)
        {
            if (background == null) return null;
            var native = new BackgroundLayer(background.Id);
            native.SetProperties(
                PropertyFactory.BackgroundColor(background.BackgroundColor.ToAndroid())
            );
            return native;
        }
        public static FillLayer ToNative(this NxFillLayer fill)
        {
            if (fill == null) return null;
            var native = new FillLayer(fill.Id, fill.SourceId);
            native.SetProperties
                (
                    PropertyFactory.FillColor(fill.FillColor.ToAndroid())
                );
            return native;
        }
        public static SymbolLayer ToNative(this NxSymbolLayer symbol)
        {
            if (symbol == null) return null;

            var newLayer = new SymbolLayer(symbol.Id, symbol.SourceId);

            var properties = new List<PropertyValue>();

            if (symbol.IconImage != null)
            {
                properties.Add(PropertyFactory.IconImage(symbol.IconImage.ToExpression()));
            }

            if (symbol.IconColor != null)
            {
                properties.Add(PropertyFactory.IconColor(symbol.IconColor.ToExpression()));
            }

            if (symbol.IconSize != null)
            {
                properties.Add(PropertyFactory.IconSize(symbol.IconSize.ToExpression()));
            }

            if (symbol.TextField != null)
            {
                properties.Add(PropertyFactory.TextField(symbol.TextField.ToExpression()));
            }

            if (symbol.TextSize != null)
            {
                properties.Add(PropertyFactory.TextSize(symbol.TextSize.ToExpression()));
            }

            if (symbol.TextColor != null)
            {
                properties.Add(PropertyFactory.TextColor(symbol.TextColor.ToExpression()));
            }
            if (symbol.TextIgnorePlacement != null)
            {
                properties.Add(PropertyFactory.TextIgnorePlacement(symbol.TextIgnorePlacement.ToExpression()));
            }
            if (symbol.TextAllowOverlap != null)
            {
                properties.Add(PropertyFactory.TextAllowOverlap(symbol.TextAllowOverlap.ToExpression()));
            }

            if (symbol.Filter != null)
            {
                newLayer.WithFilter(symbol.Filter.ToExpression());
            }

            newLayer.SetProperties(properties.ToArray());

            // TODO Add other properties
            return newLayer;
        }

        public static RasterLayer ToNative(this NxRasterLayer raster)
        {
            if (raster == null) return null;
            var native = new RasterLayer(raster.Id, raster.SourceId);
            return native;
        }

        public static NxBackgroundLayer ToForms(this BackgroundLayer background)
        {
            if (background == null) return null;
            var forms = new NxBackgroundLayer(background.Id, "todo-set-src-id");
            if (background.BackgroundColor.IsNull && background.BackgroundColor.ColorInt != null)
            {
                Android.Graphics.Color backgroundColor = new Android.Graphics.Color((int)background.BackgroundColor.ColorInt);
                forms.BackgroundColor = Xamarin.Forms.Color.FromRgb(backgroundColor.R, backgroundColor.G, backgroundColor.B);
            }
            return forms;
        }
        public static NxFillLayer ToForms(this FillLayer fill)
        {
            if (fill == null) return null;
            var forms = new NxFillLayer(fill.Id, fill.SourceLayer);
            if (!fill.FillColor.IsNull && fill.FillColor.ColorInt != null)
            {
                Android.Graphics.Color fillColor = new Android.Graphics.Color((int)fill.FillColor.ColorInt);
                forms.FillColor = Xamarin.Forms.Color.FromRgb(fillColor.R, fillColor.G, fillColor.B);
            }
            return forms;
        }
        public static NxCircleLayer ToForms(this CircleLayer circle)
        {
            if (circle == null) { return null; }

            var forms = new NxCircleLayer(circle.Id, circle.SourceLayer);
            if (circle.CircleColor != null && circle.CircleColor.ColorInt != null)
            {
                Android.Graphics.Color circleColor = new Android.Graphics.Color((int)circle.CircleColor.ColorInt);
                //forms.CircleColor = Xamarin.Forms.Color.FromRgb(circleColor.R, circleColor.G, circleColor.B);
            }
            return forms;
        }
        public static NxLineLayer ToForms(this LineLayer line)
        {
            if (line == null) { return null; }
            var forms = new NxLineLayer(line.Id, line.SourceLayer);
            if (line.LineColor.IsNull && line.LineColor.ColorInt != null)
            {
                var lineColor = new Android.Graphics.Color((int)line.LineColor.ColorInt);
                forms.LineColor = Xamarin.Forms.Color.FromRgb(lineColor.R, lineColor.G, lineColor.B);
            }
            return forms;
        }
        public static NxRasterLayer ToForms(this RasterLayer raster)
        {
            if (raster == null) { return null; }
            var forms = new NxRasterLayer(raster.Id, raster.SourceId);
            return forms;
        }
        public static NxSymbolLayer ToForms(this SymbolLayer symbol)
        {
            if (symbol == null) { return null; }
            var forms = new NxSymbolLayer(symbol.Id, symbol.SourceId);
            return forms;
        }
    }
}
