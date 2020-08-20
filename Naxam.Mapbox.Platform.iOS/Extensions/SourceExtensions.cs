using System;
using Foundation;
using Mapbox;
using Naxam.Mapbox.Sources;
using Newtonsoft.Json;
using UIKit;
using Xamarin.Forms;

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
                    {
                        var options = geojsonSource.Options.ToOptions();

                        if (geojsonSource.Data != null)
                        {
                            var shape = geojsonSource.Data.ToShape();
                            return new MGLShapeSource(source.Id, shape, options);
                        }

                        if (string.IsNullOrWhiteSpace(geojsonSource.Url))
                        {
                            return new MGLComputedShapeSource(geojsonSource.Id, options);
                        }

                        var url = geojsonSource.IsLocal
                            ? NSUrl.FromFilename(geojsonSource.Url.Replace("asset://",string.Empty))
                            : NSUrl.FromString(geojsonSource.Url);
                        return new MGLShapeSource(source.Id, url, options);
                    }
                case VectorSource vectorSource:
                    //TODO VectorSource Add other options
                    return new MGLVectorTileSource(vectorSource.Id, vectorSource.TileSet.Tiles, null);
                case RasterSource rasterSource:
                    if (rasterSource.TileSet != null)
                    {
                        var options = new NSMutableDictionary<NSString, NSObject>();
                        if (rasterSource.TileSize.HasValue)
                        {
                            options.Add(MGLTileSourceOptions.TileSize, NSNumber.FromInt32(rasterSource.TileSize.Value));
                        }

                        // TODO No TileSet for iOS??
                        return  new MGLRasterTileSource(rasterSource.Id, rasterSource.TileSet.Tiles, new NSDictionary<NSString, NSObject>(options.Keys, options.Values));
                    }
                    return rasterSource.TileSize.HasValue
                        ? new MGLRasterTileSource(rasterSource.Id, NSUrl.FromString(rasterSource.ConfigurationURL), rasterSource.TileSize.Value)
                        : new MGLRasterTileSource(rasterSource.Id, NSUrl.FromString(rasterSource.ConfigurationURL));
                case RasterDemSource rasterDemSource:
                    if (rasterDemSource.TileSet != null)
                    {
                        var options = new NSMutableDictionary<NSString, NSObject>();
                        if (rasterDemSource.TileSize.HasValue)
                        {
                            options.Add(MGLTileSourceOptions.TileSize, NSNumber.FromInt32(rasterDemSource.TileSize.Value));
                        }

                        // TODO No TileSet for iOS??
                        return  new MGLRasterDEMSource(rasterDemSource.Id, rasterDemSource.TileSet.Tiles, new NSDictionary<NSString, NSObject>(options.Keys, options.Values));
                    }
                    return rasterDemSource.TileSize.HasValue
                        ? new MGLRasterDEMSource(rasterDemSource.Id, NSUrl.FromString(rasterDemSource.Url), rasterDemSource.TileSize.Value)
                        : new MGLRasterDEMSource(rasterDemSource.Id, NSUrl.FromString(rasterDemSource.Url));
                
                case MapboxImageSource imageSource:
                    //TODO Other image source
                    return new MGLImageSource(imageSource.Id, imageSource.Coordinates.ToNative(), imageSource.Source.GetImage());
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
