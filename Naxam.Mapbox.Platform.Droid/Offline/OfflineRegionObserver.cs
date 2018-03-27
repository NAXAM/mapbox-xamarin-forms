using System;
using Com.Mapbox.Mapboxsdk.Offline;
namespace Naxam.Mapbox.Platform.Droid.Offline
{
    public class OfflineRegionObserver: Java.Lang.Object, OfflineRegion.IOfflineRegionObserver
    {
        public OfflineRegionObserver()
        {
        }

        internal Action<long> MapboxTileCountLimitExceededHandle;
        internal Action<OfflineRegionError> OnErrorHandle;
        internal Action<OfflineRegionStatus> OnStatusChangedHandle;

        public void MapboxTileCountLimitExceeded(long p0)
        {
            MapboxTileCountLimitExceededHandle?.Invoke(p0);
        }

        public void OnError(OfflineRegionError p0)
        {
            OnErrorHandle?.Invoke(p0);
        }

        public void OnStatusChanged(OfflineRegionStatus p0)
        {
            OnStatusChangedHandle?.Invoke(p0);
        }
    }
}
