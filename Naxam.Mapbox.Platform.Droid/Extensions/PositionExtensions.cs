using Com.Mapbox.Mapboxsdk.Geometry;
using NxLatLng = Naxam.Mapbox.LatLng;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public static class PositionExtensions
    {
        public static LatLng ToLatLng(this NxLatLng pos)
        {
            return new LatLng(pos.Lat, pos.Long);
        }
    }
}
