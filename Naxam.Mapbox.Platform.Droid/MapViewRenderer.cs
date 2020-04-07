using System;
using Android.Content;
using AndroidX.AppCompat.App;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using Naxam.Controls.Forms;
using Xamarin.Forms.Platform.Android;
using MapView = Naxam.Controls.Forms.MapView;
using View = Android.Views.View;
using NxLatLng = Naxam.Mapbox.LatLng;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer : ViewRenderer<MapView, View>, IOnMapReadyCallback
    {
        protected MapboxMap map;
        protected MapViewFragment fragment;
        NxLatLng currentCamera;
        bool mapReady;

        public MapViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                e.OldElement.AnnotationsChanged -= Element_AnnotationsChanged;
                e.OldElement.Functions = null;

                if (map != null)
                {
                    RemoveMapEvents();
                }
            }

            if (e.NewElement == null)
                return;

            if (Control == null)
            {
                var activity = (AppCompatActivity) Context;
                var view = new Android.Widget.FrameLayout(activity)
                {
                    Id = GenerateViewId()
                };

                SetNativeControl(view);

                fragment = MapViewFragment.Create(Element, Context);

                activity.SupportFragmentManager
                    .BeginTransaction()
                    .Replace(view.Id, fragment)
                    .CommitAllowingStateLoss();

                fragment.GetMapAsync(this);
                currentCamera = new NxLatLng();
            }
        }

        protected override void Dispose(bool disposing)
        {
            RemoveMapEvents();
            Element.Functions = null;

            if (fragment != null)
            {
                if (fragment.StateSaved)
                {
                    var activity = (AppCompatActivity) Context;
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
            if (map == null || mapReady == false)
            {
                return;
            }

            var builder = new CameraPosition.Builder()
                .Target(latLng);

            if (Element.ZoomLevel.HasValue &&
                Math.Abs(Element.ZoomLevel.Value - map.CameraPosition.Zoom) > double.Epsilon)
            {
                builder.Zoom(Element.ZoomLevel.Value);
            }

            map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(builder.Build()), 1000);
        }

        protected override void OnElementPropertyChanged(object sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == MapView.VisibleBoundsProperty.PropertyName)
            {
                OnMapRegionChanged();
            }
            else if (e.PropertyName == MapView.CenterProperty.PropertyName)
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
                if (Element.ZoomLevel.HasValue == false)
                {
                    return;
                }

                var dif = Math.Abs(map.CameraPosition.Zoom - Element.ZoomLevel.Value);
                System.Diagnostics.Debug.WriteLine(
                    $"Current zoom: {map.CameraPosition.Zoom} - New zoom: {Element.ZoomLevel}");
                if (dif >= double.Epsilon && cameraBusy == false)
                {
                    System.Diagnostics.Debug.WriteLine("Updating zoom level");
                    map.AnimateCamera(CameraUpdateFactory.ZoomTo(Element.ZoomLevel.Value));
                }
            }
            else if (e.PropertyName == MapView.AnnotationsProperty.PropertyName)
            {
            }
            else if (e.PropertyName == MapView.RenderTextureModeProperty.PropertyName)
            {
                // TODO Set RenderTextureModeProperty
            }
            else if (e.PropertyName == MapView.RenderTextureTranslucentSurfaceProperty.PropertyName)
            {
                // TODO Set RenderTextureTranslucentSurfaceProperty
            }
        }

        void OnMapRegionChanged()
        {
            //if (false == Element.VisibleBounds.IsEmpty())
            //{
            //    map?.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(
            //        LatLngBounds.From(
            //            Element.VisibleBounds.NorthEast.Lat,
            //            Element.VisibleBounds.NorthEast.Long,
            //            Element.VisibleBounds.SouthWest.Lat,
            //            Element.VisibleBounds.SouthWest.Long
            //        ), 0));
            //}
        }

        public void OnMapReady(MapboxMap mapBox)
        {
            map = mapBox;
            mapReady = true;
            
            AddMapEvents();

            if (Element.MapStyle == null)
            {
                Element.MapStyle = new MapStyle(Style.MAPBOX_STREETS);
            }
            else
            {
                UpdateMapStyle();
            }

            if (Element.ZoomLevel.HasValue)
            {
                FocustoLocation(Element.Center.ToLatLng());
            }
        }
    }
}