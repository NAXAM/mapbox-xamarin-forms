using System;
using Com.Mapbox.Mapboxsdk.Geometry;
using Naxam.Controls.Forms;

namespace Naxam.Controls.Platform.Droid
{
    public static class PositionExtensions
    {
        public static LatLng ToLatLng(this Position pos) {
            return new LatLng(pos.Lat, pos.Long);
        }
    }
}
