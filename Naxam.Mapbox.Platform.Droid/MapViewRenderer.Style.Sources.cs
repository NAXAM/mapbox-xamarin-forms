
using Com.Mapbox.Geojson;
using GeoJSON.Net;
using Naxam.Mapbox.Platform.Droid.Extensions;
using Newtonsoft.Json;
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

        public bool UpdateSource(string sourceId, IGeoJSONObject geoJsonObject)
        {
            var source = mapStyle.GetSource(sourceId) as Com.Mapbox.Mapboxsdk.Style.Sources.GeoJsonSource;

            if (source == null) return false;

            var json = JsonConvert.SerializeObject(geoJsonObject);

            switch (geoJsonObject)
            {
                case GeoJSON.Net.Feature.Feature feature:
                    source.SetGeoJson(Feature.FromJson(json));
                    break;
                default:
                    source.SetGeoJson(FeatureCollection.FromJson(json));
                    break;
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