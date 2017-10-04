using System;
using static Com.Mapbox.Mapboxsdk.Maps.MapboxMap;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public class CancelableCallback: Java.Lang.Object, ICancelableCallback
    {
        public Action FinishHandler;
        public Action CancelHandler;

        public CancelableCallback()
        {
        }

        public void OnCancel()
        {
            CancelHandler?.Invoke();
        }

        public void OnFinish()
        {
            FinishHandler?.Invoke();
        }
    }
}
