using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Mapbox;
using Naxam.Controls.Mapbox.Forms;
using Naxam.Controls.Mapbox.Platform.iOS;
using Naxam.Mapbox.Forms.AnnotationsAndFeatures;
using Naxam.Mapbox.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsMap = Naxam.Controls.Mapbox.Forms.MapView;
using FormsMB = Naxam.Controls.Mapbox.Forms;
using Naxam.Mapbox.Platform.iOS.Extensions;

[assembly: Xamarin.Forms.ExportRenderer(typeof(FormsMap), typeof(MapViewRenderer))]
namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : ViewRenderer<Naxam.Controls.Mapbox.Forms.MapView, MGLMapView>, IMGLMapViewDelegate, IUIGestureRecognizerDelegate
    {
        MGLMapView MapView { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                if(e.OldElement.Annotations is INotifyCollectionChanged notifyCollection && notifyCollection != null)
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
                    SetupFunctions();
                    SetNativeControl(MapView);
                    if(e.NewElement.Annotations is INotifyCollectionChanged notifyCollection && notifyCollection != null)
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

            if (MapView == null || Element == null)
            {
                return;
            }
            if (e.PropertyName == FormsMap.RegionProperty.PropertyName)
            {
                UpdateRegion();
            }
            if (e.PropertyName == FormsMap.CenterProperty.PropertyName)
            {
                UpdateCenter();
            }
            else if (e.PropertyName == FormsMap.ZoomLevelProperty.PropertyName
                     && !Math.Round(Element.ZoomLevel * 100).Equals(Math.Round(MapView.ZoomLevel * 100)))
            {
                //MapView.SetZoomLevel(Element.ZoomLevel, true);
                MapView.ZoomLevel = Element.ZoomLevel;
            }
            else if (e.PropertyName == FormsMap.PitchEnabledProperty.PropertyName && MapView.PitchEnabled != Element.PitchEnabled)
            {
                MapView.PitchEnabled = Element.PitchEnabled;
            }
            else if (e.PropertyName == FormsMap.ScrollEnabledProperty.PropertyName && MapView.ScrollEnabled != Element.ScrollEnabled)
            {
                MapView.ScrollEnabled = Element.ScrollEnabled;
            }
            else if (e.PropertyName == FormsMap.RotateEnabledProperty.PropertyName && MapView.RotateEnabled != Element.RotateEnabled)
            {
                MapView.RotateEnabled = Element.RotateEnabled;
            }
            else if (e.PropertyName == FormsMap.AnnotationsProperty.PropertyName)
            {
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
            //else if (e.PropertyName == FormsMap.StyleUrlProperty.PropertyName
            //		 && !string.IsNullOrEmpty(Element.StyleUrl)
            //		   && (MapView.StyleURL == null
            //			   || MapView.StyleURL.AbsoluteString != Element.StyleUrl))
            //{
            //	MapView.StyleURL = new NSUrl(Element.StyleUrl);
            //}
            else if (e.PropertyName == FormsMap.MapStyleProperty.PropertyName
                     && Element.MapStyle != null
                     && !string.IsNullOrEmpty(Element.MapStyle.UrlString)
                     && (MapView.StyleURL == null || MapView.StyleURL.AbsoluteString != Element.MapStyle.UrlString))
            {
                UpdateMapStyle();
            }
            else if (e.PropertyName == FormsMap.PitchProperty.PropertyName
                     && !Element.Pitch.Equals(MapView.Camera.Pitch))
            {
                var currentCamera = MapView.Camera;
                var newCamera = MGLMapCamera.CameraLookingAtCenterCoordinate(currentCamera.CenterCoordinate,
                                                                             currentCamera.Altitude,
                                                                              (nfloat)Element.Pitch,
                                                                             currentCamera.Heading);
                MapView.SetCamera(newCamera, true);
            }
            else if (e.PropertyName == FormsMap.RotatedDegreeProperty.PropertyName
                     && !Element.RotatedDegree.Equals(MapView.Camera.Heading))
            {
                var currentCamera = MapView.Camera;
                var newCamera = MGLMapCamera.CameraLookingAtCenterCoordinate(currentCamera.CenterCoordinate,
                                                                              currentCamera.Altitude,
                                                                              currentCamera.Pitch,
                                                                              (nfloat)Element.RotatedDegree);
                MapView.SetCamera(newCamera, true);
            }
            else if (e.PropertyName == FormsMap.ShowUserLocationProperty.PropertyName)
            {
                MapView.ShowsUserLocation = Element.ShowUserLocation;
            } else if(e.PropertyName == FormsMap.InfoWindowTemplateProperty.PropertyName)
            {
            }
        }

        void OnElementPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            if (Element == null) return;
            if (e.PropertyName == FormsMap.AnnotationsProperty.PropertyName
                && Element.Annotations != null)
            {
                RemoveAllAnnotations();
                var notifyCollection = Element.Annotations as INotifyCollectionChanged;
                if (notifyCollection != null)
                {
                    notifyCollection.CollectionChanged -= OnAnnotationsCollectionChanged;
                }
            }
            else if (e.PropertyName == FormsMap.MapStyleProperty.PropertyName
                     && Element.MapStyle != null)
            {
                if (Element.MapStyle.CustomSources != null)
                {
                    var notifiyCollection = Element.MapStyle.CustomSources as INotifyCollectionChanged;
                    if (notifiyCollection != null)
                    {
                        notifiyCollection.CollectionChanged -= OnShapeSourcesCollectionChanged;
                    }
                    RemoveSources(Element.MapStyle.CustomSources.ToList());
                }
                if (Element.MapStyle.CustomLayers != null)
                {
                    var notifiyCollection = Element.MapStyle.CustomLayers as INotifyCollectionChanged;
                    if (notifiyCollection != null)
                    {
                        notifiyCollection.CollectionChanged -= OnLayersCollectionChanged;
                    }
                    RemoveLayers(Element.MapStyle.CustomLayers.ToList());
                }
                Element.MapStyle.PropertyChanging -= OnMapStylePropertyChanging;
                Element.MapStyle.PropertyChanged -= OnMapStylePropertyChanged;
            }
        }

        void SetupUserInterface()
        {
            try
            {
                MapView = new MGLMapView(Bounds)
                {
                    ShowsUserLocation = Element.ShowUserLocation,
                    WeakDelegate = this,
                    PitchEnabled = Element.PitchEnabled,
                    RotateEnabled = Element.RotateEnabled,
                    UserTrackingMode = MGLUserTrackingMode.FollowWithHeading
                };
                MapView.ZoomLevel = Element.ZoomLevel;
                UpdateMapStyle();
                UpdateCenter();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
            }
        }
        void UpdateRegion()
        {
            if (Element?.Region != MapRegion.Empty)
            {
                var ne = new CLLocationCoordinate2D(Element.Region.NorthEast.Lat, Element.Region.NorthEast.Long);
                var sw = new CLLocationCoordinate2D(Element.Region.SouthWest.Lat, Element.Region.SouthWest.Long);
                var bounds = new MGLCoordinateBounds() { ne = ne, sw = sw };
                MapView.SetVisibleCoordinateBounds(bounds, true);
            }
        }

        void UpdateCenter()
        {
            if (Element.Center != null && MapView != null
                && (!Element.Center.Lat.Equals(MapView.CenterCoordinate.Latitude) || !Element.Center.Long.Equals(MapView.CenterCoordinate.Longitude)))
            {
                MapView.SetCenterCoordinate(new CLLocationCoordinate2D(Element.Center.Lat, Element.Center.Long), true);
            }
        }

        void UpdateMapStyle()
        {
            if (Element.MapStyle != null && !string.IsNullOrEmpty(Element.MapStyle.UrlString))
            {
                MapView.StyleURL = new NSUrl(Element.MapStyle.UrlString);
                Element.MapStyle.PropertyChanging += OnMapStylePropertyChanging;
                Element.MapStyle.PropertyChanged += OnMapStylePropertyChanged;
            }
        }

        void OnMapStylePropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            if (e.PropertyName == MapStyle.CustomSourcesProperty.PropertyName
                && (sender as MapStyle).CustomSources != null)
            {
                var notifiyCollection = (sender as MapStyle).CustomSources as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged -= OnShapeSourcesCollectionChanged;
                }
                RemoveSources(Element.MapStyle.CustomSources.ToList());
            }
            else if (e.PropertyName == MapStyle.CustomLayersProperty.PropertyName
                       && (sender as MapStyle).CustomLayers != null)
            {
                var notifiyCollection = Element.MapStyle.CustomLayers as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged -= OnLayersCollectionChanged;
                }
                RemoveLayers(Element.MapStyle.CustomLayers.ToList());
            }
        }

        void OnMapStylePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == MapStyle.CustomSourcesProperty.PropertyName
                && (sender as MapStyle).CustomSources != null)
            {
                var notifiyCollection = Element.MapStyle.CustomSources as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged += OnShapeSourcesCollectionChanged;
                }
                AddSources(Element.MapStyle.CustomSources.ToList());
            }
            else if (e.PropertyName == MapStyle.CustomLayersProperty.PropertyName && (sender as MapStyle).CustomLayers != null)
            {
                var notifiyCollection = Element.MapStyle.CustomLayers as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged += OnLayersCollectionChanged;
                }
                AddLayers(Element.MapStyle.CustomLayers.ToList());
            }
        }

        void SetupEventHandlers()
        {
            var tapGest = new UITapGestureRecognizer();
            tapGest.NumberOfTapsRequired = 1;
            tapGest.CancelsTouchesInView = false;
            tapGest.Delegate = this;
            MapView.AddGestureRecognizer(tapGest);
            tapGest.AddTarget((NSObject obj) =>
            {
                var gesture = obj as UITapGestureRecognizer;
                if (gesture.State == UIGestureRecognizerState.Ended)
                {
                    var point = gesture.LocationInView(MapView);
                    var touchedCooridinate = MapView.ConvertPoint(point, MapView);
                    var position = new Position(touchedCooridinate.Latitude, touchedCooridinate.Longitude);
                    Element.DidTapOnMapCommand?.Execute(new Tuple<Position, Point>(position,
                                                                                   new Point((double)point.X, (double)point.Y)));
                }
            });
            Element.PropertyChanging += OnElementPropertyChanging;
        }

        protected void SetupFunctions()
        {
            Element.TakeSnapshotFunc = () =>
            {
                var image = MapView.Capture(true);
                var imageData = image.AsJPEG();
                Byte[] imgByteArray = new Byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes,
                                                            imgByteArray,
                                                            0,
                                                            Convert.ToInt32(imageData.Length));
                return Task.FromResult(imgByteArray);
            };

            Element.GetFeaturesAroundPointFunc = GetFeaturesArroundPoint;

            Element.ResetPositionAction = () =>
            {
                MapView.ResetPosition();
            };

            Element.ReloadStyleAction = () =>
            {
                MapView.ReloadStyle(MapView);
            };

            Element.UpdateShapeOfSourceFunc = (Annotation annotation, string sourceId) =>
            {
                if (annotation != null && !string.IsNullOrEmpty(sourceId))
                {
                    var mglSource = MapView.Style.SourceWithIdentifier(sourceId.ToCustomId());
                    if (mglSource != null && mglSource is MGLShapeSource)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            (mglSource as MGLShapeSource).Shape = ShapeFromAnnotation(annotation);
                        });
                        if (Element.MapStyle.CustomSources != null)
                        {
                            var count = Element.MapStyle.CustomSources.Count();
                            if (Element.MapStyle.CustomSources.FirstOrDefault((arg) => arg.Id == sourceId) is ShapeSource shapeSource)
                            {
                                shapeSource.Shape = annotation;
                            }
                        }
                        return true;
                    }
                }
                return false;
            };

            Element.UpdateLayerFunc = (string layerId, bool isVisible, bool IsCustom) =>
            {
                if (string.IsNullOrEmpty(layerId) || MapView == null || MapView.Style == null)
                    return false;
                NSString layerIdStr = IsCustom ? layerId.ToCustomId() : (NSString)layerId;
                var layer = MapView.Style.LayerWithIdentifier(layerIdStr);
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

            Element.UpdateViewPortAction = (Position centerLocation, double? zoomLevel, double? bearing, bool animated, Action completionBlock) =>
            {
                MapView?.SetCenterCoordinate(
                    centerLocation?.ToCLCoordinate() ?? MapView.CenterCoordinate,
                    zoomLevel ?? MapView.ZoomLevel,
                    bearing ?? MapView.Direction,
                    animated,
                    completionBlock
                );
            };

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
                if (MapView == null || MapView.ScaleBar == null) return false;
                InvokeOnMainThread(() =>
                {
                    Debug.WriteLine($"Toggle scale bar: {show}");
                    MapView.ScaleBar.Hidden = !show;
                });

                return true;
            };

            Element.GetStyleImageFunc = (imageName) =>
            {
                if (string.IsNullOrEmpty(imageName)
                    || MapView == null
                    || MapView.Style == null) return null;
                return MapView.Style.ImageForName(imageName)?.AsPNG().ToArray();
            };

            Element.GetStyleLayerFunc = (string layerId, bool isCustom) =>
            {
                if (string.IsNullOrEmpty(layerId)
                    || MapView == null
                    || MapView.Style == null) return null;
                NSString layerIdStr = isCustom ? layerId.ToCustomId() : (NSString)layerId;
                var layer = MapView.Style.LayerWithIdentifier(layerIdStr);
                if (layer is MGLVectorStyleLayer vLayer)
                {
                    return CreateStyleLayer(vLayer, layerId);
                }

                return null;
            };

            Element.InsertLayerAboveLayerFunc = (newLayer, siblingLayerId) =>
            {
                if (MapView.Style?.LayerWithIdentifier(siblingLayerId) is MGLStyleLayer siblingLayer
                    && GetStyleLayer(newLayer, newLayer.Id.ToCustomId()) is MGLStyleLayer layerToInsert)
                {
                    MapView.Style.InsertLayerAbove(layerToInsert, siblingLayer);
                    return true;
                }
                return false;
            };

            Element.InsertLayerBelowLayerFunc = (newLayer, siblingLayerId) =>
            {
                if (MapView.Style?.LayerWithIdentifier(siblingLayerId) is MGLStyleLayer siblingLayer
                    && GetStyleLayer(newLayer, newLayer.Id.ToCustomId()) is MGLStyleLayer layerToInsert)
                {
                    MapView.Style.InsertLayerBelow(layerToInsert, siblingLayer);
                    return true;
                }
                return false;
            };

            Element.SelectAnnotationAction = (Tuple<string, bool> obj) =>
            {
                if (obj == null || MapView == null || MapView.Annotations == null) return;
                foreach (NSObject childObj in MapView.Annotations)
                {
                    if (childObj is MGLShape shape
                        && shape.Id() == obj.Item1)
                    {
                        MapView.SelectAnnotation(shape, obj.Item2);
                        break;
                    }
                }
            };

            Element.DeselectAnnotationAction = (Tuple<string, bool> obj) =>
            {
                if (obj == null || MapView == null || MapView.Annotations == null) return;
                foreach (NSObject childObj in MapView.Annotations)
                {
                    if (childObj is MGLShape shape
                        && shape.Id() == obj.Item1)
                    {
                        MapView.DeselectAnnotation(shape, obj.Item2);
                        break;
                    }
                }
            };
        }

        private IFeature[] GetFeaturesArroundPoint(Point point, double radius, string[] layers)
        {
            var selectableLayers = SelectableLayersFromSources(layers);
            NSObject[] features;
            var cgPoint = new CGPoint((nfloat)point.X, (nfloat)point.Y);
            if (radius <= 0)
            {
                features = MapView.VisibleFeaturesAtPoint(cgPoint, selectableLayers);
            }
            else
            {
                var rect = new CGRect(cgPoint.X - (nfloat)radius, cgPoint.Y - (nfloat)radius, (nfloat)radius * 2, (nfloat)radius * 2);
                features = MapView.VisibleFeaturesInRect(rect, selectableLayers);
            }

            var output = new List<IFeature>();

            foreach (NSObject obj in features)
            {
                var feature = obj as IMGLFeature;
                if (feature == null || feature.Attributes == null)
                {
                    continue;
                }
                string id = null;
                if (feature.Identifier != null)
                {
                    if (feature.Identifier is NSNumber)
                    {
                        id = ((NSNumber)feature.Identifier).StringValue;
                    }
                    else
                    {
                        id = feature.Identifier.ToString();
                    }
                }

                var geoData = feature.GeoJSONDictionary;
                if (geoData == null) continue;

                IFeature ifeat = null;
                if (feature is MGLPointFeature pointFeature)
                {
                    ifeat = new PointFeature();
                    (ifeat as PointFeature).Title = pointFeature.Title;
                    (ifeat as PointFeature).SubTitle = pointFeature.Subtitle;
                    (ifeat as PointFeature).Coordinate = TypeConverter.FromCoordinateToPosition(pointFeature.Coordinate);
                }
                else
                {
                    NSArray coorArr = null;
                    if (geoData.TryGetValue((NSString)"geometry", out NSObject geometryObj)
                        && geometryObj is NSDictionary geometry
                        && geometry.TryGetValue((NSString)"coordinates", out NSObject coordinates))
                    {
                        coorArr = coordinates as NSArray;
                    }
                    if (feature is MGLPolylineFeature)
                    {
                        ifeat = new PolylineFeature();
                        (ifeat as PolylineFeature).Title = ((MGLPolylineFeature)feature).Title;
                        (ifeat as PolylineFeature).SubTitle = ((MGLPolylineFeature)feature).Subtitle;

                        if (coorArr != null)
                        {
                            var coorsList = new List<Position>();
                            (ifeat as PolylineFeature).Coordinates = new Position[coorArr.Count];
                            for (nuint i = 0; i < coorArr.Count; i++)
                            {
                                var childArr = coorArr.GetItem<NSArray>(i);
                                if (childArr != null && childArr.Count == 2)
                                {
                                    var coord = new Position(childArr.GetItem<NSNumber>(1).DoubleValue, //lat
                                                            childArr.GetItem<NSNumber>(0).DoubleValue); //long
                                    coorsList.Add(coord);
                                }
                            }
                            (ifeat as PolylineFeature).Coordinates = new ObservableCollection<Position>(coorsList);
                        }

                    }
                    else if (feature is MGLMultiPolylineFeature mplFeature)
                    {
                        ifeat = new MultiPolylineFeature();
                        (ifeat as MultiPolylineFeature).Title = mplFeature.Title;
                        (ifeat as MultiPolylineFeature).SubTitle = mplFeature.Subtitle;
                        if (coorArr != null)
                        {
                            (ifeat as MultiPolylineFeature).Coordinates = new Position[coorArr.Count][];
                            for (nuint i = 0; i < coorArr.Count; i++)
                            {
                                var childArr = coorArr.GetItem<NSArray>(i);
                                if (childArr != null)
                                {
                                    (ifeat as MultiPolylineFeature).Coordinates[i] = new Position[childArr.Count];
                                    for (nuint j = 0; j < childArr.Count; j++)
                                    {
                                        var anscArr = childArr.GetItem<NSArray>(j);
                                        if (anscArr != null && anscArr.Count == 2)
                                        {
                                            (ifeat as MultiPolylineFeature).Coordinates[i][j] = new Position(anscArr.GetItem<NSNumber>(1).DoubleValue, //lat
                                                                                                            anscArr.GetItem<NSNumber>(0).DoubleValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (ifeat != null)
                {
                    (ifeat as Annotation).Id = id;
                    ifeat.Attributes = ConvertDictionary(feature.Attributes);


                    output.Add(ifeat);
                }
            }

            return output.ToArray(); ;
        }

        NSSet<NSString> SelectableLayersFromSources(string[] layersId)
        {
            if (layersId == null)
            {
                return null;
            }

            NSMutableSet<NSString> output = new NSMutableSet<NSString>();
            foreach (string layerId in layersId)
            {
                var acceptedId = layerId.Replace("_", "-");
                output.Add((NSString)acceptedId);
                output.Add((NSString)(acceptedId + " (1)"));
            }
            return new NSSet<NSString>(output);
        }

        void AddAnnotation(Annotation annotation)
        {
            var shape = ShapeFromAnnotation(annotation);
            if (shape != null)
            {
                MapView.AddAnnotation(shape);
                annotation.Id = shape.Handle.ToInt64().ToString();
            }
        }

        void AddAnnotations(Annotation[] annotations)
        {

            var annots = new List<IMGLAnnotation>();
            foreach (Annotation at in annotations)
            {
                var shape = ShapeFromAnnotation(at);
                if (shape == null)
                {
                    continue;
                }
                annots.Add(shape);
            }
            MapView.AddAnnotations(annots.ToArray());
        }

        [Export("mapView:calloutViewForAnnotation:")]
        public IMGLCalloutView MapView_CalloutViewForAnnotation(MGLMapView mapView, IMGLAnnotation annotation)
        {
            var id = annotation.Handle.ToInt64().ToString();
            var bindingContext = mapView.Annotations.FirstOrDefault(a => a.Handle.ToInt64().ToString() == id);
            UIView calloutContent = Element.InfoWindowTemplate.DataTemplateToNativeView(bindingContext, Element);
            return new MGLCustomCalloutView(null, calloutContent);
        }

        [Export("mapView:viewForAnnotation:")]
        public MGLAnnotationView MapView_ViewForAnnotation(MGLMapView mapView, MGLPointAnnotation annotation)
        {
            var annotationView = mapView.DequeueReusableAnnotationViewWithIdentifier("draggablePoint");
            if (annotationView != null) return annotationView;
            var view = new DraggableAnnotationView(reuseIdentifier: "draggablePoint", size: 24);
            view.DragFinished += (sender, e) =>
            {
                var point = new PointAnnotation();
                point.HandleId = annotation.Handle.ToString();
                point.Coordinate = TypeConverter.FromCoordinateToPosition(annotation.Coordinate);
                Element.DragFinishedCommand?.Execute(point);
            };

            return view;
        }

        void RemoveAnnotations(Annotation[] annotations)
        {
            var currentAnnotations = MapView.Annotations;
            if (currentAnnotations == null)
            {
                return;
            }
            var annots = new List<MGLShape>();
            foreach (Annotation at in annotations)
            {
                foreach (NSObject curAnnot in currentAnnotations)
                {
                    if (curAnnot is MGLShape shape)
                    {
                        if (string.IsNullOrEmpty(shape.Id()))
                        {
                            continue;
                        }
                        if (shape.Id() == at.Id)
                        {
                            annots.Add(shape);
                        }
                    }
                }
            }
            MapView.RemoveAnnotations(annots.ToArray());
        }

        void RemoveAllAnnotations()
        {
            if (MapView == null) return;
            if (MapView.Annotations != null)
            {
                MapView.RemoveAnnotations(MapView.Annotations);
            }
        }



        private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var annotations = new List<MGLShape>();
                foreach (Annotation annotation in e.NewItems)
                {
                    var shape = ShapeFromAnnotation(annotation);
                    if (shape != null)
                    {
                        annotations.Add(shape);
                    }
                }
                MapView.AddAnnotations(annotations.ToArray());
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
            else if (e.Action == NotifyCollectionChangedAction.Reset) //The content of the collection was cleared.
            {
                RemoveAllAnnotations();
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var itemsToRemove = new List<Annotation>();
                foreach (Annotation annotation in e.OldItems)
                {
                    itemsToRemove.Add(annotation);
                }
                RemoveAnnotations(itemsToRemove.ToArray());
                var annots = new List<MGLShape>();
                foreach (Annotation annotation in e.NewItems)
                {
                    var shape = ShapeFromAnnotation(annotation);
                    if (shape != null)
                    {
                        annots.Add(shape);
                    }
                }
                MapView.AddAnnotations(annots.ToArray());
            }
        }
        void OnLayersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var currentLayersCount = MapView.Style.Layers.Length;
                    var index = currentLayersCount + e.NewStartingIndex - Element.MapStyle.CustomLayers.Count() + e.NewItems.Count;
                    if (index < currentLayersCount)
                    {
                        AddLayers(e.NewItems, index);
                    }
                    else
                    {
                        AddLayers(e.NewItems);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveLayers(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    var layersToRemove = new List<MGLStyleLayer>();
                    foreach (MGLStyleLayer layer in MapView.Style.Layers)
                    {
                        if (layer.Identifier.IsCustomId())
                        {
                            layersToRemove.Add(layer);
                        }
                    }
                    foreach (MGLStyleLayer layer in layersToRemove)
                    {
                        MapView.Style.RemoveLayer(layer);

                    }
                    layersToRemove.Clear();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveLayers(e.OldItems);
                    AddLayers(e.NewItems);
                    break;
                default:
                    break;
            }
        }

        void AddLayers(System.Collections.IList layers, int startIndex = -1)
        {
            if (layers == null)
            {
                return;
            }
            nuint index = (nuint)Math.Max(0, startIndex);
            foreach (Layer layer in layers)
            {
                if (string.IsNullOrEmpty(layer.Id))
                {
                    continue;
                }
                NSString id = layer.Id.ToCustomId();
                var oldLayer = MapView.Style.LayerWithIdentifier(id);
                if (oldLayer != null)
                {
                    MapView.Style.RemoveLayer(oldLayer);
                }
                if (layer is StyleLayer sl)
                {
                    var newLayer = GetStyleLayer(sl, id);
                    if (newLayer != null)
                    {
                        if (startIndex == -1)
                        {
                            MapView.Style.AddLayer(newLayer);
                        }
                        else
                        {
                            MapView.Style.InsertLayer(newLayer, index);
                            index += 1;
                        }
                    }
                }
            }
        }

        void RemoveLayers(System.Collections.IList layers)
        {
            if (layers == null)
            {
                return;
            }
            foreach (Layer layer in layers)
            {
                if (string.IsNullOrEmpty(layer.Id))
                {
                    continue;
                }
                NSString id = layer.Id.ToCustomId();
                var oldLayer = MapView.Style.LayerWithIdentifier(id);
                if (oldLayer != null)
                {
                    MapView.Style.RemoveLayer(oldLayer);
                }
            }
        }

        void OnShapeSourcesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddSources(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveSources(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    var sourcesToRemove = new List<MGLSource>();
                    foreach (MGLSource source in MapView.Style.Sources)
                    {
                        if (source.Identifier.IsCustomId())
                        {
                            sourcesToRemove.Add(source);
                        }
                    }
                    foreach (MGLSource source in sourcesToRemove)
                    {
                        MapView.Style.RemoveSource(source);

                    }
                    sourcesToRemove.Clear();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveSources(e.OldItems);
                    AddSources(e.NewItems);
                    break;
                default: break;
            }
        }

        void AddSources(System.Collections.IList sources)
        {
            if (sources == null || MapView == null || MapView.Style == null)
            {
                return;
            }
            foreach (MapSource source in sources)
            {
                MGLSource mglSource = null;
                if (source.Id != null)
                {
                    var sourceId = source.Id.ToCustomId();
                    if (source is ShapeSource shapeSource)
                    {
                        if (shapeSource.Shape == null) continue;
                        var shape = ShapeFromAnnotation(shapeSource.Shape);
                        var oldSource = MapView.Style?.SourceWithIdentifier(sourceId);
                        if (oldSource != null && oldSource is MGLShapeSource)
                        {
                            (oldSource as MGLShapeSource).Shape = shape;
                        }
                        else
                        {
                            mglSource = new MGLShapeSource(sourceId, shape, null);
                        }
                    }
                    else if (source is RasterSource rasterSource)
                    {
                        if (string.IsNullOrEmpty(rasterSource.ConfigurationURL) == false
                            && NSUrl.FromString(rasterSource.ConfigurationURL) is NSUrl url)
                        {
                            if (rasterSource.TileSize <= 0)
                            {
                                mglSource = new MGLRasterTileSource(sourceId, url);
                            }
                            else
                            {
                                mglSource = new MGLRasterTileSource(sourceId, url, (nfloat)rasterSource.TileSize);
                            }
                        }
                        else if (rasterSource.TileURLTemplates != null)
                        {
                            if (rasterSource.Options != null)
                            {
                                var keys = new List<NSString>();
                                var values = new List<NSObject>();
                                foreach (TileSourceOption key in rasterSource.Options.Keys)
                                {
                                    switch (key)
                                    {
                                        case TileSourceOption.AttributionHTMLString:
                                            keys.Add(MGLTileSourceOptions.AttributionHTMLString);
                                            values.Add((NSString)(rasterSource.Options[key] as string));
                                            break;
                                        case TileSourceOption.AttributionInfos:
                                            if (rasterSource.Options[key] is AttributionInfo[] infos)
                                            {
                                                var infosList = new NSMutableArray<MGLAttributionInfo>();
                                                foreach (AttributionInfo info in infos)
                                                {
                                                    var attr = new MGLAttributionInfo(new NSAttributedString(info.Title), info.Url != null ? NSUrl.FromString(info.Url) : null);
                                                    infosList.Add(attr);
                                                }
                                                keys.Add(MGLTileSourceOptions.AttributionInfos);
                                                values.Add(infosList);
                                            }
                                            break;
                                        case TileSourceOption.MaximumZoomLevel:
                                            keys.Add(MGLTileSourceOptions.MaximumZoomLevel);
                                            values.Add(NSNumber.FromDouble((double)rasterSource.Options[key]));
                                            break;
                                        case TileSourceOption.MinimumZoomLevel:
                                            keys.Add(MGLTileSourceOptions.MinimumZoomLevel);
                                            values.Add(NSNumber.FromDouble((double)rasterSource.Options[key]));
                                            break;
                                        case TileSourceOption.TileCoordinateSystem:
                                            if (rasterSource.Options[key] is TileCoordinateSystem sys)
                                            {
                                                if (sys == TileCoordinateSystem.TileCoordinateSystemTMS)
                                                {
                                                    keys.Add(MGLTileSourceOptions.TileCoordinateSystem);
                                                    values.Add(NSNumber.FromUInt64((ulong)MGLTileCoordinateSystem.Tms));
                                                }
                                                else
                                                {
                                                    keys.Add(MGLTileSourceOptions.TileCoordinateSystem);
                                                    values.Add(NSNumber.FromUInt64((ulong)MGLTileCoordinateSystem.Xyz));
                                                }
                                            }
                                            break;
                                        default: break;
                                    }
                                }
                                mglSource = new MGLRasterTileSource(sourceId,
                                                                tileURLTemplates: rasterSource.TileURLTemplates,
                                                                options: new NSDictionary<NSString, NSObject>(keys.ToArray(), values.ToArray()));
                            }
                            else
                            {
                                mglSource = new MGLRasterTileSource(sourceId,
                                                                tileURLTemplates: rasterSource.TileURLTemplates,
                                                                options: null);
                            }
                        }
                    }
                    else
                    {
                        mglSource = new MGLSource(sourceId);
                    }
                }
                if (mglSource != null)
                {
                    MapView.Style.AddSource(mglSource);
                }
            }
        }

        void RemoveSources(System.Collections.IList sources)
        {
            if (sources == null)
            {
                return;
            }
            foreach (MapSource source in sources)
            {
                if (source.Id != null)
                {
                    var oldSource = MapView.Style.SourceWithIdentifier(source.Id.ToCustomId());
                    if (oldSource != null)
                    {
                        MapView.Style.RemoveSource(oldSource);
                    }
                }
            }
        }

        MGLShape ShapeFromAnnotation(FormsMB.Annotation annotation)
        {
            MGLShape shape = null;
            if (annotation is PointAnnotation pointAnnotation)
            {
                shape = new MGLPointAnnotation()
                {
                    Coordinate = pointAnnotation.Coordinate.ToCLCoordinate()
                };
            }
            else if (annotation is PolylineAnnotation)
            {
                var polyline = annotation as PolylineAnnotation;
                shape = PolyLineWithCoordinates(polyline.Coordinates.ToArray());
                if (polyline.Coordinates is INotifyCollectionChanged notifiyCollection)
                {
                    notifiyCollection.CollectionChanged += (sender, e) =>
                    {
                        //TODO Move to a separated method
                        if (e.Action == NotifyCollectionChangedAction.Add)
                        {
                            foreach (Position pos in e.NewItems)
                            {
                                var coord = TypeConverter.FromPositionToCoordinate(pos);
                                (shape as MGLPolyline).AppendCoordinates(ref coord, 1);
                            }
                        }
                        else if (e.Action == NotifyCollectionChangedAction.Remove)
                        {
                            (shape as MGLPolyline).RemoveCoordinatesInRange(new NSRange(e.OldStartingIndex, e.OldItems.Count));
                        }
                    };
                }

            }
            else if (annotation is MultiPolylineAnnotation polyline)
            {
                if (polyline.Coordinates == null || polyline.Coordinates.Length == 0)
                {
                    return null;
                }
                var lines = new MGLPolyline[polyline.Coordinates.Length];
                for (var i = 0; i < polyline.Coordinates.Length; i++)
                {
                    if (polyline.Coordinates[i].Length == 0)
                    {
                        continue;
                    }
                    lines[i] = PolyLineWithCoordinates(polyline.Coordinates[i]);
                }
                shape = MGLMultiPolyline.MultiPolylineWithPolylines(lines);
            }
            if (shape != null)
            {
                if (annotation.Title != null)
                {
                    shape.Title = annotation.Title;
                }
                if (annotation.SubTitle != null)
                {
                    shape.Subtitle = annotation.SubTitle;
                }
                if (!string.IsNullOrEmpty(annotation.Id))
                {
                    shape.SetId(annotation.Id);
                }

                annotation.HandleId = shape.Handle.ToString();
            }

            return shape;
        }

        MGLPolyline PolyLineWithCoordinates(Position[] positions)
        {
            if (positions == null || positions.Length == 0)
            {
                return null;
            }
            var first = positions[0].ToCLCoordinate();
            var output = MGLPolyline.PolylineWithCoordinates(ref first, 1);
            var i = 1;
            while (i < positions.Length)
            {
                var coord = positions[i].ToCLCoordinate();
                output.AppendCoordinates(ref coord, 1);
                i++;
            }
            return output;
        }

        #region MGLMapViewDelegate
        [Export("mapViewDidFinishRenderingMap:fullyRendered:"),]
        void DidFinishRenderingMap(MGLMapView mapView, bool fullyRendered)
        {
            Element.DidFinishRenderingCommand?.Execute(
                fullyRendered);
        }

        [Export("mapView:didUpdateUserLocation:")]
        public void MapViewDidUpdateUserLocation(MGLMapView mapView, MGLUserLocation userLocation)
        {
            if (userLocation != null)
            {
                Element.UserLocation = new Position(
                    userLocation.Location.Coordinate.Latitude,
                    userLocation.Location.Coordinate.Longitude
                );
            }
        }

        //[Export("mapView:didUpdateUserLocation:"),]
        //void DidUpdateUserLocation(MGLMapView mapView, MGLUserLocation userLocation)
        //{
        //    if (userLocation != null)
        //    {
        //        Element.UserLocation = new Position(
        //            userLocation.Location.Coordinate.Latitude,
        //            userLocation.Location.Coordinate.Longitude
        //        );
        //    }
        //}

        [Export("mapView:didFinishLoadingStyle:"),]
        void DidFinishLoadingStyle(MGLMapView mapView, MGLStyle style)
        {
            MapStyle newStyle;
            if (Element.MapStyle == null)
            {
                newStyle = new MapStyle(mapView.StyleURL.AbsoluteString);
                newStyle.Name = style.Name;
                Element.MapStyle = newStyle;
            }
            else
            {
                if (Element.MapStyle.UrlString == null
                || Element.MapStyle.UrlString != mapView.StyleURL.AbsoluteString)
                {
                    Element.MapStyle.SetUrl(mapView.StyleURL.AbsoluteString);
                    Element.MapStyle.Name = style.Name;
                }
                newStyle = Element.MapStyle;
            }
            if (Element.MapStyle.CustomSources != null)
            {
                var notifiyCollection = Element.MapStyle.CustomSources as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged += OnShapeSourcesCollectionChanged;
                }

                AddSources(Element.MapStyle.CustomSources.ToList());
            }
            if (Element.MapStyle.CustomLayers != null)
            {
                if (Element.MapStyle.CustomLayers is INotifyCollectionChanged notifiyCollection)
                {
                    notifiyCollection.CollectionChanged += OnLayersCollectionChanged;
                }

                AddLayers(Element.MapStyle.CustomLayers.ToList());
            }

            newStyle.OriginalLayers = style.Layers.Select((MGLStyleLayer arg) => new Layer(arg.Identifier)
            {
                IsVisible = arg.Visible
            }).ToArray();
            newStyle.Name = style.Name;
            Element.DidFinishLoadingStyleCommand?.Execute(newStyle);
        }

        [Export("mapViewRegionIsChanging:"),]
        void RegionIsChanging(MGLMapView mapView)
        {
            Element.Center = new Position(mapView.CenterCoordinate.Latitude, mapView.CenterCoordinate.Longitude);
        }

        [Export("mapView:regionDidChangeAnimated:"),]
        void RegionDidChangeAnimated(MGLMapView mapView, bool animated)
        {
            Element.ZoomLevel = mapView.ZoomLevel;
            Element.Pitch = (double)mapView.Camera.Pitch;
            Element.RotatedDegree = (double)mapView.Camera.Heading;
            Element?.RegionDidChangeCommand?.Execute(animated);
        }

        [Export("mapView:annotationCanShowCallout:"),]
        bool AnnotationCanShowCallout(MGLMapView mapView, NSObject annotation)
        {
            if (annotation is MGLShape && Element.CanShowCalloutChecker != null)
            {
                return Element.CanShowCalloutChecker.Invoke(((MGLShape)annotation).Id());
            }
            return true;
        }

        [Export("mapView:tapOnCalloutForAnnotation:")]
        void MapView_TapOnCalloutForAnnotation(MGLMapView mapView, NSObject annotation)
        {
            if (annotation is MGLShape shape)
            {
                Element.DidTapOnCalloutViewCommand?.Execute(shape.Id());
            }
            else
            {
                Element.DidTapOnCalloutViewCommand?.Execute(null);
            }
        }

        MGLAnnotationImage MapView_ImageForAnnotation(MGLMapView mapView, IMGLAnnotation annotation)
        {
            if (annotation is MGLShape shape)
            {
                var result = Element.GetImageForAnnotationFunc?.Invoke(shape.Id());
                if (result != null
                    && false == string.IsNullOrEmpty(result.Item1)
                    && false == string.IsNullOrEmpty(result.Item2))
                {
                    var image = MapView.DequeueReusableAnnotationImageWithIdentifier(result.Item1);
                    if (image == null)
                    {
                        var iosImage = new UIImage(result.Item2);
                        if (iosImage != null)
                        {
                            iosImage = iosImage.ImageWithAlignmentRectInsets(new UIEdgeInsets(0, 0, iosImage.Size.Height / 2, 0));
                            return MGLAnnotationImage.AnnotationImageWithImage(iosImage, result.Item1);
                        }
                    }
                    return image;
                }
            }
            return null;

        }
        #endregion

        #region UIGestureRecognizerDelegate
        [Export("gestureRecognizer:shouldRecognizeSimultaneouslyWithGestureRecognizer:")]
        public bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            return true;
        }

        #endregion

        Dictionary<string, object> ConvertDictionary(NSDictionary fromDict)
        {
            var output = new Dictionary<string, object>();
            foreach (NSString key in fromDict.Keys)
            {
                var value = fromDict[key];
                switch (value)
                {
                    case NSString str:
                        if (str == "<NULL>")
                        {
                            continue;
                        }
                        output[key] = (string)str;
                        break;
                    case NSNumber number:
                        output[key] = number.DoubleValue;
                        break;
                    case NSDate date:
                        output[key] = date.ToDateTimeOffset();
                        break;
                    default:
                        output[key] = value.ToString();
                        break;
                }
            }
            return output;
        }
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
