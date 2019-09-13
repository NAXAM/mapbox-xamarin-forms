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

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public static class LayerExtensions
    {
        public static Sdk.Style.Layers.Layer ToLayer(this NxLayer layer)
        {
            switch (layer)
            {
                case NxCircleLayer circleLayer:
                    return circleLayer.ToNative();
                case NxLineLayer lineLayer:
                    return lineLayer.ToNative();
                case NxBackgroundLayer backgroundLayer:
                    return backgroundLayer.ToNative();
                case NxFillLayer fillLayer:
                    return fillLayer.ToNative();
                case NxSymbolLayer symbolLayer:
                    return symbolLayer.ToNative();
                case NxRasterLayer circleLayer:
                    return circleLayer.ToNative();
            }

            return null;
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
            native.SetProperties(
                PropertyFactory.CircleColor(layer.CircleColor.ToAndroid()),
                PropertyFactory.CircleOpacity(new Java.Lang.Float(layer.CircleOpacity)),
                PropertyFactory.CircleRadius(new Java.Lang.Float(layer.CircleRadius))
            );

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

            if (symbol.IconImageName != null)
            {
                // TODO Need to ensure ID if it's a local resource
                var property = PropertyFactory.IconImage(symbol.IconImageName.Id);
                newLayer.SetProperties(property);
            }

            if (symbol.IconOpacity.HasValue)
            {
                var property = PropertyFactory.IconOpacity(new Java.Lang.Float(symbol.IconOpacity.Value));
                newLayer.SetProperties(property);
            }

            if (symbol.IconAllowOverlap.HasValue)
            {
                var property = PropertyFactory.IconAllowOverlap(new Java.Lang.Boolean(symbol.IconAllowOverlap.Value));
                newLayer.SetProperties(property);
            }

            if (symbol.IconOffset?.Length >= 2)
            {
                var property = PropertyFactory.IconOffset(new[] {
                    new Java.Lang.Float(symbol.IconOffset[0]),
                    new Java.Lang.Float(symbol.IconOffset[1])
                });
                newLayer.SetProperties(property);
            }

            if (symbol.FilterExpression != null)
            {
                newLayer.WithFilter(symbol.FilterExpression.ToExpression());
            }

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
                forms.CircleColor = Xamarin.Forms.Color.FromRgb(circleColor.R, circleColor.G, circleColor.B);
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
