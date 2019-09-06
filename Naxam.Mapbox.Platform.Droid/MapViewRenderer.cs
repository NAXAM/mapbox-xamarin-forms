using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Content;
using Android.Support.V7.App;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using Naxam.Controls.Forms;
using Naxam.Mapbox.Platform.Droid;
using Xamarin.Forms.Platform.Android;
using Bitmap = Android.Graphics.Bitmap;
using MapView = Naxam.Controls.Forms.MapView;
using Sdk = Com.Mapbox.Mapboxsdk;
using View = Android.Views.View;
using NxLatLng = Naxam.Mapbox.LatLng;
using NxLatLngBounds = Naxam.Mapbox.LatLngBounds;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer : ViewRenderer<MapView, View>, IOnMapReadyCallback
    {
        protected MapboxMap map;
        protected MapViewFragment fragment;
        private const int SIZE_ZOOM = 13;

        NxLatLng currentCamera;
        bool mapReady;

        readonly Dictionary<string, Annotation> _annotationDictionaries;
        public MapViewRenderer(Context context) : base(context)
        {
            _annotationDictionaries = new Dictionary<string, Annotation>();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                e.OldElement.AnnotationsChanged -= Element_AnnotationsChanged;
                e.OldElement.TakeSnapshotFunc -= TakeMapSnapshot;
                //e.OldElement.GetFeaturesAroundPointFunc -= GetFeaturesAroundPoint;
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

                activity.SupportFragmentManager
                    .BeginTransaction()
                    .Replace(view.Id, fragment)
                    .CommitAllowingStateLoss();

                fragment.GetMapAsync(this);
                currentCamera = new NxLatLng();
            }
        }

        public virtual void SetupFunctions()
        {
            Element.TakeSnapshotFunc += TakeMapSnapshot;
            //Element.GetFeaturesAroundPointFunc += GetFeaturesAroundPoint;
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

            //Element.UpdateShapeOfSourceFunc = (Annotation annotation, string sourceId) =>
            //{
            //    if (annotation != null && !string.IsNullOrEmpty(sourceId))
            //    {
            //        var shape = annotation.ToFeatureCollection();
            //        var source = map.Style.GetSource(sourceId.Prefix()) as Sdk.Style.Sources.GeoJsonSource;
            //        if (source != null)
            //        {
            //            Device.BeginInvokeOnMainThread(() =>
            //            {
            //                source.SetGeoJson(shape);
            //            });
            //            if (Element.MapStyle.CustomSources?.FirstOrDefault((arg) => arg.Id == sourceId) is ShapeSource shapeSource)
            //            {
            //                shapeSource.Shape = annotation;
            //            }
            //            return true;
            //        }
            //    }
            //    return false;
            //};

            Element.ReloadStyleAction = () =>
            {
                //https://github.com/mapbox/mapbox-gl-native/issues/9511
                map.SetStyle(map.Style.Url);
            };

            Element.UpdateViewPortAction = (NxLatLng centerLocation, double? zoomLevel, double? bearing, bool animated, Action completionHandler) =>
            {
                var tartget = centerLocation != NxLatLng.Zero ? new LatLng(centerLocation.Lat, centerLocation.Long) : map.CameraPosition.Target;
                var newPosition = new CameraPosition.Builder()
                                                    .Bearing(bearing ?? map.CameraPosition.Bearing)
                                                    .Target(tartget)
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

        //IFeature[] GetFeaturesAroundPoint(Point point, double radius, string[] layers)
        //{
        //    var output = new List<IFeature>();
        //    RectF rect = point.ToRect(Context.ToPixels(radius));
        //    var listFeatures = map.QueryRenderedFeatures(rect, layers);
        //    return listFeatures.Select(x => x.ToFeature())
        //                       .Where(x => x != null)
        //                       .ToArray();
        //}

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
                FocustoLocation(new LatLng(Element.Center.Lat, Element.Center.Long));
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
            if (false == Element.Region.IsEmpty())
            {
                map?.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(
                    LatLngBounds.From(
                        Element.Region.NorthEast.Lat,
                        Element.Region.NorthEast.Long,
                        Element.Region.SouthWest.Lat,
                        Element.Region.SouthWest.Long
                    ), 0));
            }
        }

        public void OnMapReady(MapboxMap mapBox)
        {
            map = mapBox;
            mapReady = true;
            OnMapRegionChanged();
            map.UiSettings.RotateGesturesEnabled = Element.RotateEnabled;
            map.UiSettings.TiltGesturesEnabled = Element.PitchEnabled;
            //RemoveAllAnnotations();
            OnMapRegionChanged();
            if (Element.Center != NxLatLng.Zero)
            {
                FocustoLocation(new LatLng(Element.Center.Lat, Element.Center.Long));
            }
            else
            {
                FocustoLocation(new LatLng(21.0278, 105.8342));
            }

            AddMapEvents();
            SetupFunctions();
            if (Element.MapStyle == null)
            {
                Element.MapStyle = new MapStyle(Style.MAPBOX_STREETS);
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
