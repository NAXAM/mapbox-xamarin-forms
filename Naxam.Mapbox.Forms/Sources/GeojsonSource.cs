namespace Naxam.Mapbox.Sources
{
    public class GeojsonSource : Source
    {
        public string Url { get; set; }

        public GeoJSON.Net.IGeoJSONObject Data { get; set; }
    }
}
