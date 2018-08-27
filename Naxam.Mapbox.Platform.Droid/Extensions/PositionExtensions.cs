using System;
using Com.Mapbox.Mapboxsdk.Geometry;
using Naxam.Controls.Mapbox.Forms;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public static class PositionExtensions
    {
        public static LatLng ToLatLng(this Position pos)
        {
            return new LatLng(pos.Lat, pos.Long);
        }
    }
}
