using System;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Naxam.Mapbox.Platform.Droid;
using Sdk = Mapbox.Sdk;//alias

[assembly: Xamarin.Forms.ExportRenderer(typeof(Naxam.Mapbox.Forms.MapView), typeof(MapViewRenderer))]
namespace Naxam.Mapbox.Platform.Droid
{
	public class MapViewRenderer : Xamarin.Forms.Platform.Android.ViewRenderer<Naxam.Mapbox.Forms.MapView, View>
	{
		MapViewFragment fragment;

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Naxam.Mapbox.Forms.MapView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				//Remove event handlers
				fragment.MapReady -= MapReady;
			}

			if (e.NewElement == null) return;

			if (Control == null)
			{
				var view = LayoutInflater.FromContext(Context)
										 .Inflate(Resource.Layout.map_view_container, ViewGroup, false);

				var activity = (AppCompatActivity)Context;
				fragment = (MapViewFragment)activity.SupportFragmentManager.FindFragmentById(Resource.Id.map);
				fragment.MapReady += MapReady;

				SetNativeControl(view);
			}
		}

        Sdk.Maps.MapboxMap map;

		void MapReady(object sender, MapboxMapReadyEventArgs e)
		{
            map = e.Map;

            var y = Sdk.Camera.CameraUpdateFactory.ZoomBy((float)Element.ZoomLevel);
            map.AnimateCamera(y);

            if (Element.Center == null) return;

            var x = Sdk.Camera.CameraUpdateFactory.NewLatLng(new Sdk.Geometry.LatLng(Element.Center.Lat, Element.Center.Long));

			map.AnimateCamera(x);
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
		}
	}

    public class MapViewFragment : Android.Support.V4.App.Fragment, Sdk.Maps.IOnMapReadyCallback
	{
		public event EventHandler<MapboxMapReadyEventArgs> MapReady;

		public MapViewFragment(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{

		}

		public MapViewFragment() : base()
		{

		}

		Sdk.Maps.MapView mapView;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			mapView = new Sdk.Maps.MapView(Context);
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

        public void OnMapReady(Sdk.Maps.MapboxMap p0)
		{
			MapReady?.Invoke(this, new MapboxMapReadyEventArgs(p0));

			//throw new NotImplementedException();
		}
	}

	public class MapboxMapReadyEventArgs : EventArgs
	{
		public Sdk.Maps.MapboxMap Map { get; private set; }

        public MapboxMapReadyEventArgs(Sdk.Maps.MapboxMap map)
		{
			Map = map;
		}
	}
}
