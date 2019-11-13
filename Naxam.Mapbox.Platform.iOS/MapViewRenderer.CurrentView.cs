using Naxam.Mapbox;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer
    {
        public LatLngBounds GetVisibleBounds()
        {
            return map.VisibleCoordinateBounds.ToLatLngBounds();
        }
    }
}