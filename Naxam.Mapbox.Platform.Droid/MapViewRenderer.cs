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
using Naxam.Mapbox.Platform.Droid;
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
using FAnnotation = Naxam.Controls.Mapbox.Forms.Annotation;

using FMarker = Naxam.Controls.Mapbox.Forms.PointAnnotation;
using MMarker = Com.Mapbox.Mapboxsdk.Annotations.MarkerOptions;

using FPolyline = Naxam.Controls.Mapbox.Forms.PolylineAnnotation;
using MPolyline = Com.Mapbox.Mapboxsdk.Annotations.PolylineOptions;
using Com.Mapbox.Mapboxsdk.Offline;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer : ViewRenderer<MapView, View>, IOnMapReadyCallback
    {
        protected MapboxMap map;
        protected MapViewFragment fragment;
        private const int SIZE_ZOOM = 13;
        private Position currentCamera;
        bool mapReady;
        Dictionary<string, Sdk.Annotations.Annotation> _annotationDictionaries;
        public MapViewRenderer(Context context) : base(context)
        {
            _annotationDictionaries = new Dictionary<string, Sdk.Annotations.Annotation>();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                e.OldElement.AnnotationChanged -= Element_AnnotationChanged;
                e.OldElement.TakeSnapshotFunc -= TakeMapSnapshot;
                e.OldElement.GetFeaturesAroundPointFunc -= GetFeaturesAroundPoint;
                if (map != null)
                {
                    RemoveMapEvents();
                }
            }

            if (e.NewElement == null)
                return;
            e.NewElement.AnnotationChanged += Element_AnnotationChanged;
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
                .CommitAllowingStateLoss();
                fragment.GetMapAsync(this);
                currentCamera = new Position();

                if (mapReady)
                {
                    if (Element.Annotations != null)
                    {
                        AddAnnotations(Element.Annotations.ToArray());
                        if (Element.Annotations is INotifyCollectionChanged notifyCollection)
                        {
                            notifyCollection.CollectionChanged -= OnAnnotationsCollectionChanged;
                            notifyCollection.CollectionChanged += OnAnnotationsCollectionChanged;
                        }
                    }

                    OnMapRegionChanged();
                }
            }
        }

        private void Element_AnnotationChanged(object sender, AnnotationChangeEventArgs e)
        {
            if (e.OldAnnotation is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= OnAnnotationsCollectionChanged;
            }

            if (e.NewAnnotation is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += OnAnnotationsCollectionChanged;
            }

            if (mapReady)
            {
                RemoveAllAnnotations();
                AddAnnotations(Element?.Annotations?.ToArray());
            }
        }

        public virtual void SetupFunctions()
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
                    var layer = map.Style.GetLayer(layerIdStr);
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
                    var source = map.Style.GetSource(sourceId.Prefix()) as Sdk.Style.Sources.GeoJsonSource;
                    if (source != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            source.SetGeoJson(shape);
                        });
                        if (Element.MapStyle.CustomSources?.FirstOrDefault((arg) => arg.Id == sourceId) is ShapeSource shapeSource)
                        {
                            shapeSource.Shape = annotation;
                        }
                        return true;
                    }
                }
                return false;
            };

            Element.ReloadStyleAction = () =>
            {
                //https://github.com/mapbox/mapbox-gl-native/issues/9511
                map.SetStyle(map.Style.Url);
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

            Element.GetStyleLayerFunc = GetStyleLayer;

            Element.SelectAnnotationAction = (Tuple<string, bool> obj) =>
            {
                if (obj == null || map == null || map.Annotations == null) return;
                foreach (var item in map.Annotations)
                {
                    if (item is Marker marker && marker.Id.ToString() == obj.Item1)
                    {
                        map.SelectMarker(marker);
                    }
                }
            };

            Element.DeselectAnnotationAction = (Tuple<string, bool> obj) =>
            {
                if (obj == null || map == null || map.Annotations == null) return;
                foreach (var item in map.Annotations)
                {
                    if (item is Marker marker && marker.Id.ToString() == obj.Item1)
                    {
                        map.DeselectMarker(marker);
                    }
                }
            };

            Element.ApplyOfflinePackFunc = (offlinePack) =>
            {
                //var region = offlinePack.Region;
                //OfflineTilePyramidRegionDefinition definition = new OfflineTilePyramidRegionDefinition(
                //    region.StyleURL,
                //    LatLngBounds.From(offlinePack.Region.Bounds.NorthEast.Lat, offlinePack.Region.Bounds.NorthEast.Long, offlinePack.Region.Bounds.SouthWest.Lat, offlinePack.Region.Bounds.SouthWest.Long),
                //    region.MinimumZoomLevel,
                //    region.MaximumZoomLevel,
                //    Android.App.Application.Context.Resources.DisplayMetrics.Density);
                //var xxx = new OfflineTilePyramidRegionDefinition(null);
                LatLngBounds bounds = LatLngBounds.From(offlinePack.Region.Bounds.NorthEast.Lat, offlinePack.Region.Bounds.NorthEast.Long, offlinePack.Region.Bounds.SouthWest.Lat, offlinePack.Region.Bounds.SouthWest.Long);
                double regionZoom = offlinePack.Region.MaximumZoomLevel;
                CameraPosition cameraPosition = new CameraPosition.Builder()
                  .Target(bounds.Center)
                  .Zoom(regionZoom)
                  .Build();
                map.MoveCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
                return true;
            };
        }

        private byte[] GetStyleImage(string imageName)
        {
            var img = map.Style.GetImage(imageName);
            if (img != null)
            {
                var stream = new MemoryStream();
                img.Compress(Bitmap.CompressFormat.Png, 100, stream);
                return stream.ToArray();
            }
            return null;
        }

        private StyleLayer GetStyleLayer(string layerId, bool isCustomLayer)
        {
            var layer = map.Style.GetLayer(isCustomLayer ? layerId.Prefix() : layerId);
            if (layer is Com.Mapbox.Mapboxsdk.Style.Layers.BackgroundLayer background)
            {
                return background.ToForms();
            }
            if (layer is Com.Mapbox.Mapboxsdk.Style.Layers.CircleLayer circle)
            {
                return circle.ToForms();
            }
            if (layer is Com.Mapbox.Mapboxsdk.Style.Layers.LineLayer line)
            {
                return line.ToForms();
            }
            if (layer is Com.Mapbox.Mapboxsdk.Style.Layers.FillLayer fill)
            {
                return fill.ToForms();
            }
            if (layer is Com.Mapbox.Mapboxsdk.Style.Layers.SymbolLayer symbol)
            {
                return symbol.ToForms();
            }
            if (layer is Com.Mapbox.Mapboxsdk.Style.Layers.RasterLayer raster)
            {
                return raster.ToForms();
            }

            return null;
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
            RemoveMapEvents();

            if (fragment != null)
            {
                if (fragment.StateSaved)
                {
                    var activity = (AppCompatActivity)Context;
                    var fm = activity.SupportFragmentManager;

                    fm.BeginTransaction()
                        .Remove(fragment)
                        .CommitAllowingStateLoss();
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
            if (map == null || mapReady == false) { return; }
            CameraPosition position = new CameraPosition.Builder()
                .Target(latLng)
                .Zoom(Math.Abs(Element.ZoomLevel - 0) < 0.01 ? SIZE_ZOOM : Element.ZoomLevel)
                .Build();
            map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(position), 1000);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            System.Diagnostics.Debug.WriteLine("MapViewRenderer:" + e.PropertyName);
            if (e.PropertyName == MapView.RegionProperty.PropertyName)
            {
                OnMapRegionChanged();
                return;
            }

            if (e.PropertyName == MapView.CenterProperty.PropertyName)
            {
                if (Element.Center == null) return;

                FocustoLocation(Element.Center.ToLatLng());
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
                map?.AnimateCamera(CameraUpdateFactory.TiltTo(Element.Pitch));
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
                map?.AnimateCamera(CameraUpdateFactory.BearingTo(Element.RotatedDegree));
            }
            else if (e.PropertyName == MapView.ZoomLevelProperty.PropertyName && map != null)
            {
                var dif = Math.Abs(map.CameraPosition.Zoom - Element.ZoomLevel);
                System.Diagnostics.Debug.WriteLine($"Current zoom: {map.CameraPosition.Zoom} - New zoom: {Element.ZoomLevel}");
                if (dif >= 0.01 && cameraBusy == false)
                {
                    System.Diagnostics.Debug.WriteLine("Updating zoom level");
                    map.AnimateCamera(CameraUpdateFactory.ZoomTo(Element.ZoomLevel));
                }
            }
        }

        void OnMapRegionChanged()
        {
            if (Element.Region != Naxam.Mapbox.Forms.AnnotationsAndFeatures.MapRegion.Empty)
            {
                map?.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(
                    Com.Mapbox.Mapboxsdk.Geometry.LatLngBounds.From(
                        Element.Region.NorthEast.Lat,
                        Element.Region.NorthEast.Long,
                        Element.Region.SouthWest.Lat,
                        Element.Region.SouthWest.Long
                    ), 0));
            }
        }

        private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddAnnotations(e.NewItems.Cast<FAnnotation>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveAnnotations(e.OldItems.Cast<FAnnotation>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Reset:
                    map.RemoveAnnotations();
                    _annotationDictionaries.Clear();
                    AddAnnotations(Element.Annotations.ToList());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    var itemsToRemove = new List<Annotation>();
                    foreach (Annotation annotation in e.OldItems)
                    {
                        itemsToRemove.Add(annotation);
                    }
                    RemoveAnnotations(itemsToRemove.ToArray());

                    var itemsToAdd = new List<Annotation>();
                    foreach (Annotation annotation in e.NewItems)
                    {
                        itemsToRemove.Add(annotation);
                    }
                    AddAnnotations(itemsToAdd.ToArray());
                    break;
            }
        }

        void RemoveAnnotations(IList<FAnnotation> annotations)
        {
            var currentAnnotations = map.Annotations.Where(d => annotations.Any(annotation => annotation.Id == d.Id.ToString()));
            if (currentAnnotations == null || currentAnnotations.Count() == 0)
            {
                return;
            }
            for (int i = 0; i < annotations.Count(); i++)
            {
                _annotationDictionaries.Remove(annotations[i].Id);
            }

            map.RemoveAnnotations(currentAnnotations.ToList());
        }

        void AddAnnotations(IList<Annotation> annotations)
        {
            if (map == null)
                return;
            AddMakers(annotations.Where(d => d is FMarker).Cast<FMarker>().ToList());
            AddPolylines(annotations.Where(d => d is FPolyline).Cast<FPolyline>().ToList());
        }

        IList<Marker> AddMakers(IList<FMarker> markers)
        {
            if (markers == null || markers.Count == 0)
                return null;
            var iconSource = new Dictionary<string, Icon>();
            var markerOptions = new List<MMarker>();
            for (int i = 0; i < markers.Count; i++)
            {
                var annotation = markers[i];
                var marker = new MarkerOptions();
                marker.SetTitle(annotation.Title);
                marker.SetSnippet(annotation.SubTitle);
                marker.SetPosition(annotation.Coordinate.ToLatLng());
                if (string.IsNullOrEmpty(annotation.Icon) == false)
                {
                    if (iconSource.ContainsKey(annotation.Icon))
                    {
                        marker.SetIcon(iconSource[annotation.Icon]);
                    }
                    else
                    {
                        var icon = Context.GetIconFromResource(annotation.Icon);
                        if (icon != null)
                        {
                            marker.SetIcon(icon);
                            iconSource.Add(annotation.Icon, icon);
                        }
                    }
                }

                markerOptions.Add(marker);
            }
            var options = map.AddMarkers(markerOptions.Cast<BaseMarkerOptions>().ToList());
            for (int i = 0; i < options.Count; i++)
            {
                markers[i].Id = options[i].Id.ToString();
            }
            return options;
        }

        IList<Polyline> AddPolylines(IList<FPolyline> polylines)
        {
            if (polylines == null || polylines.Count == 0)
                return null;
            var polylineOptions = new List<MPolyline>();
            for (int i = 0; i < polylines.Count; i++)
            {
                var polyline = polylines[i];
                if (polyline.Coordinates == null || polyline.Coordinates.Count() == 0)
                {
                    continue;
                }
                var notifyCollection = polyline.Coordinates as INotifyCollectionChanged;

                var coords = new ArrayList(polyline.Coordinates.Select(d => d.ToLatLng()).ToArray());
                var polylineOpt = new PolylineOptions();
                var width = polyline.Width == 0 ? 1 : polyline.Width;
                polylineOpt.Polyline.Width = Context.ToPixels(width);
                var color = string.IsNullOrEmpty(polyline.HexColor) ? Android.Graphics.Color.Red : Android.Graphics.Color.ParseColor(polyline.HexColor);
                polylineOpt.Polyline.Color = color;
                polylineOpt.AddAll(coords);
                polylineOptions.Add(polylineOpt);

                if (notifyCollection != null)
                {
                    notifyCollection.CollectionChanged += (s, e) =>
                    {
                        switch (e.Action)
                        {
                            case NotifyCollectionChangedAction.Remove:
                                if (_annotationDictionaries.ContainsKey(polyline.Id))
                                {
                                    var poly = _annotationDictionaries[polyline.Id] as Polyline;
                                    poly.Points.Remove(polyline.Coordinates.ElementAt(e.OldStartingIndex).ToLatLng());
                                }
                                break;
                            case NotifyCollectionChangedAction.Add:
                                if (_annotationDictionaries.ContainsKey(polyline.Id))
                                {
                                    var poly = _annotationDictionaries[polyline.Id] as Polyline;
                                    poly.AddPoint(polyline.Coordinates.ElementAt(e.NewStartingIndex).ToLatLng());
                                }
                                break;
                            case NotifyCollectionChangedAction.Reset:
                                if (_annotationDictionaries.ContainsKey(polyline.Id))
                                {
                                    var poly = _annotationDictionaries[polyline.Id] as Polyline;
                                    poly.Points = polyline.Coordinates.Select(d => d.ToLatLng()).ToList();
                                }
                                break;
                        }
                    };

                }
            }
            var options = map.AddPolylines(polylineOptions.ToList());
            for (int i = 0; i < options.Count; i++)
            {
                polylines[i].Id = options[i].Id.ToString();
            }
            return options;
        }

        private Sdk.Annotations.Annotation AddAnnotation(Annotation annotation)
        {
            Sdk.Annotations.Annotation options = null;
            if (annotation is PointAnnotation)
            {
                var marker = new MarkerOptions();
                marker.SetTitle(annotation.Title);
                marker.SetSnippet(annotation.Title);
                marker.SetPosition(((PointAnnotation)annotation).Coordinate.ToLatLng());
                var output = Element.GetImageForAnnotationFunc?.Invoke(annotation.Id.ToString());
                if (output?.Item2 is string imgName)
                {
                    try
                    {
                        IconFactory iconFactory = IconFactory.GetInstance(Context);
                        Icon icon = iconFactory.FromResource(Context.Resources.GetIdentifier(imgName, "drawable", Context.PackageName));
                        marker.SetIcon(icon);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("MapRendererAndroid:" + e.Message);
                    }
                }
                options = map.AddMarker(marker);
            }
            else if (annotation is PolylineAnnotation)
            {
                var polyline = annotation as PolylineAnnotation;
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
                            if (_annotationDictionaries.ContainsKey(annotation.Id))
                            {
                                var poly = _annotationDictionaries[annotation.Id] as Polyline;
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
                                _annotationDictionaries.Add(annotation.Id, options);
                            }
                        }
                        else if (e.Action == NotifyCollectionChangedAction.Remove)
                        {
                            if (_annotationDictionaries.ContainsKey(annotation.Id))
                            {
                                var poly = _annotationDictionaries[annotation.Id] as Polyline;
                                poly.Points.Remove(polyline.Coordinates.ElementAt(e.OldStartingIndex).ToLatLng());
                            }
                        }
                    };
                }
            }
            else if (annotation is MultiPolylineAnnotation)
            {
                var polyline = annotation as MultiPolylineAnnotation;
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
                if (annotation.Id != null)
                {
                    if (_annotationDictionaries.ContainsKey(annotation.Id))
                    {
                        _annotationDictionaries[annotation.Id] = options;
                    }
                    else
                    {
                        _annotationDictionaries.Add(annotation.Id, options);
                    }
                }
            }

            return options;
        }

        void RemoveAllAnnotations()
        {
            if (map?.Annotations != null)
            {
                map.RemoveAnnotations(map.Annotations);
            }
        }

        public void OnMapReady(MapboxMap mapBox)
        {
            map = mapBox;
            mapReady = true;
            OnMapRegionChanged();
            map.UiSettings.RotateGesturesEnabled = Element.RotateEnabled;
            map.UiSettings.TiltGesturesEnabled = Element.PitchEnabled;
            RemoveAllAnnotations();
            OnMapRegionChanged();
            if (Element.Center != null)
            {
                FocustoLocation(Element.Center.ToLatLng());
            }
            else
            {
                FocustoLocation(new LatLng(21.0278, 105.8342));
            }

            AddMapEvents();
            SetupFunctions();
            if (Element.MapStyle == null)
            {
                Element.MapStyle = new MapStyle(Sdk.Maps.Style.MAPBOX_STREETS);
            }
            else
            {
                UpdateMapStyle();
            }

            if (Element.InfoWindowTemplate != null)
            {
                var info = new CustomInfoWindowAdapter(Context, Element);
                map.InfoWindowAdapter = info;
            }

            if (Element.Annotations != null)
            {
                AddAnnotations(Element.Annotations.ToArray());
                if (Element.Annotations is INotifyCollectionChanged notifyCollection)
                {
                    notifyCollection.CollectionChanged -= OnAnnotationsCollectionChanged;
                    notifyCollection.CollectionChanged += OnAnnotationsCollectionChanged;
                }
            }
        }

    }

    public static class StringExtensions
    {
        static string CustomPrefix = "NXCustom_";
        public static string ToCustomId(this string str)
        {
            if (str == null) return null;
            return CustomPrefix + str;
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
