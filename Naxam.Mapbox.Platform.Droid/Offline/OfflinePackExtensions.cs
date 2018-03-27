using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
            if (mbRegion.GetMetadata() is byte[] metadata) {
                var mStream = new MemoryStream();
                var binFormatter = new BinaryFormatter();

                mStream.Write(metadata, 0, metadata.Length);
                mStream.Position = 0;

                output.Info = binFormatter.Deserialize(mStream) as Dictionary<string, string>;
            }
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
