using System;
using Naxam.Controls.Forms;
using CoreLocation;
using System.Collections.Generic;
using Naxam.Mapbox;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public static class TypeConverter
    {
        public static CLLocationCoordinate2D FromPositionToCoordinate(LatLng position)
        {
            if (position == null)
            {
                return default;
            }
            return position.ToCLCoordinate();
        }

        public static LatLng FromCoordinateToPosition(CLLocationCoordinate2D coordinate)
        {
            return new LatLng(coordinate.Latitude, coordinate.Longitude);
        }

        public static LatLng[] FromCoordinatesToPosition(CLLocationCoordinate2D[] coordinates)
        {
            if (coordinates == null)
            {
                return null;
            }
            var output = new List<LatLng>();
            foreach (CLLocationCoordinate2D coord in coordinates)
            {
                output.Add(FromCoordinateToPosition(coord));
            }
            return output.ToArray();
        }

        public static CLLocationCoordinate2D[] FromPositionsToCoordinates(LatLng[] positions)
        {
            if (positions == null)
            {
                return new CLLocationCoordinate2D[0];
            }
            var output = new List<CLLocationCoordinate2D>();
            foreach (LatLng pos in positions)
            {
                output.Add((FromPositionToCoordinate(pos)));
            }
            return output.ToArray();
        }
    }
}
