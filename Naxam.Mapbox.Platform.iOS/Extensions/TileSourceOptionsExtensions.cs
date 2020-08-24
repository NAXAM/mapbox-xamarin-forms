using Mapbox;
using Foundation;
using NxTileSourceOptions = Naxam.Mapbox.Sources.TileSourceOptions;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class TileSourceOptionsExtensions
    {
        public static NSDictionary<NSString, NSObject> ToOptions(this NxTileSourceOptions nxoptions)
        {
            if (nxoptions == null) return null;

            var options = new NSMutableDictionary<NSString, NSObject>();
            if (nxoptions.MinimumZoomLevel.HasValue)
            {
                options.Add(MGLTileSourceOptions.MinimumZoomLevel, NSNumber.FromNInt(nxoptions.MinimumZoomLevel.Value));
            }
            if (nxoptions.MaximumZoomLevel.HasValue)
            {
                options.Add(MGLTileSourceOptions.MaximumZoomLevel, NSNumber.FromNInt(nxoptions.MaximumZoomLevel.Value));
            }

            // TODO add other options

            return new NSDictionary<NSString, NSObject>(options.Keys, options.Values);
        }
    }
}
