using CoreLocation;
using Naxam.Mapbox;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public static class PositionExtensions
    {
        public static CLLocationCoordinate2D ToCLCoordinate(this LatLng pos)
        {
            return new CLLocationCoordinate2D(pos.Lat, pos.Long);
        }
    }
}
