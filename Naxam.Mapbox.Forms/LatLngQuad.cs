using System;

namespace Naxam.Mapbox
{
    public struct LatLngQuad : IEquatable<LatLngQuad>
    {
        public LatLngQuad(LatLng tl, LatLng tr, LatLng br, LatLng bl)
        {
            TopLeft = tl;
            TopRight = tr;
            BottomRight = br;
            BottomLeft = bl;
        }

        public LatLng TopLeft { get; private set; }
        public LatLng TopRight { get; private set; }
        public LatLng BottomRight { get; private set; }
        public LatLng BottomLeft { get; private set; }

        public override int GetHashCode()
        {
            return TopLeft.GetHashCode() ^ TopRight.GetHashCode() ^ BottomRight.GetHashCode() ^ BottomLeft.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is LatLngQuad other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(LatLngQuad other)
        {
            return TopLeft.Equals(other.TopLeft)
                && TopRight.Equals(other.TopRight)
                && BottomLeft.Equals(other.BottomLeft)
                && BottomRight.Equals(other.BottomRight);
        }

        public static bool operator ==(LatLngQuad left, LatLngQuad right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(LatLngQuad left, LatLngQuad right)
        {
            return false == left.Equals(right);
        }
    }
}
