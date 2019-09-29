using Naxam.Controls.Mapbox.Platform.Droid;

namespace Naxam.Mapbox.Platform.Droid.Extensions
{
    public static class LatLngQuadExtensions
    {
        public static Com.Mapbox.Mapboxsdk.Geometry.LatLngQuad ToNative(this LatLngQuad quad)
        {
            return new Com.Mapbox.Mapboxsdk.Geometry.LatLngQuad(
                quad.TopLeft.ToLatLng(),
                quad.TopRight.ToLatLng(),
                quad.BottomRight.ToLatLng(),
                quad.BottomLeft.ToLatLng()
                );
        }
    }
}