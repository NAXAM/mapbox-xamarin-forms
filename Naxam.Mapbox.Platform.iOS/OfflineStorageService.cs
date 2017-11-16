using System;
using System.Collections.Generic;
using Foundation;
using Naxam.Controls.Mapbox.Forms;
using Mapbox;
using System.Threading.Tasks;
using Naxam.Controls.Mapbox.Platform.iOS.Extensions;
using System.Linq;
using ObjCRuntime;

[assembly: Xamarin.Forms.Dependency(typeof(Naxam.Controls.Mapbox.Platform.iOS.OfflineStorageService))]
namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public class OfflineStorageService : NSObject, IOfflineStorageService
    {
        Dictionary<nuint, OfflinePack> tempPacks;
        IOfflineStorageDelegate downloadDelegate;

        public OfflineStorageService()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(MGLOfflinePackKeys.ProgressChangedNotification, OnOfflinePackProgressChanged, null);
            NSNotificationCenter.DefaultCenter.AddObserver(MGLOfflinePackKeys.ErrorNotification, OnOfflinePackError, null);
            NSNotificationCenter.DefaultCenter.AddObserver(MGLOfflinePackKeys.MaximumMapboxTilesReachedNotification, OnMaximumMapboxTilesReached, null);

            tempPacks = new Dictionary<nuint, OfflinePack>();
        }

        protected override void Dispose(bool disposing)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(this);
            base.Dispose(disposing);
        }

        private void OnOfflinePackError(NSNotification notification)
        {
            if (downloadDelegate == null)
            {
                return;
            }
            MGLOfflinePack pack = notification.Object as MGLOfflinePack;
            NSError error = notification.UserInfo[MGLOfflinePackKeys.UserInfoKeyError] as NSError;
            OfflinePack formsPack;
            var hash = pack.GetNativeHash();
            if (tempPacks.ContainsKey(hash))
            {
                formsPack = tempPacks[hash];
                formsPack.State = (OfflinePackState)pack.State;
                tempPacks.Remove(hash);
            }
            else
            {
                formsPack = pack.ToFormsPack();
            }
            downloadDelegate.OfflinePackDidGetError(formsPack, error.LocalizedFailureReason);
        }

        private void OnMaximumMapboxTilesReached(NSNotification notification)
        {
            if (downloadDelegate == null)
            {
                return;
            }
            MGLOfflinePack pack = notification.Object as MGLOfflinePack;
          
            var maximumCount = notification.UserInfo[MGLOfflinePackKeys.UserInfoKeyMaximumCount] as NSNumber;
            var hash = pack.GetNativeHash();
            OfflinePack formsPack;
            if (tempPacks.ContainsKey(hash)) {
                formsPack = tempPacks[hash];
                formsPack.State = (OfflinePackState)pack.State;
                tempPacks.Remove(hash);
            }
            else {
                formsPack = pack.ToFormsPack();
            }
            downloadDelegate.MaximumMapboxTilesWasReached(formsPack, maximumCount.UInt64Value);
        }

        private void OnOfflinePackProgressChanged(NSNotification notification)
        {
            if (downloadDelegate == null)
            {
                return;
            }

            MGLOfflinePack pack = notification.Object as MGLOfflinePack;
            var hash = pack.GetNativeHash();
            var completed = pack.State == MGLOfflinePackState.Complete || (pack.Progress.countOfResourcesExpected == pack.Progress.countOfResourcesCompleted);
            OfflinePack formsPack;
            if (tempPacks.ContainsKey(hash))
            {
                formsPack = tempPacks[hash];
                formsPack.Progress = pack.Progress.ToFormsProgress();
                formsPack.State = (OfflinePackState)pack.State;
                if (completed) {
                    tempPacks.Remove(hash);
                }
            }
            else
            {
                formsPack = pack.ToFormsPack();
                if (!completed) {
                    tempPacks.Add(hash, formsPack);
                }
            }
            downloadDelegate.OfflinePackProgressDidChange(formsPack);
        }

        public Task<OfflinePack> DownloadMap(OfflinePackRegion formsRegion, Dictionary<string, string> packInfo, IOfflineStorageDelegate downloadDelegate = null)
        {
            this.downloadDelegate = downloadDelegate;
            var tsc = new TaskCompletionSource<OfflinePack>();
            var region = new MGLTilePyramidOfflineRegion(
                new NSUrl(formsRegion.StyleURL),
                new MGLCoordinateBounds()
                {
                    sw = TypeConverter.FromPositionToCoordinate(formsRegion.Bounds.SouthWest),
                    ne = TypeConverter.FromPositionToCoordinate(formsRegion.Bounds.NorthEast)
                },
                formsRegion.MinimumZoomLevel,
                formsRegion.MaximumZoomLevel);
            NSData context = null;
            if (packInfo != null) {
                var keys = new List<NSString>();
                var values = new List<NSString>();
                foreach (string key in packInfo.Keys) {
                    keys.Add((NSString) key);
                    values.Add((NSString) packInfo[key]);
                }
                var userInfo = NSDictionary.FromObjectsAndKeys(keys.ToArray(), values.ToArray());
                context = NSKeyedArchiver.ArchivedDataWithRootObject(userInfo);
            }

            MGLOfflineStorage.SharedOfflineStorage().AddPackForRegion(region, context, (pack, error) =>
            {
                if (error != null) {
                    System.Diagnostics.Debug.WriteLine("Couldn't create offline pack: " + error.LocalizedFailureReason);
                    tsc.TrySetResult(null);
                }
                else {
                    pack.Resume();
                    var formsPack = pack.ToFormsPack();
                    tempPacks.Add(pack.GetNativeHash(), formsPack);
                    tsc.TrySetResult(formsPack);
                }
            });

            return tsc.Task;
        }

        public OfflinePack[] GetPacks()
        {
            var packs = MGLOfflineStorage.SharedOfflineStorage().Packs;
            return packs?.Select((arg) => arg.ToFormsPack()).ToArray();
        }

        public Task<bool> RemovePack(OfflinePack pack)
        {
            var tsc = new TaskCompletionSource<bool>();
            try {
                var mbPack = Runtime.GetNSObject<MGLOfflinePack>(pack.Handle);
                MGLOfflineStorage.SharedOfflineStorage().RemovePack(mbPack, (error) =>
                {
                    if (error == null) {
                        tsc.TrySetResult(true);
                    }
                    else {
                        System.Diagnostics.Debug.WriteLine("Removing offline pack failed: " + error.LocalizedFailureReason);
                        tsc.TrySetResult(false);
                    }
                });
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[Exception]: " + ex.Message);
                tsc.TrySetResult(false);
            }
            return tsc.Task;
        }

        public bool Resume(OfflinePack pack)
        {
            try
            {
                var mbPack = Runtime.GetNSObject<MGLOfflinePack>(pack.Handle);
                mbPack.Resume();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Exception]: " + ex.Message);
                return false;
            }
        }
    }
}
