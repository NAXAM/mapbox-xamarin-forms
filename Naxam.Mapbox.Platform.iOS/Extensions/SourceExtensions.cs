using Foundation;
using Mapbox;
using Naxam.Mapbox.Sources;
using Newtonsoft.Json;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class SourceExtensions
    {
        public static MGLSource ToSource(this Source source)
        {
            MGLShapeSource result = null;

            switch (source)
            {
                case GeoJsonSource geojsonSource:
                    var options = geojsonSource.Options.ToOptions();

                    if (geojsonSource.Data != null)
                    {
                        return new MGLShapeSource(source.Id, geojsonSource.Data.ToShape(), options);
                    }

                    if (string.IsNullOrWhiteSpace(geojsonSource.Url))
                    {
                        return new MGLShapeSource()
                        {
                            Identifier = geojsonSource.Id
                        };
                    }

                    var url = geojsonSource.IsLocal
                        ? NSUrl.FromFilename(geojsonSource.Url)
                        : NSUrl.FromString(geojsonSource.Url);
                    return new MGLShapeSource(source.Id, url, options);
                case VectorSource vectorSource:
                    //TODO VectorSource Add other options
                    return new MGLVectorTileSource(vectorSource.Id, NSUrl.FromString(vectorSource.Url));
                case RasterSource rasterSource:
                    if (rasterSource.TileSet != null)
                    {
                        // TOPDO Ensure all detail set
                        return  new MGLRasterTileSource(rasterSource.Id, rasterSource.TileSet.Tiles, new NSDictionary<string, object>());
                    }
                    return rasterSource.TileSize.HasValue
                        ? new MGLRasterTileSource(rasterSource.Id, NSUrl.FromString(rasterSource.ConfigurationURL), rasterSource.TileSize.Value)
                        : new MGLRasterTileSource(rasterSource.Id, NSUrl.FromString(rasterSource.ConfigurationURL));
            }

            return result;
        }

        public static MGLShape ToShape(this GeoJSON.Net.IGeoJSONObject geoJSONObject)
        {
            if (geoJSONObject == null) return null;

            var json = JsonConvert.SerializeObject(geoJSONObject);
            var data = NSData.FromString(json);

            var shape = MGLShape.ShapeWithData(data, (int)NSStringEncoding.UTF8, out var error);

            // TODO Handle error
            if (error != null) return null;

            return shape;
        }
    }
}
