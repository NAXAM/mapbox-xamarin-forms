using System.Collections.Generic;
namespace Naxam.Controls.Forms
{
    public enum TileSourceOption
    {
        MinimumZoomLevel,
        MaximumZoomLevel,
        AttributionHTMLString,
        AttributionInfos,
        TileCoordinateSystem,
    }

    public enum TileCoordinateSystem
    {
        TileCoordinateSystemXYZ = 0,
        TileCoordinateSystemTMS
    }

    public class AttributionInfo
    {
        public AttributionInfo(string title, string url)
        {
            Title = title;
            Url = url;
        }

        public string Title
        {
            get;
            private set;
        }

        public string Url
        {
            get;
            private set;
        }
    }

    public class RasterSource : MapSource
    {
        public RasterSource(string id, string configurationURL, double tileSize = -1) : base(id)
        {
            ConfigurationURL = configurationURL;
            TileSize = tileSize;
        }

        public RasterSource(string id, string[] tileURLTemplates, Dictionary<TileSourceOption, object> options = null) : base(id)
        {
            TileURLTemplates = tileURLTemplates;
        }

        public string ConfigurationURL
        {
            get;
            private set;
        }

        public double TileSize
        {
            get;
            private set;
        }

        public string[] TileURLTemplates
        {
            get;
            private set;
        }

        public Dictionary<TileSourceOption, object> Options
        {
            get;
            private set;
        }
    }
}
