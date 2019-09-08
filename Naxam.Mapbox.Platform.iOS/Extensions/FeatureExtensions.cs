using System.Linq;
using CoreGraphics;
using Foundation;
using GeoJSON.Net.Feature;
using Mapbox;
using Newtonsoft.Json;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class FeatureExtensions
    {
        public static Feature ToFeature(this IMGLFeature feature, bool shouldDipose)
        {
            var json = NSObjectToJSON(feature.GeoJSONDictionary);

            if (shouldDipose) feature.Dispose();

            return JsonConvert.DeserializeObject<Feature>(json);
        }

        public static IMGLFeature[] VisibleFeaturesAtPoint(this MGLMapView map, CGPoint point, params string[] layers)
        {
            if (layers.Length > 0)
            {
                using (var layerIds = new NSSet<NSString>(layers.Select(x => (NSString)x).ToArray()))
                {
                    return map.VisibleFeaturesAtPoint(point, layerIds);
                }
            }

            return map.VisibleFeaturesAtPoint(point);
        }

        public static IMGLFeature[] VisibleFeaturesInRect(this MGLMapView map, CGRect point, params string[] layers)
        {
            if (layers.Length > 0)
            {
                using (var layerIds = new NSSet<NSString>(layers.Select(x => (NSString)x).ToArray()))
                {
                    return map.VisibleFeaturesInRect(point, layerIds);
                }
            }

            return map.VisibleFeaturesInRect(point);
        }

        static string NSObjectToJSON(NSObject obj)
        {
            NSError err;
            NSData jsonData = NSJsonSerialization.Serialize(obj, 0, out err);
            if (err != null) return string.Empty;

            NSString myString = NSString.FromData(jsonData, NSStringEncoding.UTF8);
            jsonData.Dispose();

            var json = myString.ToString();
            myString.Dispose();
            return json;
        }
    }
}
