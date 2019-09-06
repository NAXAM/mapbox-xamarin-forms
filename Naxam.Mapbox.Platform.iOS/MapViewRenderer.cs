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
                    SetupFunctions();
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

            if (e.PropertyName == MapView.RegionProperty.PropertyName)
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
            if (false == Element?.Region.IsEmpty())
            {
                var ne = new CLLocationCoordinate2D(Element.Region.NorthEast.Lat, Element.Region.NorthEast.Long);
                var sw = new CLLocationCoordinate2D(Element.Region.SouthWest.Lat, Element.Region.SouthWest.Long);
                var bounds = new MGLCoordinateBounds() { ne = ne, sw = sw };
                map.SetVisibleCoordinateBounds(bounds, true);
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

        protected virtual void SetupFunctions()
        {
            Element.TakeSnapshotFunc = () =>
            {
                var image = map.Capture(true);
                var imageData = image.AsJPEG();
                Byte[] imgByteArray = new Byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes,
                                                            imgByteArray,
                                                            0,
                                                            Convert.ToInt32(imageData.Length));
                return Task.FromResult(imgByteArray);
            };

            //Element.GetFeaturesAroundPointFunc = GetFeaturesArroundPoint;

            Element.ResetPositionAction = () =>
            {
                map.ResetPosition();
            };

            Element.ReloadStyleAction = () =>
            {
                map.ReloadStyle(map);
            };

            //Element.UpdateShapeOfSourceFunc = (Annotation annotation, string sourceId) =>
            //{
            //    if (annotation != null && !string.IsNullOrEmpty(sourceId))
            //    {
            //        var mglSource = MapView.Style.SourceWithIdentifier(sourceId.ToCustomId());
            //        if (mglSource != null && mglSource is MGLShapeSource)
            //        {
            //            Device.BeginInvokeOnMainThread(() =>
            //            {
            //                (mglSource as MGLShapeSource).Shape = ShapeFromAnnotation(annotation);
            //            });
            //            if (Element.MapStyle.CustomSources != null)
            //            {
            //                var count = Element.MapStyle.CustomSources.Count();
            //                if (Element.MapStyle.CustomSources.FirstOrDefault((arg) => arg.Id == sourceId) is ShapeSource shapeSource)
            //                {
            //                    shapeSource.Shape = annotation;
            //                }
            //            }
            //            return true;
            //        }
            //    }
            //    return false;
            //};

            Element.UpdateLayerFunc = (string layerId, bool isVisible, bool IsCustom) =>
            {
                if (string.IsNullOrEmpty(layerId) || map == null || map.Style == null)
                    return false;
                NSString layerIdStr = IsCustom ? layerId.ToCustomId() : (NSString)layerId;
                var layer = map.Style.LayerWithIdentifier(layerIdStr);
                if (layer != null)
                {
                    layer.Visible = isVisible;
                    if (IsCustom && Element.MapStyle != null && Element.MapStyle.CustomLayers != null)
                    {
                        var count = Element.MapStyle.CustomLayers.Count();
                        for (var i = 0; i < count; i++)
                        {
                            if (Element.MapStyle.CustomLayers.ElementAt(i).Id == layerId)
                            {
                                Element.MapStyle.CustomLayers.ElementAt(i).IsVisible = isVisible;
                                break;
                            }
                        }
                    }
                    return true;
                }
                return false;
            };

            //Element.UpdateViewPortAction = (LatLng centerLocation, double? zoomLevel, double? bearing, bool animated, Action completionBlock) =>
            //{
            //    MapView?.SetCenterCoordinate(
            //        centerLocation?.ToCLCoordinate() ?? MapView.CenterCoordinate,
            //        zoomLevel ?? MapView.ZoomLevel,
            //        bearing ?? MapView.Direction,
            //        animated,
            //        completionBlock
            //    );
            //};

            //Element.GetMapScaleReciprocalFunc = () => {
            //             if (MapView == null) return 500000000;
            //             System.Diagnostics.Debug.WriteLine($"Center latitude: {MapView.CenterCoordinate.Latitude}");
            //	var metersPerPoint = MapView.MetersPerPointAtLatitude(MapView.CenterCoordinate.Latitude);
            //	System.Diagnostics.Debug.WriteLine($"metersPerPoint: {metersPerPoint}");
            //	var resolution = metersPerPoint / Math.Pow(2, MapView.ZoomLevel);
            //             System.Diagnostics.Debug.WriteLine($"resolution: {resolution}");
            //	var output = UIDeviceExtensions.DPI() * 39.37 * resolution;
            //	return output;
            //};

            Element.ToggleScaleBarFunc = (bool show) =>
            {
                if (map == null || map.ScaleBar == null) return false;
                InvokeOnMainThread(() =>
                {
                    Debug.WriteLine($"Toggle scale bar: {show}");
                    map.ScaleBar.Hidden = !show;
                });

                return true;
            };

            Element.GetStyleImageFunc = (imageName) =>
            {
                if (string.IsNullOrEmpty(imageName)
                    || map == null
                    || map.Style == null) return null;
                return map.Style.ImageForName(imageName)?.AsPNG().ToArray();
            };

            Element.GetStyleLayerFunc = (string layerId, bool isCustom) =>
            {
                if (string.IsNullOrEmpty(layerId)
                    || map == null
                    || map.Style == null) return null;
                NSString layerIdStr = isCustom ? layerId.ToCustomId() : (NSString)layerId;
                var layer = map.Style.LayerWithIdentifier(layerIdStr);
                if (layer is MGLVectorStyleLayer vLayer)
                {
                    return CreateStyleLayer(vLayer, layerId);
                }

                return null;
            };

            Element.InsertLayerAboveLayerFunc = (newLayer, siblingLayerId) =>
            {
                if (map.Style?.LayerWithIdentifier(siblingLayerId) is MGLStyleLayer siblingLayer
                    && GetStyleLayer(newLayer, newLayer.Id.ToCustomId()) is MGLStyleLayer layerToInsert)
                {
                    map.Style.InsertLayerAbove(layerToInsert, siblingLayer);
                    return true;
                }
                return false;
            };

            Element.InsertLayerBelowLayerFunc = (newLayer, siblingLayerId) =>
            {
                if (map.Style?.LayerWithIdentifier(siblingLayerId) is MGLStyleLayer siblingLayer
                    && GetStyleLayer(newLayer, newLayer.Id.ToCustomId()) is MGLStyleLayer layerToInsert)
                {
                    map.Style.InsertLayerBelow(layerToInsert, siblingLayer);
                    return true;
                }
                return false;
            };

            Element.ApplyOfflinePackFunc = (mapPack) =>
            {
                var pack = Runtime.GetNSObject<MGLOfflinePack>(mapPack.Handle);
                var region = Runtime.GetNSObject<MGLTilePyramidOfflineRegion>(pack.Region.Handle);
                map.StyleURL = region.StyleURL;
                map.VisibleCoordinateBounds = region.Bounds;
                map.ZoomLevel = Math.Min(map.MaximumZoomLevel, Math.Max(map.MinimumZoomLevel, map.ZoomLevel));
                return true;
            };

            Element.SelectAnnotationAction = (Tuple<string, bool> obj) =>
            {
                if (obj == null || map == null || map.Annotations == null) return;
                foreach (var childObj in map.Annotations)
                {
                    var anno = Runtime.GetNSObject<MGLPointAnnotation>(childObj.Handle);
                    if (anno is MGLShape shape
                        && shape.Handle.ToString() == obj.Item1)
                    {
                        map.SelectAnnotation(shape, obj.Item2);
                        break;
                    }
                }
            };

            Element.DeselectAnnotationAction = (Tuple<string, bool> obj) =>
            {
                if (obj == null || map == null || map.Annotations == null) return;
                foreach (var childObj in map.Annotations)
                {
                    var anno = Runtime.GetNSObject<MGLPointAnnotation>(childObj.Handle);
                    if (anno is MGLShape shape
                        && shape.Handle.ToString() == obj.Item1)
                    {
                        map.DeselectAnnotation(shape, obj.Item2);
                        break;
                    }
                }
            };
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
