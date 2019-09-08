using System.Threading.Tasks;
using Xamarin.Forms;
using NFeature = GeoJSON.Net.Feature.Feature;

namespace Naxam.Mapbox
{
    public interface IMapFunctions
    {
        Task<byte[]> TakeSnapshotAsync(LatLngBounds bounds = default);

        NFeature[] QueryFeatures(LatLng latLng, params string[] layers);
        NFeature[] QueryFeatures(LatLng latLng, float radius, params string[] layers);
        NFeature[] QueryFeatures(LatLngBounds bounds, params string[] layers);

        NFeature[] QueryFeatures(Point position, params string[] layers);
        NFeature[] QueryFeatures(Point position, float radius, params string[] layers);
        NFeature[] QueryFeatures(Rectangle rectangle, params string[] layers);
    }
}
