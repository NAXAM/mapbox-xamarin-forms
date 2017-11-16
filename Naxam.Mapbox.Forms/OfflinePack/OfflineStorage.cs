using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Naxam.Controls.Mapbox.Forms
{
    public interface IOfflineStorageDelegate {
        void OfflinePackProgressDidChange(OfflinePack pack);
        void OfflinePackDidGetError(OfflinePack pack, string errorMessage);
        void MaximumMapboxTilesWasReached(OfflinePack pack, ulong maximumCount);
    }

    public interface IOfflineStorageService {
        Task<OfflinePack> DownloadMap(OfflinePackRegion region, Dictionary<string, string> packInfo, IOfflineStorageDelegate downloadDelegate = null);
        OfflinePack[] GetPacks();
        bool Resume(OfflinePack pack);
        Task<bool> RemovePack(OfflinePack pack);
    }
}