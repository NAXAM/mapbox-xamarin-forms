using System;
using Mapbox;
using Naxam.Controls.Forms;
using Naxam.Mapbox;

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
                output.Bounds = tpoRegion.Bounds.ToLatLngBounds();
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
            var output = new OfflinePackRegion
            {
                Bounds = region.Bounds.ToLatLngBounds(),
                MaximumZoomLevel = region.MaximumZoomLevel,
                MinimumZoomLevel = region.MinimumZoomLevel,
                StyleURL = region.StyleURL?.AbsoluteString
            };
            return output;
        }
    }
}
