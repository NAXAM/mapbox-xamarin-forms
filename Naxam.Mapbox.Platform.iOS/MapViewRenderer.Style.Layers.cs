using System.Linq;
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
                var layer = layers[i];

                if (string.IsNullOrWhiteSpace(layer.Id)
                    || false == layer.IsVisible) continue;


                var styleLayer = layer as StyleLayer;

                if (string.IsNullOrWhiteSpace(styleLayer.SourceId)) continue;

                var source = map.Style.SourceWithIdentifier(styleLayer.SourceId);

                if (source == null) continue;

                map.Style.AddLayer(layers[i].ToLayer(source));
            }

            return true;
        }
    }
}
