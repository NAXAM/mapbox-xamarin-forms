using System;
namespace Naxam.Controls.Mapbox.Forms
{
    public class CoordinateBounds
    {
        public CoordinateBounds()
        {
        }

        public Position SouthWest
        {
            get;
            set;
        }

        public Position NorthEast
        {
            get;
            set;
        }
    }
}
