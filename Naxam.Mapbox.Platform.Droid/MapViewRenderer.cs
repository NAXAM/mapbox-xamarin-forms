using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;

using Java.Lang;
using Java.Util;

using Mapbox.Sdk.Annotations;
using Mapbox.Sdk.Camera;
using Mapbox.Sdk.Geometry;
using Mapbox.Sdk.Maps;
using Mapbox.Services.Commons.GeoJson;

using Naxam.Mapbox.Forms;
using Naxam.Mapbox.Platform.Droid;

using Newtonsoft.Json;

using Annotation = Naxam.Mapbox.Forms.Annotation;
using MapView = Naxam.Mapbox.Forms.MapView;
using Point = Xamarin.Forms.Point;
using Sdk = Mapbox.Sdk;
using View = Android.Views.View;

//alias

[assembly: Xamarin.Forms.ExportRenderer(typeof(Naxam.Mapbox.Forms.MapView), typeof(MapViewRenderer))]

namespace Naxam.Mapbox.Platform.Droid
{
    public class MapViewRenderer : Xamarin.Forms.Platform.Android.ViewRenderer<Naxam.Mapbox.Forms.MapView, View>
    {
        MapViewFragment fragment;
        private const int SIZE_ZOOM = 13;
        private Position _currentCamera;
        private Marker _markerAddress;

        Dictionary<string, Sdk.Annotations.Annotation> _annotationDictionaries =
            new Dictionary<string, Sdk.Annotations.Annotation>();

        protected override void OnElementChanged(
            Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Naxam.Mapbox.Forms.MapView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //Remove event handlers
                fragment.MapReady -= MapReady;
                fragment.MapTouch -= MapTouch;
            }

            if (e.NewElement == null)
                return;

            if (Control == null)
            {
                var view = LayoutInflater.FromContext(Context)
                                         .Inflate(Resource.Layout.map_view_container, ViewGroup, false);

                var activity = (AppCompatActivity)Context;
                fragment = (MapViewFragment)activity.SupportFragmentManager.FindFragmentById(Resource.Id.map);
                fragment.MapReady += MapReady;
                fragment.MapTouch += MapTouch;
                _currentCamera = new Position();
                SetNativeControl(view);
            }
        }

        Sdk.Maps.MapboxMap map;
        private Point _point;
        void MapTouch(object sender, MotionEvent e)
        {
            _point = new Point(e.GetX(), e.GetY());
        }
        void MapReady(object sender, MapboxMapReadyEventArgs e)
        {
            map = e.Map;
            map.MyLocationEnabled = true;
            // Element.Center = new Position();
            map.MyLocationChange += delegate(object o, MapboxMap.MyLocationChangeEventArgs args)
            {
                if (Element.UserLocation == null)
                    Element.UserLocation = new Position();
                Element.UserLocation.Lat = args.P0.Latitude;
                Element.UserLocation.Long = args.P0.Longitude;
            };

            map.CameraChange += delegate(object o, MapboxMap.CameraChangeEventArgs args)
            {
                _currentCamera.Lat = args.P0.Target.Latitude;
                _currentCamera.Long = args.P0.Target.Longitude;
                Element.Center = _currentCamera;
            };
            map.MapClick += delegate(object o, MapboxMap.MapClickEventArgs args)
            {
                // Need to be false to hide searchbar in view
                Element.IsTouchInMap = false;
                Element.DidTapOnMapCommand?.Execute(new Tuple<Position, Point>(new Position(args.P0.Latitude,args.P0.Longitude), 
                                                                                   new Point(_point.X, _point.Y)));
            };

            map.MarkerClick += delegate(object o, MapboxMap.MarkerClickEventArgs args)
            {
                Element.Center.Lat = args.P0.Position.Latitude;
                Element.Center.Long = args.P0.Position.Longitude;
                Element.IsMarkerClicked = true;
            };
            map.UiSettings.RotateGesturesEnabled = Element.RotateEnabled;
           map.UiSettings.TiltGesturesEnabled = Element.PitchEnabled;

            SetupFunctions();
        }

        private void SetupFunctions()
        {
            Element.GetFeaturesAroundPoint += delegate(Point point, double radius, string[] layers)
            {
                var output = new List<IFeature>();
                RectF rect = new RectF((float)(point.X - radius/2), (float)(point.Y - radius / 2),(float)radius,(float)radius);
                var listFeatures = map.QueryRenderedFeatures(rect, layers);
                if (listFeatures.Count != 0)
                {
                    IFeature ifeat = null;
                    foreach (Feature feature in listFeatures)
                    {
                        System.Diagnostics.Debug.WriteLine(feature.ToJson());
                        string id = feature.Id;
                        if (id == null || output.Any((arg) =>
                        {
                            var annotation = arg as Annotation;
                            return annotation != null && annotation.Id == id;
                        }))
                        {
                            continue;
                        }
                        if (feature.Geometry is global::Mapbox.Services.Commons.GeoJson.Point)
                        {
                            ifeat = new PointFeature();
                            var pointFeature = feature.Geometry.Coordinates as global::Mapbox.Services.Commons.Models.Position;
                            if(pointFeature ==null) continue;
                            ((PointAnnotation)ifeat).Coordinate = new Position(pointFeature.Latitude, pointFeature.Longitude);
                            AddAnnotation((PointAnnotation)ifeat);
                        }
                        else if (feature.Geometry is LineString)
                        {
                            ifeat = new PolylineFeature();
                        }
                        else if (feature.Geometry is MultiLineString)
                        {
                            ifeat = new MultiPolylineFeature();

                        }
                        if (ifeat != null)
                        {
                          (ifeat as Annotation).Id = feature.Id;
                          ifeat.Attributes = ConvertToDictionary(feature.ToJson());
                            
                        }
                       
                        output.Add(ifeat);
                    }
                }
                return output.ToArray();
            };
        }

    
        private Dictionary<string,object> ConvertToDictionary(string featureProperties)
        {
            Dictionary<string, object> objectFeature =
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(featureProperties);
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(objectFeature["properties"].ToString());
            return result;
        }

        #region SetupEnvent

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
                if (!ReferenceEquals(Element.Center, _currentCamera))
                {
                    if (Element.Center == null)
                        return;
                    FocustoLocation(new LatLng(Element.Center.Lat, Element.Center.Long));
                    return;
                    if (ReferenceEquals(Element.Center, Element.UserLocation))
                    {
                        //Users go back their location
                        if (Element.Center == null)
                            return;
                        FocustoLocation(new LatLng(Element.Center.Lat, Element.Center.Long));
                    }
                    else
                    {
                        // User search location , need to focus and add marker
                        FocustoLocation(new LatLng(Element.Center.Lat, Element.Center.Long));
                        //                        _markerAddress = AddMarkerAddress(new LatLng(Element.Center.Lat, Element.Center.Long));
                    }
                }
            }
            else if (e.PropertyName == MapView.MapStyleProperty.PropertyName && map != null)
            {
                map.StyleUrl = Element.MapStyle.UrlString;
                FocustoLocation(new LatLng(Element.MapStyle.Center[1], Element.MapStyle.Center[0]));
            }
            else if (e.PropertyName == MapView.PitchEnabledProperty.PropertyName)
            {
                if (map != null)
                {
                    map.UiSettings.TiltGesturesEnabled = Element.PitchEnabled;
                }
            }
            else if (e.PropertyName == MapView.RotateEnabledProperty.PropertyName)
            {
                if (map != null)
                {
                    map.UiSettings.RotateGesturesEnabled = Element.RotateEnabled;
                }
            }
            else if (e.PropertyName == MapView.AnnotationsProperty.PropertyName)
            {
                RemoveAllAnnotations();
                if (Element.Annotations != null)
                {
                    AddAnnotations(Element.Annotations.ToArray());
                    var notifyCollection = Element.Annotations as INotifyCollectionChanged;
                    if (notifyCollection != null)
                    {
                        notifyCollection.CollectionChanged += OnAnnotationsCollectionChanged;
                    }
                }
            }
        }

        private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var annots = new List<PolylineOptions>();
                foreach (Annotation annot in e.NewItems)
                {
                    var shape = AddAnnotation(annot);
                    if (shape != null)
                    {
                        // annots.Add(shape);
                    }
                }
                //  map.AddPolylines(annots.ToArray());
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var items = new List<Annotation>();
                foreach (Annotation annot in e.OldItems)
                {
                    items.Add(annot);
                }
                RemoveAnnotations(items.ToArray());
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //TODO Update pins
            }
        }

        void RemoveAnnotations(Annotation[] annotations)
        {
            var currentAnnotations = map.Annotations;
            if (currentAnnotations == null)
            {
                return;
            }
            var annots = new List<Sdk.Annotations.Annotation>();
            foreach (Annotation at in annotations)
            {
                if (_annotationDictionaries.ContainsKey(at.Id))
                {
                    annots.Add(_annotationDictionaries[at.Id]);
                }
            }
            map.RemoveAnnotations(annots.ToArray());
        }

        void AddAnnotations(Annotation[] annotations)
        {
            foreach (Annotation at in annotations)
            {
                var shape = AddAnnotation(at);
            }
        }

        private Sdk.Annotations.Annotation AddAnnotation(Annotation at)
        {
            Sdk.Annotations.Annotation options = null;
            if (at is PointAnnotation)
            {
                var marker = new MarkerOptions();
                marker.SetPosition(new LatLng(((PointAnnotation)at).Coordinate.Lat,
                    ((PointAnnotation)at).Coordinate.Long));
                options = map.AddMarker(marker);
            }
            else if (at is PolylineAnnotation)
            {
                var polyline = at as PolylineAnnotation;
                if (polyline.Coordinates == null || polyline.Coordinates.Length == 0)
                {
                    return null;
                }
                var coords = new ArrayList();
                for (var i = 0; i < polyline.Coordinates.Length; i++)
                {
                    coords.Add(new LatLng(polyline.Coordinates[i].Lat, polyline.Coordinates[i].Long));
                }
                var polylineOpt = new PolylineOptions();
                polylineOpt.AddAll(coords);
                options = map.AddPolyline(polylineOpt);
            }
            else if (at is MultiPolylineAnnotation)
            {
                var polyline = at as MultiPolylineAnnotation;
                if (polyline.Coordinates == null || polyline.Coordinates.Length == 0)
                {
                    return null;
                }

                                var lines = new List<PolylineOptions>();
                                for (var i = 0; i < polyline.Coordinates.Length; i++)
                                {
                                    if (polyline.Coordinates[i].Length == 0)
                                    {
                                        continue;
                                    }
                                    var coords = new PolylineOptions();
                                    for (var j = 0; j < polyline.Coordinates[i].Length; j++)
                                    {
                                        coords.Add(new LatLng(polyline.Coordinates[i][j].Lat, polyline.Coordinates[i][j].Long));
                                    }
                                    lines.Add(coords);
                                }
                        IList<Polyline> listPolylines =    map.AddPolylines(lines);
                        //todo  handle add listPolyline . Need to identify to remove after that
              
            }
            if (options != null)
            {
                if (at.Id != null)
                {
                    _annotationDictionaries.Add(at.Id, options);
                }
            }

            return options;
        }

        void RemoveAllAnnotations()
        {
            if (map.Annotations != null)
            {
                map.RemoveAnnotations(map.Annotations);
            }
        }

    }

    #region Map Fragment

    

    //Fragment MapView
    public class MapViewFragment : Android.Support.V4.App.Fragment, Sdk.Maps.IOnMapReadyCallback,View.IOnTouchListener
    {
        public event EventHandler<MapboxMapReadyEventArgs> MapReady;
        public event EventHandler<MotionEvent> MapTouch;

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

        public bool OnTouch(View v, MotionEvent e)
        {
            MapTouch?.Invoke(this,e);
            return false;
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

    #endregion
}
