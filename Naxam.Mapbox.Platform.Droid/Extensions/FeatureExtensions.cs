using Com.Mapbox.Geojson;

using NFeature = GeoJSON.Net.Feature.Feature;
using Newtonsoft.Json;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public static class FeatureExtensions
    {
        public static NFeature ToFeature(this Feature feature, bool shouldDispose = false)
        {
            if (feature == null)
            {
                return null;
            }

            var json = feature.ToJson();

            if (shouldDispose) feature.Dispose();

            return JsonConvert.DeserializeObject<NFeature>(json);
        }
    }
}
