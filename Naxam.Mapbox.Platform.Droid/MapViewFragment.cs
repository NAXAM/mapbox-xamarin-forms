using System;
using Android.Runtime;
using Mapbox.MapboxSdk.Maps;

namespace Naxam.Mapbox.Platform.Droid
{
    public class MapViewFragment : SupportMapFragment
    {
        public MapViewFragment (IntPtr javaReference, JniHandleOwnership transfer)
            : base (javaReference, transfer)
        {
        }

        public MapViewFragment () : base ()
        {
        }
    }
}
