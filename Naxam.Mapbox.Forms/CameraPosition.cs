using System;
using Xamarin.Forms;

namespace Naxam.Mapbox
{
    public interface ICameraUpdate { }

    public struct CameraPosition : ICameraUpdate, IEquatable<CameraPosition>
    {
        public static CameraPosition Default = new CameraPosition(LatLng.Zero, 0, 0, 0);

        public double? Bearing { get; private set; }
        public LatLng? Target { get; private set; }
        public double? Tilt { get; private set; }
        public double? Zoom { get; private set; }

        public CameraPosition(LatLng target, double? zoom, double? tilt, double? bearing)
        {
            Bearing = bearing;
            Target = target;
            Tilt = tilt;
            Zoom = zoom;
        }

        public override int GetHashCode()
        {
            return Bearing.GetHashCode() ^ Target.GetHashCode() ^ Tilt.GetHashCode() ^ Zoom.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is CameraPosition other) {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(CameraPosition other)
        {
            return other.Bearing.Equals(Bearing)
                && other.Tilt.Equals(Tilt)
                && other.Zoom.Equals(Zoom)
                && Target.Equals(other.Target);
        }

        public static bool operator ==(CameraPosition left, CameraPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CameraPosition left, CameraPosition right)
        {
            return false == left.Equals(right);
        }
    }

    public struct CameraBounds : ICameraUpdate
    {
        public LatLngBounds Bounds { get; private set; }

        public Thickness Padding { get; private set; }

        public double? Bearing { get; private set; }

        public double? Tilt { get; private set; }

        public CameraBounds(
            LatLngBounds bounds,
            Thickness padding,
            double? bearing = null,
            double? tilt = null)
        {
            Bounds = bounds;
            Padding = padding;
            Bearing = bearing;
            Tilt = tilt;
        }
    }
}

