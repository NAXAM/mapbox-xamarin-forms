using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Com.Mapbox.Mapboxsdk.Snapshotter;
using Sdk = Com.Mapbox.Mapboxsdk;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
    {
        Task<byte[]> TakeMapSnapshot()
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
                if (rendererReference.TryGetTarget(out var renderer))
                {
                    var options = new MapSnapshotter.Options(
                        renderer.Control.MeasuredWidth,
                        renderer.Control.MeasuredHeight
                        ).WithRegion(
                            renderer.map.Projection.VisibleRegion.LatLngBounds
                        ).WithStyle(style.Url);
                    var mapSnapshotter = new MapSnapshotter(renderer.Context, options);
                    mapSnapshotter.Start(new GetStyleSnapshotReadyCallback(tcs));
                }
                else
                {
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
                using (var stream = new MemoryStream())
                {
                    p0.Bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                    tcs.TrySetResult(stream.ToArray());
                }
            }
        }
    }
}