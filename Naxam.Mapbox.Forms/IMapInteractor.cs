using System.Threading.Tasks;
using GeoJSON.Net;
using GeoJSON.Net.Feature;
using Naxam.Mapbox.Annotations;
using Xamarin.Forms;
using NFeature = GeoJSON.Net.Feature.Feature;

namespace Naxam.Mapbox
{

    public partial interface IMapFunctions
    {
        void AddStyleImage(IconImageSource source);
        void AnimiateCamera(CameraPosition cameraPosition, int durationInMillisecond);
    }

    public partial interface IMapFunctions
    {
        bool AddSource(params Sources.Source[] sources);
        bool UpdateSource(string sourceId, IGeoJSONObject featureCollection);
        bool UpdateSource(string sourceId, ImageSource featureCollection);
        void RemoveSource(params string[] sourceIds);

        bool AddLayer(params Layers.Layer[] layers);
        bool AddLayerBelow(Layers.Layer layer, string layerId);
        bool AddLayerAbove(Layers.Layer layer, string layerId);
        bool AddLayerAt(Layers.Layer layer, int index);
        bool UpdateLayer(Layers.Layer layer);
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
