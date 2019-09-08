using System;
using Foundation;
using Mapbox;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class SourceAndLayerExtensions
    {
        public static MGLShapeSource ToSource(this Source source)
        {
            MGLShapeSource result = null;

            switch (source)
            {
                case GeojsonSource geojsonSource:
                    result = geojsonSource.Data != null
                        ? new MGLShapeSource(source.Id, geojsonSource.Data.ToShape(), null)
                        : new MGLShapeSource(source.Id, NSUrl.FromString(geojsonSource.Url), null);

                    break;
            }

            return result;
        }

        public static MGLShape ToShape(this GeoJSON.Net.IGeoJSONObject geoJSONObject)
        {
            if (geoJSONObject == null) return null;

            var json = JsonConvert.SerializeObject(geoJSONObject);
            var data = NSData.FromString(json);

            var shape = MGLShape.ShapeWithData(data, (int)NSStringEncoding.UTF8, out var error);

            if (error != null) return null;

            return shape;
        }


        public static MGLStyleLayer ToLayer(this Layer layer, MGLSource source)
        {
            MGLStyleLayer result = null;
            var id = layer.Id;

            switch (layer)
            {
                case CircleLayer circleLayer:
                    {
                        var newLayer = new MGLCircleStyleLayer(id, source)
                        {
                            CircleColor = NSExpression.FromConstant(circleLayer.CircleColor.ToUIColor()),
                            CircleOpacity = NSExpression.FromConstant(NSNumber.FromDouble(circleLayer.CircleOpacity)),
                            CircleRadius = NSExpression.FromConstant(NSNumber.FromDouble(circleLayer.CircleRadius))
                        };
                        if (circleLayer.StrokeColor is Color strokeColor)
                        {
                            newLayer.CircleStrokeColor = NSExpression.FromConstant(strokeColor.ToUIColor());
                            newLayer.CircleStrokeOpacity = NSExpression.FromConstant(NSNumber.FromDouble(circleLayer.StrokeOpacity));
                            newLayer.CircleStrokeWidth = NSExpression.FromConstant(NSNumber.FromDouble(circleLayer.StrokeWidth));
                        }
                        return newLayer;
                    }

                case LineLayer lineLayer:
                    {
                        var newLayer = new MGLLineStyleLayer(id, source)
                        {
                            LineWidth = NSExpression.FromConstant(NSNumber.FromDouble(lineLayer.LineWidth)),
                            LineColor = NSExpression.FromConstant(lineLayer.LineColor.ToUIColor())
                        };
                        if (lineLayer.Dashes != null && lineLayer.Dashes.Length != 0)
                        {
                            var arr = new NSMutableArray<NSNumber>();
                            foreach (double dash in lineLayer.Dashes)
                            {
                                arr.Add(NSNumber.FromDouble(dash));
                            }
                            newLayer.LineDashPattern = NSExpression.FromConstant(arr);
                        }
                        //TODO lineCap
                        return newLayer;
                    }

                case FillLayer fl:
                    {
                        var newLayer = new MGLFillStyleLayer(id, source)
                        {
                            FillColor = NSExpression.FromConstant(fl.FillColor.ToUIColor()),
                            FillOpacity = NSExpression.FromConstant(NSNumber.FromDouble(fl.FillOpacity))
                        };
                        return newLayer;
                    }

                case SymbolLayer sl:
                    {
                        var newLayer = new MGLSymbolStyleLayer(id, source)
                        {
                            IconImageName = NSExpression.FromConstant(new NSString(sl.IconImageName)),
                            IconOpacity = NSExpression.FromConstant(NSNumber.FromDouble(sl.IconOpacity))
                        };
                        return newLayer;
                    }

                case RasterLayer rl:
                    {

                        var newLayer = new MGLRasterStyleLayer(id, source);
                        return newLayer;
                    }
            }

            return result;
        }

    }
}
