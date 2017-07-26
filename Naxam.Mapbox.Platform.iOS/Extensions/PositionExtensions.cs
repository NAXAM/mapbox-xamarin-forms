using System;
using CoreLocation;
using Naxam.Controls.Forms;

namespace Naxam.Controls.Platform.iOS
{
    public static class PositionExtensions
    {
        public static CLLocationCoordinate2D ToCLCoordinate(this Position pos)
        {
            return new CLLocationCoordinate2D(pos.Lat, pos.Long);
        }
    }
}
