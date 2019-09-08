using System;
using System.Linq;
using CoreGraphics;
using CoreLocation;
using Mapbox;
using Naxam.Mapbox;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public static class LatLngExtensions
    {
        public static CLLocationCoordinate2D ToCLCoordinate(this LatLng pos)
        {
            return new CLLocationCoordinate2D(pos.Lat, pos.Long);
        }

        public static LatLng ToLatLng(this CLLocationCoordinate2D pos)
        {
            return new LatLng(pos.Latitude, pos.Longitude);
        }

        public static LatLng[] ToLatLngs(CLLocationCoordinate2D[] coordinates)
        {
            if (coordinates == null)
            {
                return new LatLng[0];
            }

            return coordinates.Select(x => x.ToLatLng()).ToArray();
        }

        public static CLLocationCoordinate2D[] ToCLCoordinates(LatLng[] positions)
        {
            if (positions == null)
            {
                return new CLLocationCoordinate2D[0];
            }
            return positions.Select(x => x.ToCLCoordinate()).ToArray();
        }

        public static MGLCoordinateBounds ToCLRegion(this LatLngBounds pos)
        {
            return new MGLCoordinateBounds {
                ne = pos.NorthEast.ToCLCoordinate(),
                sw = pos.SouthWest.ToCLCoordinate()
            };
        }

        public static LatLngBounds ToLatLngBounds(this MGLCoordinateBounds pos)
        {
            return new LatLngBounds(pos.sw.ToLatLng(), pos.ne.ToLatLng());
        }

        public static CGRect ToRect(this CGPoint pos, nfloat radius)
        {
            return CGRect.FromLTRB(
                pos.X - radius,
                pos.Y - radius,
                pos.X + radius,
                pos.Y + radius
                );
        }

        public static CGRect ToRect(this CGPoint tl, CGPoint rb)
        {
            return CGRect.FromLTRB(
                tl.X,
                tl.Y,
                rb.X,
                rb.Y
                );
        }

        public static CGRect ToRect(this Xamarin.Forms.Rectangle rectangle)
        {
            return CGRect.FromLTRB(
               (nfloat)rectangle.Left,
               (nfloat)rectangle.Top,
               (nfloat)rectangle.Right,
               (nfloat)rectangle.Bottom
            );
        }

        public static CGPoint ToPoint(this Xamarin.Forms.Point tl)
        {
            return new CGPoint(tl.X, tl.Y);
        }
    }
}
