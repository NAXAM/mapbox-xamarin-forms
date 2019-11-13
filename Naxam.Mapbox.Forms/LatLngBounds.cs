using System;
using System.Collections.Generic;

namespace Naxam.Mapbox
{
    public struct LatLngBounds : IEquatable<LatLngBounds>
    {
        public LatLngBounds(LatLng sw, LatLng ne)
        {
            SouthWest = sw;
            NorthEast = ne;
        }

        public LatLng SouthWest { get; set; }
        public LatLng NorthEast { get; set; }

        public bool IsEmpty()
        {
            return SouthWest == NorthEast;
        }

        public override int GetHashCode()
        {
            return SouthWest.GetHashCode() ^ NorthEast.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is LatLngBounds other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(LatLngBounds other)
        {
            return SouthWest.Equals(other.SouthWest)
                && NorthEast.Equals(other.NorthEast);
        }

        public static bool operator ==(LatLngBounds left, LatLngBounds right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(LatLngBounds left, LatLngBounds right)
        {
            return false == left.Equals(right);
        }

        public static LatLngBounds FromLatLngs(List<LatLng> latLngs)
        {
            double minLat = GeometryConstants.MAX_LATITUDE;
            double minLon = GeometryConstants.MAX_LONGITUDE;
            double maxLat = GeometryConstants.MIN_LATITUDE;
            double maxLon = GeometryConstants.MIN_LONGITUDE;

            foreach (var gp in latLngs) {
                minLat = Math.Min(minLat, gp.Lat);
                minLon = Math.Min(minLon, gp.Long);
                maxLat = Math.Max(maxLat, gp.Lat);
                maxLon = Math.Max(maxLon, gp.Long);
            }

            return new LatLngBounds(
                new LatLng(minLat, minLon),
                new LatLng(maxLat, maxLon)
                );
        }
    }

    /**
     * Contains constants used throughout the sdk classes.
     */
    public static class GeometryConstants
    {

        /**
         * The <a href='http://en.wikipedia.org/wiki/Earth_radius#Equatorial_radius'>equatorial radius</a>
         * value in meters
         */
        public const int RADIUS_EARTH_METERS = 6378137;

        /**
         * This constant represents the lowest longitude value available to represent a wrapped geolocation.
         */
        public const double MIN_WRAP_LONGITUDE = -180;

        /**
         * This constant represents the highest longitude value available to represent a wrapped geolocation.
         */
        public const double MAX_WRAP_LONGITUDE = 180;

        /**
         * This constant represents the lowest longitude value available to represent a geolocation.
         */
        public const double MIN_LONGITUDE = -Double.MaxValue;

        /**
         * This constant represents the highest longitude value available to represent a geolocation.
         */
        public const double MAX_LONGITUDE = Double.MaxValue;

        /**
         * This constant represents the lowest latitude value available to represent a geolocation.
         */
        public const double MIN_LATITUDE = -90;

        /**
         * This constant represents the latitude span when representing a geolocation.
         */
        public const double LATITUDE_SPAN = 180;

        /**
         * This constant represents the longitude span when representing a geolocation.
         */
        public const double LONGITUDE_SPAN = 360;

        /**
         * This constant represents the highest latitude value available to represent a geolocation.
         */
        public const double MAX_LATITUDE = 90;

        /**
         * Maximum latitude value in Mercator projection.
         */
        public const double MAX_MERCATOR_LATITUDE = 85.05112877980659;

        /**
         * Minimum latitude value in Mercator projection.
         */
        public const double MIN_MERCATOR_LATITUDE = -85.05112877980659;

    }
}
