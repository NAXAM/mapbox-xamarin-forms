using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Naxam.Mapbox;
using GeoJSON.Net.Feature;
using System.Linq;
using Naxam.Mapbox.Platform.iOS.Extensions;
using Mapbox;
using CoreAnimation;
using CoreGraphics;
using UIKit;

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
        CGSize GetSize()
        {
            var size = map.Bounds.Size;
            if (size == CGSize.Empty)
            {
                size = Bounds.Size;
            }

            if (size == CGSize.Empty)
            {
                size = UIScreen.MainScreen.Bounds.Size;
            }

            return size;
        }
        
        public void AnimateCamera(ICameraUpdate cameraPosition, int durationInMillisecond)
        {
            MGLMapCamera camera = cameraPosition switch
            {
                CameraPosition cp => cp.ToNative(GetSize()),
                CameraBounds cb => cb.ToNative(map),
                _ => null
            };

            Task.Delay(200) // TODO Should not have this delay
                .ContinueWith(t =>
                {
                    map.SetCamera(
                        camera,
                        durationInMillisecond / 1000.0,
                        CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut)
                    );
                })
                .ConfigureAwait(true);
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
