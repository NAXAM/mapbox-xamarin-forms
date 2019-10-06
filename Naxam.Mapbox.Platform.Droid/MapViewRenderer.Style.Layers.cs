using System.Linq;
using Com.Mapbox.Mapboxsdk.Style.Light;
using Com.Mapbox.Mapboxsdk.Utils;
using Naxam.Mapbox.Layers;
using Xamarin.Forms.Platform.Android;
using Light = Naxam.Mapbox.Light;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
    {
        public void RemoveLayer(params string[] layerIds)
        {
            for (int i = 0; i < layerIds.Length; i++)
            {
                var native = map.Style.GetLayer(layerIds[i]);

                if (native == null) continue;

                map.Style.RemoveLayer(native);
            }
        }

        public bool AddLayer(params Layer[] layers)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(layers[i].Id)) continue;

                var styleLayer = layers[i] as StyleLayer;

                if (styleLayer == null) continue;

                var source = mapStyle.GetSource(styleLayer.SourceId);

                if (source == null) continue;

                var layer = layers[i].ToNative();

                mapStyle.AddLayer(layer);
            }

            return true;
        }

        public bool AddLayerBelow(Layer layer, string layerId)
        {
            map.Style.AddLayerBelow(layer.ToNative(), layerId);

            return true;
        }

        public bool AddLayerAbove(Layer layer, string layerId)
        {
            map.Style.AddLayerAbove(layer.ToNative(), layerId);

            return true;
        }

        public bool UpdateLayer(Layer layer)
        {
            var nativeLayer = map.Style.GetLayer(layer.Id);

            if (nativeLayer == null) return false;

            layer.UpdateLayer(nativeLayer);

            return true;
        }

        public bool AddLayerAt(Layer layer, int index)
        {
            map.Style.AddLayerAt(layer.ToNative(), index);

            return true;
        }

        public StyleLayer[] GetLayers()
        {
            return map.Style.Layers.Select(x => x.ToForms()).Where(x => x != null).ToArray();
        }
        
        public void UpdateLight(Light light)
        {
            var native = mapStyle.Light;
            if (!string.IsNullOrWhiteSpace(light.Anchor))
            {
                native.Anchor = light.Anchor;
            }

            if (light.Color != null)
            {
                native.Color = ColorUtils.ColorToRgbaString(light.Color.Value.ToAndroid());
            }

            if (light.ColorTransition != null)
            {
                native.ColorTransition = light.ColorTransition.ToNative();
            }

            if (light.Intensity.HasValue)
            {
                native.Intensity = light.Intensity.Value;
            }

            if (light.IntensityTransition != null)
            {
                native.IntensityTransition = light.IntensityTransition.ToNative();
            }

            if (light.Position.HasValue)
            {
                native.Position = new Position(
                    light.Position.Value.Radial, 
                    light.Position.Value.Azimuthal, 
                    light.Position.Value.Polar);
            }

            if (light.PositionTransition != null)
            {
                native.PositionTransition = light.PositionTransition.ToNative();
            }
        }
    }
}