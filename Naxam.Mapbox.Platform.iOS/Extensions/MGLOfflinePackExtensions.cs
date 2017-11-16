using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Foundation;
using Mapbox;
using Naxam.Controls.Mapbox.Forms;
using ObjCRuntime;
namespace Naxam.Controls.Mapbox.Platform.iOS.Extensions
{
    public static class MGLOfflinePackExtensions
    {
        public static OfflinePack ToFormsPack(this MGLOfflinePack mbPack) {
            if (mbPack == null) return null;
            var output = new OfflinePack()
            {
                Progress = mbPack.Progress.ToFormsProgress(),
                State = (OfflinePackState)mbPack.State,
                Handle = mbPack.Handle
            };
            var mbRegion = mbPack.Region;
            var region = ObjCRuntime.Runtime.GetINativeObject<MGLTilePyramidOfflineRegion>(mbRegion.Handle, false);
            output.Region = region?.ToFormsRegion();
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
