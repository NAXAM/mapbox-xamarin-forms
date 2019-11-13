using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Snapshotter;
using Sdk = Com.Mapbox.Mapboxsdk;
using NxCameraPosition = Naxam.Mapbox.CameraPosition;
using NxLatLngBounds = Naxam.Mapbox.LatLngBounds;
using Naxam.Mapbox.Platform.Droid.Extensions;
using Naxam.Mapbox;
using Xamarin.Forms.Platform.Android;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer : Naxam.Mapbox.IMapFunctions
    {
        public void AnimateCamera(Naxam.Mapbox.ICameraUpdate camera, int durationInMillisecond)
        {
            var cameraUpdate = camera switch
            {
                NxCameraPosition cp => CameraUpdateFactory.NewCameraPosition(cp.ToNative()),
                CameraBounds cb => CameraUpdateFactory.NewLatLngBounds(
                    cb.Bounds.ToLatLngBounds(),
                    cb.Bearing ?? 0, cb.Tilt ?? 0,
                    (int)Context.ToPixels(cb.Padding.Left), (int)Context.ToPixels(cb.Padding.Top),
                    (int)Context.ToPixels(cb.Padding.Right), (int)Context.ToPixels(cb.Padding.Bottom)),
                _ => null
            };

            if (cameraUpdate == null) return;

            map.AnimateCamera(cameraUpdate, durationInMillisecond);
        }

        public Task<byte[]> TakeSnapshotAsync(NxLatLngBounds bounds = default)
        {
            var tcs = new TaskCompletionSource<byte[]>();

            map.GetStyle(new GetStyleLoadedCallback(
                new WeakReference<MapViewRenderer>(this),
                tcs
                ));

            return tcs.Task;
        }

        class GetStyleLoadedCallback : Java.Lang.Object, Sdk.Maps.Style.IOnStyleLoaded
        {
            readonly WeakReference<MapViewRenderer> rendererReference;
            readonly TaskCompletionSource<byte[]> tcs;

            public GetStyleLoadedCallback(WeakReference<MapViewRenderer> rendererReference, TaskCompletionSource<byte[]> tcs)
            {
                this.rendererReference = rendererReference;
                this.tcs = tcs;
            }

            public void OnStyleLoaded(Sdk.Maps.Style style)
            {
                if (rendererReference.TryGetTarget(out var renderer)) {
                    var options = new MapSnapshotter.Options(
                        renderer.Control.MeasuredWidth,
                        renderer.Control.MeasuredHeight
                        ).WithRegion(
                            renderer.map.Projection.VisibleRegion.LatLngBounds
                        ).WithStyle(style.Url);
                    var mapSnapshotter = new MapSnapshotter(renderer.Context, options);
                    mapSnapshotter.Start(new GetStyleSnapshotReadyCallback(tcs));
                }
                else {
                    tcs.TrySetCanceled();
                }
            }
        }

        class GetStyleSnapshotReadyCallback : Java.Lang.Object, Sdk.Snapshotter.MapSnapshotter.ISnapshotReadyCallback
        {
            readonly TaskCompletionSource<byte[]> tcs;

            public GetStyleSnapshotReadyCallback(TaskCompletionSource<byte[]> tcs)
            {
                this.tcs = tcs;
            }

            public void OnSnapshotReady(MapSnapshot p0)
            {
                using (var stream = new MemoryStream()) {
                    p0.Bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                    tcs.TrySetResult(stream.ToArray());
                }
            }
        }
    }
}