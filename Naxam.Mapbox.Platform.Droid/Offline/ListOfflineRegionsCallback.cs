using System;
using Com.Mapbox.Mapboxsdk.Offline;
namespace Naxam.Mapbox.Platform.Droid.Offline
{
    public class ListOfflineRegionsCallback: Java.Lang.Object, OfflineManager.IListOfflineRegionsCallback
    {
        public ListOfflineRegionsCallback()
        {
        }

        public Action<OfflineRegion[]> OnListHandle;
        public Action<string> OnErrorHandle;

        public void OnError(string p0)
        {
            OnErrorHandle?.Invoke(p0);
        }

        public void OnList(OfflineRegion[] p0)
        {
            OnListHandle?.Invoke(p0);
        }
    }
}
