using NxSource = Naxam.Mapbox.Sources.Source;
using NxRasterSource = Naxam.Mapbox.Sources.RasterSource;
using NxGeojsonSource = Naxam.Mapbox.Sources.GeoJsonSource;
using NxVectorSource = Naxam.Mapbox.Sources.VectorSource;
using NxGeoJsonOptions = Naxam.Mapbox.Sources.GeoJsonOptions;
using NxTileSet = Naxam.Mapbox.Sources.TileSet;
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

                        switch (geojsonSource.Data)
                        {
                            case GeoJSON.Net.Feature.Feature feature:
                                {
                                    var mapboxFeature = Feature.FromJson(json);
                                    return new GeoJsonSource(geojsonSource.Id, mapboxFeature, geojsonSource.Options.ToOptions());
                                }
                            default:
                                var feaureCollection = FeatureCollection.FromJson(json);
                                return new GeoJsonSource(geojsonSource.Id, feaureCollection, geojsonSource.Options.ToOptions());
                        }
                    }

                    if (string.IsNullOrWhiteSpace(geojsonSource.Url))
                    {
                        return new GeoJsonSource(geojsonSource.Id);
                    }

                    if (false == geojsonSource.IsLocal)
                    {
                        return new GeoJsonSource(geojsonSource.Id, new Java.Net.URL(geojsonSource.Url), geojsonSource.Options.ToOptions());
                    }

                    var localUrl = geojsonSource.Url.StartsWith("asset://", System.StringComparison.OrdinalIgnoreCase)
                        ? new Java.Net.URI(geojsonSource.Url)
                        : new Java.Net.URI("asset://" + geojsonSource.Url);

                    return new GeoJsonSource(geojsonSource.Id, localUrl, geojsonSource.Options.ToOptions());

                case NxRasterSource rasterSource:
                    if (rasterSource.TileSet != null)
                    {
                        var tileSet = rasterSource.TileSet.Convert();

                        return rasterSource.TileSize.HasValue 
                            ? new RasterSource(rasterSource.Id, tileSet, rasterSource.TileSize.Value)
                            : new RasterSource(rasterSource.Id, tileSet);
                    }

                    return rasterSource.TileSize.HasValue
                            ? new RasterSource(rasterSource.Id, rasterSource.ConfigurationURL, rasterSource.TileSize.Value)
                            : new RasterSource(rasterSource.Id, rasterSource.ConfigurationURL);
                case NxVectorSource vectorSource:
                    //TODO VectorSource Add other options
                    return new VectorSource(vectorSource.Id, vectorSource.Url);
            }

            return null;
        }

        public static TileSet Convert(this NxTileSet tileSet)
        {
            return new TileSet(tileSet.TileJson, tileSet.Tiles);
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
