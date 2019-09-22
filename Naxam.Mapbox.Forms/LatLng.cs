using System;
using System.Text.RegularExpressions;

namespace Naxam.Mapbox
{
    public struct LatLng : IEquatable<LatLng>
    {
        public static LatLng Zero = new LatLng(0, 0);

        public static LatLng FromLngLat(double lng, double lat)
        {
            return new LatLng(lat, lng);
        }

        public double Lat { get; set; }

        public double Long { get; set; }

        public LatLng(double lat, double lng)
        {
            Lat = lat;
            Long = lng;
        }

        public double DistanceTo(LatLng targetCoordinates)
        {
            return DistanceTo(targetCoordinates, UnitOfLength.Kilometers);
        }

        public double DistanceTo(LatLng targetCoordinates, UnitOfLength unitOfLength)
        {
            var baseRad = Math.PI * Lat / 180;
            var targetRad = Math.PI * targetCoordinates.Lat / 180;
            var theta = Long - targetCoordinates.Long;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(dist);
        }

        public override int GetHashCode()
        {
            return Lat.GetHashCode() ^ Long.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is LatLng other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(LatLng other)
        {
            return Math.Abs(other.Lat - Lat) < 0.000000001
                && Math.Abs(other.Long - Long) < 0.00000001;
        }

        public static bool operator ==(LatLng left, LatLng right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LatLng left, LatLng right)
        {
            return false == left.Equals(right);
        }

        public static implicit operator LatLng(double[] coordinates)
        {
            if (coordinates?.Length < 2)
            {
                throw new InvalidOperationException("Must provide an array of at least two doubles: first is Latitube, second is Longitude");
            }

            return new LatLng(coordinates[0], coordinates[1]);
        }

        static Regex COORDINATE_PATTERN = new Regex("^(-)?[\\d]+(.[\\d]*)?,(-)?[\\d]+(.[\\d]*)?$");
        public static implicit operator LatLng(string coordinatesInString)
        {
            if (string.IsNullOrWhiteSpace(coordinatesInString) || false == COORDINATE_PATTERN.IsMatch(coordinatesInString))
            {
                throw new InvalidOperationException("Must provide a strign in format of `lat,lng`. E.g. 21.0001,105.0011");
            }

            var coordinates = coordinatesInString.Split(',');

            return new LatLng(double.Parse(coordinates[0]), double.Parse(coordinates[1]));
        }
    }

    public class UnitOfLength
    {
        public static UnitOfLength Meters = new UnitOfLength(1609.344);
        public static UnitOfLength Kilometers = new UnitOfLength(1.609344);
        public static UnitOfLength NauticalMiles = new UnitOfLength(0.8684);
        public static UnitOfLength Miles = new UnitOfLength(1);

        private readonly double _fromMilesFactor;

        private UnitOfLength(double fromMilesFactor)
        {
            _fromMilesFactor = fromMilesFactor;
        }

        public double ConvertFromMiles(double input)
        {
            return input * _fromMilesFactor;
        }
    }

    public class TransitionOptions
    {
        public long Delay { get; set; }

        public long Duration { get; set; }

        public bool IsEnablePlacementTransitions { get; set; }
    }
}

