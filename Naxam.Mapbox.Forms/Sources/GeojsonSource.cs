using GeoJSON.Net;

namespace Naxam.Mapbox.Sources
{
    public class GeoJsonSource : Source
    {
        public bool IsLocal { get; set; }

        public string Url { get; set; }

        public IGeoJSONObject Data { get; set; }

        public GeoJsonOptions Options { get; set; }

        public GeoJsonSource() { }

        public GeoJsonSource(string id)
        {
            Id = id;
        }

        public GeoJsonSource(string id, IGeoJSONObject data)
        {
            Id = id;
            Data = data;
        }

        public GeoJsonSource(string id, string url)
        {
            Id = id;
            Url = url;
        }
    }

    public class GeoJsonOptions
    {
        public int? MinZoom { get; set; }
        public int? MaxZoom { get; set; }
        public int? Buffer { get; set; }
        public bool? LineMetrics { get; set; }
        public float? Tolerance { get; set; }
        public bool? Cluster { get; set; }
        public int? ClusterMaxZoom { get; set; }
        public int? ClusterRadius { get; set; }
    }
}
