using System;
using System.Collections.Generic;
using System.Text;

namespace Naxam.Mapbox.Sources
{
    public class VectorSource : Source
    {
        public string Url { get; set; }
        public TileSet TileSet { get; set; }
        public TileSourceOptions Options { get; set; }

        public VectorSource(string id, string url)
        {
            Id = id;
            Url = url;
        }

        public VectorSource(string id, TileSet tileSet)
        {
            Id = id;
            TileSet = tileSet;
        }

        public VectorSource(string id, TileSet tileSet, TileSourceOptions options)
        {
            Id = id;
            TileSet = tileSet;
            Options = options;
        }
    }

    public class TileSourceOptions
    {
        public int? MinimumZoomLevel { get; set; }
        public int? MaximumZoomLevel { get; set; }
        // TODO add other options
    }
}
