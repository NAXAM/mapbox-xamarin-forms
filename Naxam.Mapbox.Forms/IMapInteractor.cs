using System.Threading.Tasks;
using Naxam.Mapbox.Annotations;
using Xamarin.Forms;
using NFeature = GeoJSON.Net.Feature.Feature;

namespace Naxam.Mapbox
{

    public partial interface IMapFunctions
    {
        void AddStyleImage(IconImageSource source);
    }

    public partial interface IMapFunctions
    {
        bool AddSource(params Sources.Source[] sources);
        void RemoveSource(params string[] sourceIds);

        bool AddLayer(params Layers.Layer[] layers);
        void RemoveLayer(params string[] layerIds);
    }

    public partial interface IMapFunctions
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
