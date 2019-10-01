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
                TopLeft =  quad.TopLeft.ToCLCoordinate(),
                TopRight =  quad.TopRight.ToCLCoordinate(),
                BottomRight =  quad.BottomRight.ToCLCoordinate(),
                BottomLeft =  quad.BottomLeft.ToCLCoordinate(),
            };
        }
    }
}