using System.Linq;
using Mapbox;
using Naxam.Mapbox;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Platform.iOS.Extensions;
using Naxam.Mapbox.Sources;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : IMapFunctions
    {
        public bool AddSource(params Source[] sources)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(sources[i].Id)) continue;

                map.Style.AddSource(sources[i].ToSource());
            }

            return true;
        }

        public void RemoveSource(params string[] sourceIds)
        {
            for (int i = 0; i < sourceIds.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(sourceIds[i])) continue;

                var source = map.Style.Sources.FirstOrDefault(x => x is MGLSource s && s.Identifier == sourceIds[i]) as MGLSource;

                if (source == null) continue;

                map.Style.RemoveSource(source);
            }
        }

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
