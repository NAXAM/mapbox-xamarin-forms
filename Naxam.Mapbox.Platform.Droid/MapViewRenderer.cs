using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;

using Android.Graphics;
using Android.Support.V7.App;
using Android.Views;
using Java.Util;

using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using Com.Mapbox.Services.Commons.Geojson;
using Naxam.Controls.Platform.Droid;
using Naxam.Controls.Forms;

using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Annotation = Naxam.Controls.Forms.Annotation;
using Bitmap = Android.Graphics.Bitmap;
using MapView = Naxam.Controls.Forms.MapView;
using Point = Xamarin.Forms.Point;
using Sdk = Com.Mapbox.Mapboxsdk;
using View = Android.Views.View;

namespace Naxam.Controls.Platform.Droid
{
    public partial class MapViewRenderer
        : ViewRenderer<MapView, View>, MapboxMap.ISnapshotReadyCallback, IOnMapReadyCallback
    {
        MapboxMap map;

        MapViewFragment fragment;
        private const int SIZE_ZOOM = 13;
        private Position currentCamera;

        protected override void OnElementChanged(
            ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.TakeSnapshot -= TakeMapSnapshot;
                e.OldElement.GetFeaturesAroundPoint -= GetFeaturesAroundPoint;
                e.OldElement.Annotations.CollectionChanged -= OnAnnotationsCollectionChanged;
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

            Element.TakeSnapshot += TakeMapSnapshot;
            Element.GetFeaturesAroundPoint += GetFeaturesAroundPoint;
            // Monitor Annotations
            Element.Annotations.CollectionChanged += OnAnnotationsCollectionChanged;

            Element.ResetPositionFunc = new Command(x =>
            {
                //map.ResetNorth();

                map.AnimateCamera(CameraUpdateFactory.ZoomBy(Element.ZoomLevel));
             });

            Element.UpdateLayerFunc = (string layerId, bool isVisible, bool IsCustom) =>
            {
                if (!string.IsNullOrEmpty(layerId))
                {
                    string layerIdStr = IsCustom ? layerId.Prefix() : layerId;
                    var layer = map.GetLayer(layerIdStr);
                    if (layer != null)
                    {
                        layer.SetProperties(layer.Visibility, isVisible ? Sdk.Style.Layers.PropertyFactory.Visibility(Sdk.Style.Layers.Property.Visible) :
                            Sdk.Style.Layers.PropertyFactory.Visibility(Sdk.Style.Layers.Property.None));

                        if (IsCustom || Element.MapStyle.CustomLayers != null)
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
                        source.SetGeoJson(shape);
                        if (Element.MapStyle.CustomSources != null)
                        {
                            var count = Element.MapStyle.CustomSources.Count();
                            for (var i = 0; i < count; i++)
                            {
                                if (Element.MapStyle.CustomSources.ElementAt(i).Id == sourceId)
                                {
                                    Element.MapStyle.CustomSources.ElementAt(i).Shape = annotation;
                                    break;
                                }
                            }
                        }
                        return true;
                    }
                }
                return false;
            };

            Element.ReloadStyleFunc = new Command((obj) => {
                map.SetStyleUrl(map.StyleUrl, null);
            });
        }

        byte[] TakeMapSnapshot()
        {
            //TODO
            map.Snapshot(this);
            return result;
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
            if (e.PropertyName == MapView.CenterProperty.PropertyName)
            {
                if (!ReferenceEquals(Element.Center, currentCamera))
                {
                    if (Element.Center == null) return;
                    FocustoLocation(new LatLng(Element.Center.Lat, Element.Center.Long));
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
            else if (e.PropertyName == MapView.RotateEnabledProperty.PropertyName)
            {
                if (map != null)
                {
                    map.UiSettings.RotateGesturesEnabled = Element.RotateEnabled;
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
            else if (e.PropertyName == MapStyle.CustomLayersProperty.PropertyName
                     && (sender as MapStyle).CustomLayers != null)
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
                AddSources(e.NewItems.Cast<ShapeSource>().ToList());
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                RemoveSources(e.OldItems.Cast<ShapeSource>().ToList());
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
                RemoveSources(e.OldItems.Cast<ShapeSource>().ToList());
                AddSources(e.NewItems.Cast<ShapeSource>().ToList());
            }
        }

        void AddSources(List<ShapeSource> sources)
        {
            if (sources == null || map == null)
            {
                return;
            }

            foreach (ShapeSource ss in sources)
            {
                if (ss.Id != null && ss.Shape != null)
                {
                    var shape = ss.Shape.ToFeatureCollection();

                    var source = map.GetSource(ss.Id.Prefix()) as Sdk.Style.Sources.GeoJsonSource;

                    if (source == null)
                    {
                        map.AddSource(new Sdk.Style.Sources.GeoJsonSource(ss.Id.Prefix()));
                    }
                    else
                    {
                        source.SetGeoJson(shape);
                    }
                }
            }
        }

        void RemoveSources(List<ShapeSource> sources)
        {
            if (sources == null)
            {
                return;
            }
            foreach (ShapeSource source in sources)
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
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                map.RemoveAnnotations();
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
                if (at.Native != null)
                {
                    annots.Add((Sdk.Annotations.Annotation)at.Native);
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
            if (at is PointAnnotation)
            {
                var markerOpt = new MarkerOptions();
                markerOpt.SetTitle(at.Title);
                markerOpt.SetSnippet(at.Title);
                markerOpt.SetPosition(new LatLng(((PointAnnotation)at).Coordinate.Lat,
                    ((PointAnnotation)at).Coordinate.Long));
                at.Native = map.AddMarker(markerOpt);
                return (Sdk.Annotations.Annotation)at.Native;
            }
            else if (at is PolygonAnnotation)
            {
                var polygon = at as PolygonAnnotation;
                var notifyCollection = polygon.Coordinates as INotifyCollectionChanged;
                if (notifyCollection != null)
                {
                    notifyCollection.CollectionChanged += (s, e) =>
                    {
                        if (e.Action == NotifyCollectionChangedAction.Add)
                        {
                            if (at.Native != null)
                            {
                                var poly = at.Native as Sdk.Annotations.Polygon;
                                poly.AddPoint(new LatLng(polygon.Coordinates.ElementAt(e.NewStartingIndex).Lat, polygon.Coordinates.ElementAt(e.NewStartingIndex).Long));
                            }
                            else
                            {
                                CreatePolygon(at as PolygonAnnotation);
                            }
                        }
                        else if (e.Action == NotifyCollectionChangedAction.Remove)
                        {
                            if (at.Native != null)
                            {
                                var poly = at.Native as Sdk.Annotations.Polygon;
                                poly.Points.Remove(new LatLng(polygon.Coordinates.ElementAt(e.OldStartingIndex).Lat, polygon.Coordinates.ElementAt(e.OldStartingIndex).Long));
                            }
                        }
                    };
                }
                if (polygon.Coordinates?.Count() == 0)
                {
                    return null;
                }
                // We have some positions, so lets create polyline
                CreatePolygon(at as PolygonAnnotation);
                return (Sdk.Annotations.Annotation)at.Native;
            }
            else if (at is PolylineAnnotation)
            {
                var polyline = at as PolylineAnnotation;
                var notifyCollection = polyline.Coordinates as INotifyCollectionChanged;
                if (notifyCollection != null)
                {
                    notifyCollection.CollectionChanged += (s, e) =>
                    {
                        if (e.Action == NotifyCollectionChangedAction.Add)
                        {
                            if (at.Native != null)
                            {
                                var poly = at.Native as Polyline;
                                poly.AddPoint(new LatLng(polyline.Coordinates.ElementAt(e.NewStartingIndex).Lat, polyline.Coordinates.ElementAt(e.NewStartingIndex).Long));
                            }
                            else
                            {
                                CreatePolyline(at as PolylineAnnotation);
                            }
                        }
                        else if (e.Action == NotifyCollectionChangedAction.Remove)
                        {
                            if (at.Native != null)
                            {
                                var poly = at.Native as Polyline;
                                poly.Points.Remove(new LatLng(polyline.Coordinates.ElementAt(e.OldStartingIndex).Lat, polyline.Coordinates.ElementAt(e.OldStartingIndex).Long));
                            }
                        }
                    };
                }
                // Check, if up to now any positions for this polyline
                if (polyline.Coordinates?.Count() == 0)
                {
                    return null;
                }
                // We have some positions, so lets create polyline
                CreatePolyline(at as PolylineAnnotation);
                return (Sdk.Annotations.Annotation)at.Native;
            }
            return null;
        }

        void RemoveAllAnnotations()
        {
            if (map.Annotations != null)
            {
                map.RemoveAnnotations(map.Annotations);
            }
        }

        void CreatePolyline(PolylineAnnotation polyline)
        {
            var coords = new ArrayList();
            for (var i = 0; i < polyline.Coordinates.Count(); i++)
            {
                coords.Add(new LatLng(polyline.Coordinates.ElementAt(i).Lat, polyline.Coordinates.ElementAt(i).Long));
            }
            var polylineOpt = new PolylineOptions();
            polylineOpt.Polyline.Width = (float)polyline.StrokeWidth;
            polylineOpt.Polyline.Color = polyline.StrokeColor.ToAndroid();
            polylineOpt.Polyline.Alpha = (float)polyline.Alpha;
            polylineOpt.AddAll(coords);
            polyline.Native = map.AddPolyline(polylineOpt);
        }

        void CreatePolygon(PolygonAnnotation polygon)
        {
            var coords = new ArrayList();
            for (var i = 0; i < polygon.Coordinates.Count(); i++)
            {
                coords.Add(new LatLng(polygon.Coordinates.ElementAt(i).Lat, polygon.Coordinates.ElementAt(i).Long));
            }
            var polygonOpt = new PolygonOptions();
            // polygonOpt.Polygon.Width = polygon.StrokeWidth; // No width for polygon strokes
            polygonOpt.Polygon.StrokeColor = polygon.StrokeColor.ToAndroid();
            polygonOpt.Polygon.FillColor = polygon.FillColor.ToAndroid();
            polygonOpt.Polygon.Alpha = (float)polygon.Alpha;
            polygonOpt.AddAll(coords);
            polygon.Native = map.AddPolygon(polygonOpt);
        }

        private byte[] result;
        public void OnSnapshotReady(Bitmap bmp)
        {
            MemoryStream stream = new MemoryStream();
            bmp.Compress(Bitmap.CompressFormat.Png, 0, stream);
            result = stream.ToArray();
        }

        public void OnMapReady(MapboxMap p0)
        {
            map = p0;

            map.MyLocationEnabled = true;
            map.UiSettings.RotateGesturesEnabled = Element.RotateEnabled;
            map.UiSettings.TiltGesturesEnabled = Element.PitchEnabled;

            map.CameraPosition = new CameraPosition.Builder()
                .Target(new LatLng(Element.Center.Lat, Element.Center.Long))
                .Zoom(Element.ZoomLevel)
                .Build();

            AddMapEvents();

            SetupFunctions();
            UpdateMapStyle();
        }
    }
}
