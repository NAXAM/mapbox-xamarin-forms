using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Offline;
using GoogleGson;
using Naxam.Controls.Mapbox.Forms;
using Naxam.Mapbox.Platform.Droid.Offline;

[assembly: Xamarin.Forms.Dependency(typeof(Naxam.Controls.Mapbox.Platform.Droid.OfflineStorageService))]
namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public class OfflineStorageService : IOfflineStorageService
    {
        internal static string JSON_CHARSET = "UTF-8";

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
                //var binFormatter = new BinaryFormatter();
                //var mStream = new MemoryStream();
                //binFormatter.Serialize(mStream, packInfo);
                //metadata = mStream.ToArray();
                try {
                    JsonObject jsonObject = new JsonObject();

                    foreach (KeyValuePair<string, string> pair in packInfo)
                    {
                        jsonObject.AddProperty(pair.Key, pair.Value);
                    }
                    var json = new Java.Lang.String(jsonObject.ToString());
                    metadata = json.GetBytes(JSON_CHARSET);
                    System.Diagnostics.Debug.WriteLine("Encoding metadata succeeded: " + metadata.Length.ToString());
                }
                catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine("Failed to encode metadata: " + ex.Message);
                }
            }

            offlineManager.CreateOfflineRegion(definition,
                                              metadata,
                                               new CreateOfflineRegionCallback(
                                                   (reg) =>
                                                  {
                                                      reg.SetDownloadState(OfflineRegion.StateActive);
                                                      tcs.TrySetResult(reg.ToFormsPack());
                                                  },
                                                   (msg) =>
                                                    {
                                                        System.Diagnostics.Debug.WriteLine("[ERROR] Couldn't create offline pack: " + msg);
                                                        tcs.TrySetResult(null);
                                                    }
                                                  ));

            return tcs.Task;
        }

        public Task<OfflinePack[]> GetPacks()
        {
            var tcs = new TaskCompletionSource<OfflinePack[]>();
            offlineManager.ListOfflineRegions(new ListOfflineRegionsCallback()
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
            });
            return tcs.Task;
        }


        public Task<bool> RemovePack(OfflinePack pack)
        {
            var tcs = new TaskCompletionSource<bool>();
            var obj = new Java.Lang.Object(pack.Handle, Android.Runtime.JniHandleOwnership.TransferGlobalRef);
            var region = Android.Runtime.Extensions.JavaCast<OfflineRegion>(obj);
            if (region == null) {
                tcs.TrySetResult(false);

            }
            else region.Delete(new OfflineRegionDeleteCallback(
                () => tcs.TrySetResult(true),
                (msg) =>
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] Couldn't delete offline pack: " + msg);
                    tcs.TrySetResult(false);
                }
            ));
            return tcs.Task;
        }

        public void RequestPackProgress(OfflinePack pack)
        {
            var obj = new Java.Lang.Object(pack.Handle, Android.Runtime.JniHandleOwnership.TransferGlobalRef);
            var region = Android.Runtime.Extensions.JavaCast<OfflineRegion>(obj);
            region?.SetObserver(new OfflineRegionObserver(
                (status) =>
                {
                
                    pack.Progress = new OfflinePackProgress()
                    {
                        CountOfResourcesExpected = (ulong)status.RequiredResourceCount,
                        CountOfResourcesCompleted = (ulong)status.CompletedResourceCount,
                        CountOfTilesCompleted = (ulong)status.CompletedTileCount,
                        CountOfTileBytesCompleted = (ulong)status.CompletedTileSize,
                        CountOfBytesCompleted = (ulong)status.CompletedResourceSize,
                        MaximumResourcesExpected = (ulong)status.RequiredResourceCount
                    };
                    if (status.IsComplete)
                    {
                        pack.State = OfflinePackState.Completed;
                    }
                    else if (status.DownloadState == OfflineRegion.StateActive)
                    {
                        pack.State = OfflinePackState.Active;
                    }
                    else
                    {
                        pack.State = OfflinePackState.Inactive;
                    }

                    OfflinePackProgressChanged?.Invoke(this, new OSSEventArgs()
                    {
                        OfflinePack = pack
                    });
                },
                (error) =>
                {
                    System.Diagnostics.Debug.WriteLine($"[ERROR] {error.Message} {error.Reason}");
                    OfflinePackGotError?.Invoke(pack,
                                                new OSSErrorEventArgs()
                                                {
                                                    ErrorMessage = error.Message
                                                });
                },
                (maximumCount) =>
                {
                    MaximumMapboxTilesReached?.Invoke(this, new OSSMaximumMapboxTilesReachedEventArgs()
                    {
                        OfflinePack = pack,
                        MaximumCount = (ulong)maximumCount
                    });
                }
            ));
        }

        public bool Resume(OfflinePack pack)
        { 
            var obj = new Java.Lang.Object(pack.Handle, Android.Runtime.JniHandleOwnership.TransferGlobalRef);
            var region = Android.Runtime.Extensions.JavaCast<OfflineRegion>(obj);
            if (region == null) return false;
            region.SetDownloadState(OfflineRegion.StateActive);
            return true;
        }

        public bool SuspendPack(OfflinePack pack)
        {
            var obj = new Java.Lang.Object(pack.Handle, Android.Runtime.JniHandleOwnership.TransferGlobalRef);
            var region = Android.Runtime.Extensions.JavaCast<OfflineRegion>(obj);
            if (region == null) return false;
            region.SetDownloadState(OfflineRegion.StateInactive);
            return true;
        }
    }
}
