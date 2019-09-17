using System.Linq;
using Mapbox;
using Naxam.Mapbox;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Platform.iOS.Extensions;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : IMapFunctions
    {
        public void RemoveLayer(params string[] layerIds)
        {
            for (int i = 0; i < layerIds.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(layerIds[i])) continue;

                var layer = mapStyle.Layers.FirstOrDefault(x => x.Identifier == layerIds[i]);

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
            var aboveLayer = mapStyle.Layers.FirstOrDefault(x => string.Equals(x.Identifier, layerId));

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
            var belowLayer = mapStyle.Layers.FirstOrDefault(x => string.Equals(x.Identifier, layerId));

            if (belowLayer == null) return false;

            var newLayer = CreateLayer(layer);

            if (newLayer == null) return false;

            mapStyle.InsertLayerBelow(newLayer, belowLayer);
            return true;
        }

        MGLStyleLayer CreateLayer(Layer layer)
        {
            if (string.IsNullOrWhiteSpace(layer.Id)
                || false == layer.IsVisible) return null;


            var styleLayer = layer as StyleLayer;

            if (string.IsNullOrWhiteSpace(styleLayer.SourceId)) return null;

            var source = map.Style.SourceWithIdentifier(styleLayer.SourceId);

            if (source == null) return null;

            return layer.ToLayer(source);
        }
    }
}
