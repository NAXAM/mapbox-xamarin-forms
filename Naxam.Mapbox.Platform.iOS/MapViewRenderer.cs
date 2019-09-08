using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using Mapbox;
using Naxam.Controls.Forms;
using Naxam.Controls.Mapbox.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ObjCRuntime;
using Naxam.Mapbox;

[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]
namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : ViewRenderer<MapView, MGLMapView>, IMGLMapViewDelegate, IUIGestureRecognizerDelegate
    {
        protected MGLMapView map { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                e.OldElement.Functions = null;
                if (e.OldElement.Annotations is INotifyCollectionChanged notifyCollection)
                {
                    notifyCollection.CollectionChanged -= OnAnnotationsCollectionChanged;
                }
            }

            if (e.NewElement == null) return;

            try
            {
                if (Control == null)
                {
                    SetupUserInterface();
                    SetupEventHandlers();
                    SetNativeControl(map);

                    if (e.NewElement.Annotations != null)
                    {
                        AddAnnotations(e.NewElement.Annotations.ToArray());
                    }

                    map.WeakDelegate = this;

                    if (e.NewElement.Annotations is INotifyCollectionChanged notifyCollection)
                    {
                        notifyCollection.CollectionChanged += OnAnnotationsCollectionChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (map == null || Element == null) return;

            if (e.PropertyName == MapView.VisibleBoundsProperty.PropertyName)
            {
                UpdateRegion();
            }
            else if (e.PropertyName == MapView.CenterProperty.PropertyName)
            {
                UpdateCenter();
            }
            else if (e.PropertyName == MapView.ZoomLevelProperty.PropertyName)
            {
                var isSameLevel = Math.Round(Element.ZoomLevel * 100).Equals(Math.Round(map.ZoomLevel * 100));

                if (isSameLevel) return;

                map.ZoomLevel = Element.ZoomLevel;
            }
            else if (e.PropertyName == MapView.PitchEnabledProperty.PropertyName)
            {
                if (map.PitchEnabled == Element.PitchEnabled) return;

                map.PitchEnabled = Element.PitchEnabled;
            }
            else if (e.PropertyName == MapView.ScrollEnabledProperty.PropertyName)
            {
                if (map.ScrollEnabled == Element.ScrollEnabled) return;

                map.ScrollEnabled = Element.ScrollEnabled;
            }
            else if (e.PropertyName == MapView.RotateEnabledProperty.PropertyName)
            {
                if (map.RotateEnabled == Element.RotateEnabled) return;

                map.RotateEnabled = Element.RotateEnabled;
            }
            else if (e.PropertyName == MapView.AnnotationsProperty.PropertyName)
            {
                if (Element.Annotations == null || Element.Annotations.Count() == 0) return;

                AddAnnotations(Element.Annotations.ToArray());

                if (Element.Annotations is INotifyCollectionChanged observable)
                {
                    observable.CollectionChanged -= OnAnnotationsCollectionChanged;
                    observable.CollectionChanged += OnAnnotationsCollectionChanged;
                }
            }
            else if (e.PropertyName == MapView.MapStyleProperty.PropertyName)
            {
                var shouldNotUpdate = string.IsNullOrWhiteSpace(Element.MapStyle?.UrlString)
                     || string.Equals(Element.MapStyle.UrlString, map.StyleURL?.AbsoluteString);

                if (shouldNotUpdate) return;

                UpdateMapStyle();
            }
            else if (e.PropertyName == MapView.PitchProperty.PropertyName)
            {
                var shouldNotUpdate = Math.Abs(Element.Pitch - map.Camera.Pitch) < 0.05;

                if (shouldNotUpdate) return;

                var currentCamera = map.Camera;
                var newCamera = MGLMapCamera.CameraLookingAtCenterCoordinateAndAltitude(
                                    currentCamera.CenterCoordinate,
                                    currentCamera.Altitude,
                                    (nfloat)Element.Pitch,
                                    currentCamera.Heading);
                map.SetCamera(newCamera, true);
            }
            else if (e.PropertyName == MapView.RotatedDegreeProperty.PropertyName)
            {
                var shouldNotUpdate = Math.Abs(Element.RotatedDegree - map.Camera.Heading) < 0.05;

                if (shouldNotUpdate) return;

                var currentCamera = map.Camera;
                var newCamera = MGLMapCamera.CameraLookingAtCenterCoordinateAndAltitude(
                                    currentCamera.CenterCoordinate,
                                    currentCamera.Altitude,
                                    currentCamera.Pitch,
                                    (nfloat)Element.RotatedDegree);
                map.SetCamera(newCamera, true);
            }
            else if (e.PropertyName == MapView.ShowUserLocationProperty.PropertyName)
            {
                map.ShowsUserLocation = Element.ShowUserLocation;
            }
        }

        protected void OnElementPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            if (Element == null) return;
            if (e.PropertyName == MapView.AnnotationsProperty.PropertyName)
            {
                if (Element.Annotations == null || Element.Annotations.Count() == 0) return;

                RemoveAllAnnotations();
                if (Element.Annotations is INotifyCollectionChanged observable)
                {
                    observable.CollectionChanged -= OnAnnotationsCollectionChanged;
                }
            }
        }

        protected virtual void SetupUserInterface()
        {
            map = new MGLMapView(Bounds)
            {
                ShowsUserLocation = Element.ShowUserLocation,
                PitchEnabled = Element.PitchEnabled,
                RotateEnabled = Element.RotateEnabled,
                UserTrackingMode = MGLUserTrackingMode.FollowWithHeading
            };
            map.ZoomLevel = Element.ZoomLevel;
            UpdateMapStyle();
            UpdateCenter();
        }

        void UpdateRegion()
        {
            if (false == Element?.VisibleBounds.IsEmpty())
            {
                var ne = new CLLocationCoordinate2D(Element.VisibleBounds.NorthEast.Lat, Element.VisibleBounds.NorthEast.Long);
                var sw = new CLLocationCoordinate2D(Element.VisibleBounds.SouthWest.Lat, Element.VisibleBounds.SouthWest.Long);
                var bounds = new MGLCoordinateBounds() { ne = ne, sw = sw };
                map.SetVisibleCoordinateBounds(bounds, true, null);
            }
        }

        protected virtual void UpdateCenter()
        {
            if (map == null || Element == null) return;

            var center = map.CenterCoordinate.ToLatLng();

            if (center != Element.Center) return;

            map.SetCenterCoordinate(new CLLocationCoordinate2D(Element.Center.Lat, Element.Center.Long), true);
        }

        protected virtual void SetupEventHandlers()
        {
            var tapGest = new UITapGestureRecognizer();
            tapGest.NumberOfTapsRequired = 1;
            tapGest.CancelsTouchesInView = false;
            tapGest.Delegate = this;
            map.AddGestureRecognizer(tapGest);
            tapGest.AddTarget((NSObject obj) =>
            {
                var gesture = obj as UITapGestureRecognizer;
                if (gesture.State == UIGestureRecognizerState.Ended)
                {
                    var point = gesture.LocationInView(map);
                    var touchedCooridinate = map.ConvertPoint(point, map);
                    var position = new LatLng(touchedCooridinate.Latitude, touchedCooridinate.Longitude);
                    Element.DidTapOnMapCommand?.Execute(new Tuple<LatLng, Point>(
                        position,
                        new Point((double)point.X, (double)point.Y)));
                }
            });
            Element.PropertyChanging += OnElementPropertyChanging;
        }

        #region MGLMapViewDelegate

        [Export("mapViewDidFinishRenderingMap:fullyRendered:"),]
        void DidFinishRenderingMap(MGLMapView mapView, bool fullyRendered)
        {
            Element.DidFinishRenderingCommand?.Execute(fullyRendered);
        }

        [Export("mapView:didUpdateUserLocation:")]
        public void MapViewDidUpdateUserLocation(MGLMapView mapView, MGLUserLocation userLocation)
        {
            if (userLocation?.Location?.Coordinate != null)
            {
                Element.UserLocation = new LatLng(
                    userLocation.Location.Coordinate.Latitude,
                    userLocation.Location.Coordinate.Longitude
                );
            }
        }

        [Export("mapViewRegionIsChanging:"),]
        protected virtual void RegionIsChanging(MGLMapView mapView)
        {
            Element.Center = new LatLng(mapView.CenterCoordinate.Latitude, mapView.CenterCoordinate.Longitude);
        }

        [Export("mapView:regionDidChangeAnimated:"),]
        protected virtual void RegionDidChangeAnimated(MGLMapView mapView, bool animated)
        {
            Element.ZoomLevel = mapView.ZoomLevel;
            Element.Pitch = mapView.Camera.Pitch;
            Element.RotatedDegree = mapView.Camera.Heading;
            Element?.RegionDidChangeCommand?.Execute(animated);
        }

        #endregion

        #region UIGestureRecognizerDelegate

        [Export("gestureRecognizer:shouldRecognizeSimultaneouslyWithGestureRecognizer:")]
        public bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            return true;
        }

        #endregion
    }

    public static class NSDateExtensions
    {
        private static DateTime _nsRef = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Local); // last zero is millisecond

        public static DateTimeOffset ToDateTimeOffset(this NSDate date)
        {
            var interval = date.SecondsSinceReferenceDate;
            return _nsRef.AddSeconds(interval);
        }
    }

    public static class stringExtensions
    {
        static string CustomPrefix = "NXCustom_";
        public static NSString ToCustomId(this string str)
        {
            if (str == null) return null;
            return (NSString)(CustomPrefix + str);
        }

        public static bool IsCustomId(this string str)
        {
            if (str == null) return false;
            return str.StartsWith(CustomPrefix, StringComparison.OrdinalIgnoreCase);
        }

        public static string TrimCustomId(this string str)
        {
            if (str.StartsWith(CustomPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return str.Substring(CustomPrefix.Length);
            }
            return str;
        }
    }
}
