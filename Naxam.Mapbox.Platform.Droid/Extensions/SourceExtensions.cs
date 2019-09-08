using NxSource = Naxam.Mapbox.Sources.Source;
using NxRasterSource = Naxam.Mapbox.Sources.RasterSource;
using NxGeojsonSource = Naxam.Mapbox.Sources.GeojsonSource;
using Sdk = Com.Mapbox.Mapboxsdk;

namespace Naxam.Mapbox.Platform.Droid.Extensions
{
    public static class SourceExtensions
    {
        public static Sdk.Style.Sources.Source ToSource(this NxSource source)
        {
            switch (source)
            {
                case NxGeojsonSource geojsonSource:
                    return geojsonSource.Data != null
                        ? null // TODO 
                        : new Sdk.Style.Sources.GeoJsonSource(geojsonSource.Id, new Java.Net.URL(geojsonSource.Url));
                case NxRasterSource rasterSource:
                    return new Sdk.Style.Sources.RasterSource(rasterSource.Id, rasterSource.ConfigurationURL, rasterSource.TileSize);
            }

            return null;
        }
    }
}
