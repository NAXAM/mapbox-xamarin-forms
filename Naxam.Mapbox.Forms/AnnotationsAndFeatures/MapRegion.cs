using Naxam.Controls.Forms;
using System;

namespace Naxam.Mapbox.Forms.AnnotationsAndFeatures
{
    public struct MapRegion : IEquatable<MapRegion>
    {
        public static MapRegion Empty = new MapRegion();

        public Position NorthEast { get; set; }
        public Position SouthWest { get; set; }

        public MapRegion(Position ne, Position sw)
        {
            NorthEast = ne;
            SouthWest = sw;
        }

        public override bool Equals(object obj)
        {
            if (obj is MapRegion)
            {
                return this.Equals((MapRegion)obj);
            }
            return false;
        }

        public bool Equals(MapRegion p)
        {
            return (NorthEast == p.NorthEast)
                && (SouthWest == p.SouthWest);
        }

        public override int GetHashCode()
        {
            return SouthWest.GetHashCode() ^ NorthEast.GetHashCode();
        }

        public static bool operator ==(MapRegion lhs, MapRegion rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(MapRegion lhs, MapRegion rhs)
        {
            return !(lhs.Equals(rhs));
        }
    }
}
