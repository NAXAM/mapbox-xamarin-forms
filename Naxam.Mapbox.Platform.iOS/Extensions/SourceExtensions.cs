using Foundation;
using Mapbox;
using Naxam.Mapbox.Sources;
using Newtonsoft.Json;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class SourceExtensions
    {
        public static MGLShapeSource ToSource(this Source source)
        {
            MGLShapeSource result = null;

            switch (source)
            {
                case GeoJsonSource geojsonSource:
                    var options = geojsonSource.Options.ToOptions();
                    var url = geojsonSource.IsLocal
                        ? NSUrl.FromFilename(geojsonSource.Url)
                        : NSUrl.FromString(geojsonSource.Url);
                    result = geojsonSource.Data != null
                        ? new MGLShapeSource(source.Id, geojsonSource.Data.ToShape(), options)
                        : new MGLShapeSource(source.Id, url, options);

                    break;
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
