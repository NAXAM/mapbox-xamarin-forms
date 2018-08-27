using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Com.Mapbox.Mapboxsdk.Maps;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public class MapViewFragment : SupportMapFragment, MapView.IOnMapChangedListener
    {
        public MapView MapView { get; private set; }

        public MapView.IOnMapChangedListener OnMapChangedListener { get; set; }

        public bool StateSaved { get; private set; }

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

            MapView = view as MapView; 
            MapView?.AddOnMapChangedListener(this);
        }
         

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            MapView?.RemoveOnMapChangedListener(this);
        }

        public void OnMapChanged(int p0)
        {
            OnMapChangedListener?.OnMapChanged(p0);
        }

        public override void OnResume()
        {
            base.OnResume();
           //StateSaved = false;
        }

        

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState); 
            //StateSaved = true;
        }
    }
}
