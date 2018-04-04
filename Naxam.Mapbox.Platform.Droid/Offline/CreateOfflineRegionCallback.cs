using System;
using Com.Mapbox.Mapboxsdk.Offline;

namespace Naxam.Mapbox.Platform.Droid.Offline
{
    public class CreateOfflineRegionCallback: Java.Lang.Object, OfflineManager.ICreateOfflineRegionCallback
    {
        public CreateOfflineRegionCallback(Action<OfflineRegion> onCreateHandle, Action<string> onErrorHandle)
        {
            this.OnCreateHandle = onCreateHandle;
            this.OnErrorHandle = onErrorHandle;
        }

        public CreateOfflineRegionCallback(IntPtr handle, Android.Runtime.JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        public Action<OfflineRegion> OnCreateHandle { get; set; }
        public Action<string> OnErrorHandle { get; set; }

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
