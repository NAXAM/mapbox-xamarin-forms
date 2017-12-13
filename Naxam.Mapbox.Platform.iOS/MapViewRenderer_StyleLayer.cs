using System;
using System.Collections.Generic;
using Foundation;
using Mapbox;
using Naxam.Controls.Mapbox.Forms;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer
    {
		private NSObject GetValueFromCameraStyleFunction(MGLCameraStyleFunction csFunc)
		{
			if (csFunc.Stops == null || csFunc.Stops.Count == 0) return null;
			MGLStyleValue output = null;
			switch (csFunc.InterpolationMode)
			{
				case MGLInterpolationMode.Identity:
					nuint i = 0;
					while (i < csFunc.Stops.Count)
					{
						var key = csFunc.Stops.Keys[i];
						var zoomLevel = (key as NSNumber).DoubleValue;
                        if (zoomLevel < MapView.ZoomLevel)
						{
							output = csFunc.Stops[key];
						}
						else
						{
							break;
						}
						i++;
					}
					break;
				default: break;
			}
			if (output == null)
			{
				output = csFunc.Stops.Values[0];
			}
			return GetObjectFromStyleValue(output);
		}

		private NSObject GetObjectFromStyleValue(MGLStyleValue value)
		{
			if (value is MGLConstantStyleValue cValue)
			{
				return cValue.RawValue;
			}
			if (value is MGLCameraStyleFunction csFunc)
			{
				return GetValueFromCameraStyleFunction(csFunc);
			}
			if (value != null && value.RespondsToSelector(new ObjCRuntime.Selector("rawValue")))
			{
				return value.ValueForKey((NSString)"rawValue");
			}
			return value;
		}

        private MGLStyleLayer GetStyleLayer(StyleLayer styleLayer, NSString id)
		{
			if (string.IsNullOrEmpty(styleLayer.SourceId))
			{
				return null;
			}
			var sourceId = styleLayer.SourceId.ToCustomId();

			var source = MapView.Style.SourceWithIdentifier(sourceId);
			if (source == null)
			{
				return null;
			}
			if (styleLayer is CircleLayer circleLayer)
			{
				var newLayer = new MGLCircleStyleLayer(id, source)
				{
					CircleColor = MGLStyleValue.ValueWithRawValue(circleLayer.CircleColor.ToUIColor()),
					CircleOpacity = MGLStyleValue.ValueWithRawValue(NSNumber.FromDouble(circleLayer.CircleOpacity)),
					CircleRadius = MGLStyleValue.ValueWithRawValue(NSNumber.FromDouble(circleLayer.CircleRadius))
				};
				if (circleLayer.StrokeColor is Color strokeColor)
				{
					newLayer.CircleStrokeColor = MGLStyleValue.ValueWithRawValue(strokeColor.ToUIColor());
					newLayer.CircleStrokeOpacity = MGLStyleValue.ValueWithRawValue(NSNumber.FromDouble(circleLayer.StrokeOpacity));
					newLayer.CircleStrokeWidth = MGLStyleValue.ValueWithRawValue(NSNumber.FromDouble(circleLayer.StrokeWidth));
				}
				return newLayer;
			}

			if (styleLayer is LineLayer lineLayer)
			{
				var newLayer = new MGLLineStyleLayer(id, source)
				{
					LineWidth = MGLStyleValue.ValueWithRawValue(NSNumber.FromDouble(lineLayer.LineWidth)),
					LineColor = MGLStyleValue.ValueWithRawValue(lineLayer.LineColor.ToUIColor())
				};
				if (lineLayer.Dashes != null && lineLayer.Dashes.Length != 0)
				{
					var arr = new NSMutableArray<NSNumber>();
					foreach (double dash in lineLayer.Dashes)
					{
						arr.Add(NSNumber.FromDouble(dash));
					}
					newLayer.LineDashPattern = MGLStyleValue.ValueWithRawValue(arr);
				}
				//TODO lineCap
				return newLayer;
			}

			if (styleLayer is FillLayer fl)
			{
				var newLayer = new MGLFillStyleLayer(id, source)
				{
					FillColor = MGLStyleValue.ValueWithRawValue(fl.FillColor.ToUIColor()),
					FillOpacity = MGLStyleValue.ValueWithRawValue(NSNumber.FromDouble(fl.FillOpacity))
				};
				return newLayer;
			}

			if (styleLayer is SymbolLayer sl)
			{
				var newLayer = new MGLSymbolStyleLayer(id, source)
				{
					IconImageName = MGLConstantStyleValue.ValueWithRawValue((NSString)sl.IconImageName),
					IconOpacity = MGLStyleValue.ValueWithRawValue(NSNumber.FromDouble(sl.IconOpacity))
				};
				return newLayer;
			}

            if (styleLayer is RasterStyleLayer rl)
            {

                var newLayer = new MGLRasterStyleLayer(id, source);
                return newLayer;
            }
			return null;
		}

        StyleLayer CreateStyleLayer(MGLVectorStyleLayer vectorLayer, string layerId = null) {
            if (vectorLayer is MGLSymbolStyleLayer sl && sl.IconImageName != null)
			{
                var newLayer = new SymbolLayer(layerId ?? vectorLayer.Identifier, vectorLayer.SourceIdentifier.TrimCustomId());
				if (sl.IconImageName is MGLCameraStyleFunction csFunc)
				{
					var imgName = GetValueFromCameraStyleFunction(csFunc);
					if (imgName != null)
					{
                        newLayer.IconImageName = imgName.ToString();
					}
				}
				else
				{
					var imgName = GetObjectFromStyleValue(sl.IconImageName);
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
                    LineColor = (GetObjectFromStyleValue(ll.LineColor) as UIColor).ToColor()
				};

				if (ll.LineDashPattern != null)
				{

					if (GetObjectFromStyleValue(ll.LineDashPattern) is NSArray arr && arr.Count != 0)
					{
						var dash = new List<double>();
						for (nuint i = 0; i < arr.Count; i++)
						{
							var obj = arr.GetItem<NSNumber>(i);
							dash.Add(obj.DoubleValue);
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
                    CircleColor = (GetObjectFromStyleValue(cl.CircleColor) as UIColor)?.ToColor() ?? Color.Transparent
				};
                //TODO stroke, opacity ...
                return newLayer;
			}

            if (vectorLayer is MGLFillStyleLayer fl)
			{
                var newLayer = new FillLayer(layerId ?? vectorLayer.Identifier, vectorLayer.SourceIdentifier.TrimCustomId())
				{
                    FillColor = (GetObjectFromStyleValue(fl.FillColor) as UIColor)?.ToColor() ?? Color.Transparent
				};
                return newLayer;
			}

			return null;
        }
	}
}
