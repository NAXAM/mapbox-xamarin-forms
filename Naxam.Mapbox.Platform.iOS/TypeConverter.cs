using System;
using Naxam.Controls.Mapbox.Forms;
using CoreLocation;
using System.Collections.Generic;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public static class TypeConverter
    {
        public static CLLocationCoordinate2D FromPositionToCoordinate(Position position)
        {
            if (position == null)
            {
                return default(CLLocationCoordinate2D);
            }
            return new CLLocationCoordinate2D(position.Lat, position.Long);
        }

        public static Position FromCoordinateToPosition(CLLocationCoordinate2D coordinate)
        {
            return new Position(coordinate.Latitude, coordinate.Longitude);
        }

        public static Position[] FromCoordinatesToPosition(CLLocationCoordinate2D[] coordinates)
        {
            if (coordinates == null)
            {
                return null;
            }
            var output = new List<Position>();
            foreach (CLLocationCoordinate2D coord in coordinates)
            {
                output.Add(FromCoordinateToPosition(coord));
            }
            return output.ToArray();
        }

        public static CLLocationCoordinate2D[] FromPositionsToCoordinates(Position[] positions)
        {
            if (positions == null)
            {
                return new CLLocationCoordinate2D[0];
            }
            var output = new List<CLLocationCoordinate2D>();
            foreach (Position pos in positions)
            {
                output.Add((FromPositionToCoordinate(pos)));
            }
            return output.ToArray();
        }
    }
}
