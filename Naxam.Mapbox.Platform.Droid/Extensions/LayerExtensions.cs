using Naxam.Controls.Forms;
using Sdk = Com.Mapbox.Mapboxsdk;
using Xamarin.Forms.Platform.Android;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public static class LayerExtensions
    {
        public static Sdk.Style.Layers.CircleLayer ToNative(this CircleLayer layer)
        {
            if (layer == null) { return null; }

            var native = new Sdk.Style.Layers.CircleLayer(layer.Id, layer.SourceId);
            native.SetProperties(
                                    Sdk.Style.Layers.PropertyFactory.CircleColor(layer.CircleColor.ToAndroid()),
                                    Sdk.Style.Layers.PropertyFactory.CircleOpacity(new Java.Lang.Float(layer.CircleOpacity)),
                                    Sdk.Style.Layers.PropertyFactory.CircleRadius(new Java.Lang.Float(layer.CircleRadius))
                                );

            return native;
        }
        public static Sdk.Style.Layers.LineLayer ToNative(this LineLayer layer)
        {
            if (layer == null) { return null; }

            var native = new Sdk.Style.Layers.LineLayer(layer.Id, layer.SourceId);
            native.SetProperties(
                Sdk.Style.Layers.PropertyFactory.LineWidth(new Java.Lang.Float(layer.LineWidth)),
                Sdk.Style.Layers.PropertyFactory.LineColor(layer.LineColor.ToAndroid())
            );

            return native;
        }
        public static Sdk.Style.Layers.BackgroundLayer ToNative(this BackgroundLayer background)
        {
            if (background == null) return null;
            var native = new Sdk.Style.Layers.BackgroundLayer(background.Id);
            native.SetProperties
                (
                    Sdk.Style.Layers.PropertyFactory.BackgroundColor(background.BackgroundColor.ToAndroid())
                );
            return native;
        }
        public static Sdk.Style.Layers.FillLayer ToNative(this FillLayer fill)
        {
            if (fill == null) return null;
            var native = new Sdk.Style.Layers.FillLayer(fill.Id, fill.SourceId);
            native.SetProperties
                (
                    Sdk.Style.Layers.PropertyFactory.FillColor(fill.FillColor.ToAndroid())
                );
            return native;
        }
        public static Sdk.Style.Layers.SymbolLayer ToNative(this SymbolLayer symbol)
        {
            if (symbol == null) return null;
            var native = new Sdk.Style.Layers.SymbolLayer(symbol.Id, symbol.SourceId);
            return native;
        }
        public static Sdk.Style.Layers.RasterLayer ToNative(this RasterLayer raster)
        {
            if (raster == null) return null;
            var native = new Sdk.Style.Layers.RasterLayer(raster.Id, raster.SourceId);
            return native;
        }

        public static BackgroundLayer ToForms(this Sdk.Style.Layers.BackgroundLayer background)
        {
            if (background == null) return null;
            BackgroundLayer forms = new BackgroundLayer(background.Id, "");
            if (background.BackgroundColor.IsNull && background.BackgroundColor.ColorInt != null)
            {
                Android.Graphics.Color backgroundColor = new Android.Graphics.Color((int)background.BackgroundColor.ColorInt);
                forms.BackgroundColor = Xamarin.Forms.Color.FromRgb(backgroundColor.R, backgroundColor.G, backgroundColor.B);
            }
            return forms;
        }
        public static FillLayer ToForms(this Sdk.Style.Layers.FillLayer fill)
        {
            if (fill == null) return null;
            FillLayer forms = new FillLayer(fill.Id, fill.SourceLayer);
            if (!fill.FillColor.IsNull && fill.FillColor.ColorInt != null)
            {
                Android.Graphics.Color fillColor = new Android.Graphics.Color((int)fill.FillColor.ColorInt);
                forms.FillColor = Xamarin.Forms.Color.FromRgb(fillColor.R, fillColor.G, fillColor.B);
            }
            return forms;
        }
        public static CircleLayer ToForms(this Sdk.Style.Layers.CircleLayer circle)
        {
            if (circle == null) { return null; }

            var forms = new CircleLayer(circle.Id, circle.SourceLayer);
            if (circle.CircleColor != null && circle.CircleColor.ColorInt != null)
            {
                Android.Graphics.Color circleColor = new Android.Graphics.Color((int)circle.CircleColor.ColorInt);
                forms.CircleColor = Xamarin.Forms.Color.FromRgb(circleColor.R, circleColor.G, circleColor.B);
            }
            return forms;
        }
        public static LineLayer ToForms(this Sdk.Style.Layers.LineLayer line)
        {
            if (line == null) { return null; }
            var forms = new LineLayer(line.Id, line.SourceLayer);
            if (line.LineColor.IsNull && line.LineColor.ColorInt != null)
            {
                var lineColor = new Android.Graphics.Color((int)line.LineColor.ColorInt);
                forms.LineColor = Xamarin.Forms.Color.FromRgb(lineColor.R, lineColor.G, lineColor.B);
            }
            return forms;
        }
        public static RasterLayer ToForms(this Sdk.Style.Layers.RasterLayer raster)
        {
            if (raster == null) { return null; }
            var forms = new RasterLayer(raster.Id, "");
            return forms;
        }
        public static SymbolLayer ToForms(this Sdk.Style.Layers.SymbolLayer symbol)
        {
            if (symbol == null) { return null; }
            var forms = new SymbolLayer(symbol.Id, "");
            return forms;
        }
    }

    public static class IdExtensions
    {
        static string PREFIX = "__naxam_prefix__";

        public static string Prefix(this string self)
        {
            return $"{PREFIX}{self}";
        }
        public static bool HasPrefix(this string self)
        {
            return self?.StartsWith($"{PREFIX}") ?? false;
        }
    }
}
