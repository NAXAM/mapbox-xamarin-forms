using Naxam.Mapbox.Forms;
using Sdk = Mapbox.MapboxSdk;
using Xamarin.Forms.Platform.Android;

namespace Naxam.Controls.Platform.Droid
{
    public static class LayerExtensions
    {
        public static Sdk.Style.Layers.CircleLayer ToNative (this CircleLayer layer)
        {
            if (layer == null) { return null; }

            var native = new Sdk.Style.Layers.CircleLayer (layer.Id.Prefix (), layer.SourceId.Prefix ());
            native.SetProperties (
                                    Sdk.Style.Layers.PropertyFactory.CircleColor (layer.CircleColor.ToAndroid ()),
                                    Sdk.Style.Layers.PropertyFactory.CircleOpacity (new Java.Lang.Float (layer.CircleOpacity)),
                                    Sdk.Style.Layers.PropertyFactory.CircleRadius (new Java.Lang.Float (layer.CircleRadius))
                                );

            return native;
        }

        public static Sdk.Style.Layers.LineLayer ToNative (this LineLayer layer)
        {
            if (layer == null) { return null; }

            var native = new Sdk.Style.Layers.LineLayer (layer.Id.Prefix (), layer.SourceId.Prefix ());
            native.SetProperties (
                Sdk.Style.Layers.PropertyFactory.LineWidth (new Java.Lang.Float (layer.LineWidth)),
                Sdk.Style.Layers.PropertyFactory.LineColor (layer.LineColor.ToAndroid ())
            //Sdk.Style.Layers.PropertyFactory.LineCap(layer.LineCap.ToString().ToLower()),
            //Sdk.Style.Layers.PropertyFactory.LineOpacity(new Java.Lang.Float(layer.LineOpacity))
            );

            return native;
        }
    }

    public static class IdExtensions
    {
        static string PREFIX = "__naxam_prefix__";

        public static string Prefix (this string self)
        {
            return $"{PREFIX}{self}";
        }
        public static bool HasPrefix (this string self)
        {
            return self?.StartsWith ($"{PREFIX}") ?? false;
        }
    }
}
