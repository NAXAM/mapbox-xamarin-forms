using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Naxam.Controls.Mapbox.Forms
{
    public class OSSEventArgs : EventArgs
    {
        public OfflinePack OfflinePack;
    }

    public class OSSErrorEventArgs: OSSEventArgs
    {
        public string ErrorMessage;
    }

    public class OSSMaximumMapboxTilesReachedEventArgs: OSSEventArgs
    {
        public ulong MaximumCount;
    }

    public interface IOfflineStorageService {
        event EventHandler<OSSEventArgs> OfflinePackProgressChanged;
        event EventHandler<OSSErrorEventArgs> OfflinePackGotError;
        event EventHandler<OSSMaximumMapboxTilesReachedEventArgs> MaximumMapboxTilesReached;

        Task<OfflinePack> DownloadMap(OfflinePackRegion region, Dictionary<string, string> packInfo);
        OfflinePack[] GetPacks();
        bool Resume(OfflinePack pack);
        Task<bool> RemovePack(OfflinePack pack);
        bool SuspendPack(OfflinePack pack);
        void RequestPackProgress(OfflinePack pack);
    }
}