using Foundation;
using Mapbox;
using NxGeoJsonOptions = Naxam.Mapbox.Sources.GeoJsonOptions;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class GeoJsonOptionsExtensions
    {
        public static NSDictionary<NSString, NSObject> ToOptions(this NxGeoJsonOptions nxoptions)
        {
            if (nxoptions == null) return null;

            var options = new NSMutableDictionary<NSString, NSObject>();
            if (nxoptions.Buffer.HasValue)
            {
                options.Add(MGLShapeSourceOptions.Buffer, NSNumber.FromNInt(nxoptions.Buffer.Value));
            }
            if (nxoptions.Cluster.HasValue)
            {
                options.Add(MGLShapeSourceOptions.Clustered, NSNumber.FromBoolean(nxoptions.Cluster.Value));
            }
            if (nxoptions.ClusterMaxZoom.HasValue)
            {
                options.Add(MGLShapeSourceOptions.ClusterRadius, NSNumber.FromNInt(nxoptions.ClusterMaxZoom.Value));
            }
            if (nxoptions.ClusterRadius.HasValue)
            {
                options.Add(MGLShapeSourceOptions.ClusterRadius, NSNumber.FromNInt(nxoptions.ClusterRadius.Value));
            }
            if (nxoptions.LineMetrics.HasValue)
            {
                options.Add(MGLShapeSourceOptions.LineDistanceMetrics, NSNumber.FromBoolean(nxoptions.LineMetrics.Value));
            }
            if (nxoptions.MaxZoom.HasValue)
            {
                options.Add(MGLShapeSourceOptions.MaximumZoomLevel, NSNumber.FromNInt(nxoptions.MaxZoom.Value));
            }
            if (nxoptions.MinZoom.HasValue)
            {
                options.Add(MGLShapeSourceOptions.MinimumZoomLevel, NSNumber.FromNInt(nxoptions.MinZoom.Value));
            }
            if (nxoptions.Tolerance.HasValue)
            {
                options.Add(MGLShapeSourceOptions.SimplificationTolerance, NSNumber.FromFloat(nxoptions.Tolerance.Value));
            }
            return new NSDictionary<NSString, NSObject>(options.Keys, options.Values);
        }
    }
}
