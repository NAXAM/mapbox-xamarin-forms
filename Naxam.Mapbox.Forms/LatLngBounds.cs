using System;

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
    }
}
