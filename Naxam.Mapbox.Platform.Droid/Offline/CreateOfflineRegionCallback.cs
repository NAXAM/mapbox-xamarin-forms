using System;
using Com.Mapbox.Mapboxsdk.Offline;

namespace Naxam.Mapbox.Platform.Droid.Offline
{
    public class CreateOfflineRegionCallback: Java.Lang.Object, OfflineManager.ICreateOfflineRegionCallback
    {
        public CreateOfflineRegionCallback()
        {
        }

        public Action<OfflineRegion> OnCreateHandle;
        public Action<string> OnErrorHandle;

		public void OnCreate(OfflineRegion p0)
        {
            OnCreateHandle?.Invoke(p0);

        }

        public void OnError(string p0)
        {
            OnErrorHandle?.Invoke(p0);
        }
    }
}
