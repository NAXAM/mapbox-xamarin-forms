using System;
using System.Collections.Generic;
using Foundation;
using Mapbox;
using Naxam.Controls.Mapbox.Forms;

namespace Naxam.Controls.Mapbox.Platform.iOS.Extensions
{
    public static class MGLOfflinePackExtensions
    {
        public static OfflinePack ToFormsPack(this MGLOfflinePack mbPack) {
            if (mbPack == null) return null;
            var mbRegion = mbPack.Region;
            var output = new OfflinePack()
            {
                Region = mbPack.Region.ToFormsRegion(),
                Progress = mbPack.Progress.ToFormsProgress(),
                State = (OfflinePackState)mbPack.State,
                Handle = mbPack.Handle
            };

            if (mbPack.Context != null)
            {
                var info = new Dictionary<string, string>();
                NSDictionary userInfo = NSKeyedUnarchiver.UnarchiveObject(mbPack.Context) as NSDictionary;
                foreach (NSObject key in userInfo.Keys)
                {
                    info.Add(key.ToString(), userInfo[key].ToString());
                }
                output.Info = info;
            }
            return output;
        }
    }
}
