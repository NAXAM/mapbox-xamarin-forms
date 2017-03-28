using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Mapbox.Sdk.Maps;

[assembly:Xamarin.Forms.ExportRenderer(typeof(MapBoxQs.MapView), typeof(MapBoxQs.Droid.MapViewRenderer))]
namespace MapBoxQs.Droid
{
    [Activity(Label = "MapBoxQs.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

			Mapbox.Sdk.MapboxAccountManager.Start(this, "sk.eyJ1IjoiamVzcGVyZGF4IiwiYSI6ImNpemo2ajloNTAwMmwyd3I0NWoxNHZoNTYifQ.TnTUuIPwpZzGfS47cr0YMw");

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
    }

    public class MapViewRenderer : Xamarin.Forms.Platform.Android.ViewRenderer<MapBoxQs.MapView, View>
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null) { 
                //Remove event handlers
            }

            if (e.NewElement == null) return;

            if (Control == null) { 
                var view = LayoutInflater.FromContext(Context)
                                         .Inflate(Resource.Layout.map_view_container, ViewGroup, false);

                SetNativeControl(view);
            }
        }
    }

    public class MapViewFragment : Android.Support.V4.App.Fragment, Mapbox.Sdk.Maps.IOnMapReadyCallback
    {
        public MapViewFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        public MapViewFragment() : base()
        {
            
        }

        Mapbox.Sdk.Maps.MapView mapView;
        MapboxMap map;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mapView = new Mapbox.Sdk.Maps.MapView(Context);
            mapView.OnCreate(savedInstanceState);
			mapView.LayoutParameters = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, 
                ViewGroup.LayoutParams.MatchParent);

            mapView.GetMapAsync(this);

            return mapView;
        }

        public override void OnResume()
        {
            base.OnResume();
            mapView.OnResume();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mapView.OnSaveInstanceState(outState);
        }

        public override void OnPause()
		{
			mapView.OnPause();
            base.OnPause();
        }

		public override void OnDestroy()
        {
            mapView.OnDestroy();
			base.OnDestroy();
        }

        public override void OnLowMemory()
        {
            mapView.OnLowMemory();
            base.OnLowMemory();
        }

        public void OnMapReady(MapboxMap p0)
        {
            map = p0;
            //throw new NotImplementedException();
        }
    }
}
