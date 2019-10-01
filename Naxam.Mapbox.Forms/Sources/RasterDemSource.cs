namespace Naxam.Mapbox.Sources
{
    public class RasterDemSource : Source
    {
        public string Url { get; private set; }

        public int? TileSize { get; private set; }

        public TileSet TileSet { get; private set; }
        
        public RasterDemSource(string id, string url, int? tileSize = null)
        {
            Url = url;
            Id = id;
            TileSize = tileSize;
        }
        
        public RasterDemSource(string id, TileSet tileSet, int? tileSize = null)
        {
            TileSet = tileSet;
            Id = id;
            TileSize = tileSize;
        }
    }
}