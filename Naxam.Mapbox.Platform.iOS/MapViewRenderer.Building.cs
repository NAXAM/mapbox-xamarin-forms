using Naxam.Mapbox;
using Naxam.Mapbox.Platform.iOS.Extensions;
using Mapbox;
using Foundation;
using Xamarin.Forms.Platform.iOS;
using Naxam.Mapbox.Expressions;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : IMapFunctions
    {
        public void ShowBuilding(BuildingInfo buildingInfo)
        {
            if (map == null || mapStyle == null) return;

            var source = mapStyle.SourceWithIdentifier("composite");

            if (source == null) return;

            var layer = new MGLFillExtrusionStyleLayer("mapbox-android-plugin-3d-buildings", source);
            layer.SourceLayerIdentifier = "building";

            // Filter out buildings that should not extrude.
            //layer.Predicate = NSPredicate.FromFormat("extrude == 'true'");

            // Set the fill extrusion height to the value for the building height attribute.
            layer.FillExtrusionHeight = Expression.Interpolate(
                Expression.Exponential(1),
                Expression.Zoom(),
                Expression.CreateStop(15, 0),
                Expression.CreateStop(16, Expression.Get("height"))
            ).ToNative();
            layer.FillExtrusionOpacity = NSExpression.FromConstant(NSNumber.FromFloat(buildingInfo.Opacity));
            layer.FillExtrusionColor = NSExpression.FromConstant(buildingInfo.Color.ToUIColor());
            layer.Visible = buildingInfo.IsVisible;

            // Insert the fill extrusion layer below a POI label layer. If you aren’t sure what the layer is called, you can view the style in Mapbox Studio or iterate over the style’s layers property, printing out each layer’s identifier.
            if (!string.IsNullOrWhiteSpace(buildingInfo.AboveLayerId)) {
                var symbolLayer = mapStyle.LayerWithIdentifier(buildingInfo.AboveLayerId);

                if (symbolLayer != null) {
                    mapStyle.InsertLayerBelow(layer, symbolLayer);

                    return;
                }
            }

            mapStyle.AddLayer(layer);
        }
    }
}
