using Mapbox;
using Naxam.Controls.Mapbox.Platform.iOS;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class LatLngQuadExtensions
    {
        public static MGLCoordinateQuad ToNative(this LatLngQuad quad)
        {
            return new MGLCoordinateQuad
            {
                topLeft =  quad.TopLeft.ToCLCoordinate(),
                topRight =  quad.TopRight.ToCLCoordinate(),
                bottomRight =  quad.BottomRight.ToCLCoordinate(),
                bottomLeft =  quad.BottomLeft.ToCLCoordinate(),
            };
        }
    }
}