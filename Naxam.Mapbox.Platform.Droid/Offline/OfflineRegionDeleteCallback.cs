using System;
using Com.Mapbox.Mapboxsdk.Offline;
namespace Naxam.Mapbox.Platform.Droid.Offline
{
    public class OfflineRegionDeleteCallback: Java.Lang.Object, OfflineRegion.IOfflineRegionDeleteCallback
    {
        public OfflineRegionDeleteCallback(Action deleteHandle, Action<string> errorHandle)
        {
            this.OnDeleteHandle = deleteHandle;
            this.OnErrorHandle = errorHandle;
        }

        public Action OnDeleteHandle { get; set; }
        public Action<string> OnErrorHandle { get; set; }

        public void OnDelete()
        {
            OnDeleteHandle?.Invoke();
        }

        public void OnError(string error)
        {
            OnErrorHandle?.Invoke(error);
        }
    }
}
