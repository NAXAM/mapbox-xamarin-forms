using System;
using System.Collections.Generic;
using Foundation;
using Mapbox;
using Naxam.Mapbox.Layers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

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
