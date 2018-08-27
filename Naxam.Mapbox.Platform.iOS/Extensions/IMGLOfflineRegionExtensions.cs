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
                    SouthWest = TypeConverter.FromCoordinateToPosition(tpoRegion.Bounds.sw),
                    NorthEast = TypeConverter.FromCoordinateToPosition(tpoRegion.Bounds.ne),
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
                SouthWest = TypeConverter.FromCoordinateToPosition(region.Bounds.sw),
                NorthEast = TypeConverter.FromCoordinateToPosition(region.Bounds.ne),
            };
            output.MaximumZoomLevel = region.MaximumZoomLevel;
            output.MinimumZoomLevel = region.MinimumZoomLevel;
            output.StyleURL = region.StyleURL?.AbsoluteString;
            return output;
        }
    }
}
