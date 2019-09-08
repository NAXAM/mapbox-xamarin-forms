using Naxam.Mapbox.Platform.Droid.Extensions;
using NxSource = Naxam.Mapbox.Sources.Source;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
    {
        public bool AddSource(params NxSource[] sources)
        {
            for (int i = 0; i < sources.Length; i++)
            { 
                if (string.IsNullOrWhiteSpace(sources[i].Id)) continue;

                mapStyle.AddSource(sources[i].ToSource());
            }

            return true;
        }

        public void RemoveSource(params string[] sourceIds)
        {
            for (int i = 0; i < sourceIds.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(sourceIds[i])) continue;

                map.Style.RemoveSource(sourceIds[i]);
            }
        }

    }
}