using Com.Mapbox.Mapboxsdk.Offline;
using Naxam.Controls.Mapbox.Forms;

namespace Naxam.Mapbox.Platform.Droid.Offline
{
    public static class OfflinePackExtensions
    {
        public static OfflinePack ToFormsPack(this OfflineRegion mbRegion)
        {
            if (mbRegion == null) return null;
            var output = new OfflinePack()
            {
                Handle = mbRegion.Handle
            };
            var definition = mbRegion.Definition;
            if (definition is OfflineTilePyramidRegionDefinition def) {
                output.Region = def.ToFormsRegion();
            }

            //var mbRegion = mbPack.Region;
            //var region = ObjCRuntime.Runtime.GetINativeObject<MGLTilePyramidOfflineRegion>(mbRegion.Handle, false);
            //output.Region = region?.ToFormsRegion();
            //if (mbPack.Context != null)
            //{
            //    var info = new Dictionary<string, string>();
            //    NSDictionary userInfo = NSKeyedUnarchiver.UnarchiveObject(mbPack.Context) as NSDictionary;
            //    foreach (NSObject key in userInfo.Keys)
            //    {
            //        info.Add(key.ToString(), userInfo[key].ToString());
            //    }
            //    output.Info = info;
            //}
            return output;
        }

        public static OfflinePackRegion ToFormsRegion(this OfflineTilePyramidRegionDefinition definition) {
            return new OfflinePackRegion()
            {
                Bounds = new CoordinateBounds() {
                    NorthEast = new Position(definition.Bounds.NorthEast.Latitude, definition.Bounds.NorthEast.Longitude),
                    SouthWest = new Position(definition.Bounds.SouthWest.Latitude, definition.Bounds.SouthWest.Longitude)
                },
                StyleURL = definition.StyleURL,
                MinimumZoomLevel = definition.MinZoom,
                MaximumZoomLevel = definition.MaxZoom
            };
        }
    }
}
