using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Mapbox.MapboxSdk.Maps;

namespace Naxam.Mapbox.Platform.Droid
{
    public class MapViewFragment : SupportMapFragment, MapView.IOnMapChangedListener
    {
        private MapView mapView;
        public MapView.IOnMapChangedListener OnMapChangedListener { get; set; }

        public MapViewFragment (IntPtr javaReference, JniHandleOwnership transfer)
            : base (javaReference, transfer)
        {
        }

        public MapViewFragment () : base ()
        {
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            mapView = view as MapView;
            mapView?.AddOnMapChangedListener(this);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            mapView?.RemoveOnMapChangedListener(this);
        }

        public void OnMapChanged(int p0)
        {
            OnMapChangedListener?.OnMapChanged(p0);
        }
    }
}
