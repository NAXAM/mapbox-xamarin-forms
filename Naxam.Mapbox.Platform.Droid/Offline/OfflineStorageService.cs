using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Offline;
using Naxam.Controls.Mapbox.Forms;
using Naxam.Mapbox.Platform.Droid.Offline;

[assembly: Xamarin.Forms.Dependency(typeof(Naxam.Controls.Mapbox.Platform.Droid.OfflineStorageService))]
namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public class OfflineStorageService : IOfflineStorageService
    {
        OfflineManager offlineManager;
        List<TaskCompletionSource<OfflinePack>> RunningTasks;

        public OfflineStorageService()
        {
            offlineManager = OfflineManager.GetInstance(Android.App.Application.Context);
            RunningTasks = new List<TaskCompletionSource<OfflinePack>>();
        }

        public event EventHandler<OSSEventArgs> OfflinePackProgressChanged;
        public event EventHandler<OSSErrorEventArgs> OfflinePackGotError;
        public event EventHandler<OSSMaximumMapboxTilesReachedEventArgs> MaximumMapboxTilesReached;


        public Task<OfflinePack> DownloadMap(OfflinePackRegion region, Dictionary<string, string> packInfo)
        {
            var tcs = new TaskCompletionSource<OfflinePack>();
            //RunningTasks.Add(tcs);
            LatLngBounds latLngBounds = new LatLngBounds.Builder()
                                                        .Include(new LatLng(region.Bounds.NorthEast.Lat, region.Bounds.NorthEast.Long)) // Northeast 
                                                        .Include(new LatLng(region.Bounds.SouthWest.Lat, region.Bounds.SouthWest.Long)) // Southwest 
                                                        .Build();
            OfflineTilePyramidRegionDefinition definition = new OfflineTilePyramidRegionDefinition(
                region.StyleURL,
                latLngBounds,
                region.MinimumZoomLevel,
                region.MaximumZoomLevel,
                Android.App.Application.Context.Resources.DisplayMetrics.Density);
            byte[] metadata = null;
            if (packInfo != null)
            {
                var binFormatter = new BinaryFormatter();
                var mStream = new MemoryStream();
                binFormatter.Serialize(mStream, packInfo);
                metadata = mStream.ToArray();
            }
            using (var callback = new CreateOfflineRegionCallback()
            {
                OnErrorHandle = ((msg) =>
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] Couldn't create offline pack: " + msg);
                    tcs.TrySetResult(null);
                }),
                OnCreateHandle = ((reg) =>
                {
                    reg.SetDownloadState(OfflineRegion.StateActive);
                    tcs.TrySetResult(reg.ToFormsPack());
                })
            }
                  )
            {
                offlineManager.CreateOfflineRegion(definition,
                                              metadata,
                                                   callback);
            }


            return tcs.Task;
        }

        public Task<OfflinePack[]> GetPacks()
        {
            var tcs = new TaskCompletionSource<OfflinePack[]>();
            using (var callback = new ListOfflineRegionsCallback()
            {
                OnErrorHandle = ((msg) =>
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] Couldn't get offline packs: " + msg);
                    tcs.TrySetResult(null);
                }),
                OnListHandle = ((regs) =>
                {
                    var output = regs.Select((reg) => reg.ToFormsPack());
                    tcs.TrySetResult(output.ToArray());
                })
            })
                return tcs.Task;
        }


        public Task<bool> RemovePack(OfflinePack pack)
        {
            var tcs = new TaskCompletionSource<bool>();
            var obj = new Java.Lang.Object(pack.Handle, Android.Runtime.JniHandleOwnership.TransferLocalRef);
            var region = Android.Runtime.Extensions.JavaCast<OfflineRegion>(obj);
            using (var callback = new OfflineRegionDeleteCallback()
            {
                OnErrorHandle = ((msg) =>
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] Couldn't delete offline pack: " + msg);
                    tcs.TrySetResult(false);
                }),
                OnDeleteHandle = () => tcs.TrySetResult(true)
            })
            return tcs.Task;
        }

        public void RequestPackProgress(OfflinePack pack)
        {
            throw new NotImplementedException();
        }

        public bool Resume(OfflinePack pack)
        {
            throw new NotImplementedException();
        }

        public bool SuspendPack(OfflinePack pack)
        {
            throw new NotImplementedException();
        }
    }
}
