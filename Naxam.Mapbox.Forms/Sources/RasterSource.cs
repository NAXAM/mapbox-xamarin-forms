namespace Naxam.Mapbox.Sources
{
    public class RasterSource : Source
    {
        public const int DEFAULT_TILE_SIZE = 512;

        public string ConfigurationURL { get; private set; }

        public int? TileSize { get; private set; }

        public TileSet TileSet { get; private set; }

        public RasterSource(string id, string configurationURL, int tileSize = DEFAULT_TILE_SIZE)
        {
            Id = id;
            ConfigurationURL = configurationURL;
            TileSize = tileSize;
        }

        public RasterSource(string id, TileSet tileSet, int tileSize = DEFAULT_TILE_SIZE)
        {
            Id = id;
            TileSet = tileSet;
            TileSize = tileSize;
        }
    }

    public class TileSet
    {
        public string TileJson { get; set; }

        public string[] Tiles { get; set; }

        public TileSet(string tileJson, params string[] tiles)
        {
            TileJson = tileJson;
            Tiles = tiles;
        }

        public TileSet()
        {
        }
    }
}
