using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Sdk = Mapbox.MapboxSdk;
using View = Android.Views.View;

namespace Naxam.Mapbox.Platform.Droid
{
    public class MapViewFragment : Android.Support.V4.App.Fragment, Sdk.Maps.IOnMapReadyCallback, View.IOnTouchListener
    {
        public event EventHandler<MapboxMapReadyEventArgs> MapReady;
        public event EventHandler<MotionEvent> MapTouched;

        public MapViewFragment (IntPtr javaReference, JniHandleOwnership transfer)
            : base (javaReference, transfer)
        {
        }

        public MapViewFragment () : base ()
        {
        }

        Sdk.Maps.MapView mapView;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mapView = new Sdk.Maps.MapView (Context);
            mapView.OnCreate (savedInstanceState);
            mapView.LayoutParameters = new ViewGroup.LayoutParams (
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent);

            mapView.GetMapAsync (this);

            return mapView;
        }

        public override void OnStart ()
        {
            base.OnStart ();
            mapView.OnStart ();
        }

        public override void OnResume ()
        {
            base.OnResume ();
            mapView.OnResume ();
        }

        public override void OnSaveInstanceState (Bundle outState)
        {
            base.OnSaveInstanceState (outState);
            mapView.OnSaveInstanceState (outState);
        }

        public override void OnPause ()
        {
            base.OnPause ();
            mapView.OnPause ();
        }

        public override void OnStop ()
        {
            base.OnStop ();
            mapView.OnStop ();
        }

        public override void OnDestroy ()
        {
            base.OnDestroy ();
            mapView.OnDestroy ();
        }

        public override void OnLowMemory ()
        {
            base.OnLowMemory ();
            mapView.OnLowMemory ();
        }

        public void OnMapReady (Sdk.Maps.MapboxMap p0)
        {
            MapReady?.Invoke (this, new MapboxMapReadyEventArgs (p0, mapView));
        }

        public bool OnTouch (View v, MotionEvent e)
        {
            MapTouched?.Invoke (this, e);
            return false;
        }
    }
}
