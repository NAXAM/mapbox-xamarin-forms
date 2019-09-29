namespace Naxam.Mapbox.Sources
{
    public class MapboxImageSource : Source
    {
        public Xamarin.Forms.ImageSource Source { get; private set; }

        public LatLngQuad Coordinates { get;  private set; }

        public MapboxImageSource(string id, LatLngQuad coordinates, Xamarin.Forms.ImageSource source)
        {
            Id = id;
            Coordinates = coordinates;
            Source = source;
        }
    }
}