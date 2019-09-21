using Naxam.Mapbox.Layers;

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

                var layer = layers[i].ToLayer();

                mapStyle.AddLayer(layer);
            }

            return true;
        }

        public bool AddLayerBelow(Layer layer, string layerId)
        {
            map.Style.AddLayerBelow(layer.ToLayer(), layerId);

            return true;
        }

        public bool AddLayerAbove(Layer layer, string layerId)
        {
            map.Style.AddLayerAbove(layer.ToLayer(), layerId);

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
            map.Style.AddLayerAt(layer.ToLayer(), index);

            return true;
        }
    }
}