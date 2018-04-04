using System;
using Com.Mapbox.Mapboxsdk.Offline;
namespace Naxam.Mapbox.Platform.Droid.Offline
{
    public class OfflineRegionObserver: Java.Lang.Object, OfflineRegion.IOfflineRegionObserver
    {
        public OfflineRegionObserver(Action<OfflineRegionStatus> statusHandle, 
                                     Action<OfflineRegionError>  errorHandle,
                                     Action<long> tileCountLimitExceededHanle)
        {
            this.OnStatusChangedHandle = statusHandle;
            this.OnErrorHandle = errorHandle;
            this.OnMapboxTileCountLimitExceededHandle = tileCountLimitExceededHanle;
        }

        internal Action<long> OnMapboxTileCountLimitExceededHandle { get; set; }
        internal Action<OfflineRegionError> OnErrorHandle { get; set; }
        internal Action<OfflineRegionStatus> OnStatusChangedHandle { get; set; }

        public void MapboxTileCountLimitExceeded(long p0)
        {
            OnMapboxTileCountLimitExceededHandle?.Invoke(p0);
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
