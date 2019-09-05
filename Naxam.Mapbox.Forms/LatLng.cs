using System;

namespace Naxam.Mapbox
{
    public class LatLng
    {
        public double Lat { get; set; }

        public double Long { get; set; }

        public LatLng() { }

        public LatLng(double lat, double lon)
        {
            Lat = lat;
            Long = lon;
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
}

