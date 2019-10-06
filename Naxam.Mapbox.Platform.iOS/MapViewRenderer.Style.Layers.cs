using System.Linq;
using Foundation;
using Mapbox;
using Naxam.Mapbox;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Platform.iOS.Extensions;
using Xamarin.Forms.Platform.iOS;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    
    public partial class MapViewRenderer : IMapFunctions
    {
        public void RemoveLayer(params string[] layerIds)
        {
            for (int i = 0; i < layerIds.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(layerIds[i])) continue;

                var layer = mapStyle.LayerWithIdentifier(layerIds[i]);

                if (layer == null) continue;

                map.Style.RemoveLayer(layer);
                layer.Dispose();
                layer = null;
            }
        }

        public bool AddLayer(params Layer[] layers)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                var layer = CreateLayer(layers[i]);

                if (layer == null) continue;

                map.Style.AddLayer(layer);
            }

            return true;
        }

        public bool AddLayerAbove(Layer layer, string layerId)
        {
            var aboveLayer = mapStyle.LayerWithIdentifier(layerId);

            if (aboveLayer == null) return false;

            var newLayer = CreateLayer(layer);

            if (newLayer == null) return false;

            mapStyle.InsertLayerAbove(newLayer, aboveLayer);
            return true;
        }

        public bool AddLayerAt(Layer layer, int index)
        {
            if (index < 0) return false;

            var newLayer = CreateLayer(layer);

            if (newLayer == null) return false;

            mapStyle.InsertLayer(newLayer, (System.nuint)index);

            return true;
        }

        public bool AddLayerBelow(Layer layer, string layerId)
        {
            var belowLayer = mapStyle.LayerWithIdentifier(layerId);

            if (belowLayer == null) return false;

            var newLayer = CreateLayer(layer);

            if (newLayer == null) return false;

            mapStyle.InsertLayerBelow(newLayer, belowLayer);
            return true;
        }

        public bool UpdateLayer(Layer layer)
        {
            var nativeLayer = mapStyle.LayerWithIdentifier(layer.Id);

            if (nativeLayer == null) return false;

            layer.UpdateLayer(nativeLayer);

            return true;
        }

        MGLStyleLayer CreateLayer(Layer layer)
        {
            if (string.IsNullOrWhiteSpace(layer.Id)) return null;

            var styleLayer = layer as StyleLayer;

            if (string.IsNullOrWhiteSpace(styleLayer.SourceId)) return null;

            var source = map.Style.SourceWithIdentifier(styleLayer.SourceId);

            if (source == null) return null;

            return layer.ToLayer(source);
        }

        public StyleLayer[] GetLayers()
        {
            return mapStyle.Layers.Select(x => x.ToForms()).Where(x => x != null).ToArray();
        }
        
        public void UpdateLight(Light light)
        {
            var native = mapStyle.Light;
            if (!string.IsNullOrWhiteSpace(light.Anchor))
            {
                native.Anchor = NSExpression.FromConstant(new NSString(light.Anchor));
            }

            if (light.Color != null)
            {
                native.Color = NSExpression.FromConstant(light.Color.Value.ToUIColor());
            }

            if (light.ColorTransition != null)
            {
                native.ColorTransition = light.ColorTransition.ToNative();
            }

            if (light.Intensity.HasValue)
            {
                native.Intensity = NSExpression.FromConstant(NSNumber.FromFloat(light.Intensity.Value));
            }

            if (light.IntensityTransition != null)
            {
                native.IntensityTransition = light.IntensityTransition.ToNative();
            }

            if (light.Position.HasValue)
            {
                var position = NSValue_MGLAdditions.ValueWithMGLSphericalPosition(null, new MGLSphericalPosition
                {
                    radial = light.Position.Value.Radial,
                    azimuthal = light.Position.Value.Azimuthal,
                    polar = light.Position.Value.Polar
                });
                native.Position = NSExpression.FromConstant(position);
            }

            if (light.PositionTransition != null)
            {
                native.PositionTransition = light.PositionTransition.ToNative();
            }

            mapStyle.Light = native;
        }
    }
}
