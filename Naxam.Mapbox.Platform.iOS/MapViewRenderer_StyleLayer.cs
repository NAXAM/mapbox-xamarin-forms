using System;
using System.Collections.Generic;
using Foundation;
using Mapbox;
using Naxam.Mapbox.Platform.iOS;
using Naxam.Controls.Forms;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Diagnostics;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer
    {
        public T GetValueFromExpression<T>(NSExpression expr) where T : NSObject
        {
            if (expr == null) return default(T);
            switch (expr.ExpressionType)
            {
                case NSExpressionType.ConstantValue:
                    return expr.ConstantValue as T;
                case NSExpressionType.NSAggregate:
                    if (expr.Collection is T)
                        return expr.Collection as T;
                    if (expr.Collection is NSArray array
                        && array.Count != 0)
                    {
                        var first = array.GetItem<T>(0);
                        if (first is NSExpression innerExpr)
                        {
                            return GetValueFromExpression<T>(innerExpr);
                        }
                        return first;
                    }
                    return default(T);
                case NSExpressionType.Function:
                    //TODO
                    var function = expr.Function; //"mgl_interpolate:withCurveType:parameters:stops:"
                    if (expr.Arguments is NSExpression[] args)// $zoomLevel, exponential, 1.299999, {{ 13 = "0.5", 20 = 2;}}
                    {
                        //TODO
                    }
                    return default(T);
                default:
                    return default(T);
            }
        }      

        private MGLStyleLayer GetStyleLayer(StyleLayer styleLayer, NSString id)
		{
			if (string.IsNullOrEmpty(styleLayer.SourceId))
			{
				return null;
			}
			var sourceId = styleLayer.SourceId.ToCustomId();

			var source = map.Style.SourceWithIdentifier(sourceId);
			if (source == null)
			{
				return null;
			}
			if (styleLayer is CircleLayer circleLayer)
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

			if (styleLayer is LineLayer lineLayer)
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

			if (styleLayer is FillLayer fl)
			{
				var newLayer = new MGLFillStyleLayer(id, source)
				{
                    FillColor = NSExpression.FromConstant(fl.FillColor.ToUIColor()),
                    FillOpacity = NSExpression.FromConstant(NSNumber.FromDouble(fl.FillOpacity))
				};
				return newLayer;
			}

			if (styleLayer is SymbolLayer sl)
			{
				var newLayer = new MGLSymbolStyleLayer(id, source)
				{
                    IconImageName = NSExpression.FromConstant(new NSString(sl.IconImageName)),
                    IconOpacity = NSExpression.FromConstant(NSNumber.FromDouble(sl.IconOpacity))
				};
				return newLayer;
			}

            if (styleLayer is RasterLayer rl)
            {

                var newLayer = new MGLRasterStyleLayer(id, source);
                return newLayer;
            }
			return null;
		}

        StyleLayer CreateStyleLayer(MGLVectorStyleLayer vectorLayer, string layerId = null) 
        {
            if (vectorLayer is MGLSymbolStyleLayer sl )
			{
                var newLayer = new SymbolLayer(layerId ?? vectorLayer.Identifier, vectorLayer.SourceIdentifier.TrimCustomId());
                if (sl.IconImageName is NSExpression csFunc)
				{
                    var imgName = GetValueFromExpression<NSString>(sl.IconImageName);
					if (imgName != null)
					{
                        newLayer.IconImageName = imgName.ToString();
					}
				}
				else
				{
                    var imgName = GetValueFromExpression<NSString>(sl.IconImageName);
					if (imgName != null)
					{
						newLayer.IconImageName = imgName.ToString();
					}
				}
                return newLayer;
			}
		
            if (vectorLayer is MGLLineStyleLayer ll)
			{
                var newLayer = new LineLayer(layerId ?? vectorLayer.Identifier, vectorLayer.SourceIdentifier.TrimCustomId())
				{
                    LineColor = (GetValueFromExpression<UIColor>(ll.LineColor) as UIColor).ToColor()
				};

				if (ll.LineDashPattern != null)
				{

                    if (GetValueFromExpression<NSArray>(ll.LineDashPattern) is NSArray arr && arr.Count != 0)
					{
						var dash = new List<double>();
                        for (nuint i = 0; i < arr.Count; i++)
						{
                            var obj = arr.GetItem<NSObject>(i);
                            switch (obj)
                            {
                                case NSExpression expression:
                                    if(expression.ExpressionType==NSExpressionType.ConstantValue){
                                        var number = GetValueFromExpression<NSNumber>(expression);
                                        dash.Add(number.DoubleValue);
                                    }
                                    break;
                                case NSNumber number:
                                    dash.Add(number.DoubleValue);
                                    break;
                                default:
                                    break;
                            }
						}
						newLayer.Dashes = dash.ToArray();
					}
					else
					{
                        //TODO
					}
				}
                return newLayer;
			}
		
            if (vectorLayer is MGLCircleStyleLayer cl)
			{
				var newLayer = new CircleLayer(layerId ?? vectorLayer.Identifier, vectorLayer.SourceIdentifier.TrimCustomId())
				{
                    CircleColor = (GetValueFromExpression<UIColor>(cl.CircleColor) as UIColor)?.ToColor() ?? Color.Transparent
				};
                //TODO stroke, opacity ...
                return newLayer;
			}

            if (vectorLayer is MGLFillStyleLayer fl)
			{
                var newLayer = new FillLayer(layerId ?? vectorLayer.Identifier, vectorLayer.SourceIdentifier.TrimCustomId())
				{
                    FillColor = (GetValueFromExpression<UIColor>(fl.FillColor) as UIColor)?.ToColor() ?? Color.Transparent
				};
                return newLayer;
			}

			return null;
        }
	}
}
