using System;
using System.Linq;
using Mapbox;
using Naxam.Mapbox;
using Naxam.Mapbox.Platform.iOS.Extensions;
using Naxam.Mapbox.Sources;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : IMapFunctions
    {
        public void AddStyleImage(IconImageSource source)
        {
            throw new NotImplementedException();
        }

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
    }
}
