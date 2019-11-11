using Com.Mapbox.Mapboxsdk.Geometry;
using NxLatLng = Naxam.Mapbox.LatLng;
using NxLatLngBounds = Naxam.Mapbox.LatLngBounds;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public static class PositionExtensions
    {
        public static LatLng ToLatLng(this NxLatLng pos)
        {
            return new LatLng(pos.Lat, pos.Long);
        }

        public static NxLatLng ToLatLng(this LatLng pos)
        {
            return new NxLatLng(pos.Latitude, pos.Longitude);
        }

        public static LatLngBounds ToLatLngBounds(this NxLatLngBounds pos)
        {
            return LatLngBounds.From(pos.NorthEast.Lat, pos.NorthEast.Long, pos.SouthWest.Lat, pos.SouthWest.Long);
        }
    }
}
