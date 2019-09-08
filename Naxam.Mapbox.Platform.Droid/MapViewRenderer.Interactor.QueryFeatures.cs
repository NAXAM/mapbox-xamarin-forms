using System.IO;
using System.Linq;
using Naxam.Controls.Forms;
using Naxam.Mapbox;

using NFeature = GeoJSON.Net.Feature.Feature;
using Naxam.Mapbox.Platform.Droid.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer : IMapFunctions
    {
        public NFeature[] QueryFeatures(LatLng latLng, params string[] layers)
        {
            var point = map.Projection.ToScreenLocation(latLng.ToLatLng());
            var features = map.QueryRenderedFeatures(point, layers);

            return features.Select(f => f.ToFeature(true)).ToArray();
        }

        public NFeature[] QueryFeatures(LatLng latLng, float radius, params string[] layers)
        {
            var point = map.Projection.ToScreenLocation(latLng.ToLatLng());
            var region = point.ToRectF(Context.ToPixels(radius));
            var features = map.QueryRenderedFeatures(region, layers);

            point.Dispose();
            region.Dispose();

            return features.Select(f => f.ToFeature(true)).ToArray();
        }

        public NFeature[] QueryFeatures(LatLngBounds bounds, params string[] layers)
        {
            var mpbounds = bounds.ToLatLngBounds();
            var tl = map.Projection.ToScreenLocation(mpbounds.NorthEast);
            var br = map.Projection.ToScreenLocation(mpbounds.NorthEast);
            var region = tl.ToRectF(br);
            var features = map.QueryRenderedFeatures(region, layers);

            tl.Dispose();
            br.Dispose();
            region.Dispose();

            return features.Select(f => f.ToFeature(true)).ToArray();
        }

        public NFeature[] QueryFeatures(Point point, params string[] layers)
        {
            var xpoint = point.ToPointF();
            var features = map.QueryRenderedFeatures(xpoint, layers);
            xpoint.Dispose();
            
            return features.Select(f => f.ToFeature(true)).ToArray();
        }

        public NFeature[] QueryFeatures(Point point, float radius, params string[] layers)
        {
            var xpoint = point.ToPointF();
            var region = xpoint.ToRectF(radius);
            xpoint.Dispose();
            var features = map.QueryRenderedFeatures(region, layers);
            region.Dispose();

            return features.Select(f => f.ToFeature(true)).ToArray();
        }

        public NFeature[] QueryFeatures(Rectangle rectangle, params string[] layers)
        {
            var region = rectangle.ToRectF();
            var features = map.QueryRenderedFeatures(region, layers);
            region.Dispose();

            return features.Select(f => f.ToFeature(true)).ToArray();
        }
    }
}