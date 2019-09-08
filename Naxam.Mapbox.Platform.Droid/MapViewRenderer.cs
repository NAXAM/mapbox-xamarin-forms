using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.App;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using Naxam.Controls.Forms;
using Naxam.Mapbox.Platform.Droid;
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
        private const int SIZE_ZOOM = 13;

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

        protected override void Dispose(bool disposing)
        {
            RemoveMapEvents();
            Element.Functions = null;

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
                var dif = Math.Abs(map.CameraPosition.Zoom - Element.ZoomLevel);
                System.Diagnostics.Debug.WriteLine($"Current zoom: {map.CameraPosition.Zoom} - New zoom: {Element.ZoomLevel}");
                if (dif >= 0.01 && cameraBusy == false)
                {
                    System.Diagnostics.Debug.WriteLine("Updating zoom level");
                    map.AnimateCamera(CameraUpdateFactory.ZoomTo(Element.ZoomLevel));
                }
            }
            else if (e.PropertyName == MapView.AnnotationsProperty.PropertyName)
            {

            }
        }

        void OnMapRegionChanged()
        {
            if (false == Element.VisibleBounds.IsEmpty())
            {
                map?.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(
                    LatLngBounds.From(
                        Element.VisibleBounds.NorthEast.Lat,
                        Element.VisibleBounds.NorthEast.Long,
                        Element.VisibleBounds.SouthWest.Lat,
                        Element.VisibleBounds.SouthWest.Long
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

            if (Element.MapStyle == null)
            {
                Element.MapStyle = new MapStyle(Style.MAPBOX_STREETS);
            }
            else
            {
                UpdateMapStyle();
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
