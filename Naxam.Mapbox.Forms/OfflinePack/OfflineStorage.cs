using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Naxam.Controls.Mapbox.Forms
{
    public class OSSEventArgs : EventArgs
    {
        public OfflinePack OfflinePack { get; set; }
    }

    public class OSSErrorEventArgs : OSSEventArgs
    {
        public string ErrorMessage { get; set; }
    }

    public class OSSMaximumMapboxTilesReachedEventArgs : OSSEventArgs
    {
        public ulong MaximumCount { get; set; }
    }

    public interface IOfflineStorageService
    {

        event EventHandler<OSSEventArgs> OfflinePackProgressChanged;
        event EventHandler<OSSErrorEventArgs> OfflinePackGotError;
        event EventHandler<OSSMaximumMapboxTilesReachedEventArgs> MaximumMapboxTilesReached;

        Task<OfflinePack> DownloadMap(OfflinePackRegion region, Dictionary<string, string> packInfo);
        Task<OfflinePack[]> GetPacks();
        Task<bool> Resume(OfflinePack pack);
        Task<bool> RemovePack(OfflinePack pack);
        Task<bool> SuspendPack(OfflinePack pack);
        void RequestPackProgress(OfflinePack pack);
    }
}