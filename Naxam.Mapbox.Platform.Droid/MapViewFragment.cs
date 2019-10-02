using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Maps;
using Com.Mapbox.Mapboxsdk.Utils;
using Naxam.Mapbox.Platform.Droid.Extensions;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public class MapViewFragment : SupportMapFragment
    {
        public static MapViewFragment Create(Naxam.Controls.Forms.MapView mapView, Context context)
        {
            var mapOptions = mapView.CreateOptions(context);

            return new MapViewFragment {Arguments = MapFragmentUtils.CreateFragmentArgs(mapOptions)};
        }
        
        public MapView MapView { get; private set; }

        public bool StateSaved { get; private set; }

        public MapViewFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public MapViewFragment() : base() { }
        
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            MapView = view as MapView;
        }


        public override void OnDestroyView()
        {
            base.OnDestroyView();
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        internal void ToggleInfoWindow(MapboxMap mapboxMap, Marker marker)
        {
            if (marker.IsInfoWindowShown)
            {
                mapboxMap.DeselectMarker(marker);
            }
            else
            {
                mapboxMap.SelectMarker(marker);
            }
        }
    }
}
