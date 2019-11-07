using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Offline;
using GoogleGson;
using Naxam.Controls.Forms;
using Naxam.Mapbox.Platform.Droid.Offline;

[assembly: Xamarin.Forms.Dependency(typeof(Naxam.Controls.Mapbox.Platform.Droid.OfflineStorageService))]
namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public class OfflineStorageService : IOfflineStorageService
    {
        internal static string JSON_CHARSET = "UTF-8";

        OfflineManager offlineManager;

        public OfflineStorageService()
        {
            offlineManager = OfflineManager.GetInstance(Android.App.Application.Context);
        }

        public event EventHandler<OSSEventArgs> OfflinePackProgressChanged;
        public event EventHandler<OSSErrorEventArgs> OfflinePackGotError;
        public event EventHandler<OSSMaximumMapboxTilesReachedEventArgs> MaximumMapboxTilesReached;


        public Task<OfflinePack> DownloadMap(OfflinePackRegion region, Dictionary<string, string> packInfo)
        {
            var tcs = new TaskCompletionSource<OfflinePack>();
            var latLngBounds = new LatLngBounds.Builder()
                                                        .Include(region.Bounds.NorthEast.ToLatLng()) // Northeast 
                                                        .Include(region.Bounds.SouthWest.ToLatLng()) // Southwest 
                                                        .Build();
            var definition = new OfflineTilePyramidRegionDefinition(
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
                try
                {
                    var jsonObject = new JsonObject();

                    foreach (KeyValuePair<string, string> pair in packInfo)
                    {
                        jsonObject.AddProperty(pair.Key, pair.Value);
                    }
                    var json = new Java.Lang.String(jsonObject.ToString());
                    metadata = json.GetBytes(JSON_CHARSET);
                    System.Diagnostics.Debug.WriteLine("Encoding metadata succeeded: " + metadata.Length.ToString());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to encode metadata: " + ex.Message);
                }
            }

            offlineManager.CreateOfflineRegion(
                definition,
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
                )
            );

            return tcs.Task;
        }

        public Task<OfflinePack[]> GetPacks()
        {
            var tcs = new TaskCompletionSource<OfflinePack[]>();
            offlineManager.ListOfflineRegions(new ListOfflineRegionsCallback()
            {
                OnErrorHandle = (msg) =>
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] Couldn't get offline packs: " + msg);
                    tcs.TrySetResult(null);
                },
                OnListHandle = (regs) =>
                {
                    var output = regs.Select((reg) => reg.ToFormsPack());
                    tcs.TrySetResult(output.ToArray());
                }
            });
            return tcs.Task;
        }

        Task<OfflineRegion[]> GetRegions()
        {
            var tcs = new TaskCompletionSource<OfflineRegion[]>();
            offlineManager.ListOfflineRegions(new ListOfflineRegionsCallback()
            {
                OnErrorHandle = ((msg) =>
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] Couldn't get offline packs: " + msg);
                    tcs.TrySetResult(null);
                }),
                OnListHandle = ((regs) =>
                {
                    tcs.TrySetResult(regs);
                })
            });
            return tcs.Task;
        }

        public async Task<bool> RemovePack(OfflinePack pack)
        {
            var tcs = new TaskCompletionSource<bool>();
            var region = await GetRegionByPack(pack);
            if (region == null)
            {
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
            return await tcs.Task;
        }

        private async Task<OfflineRegion> GetRegionByPack(OfflinePack pack)
        {
            var regions = await GetRegions();
            var region = regions.FirstOrDefault(d => d.ID == pack.Id);
            return region;
        }

        public async void RequestPackProgress(OfflinePack pack)
        {
            var regions = await GetRegions();
            var region = regions.FirstOrDefault(d => d.ID == pack.Id);
            if (region == null)
                return;
            region.SetDownloadState(OfflineRegion.StateActive);
            region.SetObserver(new OfflineRegionObserver(
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
                    OfflinePackGotError?.Invoke(this,
                                                new OSSErrorEventArgs()
                                                {
                                                    OfflinePack = pack,
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

        public async Task<bool> Resume(OfflinePack pack)
        {
            var region = await GetRegionByPack(pack);
            if (region == null) return false;
            region.SetDownloadState(OfflineRegion.StateActive);
            return true;
        }

        public async Task<bool> SuspendPack(OfflinePack pack)
        {
            var region = await GetRegionByPack(pack);
            if (region == null) return false;
            region.SetDownloadState(OfflineRegion.StateInactive);
            return true;
        }

        public Task<bool> Sideload(string filePath)
        {
            var tcs = new TaskCompletionSource<bool>();

            offlineManager.MergeOfflineRegions(filePath, new MergeOfflineRegionsCallback(tcs));

            return tcs.Task;
        }

        class MergeOfflineRegionsCallback : Java.Lang.Object, OfflineManager.IMergeOfflineRegionsCallback
        {
            readonly WeakReference<TaskCompletionSource<bool>> tcsRef;

            public MergeOfflineRegionsCallback(TaskCompletionSource<bool> tcs)
            {
                tcsRef = new WeakReference<TaskCompletionSource<bool>>(tcs);
            }

            public void OnError(string p0)
            {
                if (tcsRef.TryGetTarget(out var tcs))
                {
                    tcs.TrySetException(new Exception(p0));
                }
            }

            public void OnMerge(OfflineRegion[] p0)
            {
                if (tcsRef.TryGetTarget(out var tcs))
                {
                    tcs.TrySetResult(true);
                }
            }
        }
    }
}
