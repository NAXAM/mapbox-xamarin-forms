using NxSource = Naxam.Mapbox.Sources.Source;
using NxRasterSource = Naxam.Mapbox.Sources.RasterSource;
using NxGeojsonSource = Naxam.Mapbox.Sources.GeoJsonSource;
using NxGeoJsonOptions = Naxam.Mapbox.Sources.GeoJsonOptions;
using Com.Mapbox.Mapboxsdk.Style.Sources;
using Newtonsoft.Json;
using Com.Mapbox.Geojson;

namespace Naxam.Mapbox.Platform.Droid.Extensions
{
    public static class SourceExtensions
    {
        public static Source ToSource(this NxSource source)
        {
            switch (source)
            {
                case NxGeojsonSource geojsonSource:
                    if (geojsonSource.Data != null)
                    {
                        var json = JsonConvert.SerializeObject(geojsonSource.Data);
                        var feaureCollection = FeatureCollection.FromJson(json);
                        return new GeoJsonSource(geojsonSource.Id, feaureCollection, geojsonSource.Options.ToOptions());
                    }

                    if (string.IsNullOrWhiteSpace(geojsonSource.Url)) return null;

                    if (false == geojsonSource.IsLocal)
                    {
                        return new GeoJsonSource(geojsonSource.Id, new Java.Net.URL(geojsonSource.Url), geojsonSource.Options.ToOptions());
                    }

                    var localUrl = geojsonSource.Url.StartsWith("asset://", System.StringComparison.OrdinalIgnoreCase)
                        ? new Java.Net.URI(geojsonSource.Url)
                        : new Java.Net.URI("asset://" + geojsonSource.Url);

                    return new GeoJsonSource(geojsonSource.Id, localUrl, geojsonSource.Options.ToOptions());

                case NxRasterSource rasterSource:
                    return new RasterSource(rasterSource.Id, rasterSource.ConfigurationURL, rasterSource.TileSize);
            }

            return null;
        }
    }

    public static class GeoJsonOptionsExtensions
    {
        public static GeoJsonOptions ToOptions(this NxGeoJsonOptions nxoptions)
        {
            if (nxoptions == null) return null;

            var options = new GeoJsonOptions();
            if (nxoptions.Buffer.HasValue)
            {
                options.WithBuffer(nxoptions.Buffer.Value);
            }
            if (nxoptions.Cluster.HasValue)
            {
                options.WithCluster(nxoptions.Cluster.Value);
            }
            if (nxoptions.ClusterMaxZoom.HasValue)
            {
                options.WithClusterMaxZoom(nxoptions.ClusterMaxZoom.Value);
            }
            if (nxoptions.ClusterRadius.HasValue)
            {
                options.WithClusterRadius(nxoptions.ClusterRadius.Value);
            }
            if (nxoptions.LineMetrics.HasValue)
            {
                options.WithLineMetrics(nxoptions.LineMetrics.Value);
            }
            if (nxoptions.MaxZoom.HasValue)
            {
                options.WithMaxZoom(nxoptions.MaxZoom.Value);
            }
            if (nxoptions.MinZoom.HasValue)
            {
                options.WithMinZoom(nxoptions.MinZoom.Value);
            }
            if (nxoptions.Tolerance.HasValue)
            {
                options.WithTolerance(nxoptions.Tolerance.Value);
            }
            return options;
        }
    }
}
