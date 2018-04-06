using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.App;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using Java.Util;
using Naxam.Controls.Mapbox.Forms;
using Naxam.Controls.Mapbox.Platform.Droid;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Com.Mapbox.Mapboxsdk.Maps.MapboxMap;
using Annotation = Naxam.Controls.Mapbox.Forms.Annotation;
using Bitmap = Android.Graphics.Bitmap;
using MapView = Naxam.Controls.Mapbox.Forms.MapView;
using Point = Xamarin.Forms.Point;
using Sdk = Com.Mapbox.Mapboxsdk;
using View = Android.Views.View;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
        : ViewRenderer<MapView, View>, IOnMapReadyCallback
    {
        MapboxMap map;

        MapViewFragment fragment;
        private const int SIZE_ZOOM = 13;
        private Position currentCamera;

        Dictionary<string, Sdk.Annotations.Annotation> _annotationDictionaries =
            new Dictionary<string, Sdk.Annotations.Annotation>();

        public MapViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(
            ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.TakeSnapshotFunc -= TakeMapSnapshot;
                e.OldElement.GetFeaturesAroundPointFunc -= GetFeaturesAroundPoint;
                if (map != null)
                {
                    RemoveMapEvents();
                }
            }

            if (e.NewElement == null)
                return;

            if (Control == null)
            {
                var activity = (AppCompatActivity)Context;
                var view = new Android.Widget.FrameLayout(activity)
                {
                    Id = GenerateViewId()
                };

                SetNativeControl(view);

                fragment = new MapViewFragment();

                activity.SupportFragmentManager.BeginTransaction()
                    .Replace(view.Id, fragment)
                    .Commit();


                fragment.GetMapAsync(this);
                currentCamera = new Position();
            }
        }

        public void SetupFunctions()
        {

            Element.TakeSnapshotFunc += TakeMapSnapshot;
            Element.GetFeaturesAroundPointFunc += GetFeaturesAroundPoint;
            Element.ResetPositionAction = () =>
            {
                //TODO handle reset position call
                //map.ResetNorth();
                //map.AnimateCamera(CameraUpdateFactory.ZoomTo(Element.ZoomLevel));
            };
            Element.UpdateLayerFunc = (string layerId, bool isVisible, bool IsCustom) =>
            {
                if (!string.IsNullOrEmpty(layerId))
                {
                    string layerIdStr = IsCustom ? layerId.Prefix() : layerId;
                    var layer = map.GetLayer(layerIdStr);
                    if (layer != null)
                    {
                        layer.SetProperties(layer.Visibility,
                                            isVisible ? Sdk.Style.Layers.PropertyFactory.Visibility(Sdk.Style.Layers.Property.Visible)
                                            : Sdk.Style.Layers.PropertyFactory.Visibility(Sdk.Style.Layers.Property.None));

                        if (IsCustom && Element.MapStyle.CustomLayers != null)
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
                }
                return false;
            };

            Element.UpdateShapeOfSourceFunc = (Annotation annotation, string sourceId) =>
            {
                if (annotation != null && !string.IsNullOrEmpty(sourceId))
                {
                    var shape = annotation.ToFeatureCollection();
                    var source = map.GetSource(sourceId.Prefix()) as Sdk.Style.Sources.GeoJsonSource;
                    if (source != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            source.SetGeoJson(shape);
                        });
                        if (Element.MapStyle.CustomSources?.FirstOrDefault((arg) => arg.Id == sourceId) is ShapeSource ss)
                        {
                            ss.Shape = annotation;
                        }
                        //if (Element.MapStyle.CustomSources != null)
                        //{
                        //    var count = Element.MapStyle.CustomSources.Count();
                        //    for (var i = 0; i < count; i++)
                        //    {
                        //        if (Element.MapStyle.CustomSources.ElementAt(i).Id == sourceId)
                        //        {
                        //            Element.MapStyle.CustomSources.ElementAt(i).Shape = annotation;
                        //            break;
                        //        }
                        //    }
                        //}
                        return true;
                    }
                }
                return false;
            };

            Element.ReloadStyleAction = () =>
            {
                //https://github.com/mapbox/mapbox-gl-native/issues/9511
                map.SetStyleUrl(map.StyleUrl, null);
            };

            Element.UpdateViewPortAction = (Position centerLocation, double? zoomLevel, double? bearing, bool animated, Action completionHandler) =>
            {
                var newPosition = new CameraPosition.Builder()
                                                    .Bearing(bearing ?? map.CameraPosition.Bearing)
                                                    .Target(centerLocation?.ToLatLng() ?? map.CameraPosition.Target)
                                                    .Zoom(zoomLevel ?? map.CameraPosition.Zoom)
                                                    .Build();
                var callback = completionHandler == null ? null : new CancelableCallback()
                {
                    FinishHandler = completionHandler,
                    CancelHandler = completionHandler
                };
                var update = CameraUpdateFactory.NewCameraPosition(newPosition);
                if (animated)
                {
                    map.AnimateCamera(update, callback);
                }
                else
                {
                    map.MoveCamera(update, callback);
                }
            };

            Element.GetStyleImageFunc += GetStyleImage;
        }

        private byte[] GetStyleImage(string imageName)
        {
            var img = map.GetImage(imageName);
            if (img != null)
            {
                var stream = new MemoryStream();
                img.Compress(Bitmap.CompressFormat.Png, 100, stream);
                return stream.ToArray();
            }
            return null;
        }

        TaskCompletionSource<byte[]> tcs;
        Task<byte[]> TakeMapSnapshot()
        {
            //TODO
            if (tcs != null && tcs.Task.IsCompleted == false)
                return tcs.Task;
            tcs = new TaskCompletionSource<byte[]>();

            map.Snapshot(new SnapshotReadyCallback((bmp) =>
            {
                MemoryStream stream = new MemoryStream();
                bmp.Compress(Bitmap.CompressFormat.Png, 0, stream);
                var result = stream.ToArray();
                tcs.TrySetResult(result);
            }));
            return tcs.Task;
        }

        IFeature[] GetFeaturesAroundPoint(Point point, double radius, string[] layers)
        {
            var output = new List<IFeature>();
            RectF rect = point.ToRect(Context.ToPixels(radius));
            var listFeatures = map.QueryRenderedFeatures(rect, layers);
            return listFeatures.Select(x => x.ToFeature())
                               .Where(x => x != null)
                               .ToArray();
        }

        protected override void Dispose(bool disposing)
        {
            if (fragment != null)
            {
                RemoveMapEvents();

                if (fragment.StateSaved)
                {
                    var activity = (AppCompatActivity)Context;
                    var fm = activity.SupportFragmentManager;

                    fm.BeginTransaction()
                        .Remove(fragment)
                        .Commit();
                }

                fragment.Dispose();
                fragment = null;
            }

            base.Dispose(disposing);
        }

        private Dictionary<string, object> ConvertToDictionary(string featureProperties)
        {
            Dictionary<string, object> objectFeature = JsonConvert.DeserializeObject<Dictionary<string, object>>(featureProperties);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(objectFeature["properties"].ToString()); ;
        }

        private void FocustoLocation(LatLng latLng)
        {
            if (map == null) { return; }

            CameraPosition position = new CameraPosition.Builder().Target(latLng).Zoom(SIZE_ZOOM).Build();
            ICameraUpdate camera = CameraUpdateFactory.NewCameraPosition(position);
            map.AnimateCamera(camera);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            System.Diagnostics.Debug.WriteLine("MapViewRenderer:" + e.PropertyName);
            if (e.PropertyName == MapView.CenterProperty.PropertyName)
            {
                if (!ReferenceEquals(Element.Center, currentCamera))
                {
                    if (Element.Center == null) return;
                    FocustoLocation(Element.Center.ToLatLng());
                }
            }
            else if (e.PropertyName == MapView.MapStyleProperty.PropertyName && map != null)
            {
                UpdateMapStyle();
            }
            else if (e.PropertyName == MapView.PitchEnabledProperty.PropertyName)
            {
                if (map != null)
                {
                    map.UiSettings.TiltGesturesEnabled = Element.PitchEnabled;
                }
            }
            else if (e.PropertyName == MapView.PitchProperty.PropertyName)
            {
                map?.SetTilt(Element.Pitch);
            }
            else if (e.PropertyName == MapView.RotateEnabledProperty.PropertyName)
            {
                if (map != null)
                {
                    map.UiSettings.RotateGesturesEnabled = Element.RotateEnabled;
                }
            }
            else if (e.PropertyName == MapView.RotatedDegreeProperty.PropertyName)
            {
                map?.SetBearing(Element.RotatedDegree);
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
            else if (e.PropertyName == MapView.ZoomLevelProperty.PropertyName && map != null)
            {
                var dif = Math.Abs(map.CameraPosition.Zoom - Element.ZoomLevel);
                System.Diagnostics.Debug.WriteLine($"Current zoom: {map.CameraPosition.Zoom} - New zoom: {Element.ZoomLevel}");
                if (dif >= 0.01)
                {
                    System.Diagnostics.Debug.WriteLine("Updating zoom level");
                    map.AnimateCamera(CameraUpdateFactory.ZoomTo(Element.ZoomLevel));
                }
            }
        }

        void UpdateMapStyle()
        {
            if (Element.MapStyle != null && !string.IsNullOrEmpty(Element.MapStyle.UrlString))
            {
                map.StyleUrl = Element.MapStyle.UrlString;
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
        }

        void OnMapStylePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var style = sender as MapStyle;
            if (style == null) return;
            if (e.PropertyName == MapStyle.CustomSourcesProperty.PropertyName
                && style.CustomSources != null)
            {
                var notifiyCollection = style.CustomSources as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged += OnShapeSourcesCollectionChanged;
                }

                AddSources(style.CustomSources.ToList());
            }
            else if (e.PropertyName == MapStyle.CustomLayersProperty.PropertyName
                     && style.CustomLayers != null)
            {
                var notifiyCollection = Element.MapStyle.CustomLayers as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged += OnLayersCollectionChanged;
                }

                AddLayers(Element.MapStyle.CustomLayers.ToList());
            }
        }

        void OnShapeSourcesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AddSources(e.NewItems.Cast<MapSource>().ToList());
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                RemoveSources(e.OldItems.Cast<MapSource>().ToList());
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                var sources = map.Sources;
                foreach (var s in sources)
                {
                    if (s.Id.HasPrefix())
                    {
                        map.RemoveSource(s);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                RemoveSources(e.OldItems.Cast<MapSource>().ToList());
                AddSources(e.NewItems.Cast<MapSource>().ToList());
            }
        }

        void AddSources(List<MapSource> sources)
        {
            if (sources == null || map == null)
            {
                return;
            }

            foreach (MapSource ms in sources)
            {
                if (ms.Id != null)
                {
                    if (ms is ShapeSource ss && ss.Shape != null)
                    {
                        var shape = ss.Shape.ToFeatureCollection();

                        var source = map.GetSource(ss.Id.Prefix()) as Sdk.Style.Sources.GeoJsonSource;

                        if (source == null)
                        {
                            source = new Sdk.Style.Sources.GeoJsonSource(ss.Id.Prefix(), shape);
                            map.AddSource(source);
                        }
                        else
                        {
                            source.SetGeoJson(shape);
                        }
                    }
                    else
                    {
                        //TODO handle RasterSource
                    }
                }
            }
        }

        void RemoveSources(List<MapSource> sources)
        {
            if (sources == null)
            {
                return;
            }
            foreach (MapSource source in sources)
            {
                if (source.Id != null)
                {
                    map.RemoveSource(source.Id.Prefix());
                }
            }
        }

        void OnLayersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AddLayers(e.NewItems.Cast<Layer>().ToList());
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                RemoveLayers(e.OldItems);
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                var layers = map.Layers;
                foreach (var layer in layers)
                {
                    if (layer.Id.HasPrefix())
                    {
                        map.RemoveLayer(layer);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                RemoveLayers(e.OldItems);

                AddLayers(e.NewItems.Cast<Layer>().ToList());
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
                var native = map.GetLayer(layer.Id.Prefix());

                if (native != null)
                {
                    map.RemoveLayer(native);
                }
            }
        }

        void AddLayers(List<Layer> layers)
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

                map.RemoveLayer(layer.Id.Prefix());

                if (layer is CircleLayer)
                {
                    var cross = (CircleLayer)layer;

                    var source = map.GetSource(cross.SourceId.Prefix());
                    if (source == null)
                    {
                        continue;
                    }

                    map.AddLayer(cross.ToNative());
                }
                else if (layer is LineLayer)
                {
                    var cross = (LineLayer)layer;

                    var source = map.GetSource(cross.SourceId.Prefix());
                    if (source == null)
                    {
                        continue;
                    }

                    map.AddLayer(cross.ToNative());
                }
            }
        }

        private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Annotation annot in e.NewItems)
                {
                    AddAnnotation(annot);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var items = new List<Annotation>();
                foreach (Annotation annot in e.OldItems)
                {
                    items.Add(annot);
                }
                RemoveAnnotations(items.ToArray());
                foreach (var item in items)
                {
                    _annotationDictionaries.Remove(item.Id);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                map.RemoveAnnotations();
                _annotationDictionaries.Clear();
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var itemsToRemove = new List<Annotation>();
                foreach (Annotation annot in e.OldItems)
                {
                    itemsToRemove.Add(annot);
                }
                RemoveAnnotations(itemsToRemove.ToArray());


                var itemsToAdd = new List<Annotation>();
                foreach (Annotation annot in e.NewItems)
                {
                    itemsToRemove.Add(annot);
                }
                AddAnnotations(itemsToAdd.ToArray());
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
                AddAnnotation(at);
            }
        }

        private Sdk.Annotations.Annotation AddAnnotation(Annotation at)
        {
            Sdk.Annotations.Annotation options = null;
            if (at is PointAnnotation)
            {
                var marker = new MarkerOptions();
                marker.SetTitle(at.Title);
                marker.SetSnippet(at.Title);
                marker.SetPosition(((PointAnnotation)at).Coordinate.ToLatLng());
                var output = Element.GetImageForAnnotationFunc?.Invoke(at.Id);
                if (output?.Item2 is string imgName)
                {
                    IconFactory iconFactory = IconFactory.GetInstance(Context);
                    Icon icon = iconFactory.FromResource(Context.Resources.GetIdentifier(imgName, "drawable", Context.PackageName));
                    marker.SetIcon(icon);
                }
                options = map.AddMarker(marker);
            }
            else if (at is PolylineAnnotation)
            {
                var polyline = at as PolylineAnnotation;
                if (polyline.Coordinates?.Count() == 0)
                {
                    return null;
                }
                var notifyCollection = polyline.Coordinates as INotifyCollectionChanged;
                if (notifyCollection != null)
                {
                    notifyCollection.CollectionChanged += (s, e) =>
                    {
                        if (e.Action == NotifyCollectionChangedAction.Add)
                        {
                            if (_annotationDictionaries.ContainsKey(at.Id))
                            {
                                var poly = _annotationDictionaries[at.Id] as Polyline;
                                poly.AddPoint(polyline.Coordinates.ElementAt(e.NewStartingIndex).ToLatLng());
                            }
                            else
                            {
                                var coords = new ArrayList();
                                for (var i = 0; i < polyline.Coordinates.Count(); i++)
                                {
                                    coords.Add(polyline.Coordinates.ElementAt(i).ToLatLng());
                                }
                                var polylineOpt = new PolylineOptions();
                                polylineOpt.Polyline.Width = Context.ToPixels(1);
                                polylineOpt.Polyline.Color = Android.Graphics.Color.Blue;
                                polylineOpt.AddAll(coords);
                                options = map.AddPolyline(polylineOpt);
                                _annotationDictionaries.Add(at.Id, options);
                            }
                        }
                        else if (e.Action == NotifyCollectionChangedAction.Remove)
                        {
                            if (_annotationDictionaries.ContainsKey(at.Id))
                            {
                                var poly = _annotationDictionaries[at.Id] as Polyline;
                                poly.Points.Remove(polyline.Coordinates.ElementAt(e.OldStartingIndex).ToLatLng());
                            }
                        }
                    };
                }
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
                map.AddPolylines(lines);
            }
            if (options != null)
            {
                if (at.Id != null)
                {
                    if (_annotationDictionaries.ContainsKey(at.Id))
                    {
                        _annotationDictionaries[at.Id] = options;
                    }
                    else
                    {
                        _annotationDictionaries.Add(at.Id, options);
                    }
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

        public void OnMapReady(MapboxMap p0)
        {
            map = p0;
            //map.MyLocationEnabled = true;
            map.UiSettings.RotateGesturesEnabled = Element.RotateEnabled;
            map.UiSettings.TiltGesturesEnabled = Element.PitchEnabled;

            if (Element.Center != null)
            {
                map.CameraPosition = new CameraPosition.Builder()
                    .Target(Element.Center.ToLatLng())
               .Zoom(Element.ZoomLevel)
                    .Tilt(Element.Pitch)
                    .Bearing(Element.RotatedDegree)
               .Build();
            }
            else
            {
                map.CameraPosition = new CameraPosition.Builder()
                    .Target(map.CameraPosition.Target)
               .Zoom(Element.ZoomLevel)
                    .Tilt(Element.Pitch)
                    .Bearing(Element.RotatedDegree)
               .Build();
            }

            AddMapEvents();
            SetupFunctions();
            if (Element.MapStyle == null)
            {
                if (map.StyleUrl != null)
                {
                    Element.MapStyle = new MapStyle(map.StyleUrl);
                }
            }
            else
            {
                UpdateMapStyle();
            }
        }
    }

    class SnapshotReadyCallback : Java.Lang.Object, ISnapshotReadyCallback
    {
        public Action<Bitmap> SnapshotReady { get; set; }
        public SnapshotReadyCallback(Action<Bitmap> SnapshotReady)
        {
            this.SnapshotReady = SnapshotReady;
        }

        public void OnSnapshotReady(Bitmap p0)
        {
            SnapshotReady?.Invoke(p0);
        }
    }
}
