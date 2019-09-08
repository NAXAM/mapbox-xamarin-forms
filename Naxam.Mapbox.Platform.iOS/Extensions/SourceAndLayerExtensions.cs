using System;
using Foundation;
using Mapbox;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using NxGeoJsonOptions = Naxam.Mapbox.Sources.GeoJsonOptions;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class SourceAndLayerExtensions
    {
        public static MGLShapeSource ToSource(this Source source)
        {
            MGLShapeSource result = null;

            switch (source)
            {
                case GeoJsonSource geojsonSource:
                    var options = geojsonSource.Options.ToOptions();
                    var url = geojsonSource.IsLocal
                        ? NSUrl.FromFilename(geojsonSource.Url)
                        : NSUrl.FromString(geojsonSource.Url);
                    result = geojsonSource.Data != null
                        ? new MGLShapeSource(source.Id, geojsonSource.Data.ToShape(), options)
                        : new MGLShapeSource(source.Id, url, options);

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

            // TODO Handle error
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

    public static class GeoJsonOptionsExtensions
    {
        public static NSDictionary<NSString, NSObject> ToOptions(this NxGeoJsonOptions nxoptions)
        {
            if (nxoptions == null) return null;

            var options = new NSMutableDictionary<NSString, NSObject>();
            if (nxoptions.Buffer.HasValue)
            {
                options.Add(MGLShapeSourceOptions.Buffer, NSNumber.FromNInt(nxoptions.Buffer.Value));
            }
            if (nxoptions.Cluster.HasValue)
            {
                options.Add(MGLShapeSourceOptions.Clustered, NSNumber.FromBoolean(nxoptions.Cluster.Value));
            }
            if (nxoptions.ClusterMaxZoom.HasValue)
            {
                options.Add(MGLShapeSourceOptions.ClusterRadius, NSNumber.FromNInt(nxoptions.ClusterMaxZoom.Value));
            }
            if (nxoptions.ClusterRadius.HasValue)
            {
                options.Add(MGLShapeSourceOptions.ClusterRadius, NSNumber.FromNInt(nxoptions.ClusterRadius.Value));
            }
            if (nxoptions.LineMetrics.HasValue)
            {
                options.Add(MGLShapeSourceOptions.LineDistanceMetrics, NSNumber.FromBoolean(nxoptions.LineMetrics.Value));
            }
            if (nxoptions.MaxZoom.HasValue)
            {
                options.Add(MGLShapeSourceOptions.MaximumZoomLevel, NSNumber.FromNInt(nxoptions.MaxZoom.Value));
            }
            if (nxoptions.MinZoom.HasValue)
            {
                options.Add(MGLShapeSourceOptions.MinimumZoomLevel, NSNumber.FromNInt(nxoptions.MinZoom.Value));
            }
            if (nxoptions.Tolerance.HasValue)
            {
                options.Add(MGLShapeSourceOptions.SimplificationTolerance, NSNumber.FromFloat(nxoptions.Tolerance.Value));
            }
            return new NSDictionary<NSString, NSObject>(options.Keys, options.Values);
        }
    }
}
