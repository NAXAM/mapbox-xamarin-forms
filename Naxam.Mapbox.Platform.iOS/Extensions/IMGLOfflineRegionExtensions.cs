using System;
using Mapbox;
using Naxam.Controls.Mapbox.Forms;

namespace Naxam.Controls.Mapbox.Platform.iOS.Extensions
{
    public static class IMGLOfflineRegionExtensions
    {
        public static OfflinePackRegion ToFormsRegion(this IMGLOfflineRegion region)
        {
            if (region == null) return null;
            var output = new OfflinePackRegion();
            if (region is MGLTilePyramidOfflineRegion tpoRegion)
            {
                output.Bounds = new CoordinateBounds()
                {
                    SouthWest = TypeConverter.FromCoordinateToPosition(tpoRegion.Bounds.Sw),
                    NorthEast = TypeConverter.FromCoordinateToPosition(tpoRegion.Bounds.Ne),
                };
                output.MaximumZoomLevel = tpoRegion.MaximumZoomLevel;
                output.MinimumZoomLevel = tpoRegion.MinimumZoomLevel;
                output.StyleURL = tpoRegion.StyleURL?.AbsoluteString;
            }
            return output;
        }
    }

    public static class MGLTilePyramidOfflineRegionExtensions
    {
        public static OfflinePackRegion ToFormsRegion(this MGLTilePyramidOfflineRegion region)
        {
            if (region == null) return null;
            var output = new OfflinePackRegion();

            output.Bounds = new CoordinateBounds()
            {
                SouthWest = TypeConverter.FromCoordinateToPosition(region.Bounds.Sw),
                NorthEast = TypeConverter.FromCoordinateToPosition(region.Bounds.Ne),
            };
            output.MaximumZoomLevel = region.MaximumZoomLevel;
            output.MinimumZoomLevel = region.MinimumZoomLevel;
            output.StyleURL = region.StyleURL?.AbsoluteString;
            return output;
        }
    }
}
