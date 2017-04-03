using System;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;

using Mapbox.Sdk.Camera;
using Mapbox.Sdk.Geometry;
using Mapbox.Sdk.Maps;

using Naxam.Mapbox.Forms;
using Naxam.Mapbox.Platform.Droid;

using MapView = Naxam.Mapbox.Forms.MapView;
using Sdk = Mapbox.Sdk;//alias
[assembly: Xamarin.Forms.ExportRenderer(typeof(Naxam.Mapbox.Forms.MapView), typeof(MapViewRenderer))]
namespace Naxam.Mapbox.Platform.Droid
{
	public class MapViewRenderer : Xamarin.Forms.Platform.Android.ViewRenderer<Naxam.Mapbox.Forms.MapView, View>
	{
		MapViewFragment fragment;
	    private const int SIZE_ZOOM = 13;
	    private Position _currentCamera;
	    private static bool _isCenterChange;
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
                _currentCamera= new Position();
				SetNativeControl(view);
			}
		}

        Sdk.Maps.MapboxMap map;

		void MapReady(object sender, MapboxMapReadyEventArgs e)
		{
            map = e.Map;
		    map.MyLocationEnabled = true;
           // Element.Center = new Position();
		    map.MyLocationChange += delegate(object o, MapboxMap.MyLocationChangeEventArgs args)
		    {
                if(Element.UserLocation==null) Element.UserLocation = new Position();
	            Element.UserLocation.Lat = args.P0.Latitude;
	            Element.UserLocation.Lon = args.P0.Longitude;
		    };

            
            map.CameraChange += delegate(object o, MapboxMap.CameraChangeEventArgs args)
            {
            
            _currentCamera.Lat = args.P0.Target.Latitude;
               _currentCamera.Lon = args.P0.Target.Longitude;
                Element.Center = _currentCamera;
            };
            map.MapClick += delegate(object o, MapboxMap.MapClickEventArgs args)
            {
                Element.IsTouchInMap = true;
            };
		   // setUpEventHandlers();
		}

	    #region SetupEnvent

	    private void setUpEventHandlers()
	    {
	        //Element.MapCenterHandler = (sender, e) =>
	        //{
	        //    var positionChangeEventArgs = e as PositionChangeEventArgs;
	        //    if (positionChangeEventArgs != null)
	        //    {
	        //        Position position = positionChangeEventArgs.NewPosition;
	        //        FocustoLocation(new LatLng(position.Lat,position.Long));
	        //    }
	        //};
	    }

	    #endregion

	    private void FocustoLocation(LatLng latLng)
	    {
            CameraPosition position = new CameraPosition.Builder().Target(latLng).Zoom(SIZE_ZOOM).Build();
            ICameraUpdate camera = CameraUpdateFactory.NewCameraPosition(position);
            map.AnimateCamera(camera);

	    }
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
		    if (e.PropertyName == MapView.CenterProperty.PropertyName)
		    {
                if (Element.Center!= _currentCamera)
                {
                    FocustoLocation(new LatLng(Element.Center.Lat, Element.Center.Lon));
                    return;
                }

            }
            if (e.PropertyName == MapView.UserLocationProperty.PropertyName)
		    {

            }
            if (e.PropertyName == MapView.FocusPositionProperty.PropertyName)
            {

               // FocustoLocation(new LatLng(Element.UserLocation.Lat, Element.UserLocation.Long));

            }


        }
	}






    //Fragment MapView
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
