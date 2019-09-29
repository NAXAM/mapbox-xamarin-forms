using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Naxam.Mapbox;
using GeoJSON.Net.Feature;
using System.Linq;
using Naxam.Mapbox.Platform.iOS.Extensions;
using Mapbox;
using CoreAnimation;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : IMapFunctions
    {
        public Feature[] QueryFeatures(LatLng latLng, params string[] layers)
        {
            var coords = latLng.ToCLCoordinate();
            var point = map.ConvertCoordinate(coords, null);
            var features = map.VisibleFeaturesAtPoint(point, layers);
            return features.Select(f => f.ToFeature(true)).ToArray();
        }

        public Feature[] QueryFeatures(LatLng latLng, float radius, params string[] layers)
        {
            var coords = latLng.ToCLCoordinate();
            var point = map.ConvertCoordinate(coords, null);
            var rect = point.ToRect(radius);
            var features = map.VisibleFeaturesInRect(rect, layers);
            return features.Select(f => f.ToFeature(true)).ToArray();
        }

        public Feature[] QueryFeatures(LatLngBounds bounds, params string[] layers)
        {
            var ne = bounds.NorthEast.ToCLCoordinate();
            var sw = bounds.SouthWest.ToCLCoordinate();
            var tl = map.ConvertCoordinate(ne, null);
            var rb = map.ConvertCoordinate(sw, null);
            var rect = tl.ToRect(rb);
            var features = map.VisibleFeaturesInRect(rect, layers);
            return features.Select(f => f.ToFeature(true)).ToArray();
        }

        public Feature[] QueryFeatures(Point position, params string[] layers)
        {
            var point = position.ToPoint();
            var features = map.VisibleFeaturesAtPoint(point, layers);
            return features.Select(f => f.ToFeature(true)).ToArray();
        }

        public Feature[] QueryFeatures(Point position, float radius, params string[] layers)
        {
            var point = position.ToPoint();
            var rect = point.ToRect(radius);
            var features = map.VisibleFeaturesInRect(rect, layers);
            return features.Select(f => f.ToFeature(true)).ToArray();
        }

        public Feature[] QueryFeatures(Rectangle rectangle, params string[] layers)
        {
            var rect = rectangle.ToRect();
            var features = map.VisibleFeaturesInRect(rect, layers);
            return features.Select(f => f.ToFeature(true)).ToArray();
        }
    }

    public partial class MapViewRenderer
    {
        public void AnimiateCamera(CameraPosition cameraPosition, int durationInMillisecond)
        {
            MGLMapCamera camera = new MGLMapCamera();

            if (cameraPosition.Bearing.HasValue)
            {
                camera.Heading = cameraPosition.Bearing.Value;
            }

            if (cameraPosition.Target.HasValue)
            {
                camera.CenterCoordinate = cameraPosition.Target.Value.ToCLCoordinate();
            }

            if (cameraPosition.Tilt.HasValue)
            {
                camera.Pitch = (nfloat)cameraPosition.Tilt.Value;
            }

            if (cameraPosition.Zoom.HasValue)
            {
                camera.Altitude = cameraPosition.Zoom.Value;
            }

            // TODO Find an equivalent method of AnimateCamera on Android
            map.SetCamera(
                 camera,
                 durationInMillisecond,
                 null
                 );
        }

        public Task<byte[]> TakeSnapshotAsync(LatLngBounds bounds = default)
        {
            var image = map.Capture(true);
            var imageData = image.AsJPEG();
            var imgByteArray = new byte[imageData.Length];

            System.Runtime.InteropServices.Marshal.Copy(
                imageData.Bytes,
                imgByteArray,
                0,
                Convert.ToInt32(imageData.Length));

            return Task.FromResult(imgByteArray);
        }
    }
}
