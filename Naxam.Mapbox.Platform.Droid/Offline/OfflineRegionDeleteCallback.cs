using System;
using Com.Mapbox.Mapboxsdk.Offline;
namespace Naxam.Mapbox.Platform.Droid.Offline
{
    public class OfflineRegionDeleteCallback: Java.Lang.Object, OfflineRegion.IOfflineRegionDeleteCallback
    {
        public OfflineRegionDeleteCallback()
        {
        }

        public Action OnDeleteHandle;
        public Action<string> OnErrorHandle;

        public void OnDelete()
        {
            OnDeleteHandle?.Invoke();
        }

        public void OnError(string p0)
        {
            OnErrorHandle?.Invoke(p0);
        }
    }
}
