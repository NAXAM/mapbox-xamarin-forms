using System;
using System.Collections.Generic;
using Foundation;
using Mapbox;
using Newtonsoft.Json;
using UIKit;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class ExpressionExtesions
    {
        public static NSExpression ToExpression(this Expressions.Expression expression)
        {
            var json = JsonConvert.SerializeObject(expression.ToArray());

#if DEBUG
            System.Diagnostics.Debug.WriteLine(json);
#endif

            var obj = NSJsonSerialization.Deserialize(NSData.FromString(json), 0, out var error);

            if (error != null)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(error.UserInfo.DebugDescription);
#endif
                return null;
            }

            if (obj is NSArray array)
            {
                var result = ToExpression(array);

#if DEBUG
                System.Diagnostics.Debug.WriteLine(result.ToString());
#endif

                return result;
            }

            return null;
        }

        public static NSPredicate ToPredicate(this Expressions.Expression expression)
        {
            var json = JsonConvert.SerializeObject(expression.ToArray());

#if DEBUG
            System.Diagnostics.Debug.WriteLine(json);
#endif

            var obj = NSJsonSerialization.Deserialize(NSData.FromString(json), 0, out var error);

            if (error != null)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(error.UserInfo.DebugDescription);
#endif
                return null;
            }

            if (obj is NSArray array)
            {
                var result = ToPredicate(array);

#if DEBUG
                System.Diagnostics.Debug.WriteLine(result.ToString());
#endif

                return result;
            }

            return null;
        }

        static readonly Dictionary<string, string> OperatorMap = new Dictionary<string, string>
        {
            {@"+", @"add:to:"},
            {@"-", @"from:subtract:"},
            {@"*", @"multiply:by:"},
            {@"/", @"divide:by:"},
            {@"%", @"modulus:by:"},
            {@"sqrt", @"sqrt:"},
            {@"log10", @"log:"},
            {@"ln", @"ln:"},
            {@"abs", @"abs:"},
            {@"round", @"mgl_round:"},
            {@"acos" , @"mgl_acos:"},
            {@"cos" , @"mgl_cos:"},
            {@"asin" , @"mgl_asin:"},
            {@"sin" , @"mgl_sin:"},
            {@"atan" , @"mgl_atan:"},
            {@"tan" , @"mgl_tan:"},
            {@"log2" , @"mgl_log2:"},
            {@"floor", @"floor:"},
            {@"ceil", @"ceiling:"},
            {@"^", @"raise:toPower:"},
            {@"upcase", @"uppercase:"},
            {@"downcase", @"lowercase:"},
            {@"let", @"MGL_LET"},
        };

        static NSExpression ToExpression(NSArray array)
        {
            var objects = SubArray(array, 0);
            var @operator = objects[0].ToString();
            var arguments = SubArray(objects, 1);

            if (OperatorMap.TryGetValue(@operator, out var functionName))
            {
                var subexpressions = ToSubexpressions(arguments);
                switch (functionName)
                {
                    case "+":
                        if (subexpressions.Count > 2)
                        {
                            var sumArguments = NSExpression.FromAggregate(subexpressions.ToArray());

                            return NSExpression.FromFunction("sum:", new[] { sumArguments });
                        }
                        break;
                    case "^":
                        if (arguments[0].ToString() == "e")
                        {
                            functionName = @"exp:";
                            subexpressions = SubArray(subexpressions, 1);
                        }
                        break;
                    default:
                        break;
                }
                return NSExpression.FromFunction(functionName, subexpressions.ToArray());
            }
            else
            {
                switch (@operator)
                {
                    case "collator":
                        return NSExpression.FromFunction("MGL_FUNCTION", ToSubexpressions(objects).ToArray());

                    case "literal":
                        if (arguments[0] is NSArray literalArray)
                        {
                            return NSExpression.FromConstant(literalArray);
                            // WARNING This may be wrong
                            //var expressions = ToSubexpressions(SubArray(literalArray, 0));
                            //return NSExpression.FromAggregate(expressions.ToArray());
                        }
                        return ToExpression(arguments[0]);
                    case "to-boolean":
                        {
                            var operand = ToExpression(arguments[0]);
                            return NSExpression.FromFunction(operand, "boolValue", new NSExpression[0]);
                        }
                    case "to-number":
                    case "number":
                        {
                            var operand = ToExpression(arguments[0]);

                            if (arguments.Count == 1)
                            {
                                return NSExpression.FromFormat("CAST(%@, 'NSNumber')", new NSObject[] { operand });
                            }

                            var innerArguments = SubArray(arguments, 1);
                            return NSExpression.FromFunction(operand, "mgl_numberWithFallbackValues", ToSubexpressions(innerArguments).ToArray());
                        }
                    case "to-string":
                    case "string":
                        {
                            var operand = ToExpression(arguments[0]);
                            return NSExpression.FromFormat("CAST(%@, 'NSString')", new NSObject[] { operand });
                        }
                    case "to-color":
                    case "color":
                        {
                            var operand = ToExpression(arguments[0]);

                            if (arguments.Count == 1)
                            {
#if __IOS__
                                return NSExpression.FromFormat("CAST(%@, 'UIColor')", new NSObject[] { operand });
#else
                                return NSExpression.FromFormat("CAST(%@, 'NSColor')", new NSObject[] { operand });
#endif
                            }

                            return NSExpression.FromFunction("MGL_FUNCTION", ToSubexpressions(objects).ToArray());
                        }
                    case "to-rgba":
                        {
                            var operand = ToExpression(arguments[0]);
                            return NSExpression.FromFormat("CAST(noindex(%@), 'NSArray')", new NSObject[] { operand });
                        }
                    case "get":
                        {
                            if (arguments.Count == 2)
                            {
                                var operand = ToExpression(arguments[arguments.Count - 1]);

                                if (arguments[0] is NSString value)
                                {
                                    return NSExpression.FromFormat("%@.%K", new NSObject[] { operand, value });
                                }

                                var keyExpression = ToExpression(arguments[0]);
                                return NSExpression.FromFormat("%@.%@", new NSObject[] { operand, keyExpression });
                            }

                            return NSExpression.FromKeyPath(arguments[0].ToString());
                        }
                    case "length":
                        {
                            var subexpressions = ToSubexpressions(arguments);
                            var function = @"count:";

                            if (subexpressions[0].ExpressionType == NSExpressionType.ConstantValue
                                && subexpressions[0].ConstantValue is NSString value)
                            {
                                function = "length:";
                            }

                            return NSExpression.FromFunction(function, new NSExpression[] { subexpressions[0] });
                        };
                    case "rgb":
                        {
                            var subexpressions = ToSubexpressions(arguments);

                            return UIColorAdditions.FromRgb(subexpressions.ToArray());
                        }
                    case "rgba":
                        {
                            var subexpressions = ToSubexpressions(arguments);

                            return UIColorAdditions.FromRgba(subexpressions.ToArray());
                        }
                    case "min":
                    case "max":
                        {
                            var subexpressions = ToSubexpressions(arguments);
                            var subexpression = NSExpression.FromAggregate(subexpressions.ToArray());
                            return NSExpression.FromFunction(@operator, new[] { subexpression });
                        }
                    case "e":
                        {
                            return NSExpression.FromConstant(NSNumber.FromDouble(Math.E));
                        }
                    case "pi":
                        {
                            return NSExpression.FromConstant(NSNumber.FromDouble(Math.PI));
                        }
                    case "concat":
                        {
                            var subexpressions = ToSubexpressions(arguments);
                            var subexpression = NSExpression.FromAggregate(subexpressions.ToArray());
                            return NSExpression.FromFunction("mgl_join:", new[] { subexpression });
                        }
                    case "at":
                        {
                            var subexpressions = ToSubexpressions(arguments);

                            return NSExpression.FromFunction("objectFrom:withIndex:", new[] { subexpressions[1], subexpressions[0] });
                        }
                    case "has":
                        {
                            var subexpressions = ToSubexpressions(arguments);
                            var operand = subexpressions.Count > 1 ? subexpressions[1] : NSExpression.ExpressionForEvaluatedObject;
                            var key = subexpressions[0];
                            return NSExpression.FromFunction("mgl_does:have:", new[] { operand, key });
                        }
                    case "interpolate":
                        {
                            var firstObject = arguments[0];
                            if (firstObject is NSArray == false) return null;

                            var interpolationOptions = SubArray((NSArray)firstObject);

                            var curveType = interpolationOptions[0].ToString();
                            var curveTypeExpression = ToExpression(interpolationOptions[0]);
                            NSObject curveParameters;
                            switch (curveType)
                            {
                                case "exponential":
                                    curveParameters = interpolationOptions[1];
                                    break;
                                case "cubic-bezier":
                                    var temp = new NSMutableArray(5);
                                    temp.Add((NSString)"literal");
                                    temp.Add(interpolationOptions[1]);
                                    temp.Add(interpolationOptions[2]);
                                    temp.Add(interpolationOptions[3]);
                                    temp.Add(interpolationOptions[4]);
                                    curveParameters = temp.Copy();
                                    temp.Dispose();
                                    break;
                                default:
                                    curveParameters = NSNull.Null;
                                    break;
                            }
                            var curveParameterExpression = ToExpression(curveParameters);
                            arguments = SubArray(arguments, 1);

                            var inputExpression = ToExpression(arguments[0]);
                            var stopExpressions = SubArray(arguments, 1);
                            var stops = new NSMutableDictionary();
                            for (int i = 0; i < stopExpressions.Count; i++)
                            {
                                var keyExpression = stopExpressions[i++];
                                var valueExpression = ToExpression(stopExpressions[i]);

                                stops[keyExpression] = valueExpression;
                            }
                            var stopExpression = NSExpression.FromConstant(stops);

                            return NSExpression.FromFunction("mgl_interpolate:withCurveType:parameters:stops:", new[] {
                                inputExpression,
                                curveTypeExpression,
                                curveParameterExpression,
                                stopExpression
                            });
                        }
                    case "step":
                        {
                            var inputExpression = ToExpression(arguments[0]);
                            var stopExpressions = SubArray(arguments, 1);
                            NSExpression miniumExpression = null;

                            if (stopExpressions.Count % 2 == 1)
                            {
                                miniumExpression = ToExpression(stopExpressions[0]);
                                stopExpressions = SubArray(stopExpressions, 1);
                            }

                            var stops = new NSMutableDictionary();
                            for (int i = 0; i < stopExpressions.Count; i++)
                            {
                                var keyExpression = stopExpressions[i++];
                                var valueExpression = ToExpression(ToExpression(stopExpressions[i]));

                                if (miniumExpression == null)
                                {
                                    miniumExpression = valueExpression;
                                    continue;
                                }

                                stops[keyExpression] = valueExpression;
                            }

                            if (miniumExpression == null) return null;

                            var stopExpression = NSExpression.FromConstant(stops);

                            return NSExpression.FromFunction("mgl_step:from:stops:", new[] {
                                inputExpression,
                                miniumExpression,
                                stopExpression
                            });
                        }
                    case "zoom":
                        {
                            return NSExpression_MGLAdditions.ZoomLevelVariableExpression(null);
                        }
                    case "heatmap-density":
                        {
                            return NSExpression_MGLAdditions.HeatmapDensityVariableExpression(null);
                        }
                    case "line-progress":
                        {
                            return NSExpression_MGLAdditions.LineProgressVariableExpression(null);
                        }
                    case "geometry-type":
                        {
                            return NSExpression_MGLAdditions.GeometryTypeVariableExpression(null);
                        }
                    case "id":
                        {
                            return NSExpression_MGLAdditions.FeatureIdentifierVariableExpression(null);
                        }
                    case "properties":
                        {
                            return NSExpression_MGLAdditions.FeatureAttributesVariableExpression(null);
                        }
                    case "var":
                        {
                            return NSExpression.FromVariable(arguments[0].ToString());
                        }
                    case "case":
                        {
                            var args = new NSMutableArray();

                            for (int i = 0; i < arguments.Count; i++)
                            {
                                if (i % 2 == 0 & i != arguments.Count - 1)
                                {
                                    var predicate = ToPredicate(arguments[i]);
                                    var arg = NSExpression.FromConstant(predicate);
                                    args.Add(arg);
                                }
                                else
                                {
                                    args.Add(ToExpression(arguments[i]));
                                }
                            }

                            if (args.Count == 3)
                            {
                                return NSExpression.FromConditional(
                                    (NSPredicate)ObjCRuntime.Runtime.GetNSObject<NSExpression>(args.ValueAt(0)).ConstantValue,
                                    ObjCRuntime.Runtime.GetNSObject<NSExpression>(args.ValueAt(1)),
                                    ObjCRuntime.Runtime.GetNSObject<NSExpression>(args.ValueAt(2)));
                            }

                            return NSExpression.FromFunction("MGL_IF", NSArray.FromArray<NSExpression>((NSArray)args.Copy()));
                        }
                    case "match":
                        {
                            var optionsArray = new NSMutableArray();

                            for (int i = 0; i < arguments.Count; i += 2)
                            {
                                NSExpression option = ToExpression(arguments[i]);

                                if (i > 0 && i < arguments.Count - 1 && !(i % 2 == 0) && arguments[i] is NSArray sub)
                                {
                                    option = NSExpression.FromAggregate(ToSubexpressions(SubArray(sub)).ToArray());
                                }

                                optionsArray.Add(option);
                            }

                            return NSExpression.FromFunction("MGL_MATCH", NSArray.FromArray<NSExpression>((NSArray)optionsArray.Copy()));
                        }
                    case "format":
                        {
                            var attributedExpressions = new NSMutableArray();

                            for (int i = 0; i < arguments.Count; i += 2)
                            {
                                var expression = ToExpression(arguments[i]);
                                var attrs = new NSMutableDictionary();

                                if (i + 1 < arguments.Count)
                                {
                                    attrs = NSMutableDictionary.FromDictionary(arguments[i + 1] as NSDictionary);
                                }

                                foreach (var key in attrs.Keys)
                                {
                                    attrs[key] = ToExpression(attrs[key]);
                                }

                                var attributedExpression = new MGLAttributedExpression(expression, attrs);

                                attributedExpressions.Add(attributedExpression);
                            }

                            return NSExpression.FromFunction("mgl_attributed", NSArray.FromArray<NSExpression>((NSArray)attributedExpressions.Copy()));
                        }
                    case "coalesce":
                        {
                            var subexpressions = ToSubexpressions(arguments);
                            return NSExpression.FromFormat("mgl_coalesce(%@)", subexpressions.ToArray());
                        }
                    default:
                        {
                            var subexpressions = ToSubexpressions(objects);
                            return NSExpression.FromFunction(@"MGL_FUNCTION", subexpressions.ToArray());
                        }
                }
            }
        }

        static readonly Dictionary<string, NSPredicateOperatorType> MGLPredicateOperatorTypesByJSONOperator = new Dictionary<string, NSPredicateOperatorType> {
            { "==", NSPredicateOperatorType.EqualTo },
            { "!=", NSPredicateOperatorType.NotEqualTo },
            { "<", NSPredicateOperatorType.LessThan },
            { "<=", NSPredicateOperatorType.LessThanOrEqualTo },
            { ">", NSPredicateOperatorType.GreaterThan },
            { ">=", NSPredicateOperatorType.GreaterThanOrEqualTo },
        };

        static NSPredicate ToPredicate(NSObject @object)
        {
            if (@object == NSNumber.FromBoolean(true))
            {
                return NSPredicate.FromValue(true);
            }

            if (@object == NSNumber.FromBoolean(false))
            {
                return NSPredicate.FromValue(false);
            }

            if (@object is NSArray == false) return null;

            var array = (NSArray)@object;
            var objects = SubArray(array);
            var op = objects[0].ToString();

            if (MGLPredicateOperatorTypesByJSONOperator.TryGetValue(op, out var operatorType))
            {
                NSComparisonPredicateOptions options = 0;

                if (objects.Count > 3)
                {
                    var collatorExpression = SubArray((NSArray)objects[3]);

                    if (collatorExpression.Count != 2) return null;

                    var collator = (NSDictionary)collatorExpression[1];

                    if (false == collator.ContainsKey((NSString)"locale"))
                    {
                        if (collator.ValueForKey((NSString)"case-sensitive") == NSNumber.FromBoolean(false))
                        {
                            options |= NSComparisonPredicateOptions.CaseInsensitive;
                        }

                        if (collator.ValueForKey((NSString)"diacritic-sensitive") == NSNumber.FromBoolean(false))
                        {
                            options |= NSComparisonPredicateOptions.DiacriticInsensitive;
                        }
                    }
                }

                var subexpressions = ToSubexpressions(SubArray(objects, 1));

                return new NSComparisonPredicate(
                    subexpressions[0],
                    subexpressions[1],
                    NSComparisonPredicateModifier.Direct,
                    operatorType,
                    options
                    );
            }

            switch (op)
            {
                case "!":
                    {
                        var subpredicates = ToSubpredicates(SubArray(objects, 1));
                        if (subpredicates.Count > 1)
                        {
                            var predicate = NSCompoundPredicate.CreateOrPredicate(subpredicates.ToArray());

                            return NSCompoundPredicate.CreateNotPredicate(predicate);
                        }

                        return NSPredicate.FromValue(true);
                    }
                case "all":
                    {
                        var subpredicates = ToSubpredicates(SubArray(objects, 1));

                        if (subpredicates.Count == 2)
                        {
                            if (subpredicates[0] is NSComparisonPredicate leftCondition
                                && subpredicates[1] is NSComparisonPredicate rightCondition
                                )
                            {
                                NSExpression[] limits = null;
                                NSExpression leftConditionExpression = null;

                                if (leftCondition.PredicateOperatorType == NSPredicateOperatorType.GreaterThanOrEqualTo
                                    && rightCondition.PredicateOperatorType == NSPredicateOperatorType.LessThanOrEqualTo
                                    )
                                {
                                    limits = new NSExpression[]
                                    {
                                        leftCondition.RightExpression,
                                        rightCondition.RightExpression
                                    };
                                    leftConditionExpression = leftCondition.LeftExpression;
                                }
                                else if (leftCondition.PredicateOperatorType == NSPredicateOperatorType.LessThanOrEqualTo
                                    && rightCondition.PredicateOperatorType == NSPredicateOperatorType.LessThanOrEqualTo
                                    )
                                {
                                    limits = new NSExpression[]
                                    {
                                        leftCondition.LeftExpression,
                                        rightCondition.RightExpression
                                    };
                                    leftConditionExpression = leftCondition.RightExpression;
                                }
                                else if (leftCondition.PredicateOperatorType == NSPredicateOperatorType.LessThanOrEqualTo
                                  && rightCondition.PredicateOperatorType == NSPredicateOperatorType.GreaterThanOrEqualTo
                                  )
                                {
                                    limits = new NSExpression[]
                                    {
                                        leftCondition.LeftExpression,
                                        rightCondition.LeftExpression
                                    };
                                    leftConditionExpression = leftCondition.RightExpression;
                                }
                                else if (leftCondition.PredicateOperatorType == NSPredicateOperatorType.GreaterThanOrEqualTo
                                  && rightCondition.PredicateOperatorType == NSPredicateOperatorType.GreaterThanOrEqualTo
                                  )
                                {
                                    limits = new NSExpression[]
                                    {
                                        leftCondition.RightExpression,
                                        rightCondition.LeftExpression
                                    };
                                    leftConditionExpression = leftCondition.LeftExpression;
                                }

                                if (limits != null && leftConditionExpression != null)
                                {
                                    return NSPredicate.FromFormat("%@ BETWEEN %@", leftConditionExpression, NSExpression.FromAggregate(limits));
                                }
                            }
                        }

                        return new NSCompoundPredicate(NSCompoundPredicateType.And, subpredicates.ToArray());
                    }
                case "any":
                    {
                        var subpredicates = ToSubpredicates(SubArray(objects, 1));

                        return new NSCompoundPredicate(NSCompoundPredicateType.Or, subpredicates.ToArray());
                    }
                default:
                    {
                        var expression = ToExpression(array);

                        return new NSComparisonPredicate(
                            expression,
                            NSExpression.FromConstant(NSNumber.FromBoolean(true)),
                            NSComparisonPredicateModifier.Direct,
                            NSPredicateOperatorType.EqualTo,
                            0
                        );
                    }
            }
        }

        static List<NSPredicate> ToSubpredicates(List<NSObject> array)
        {
            var result = new List<NSPredicate>();
            for (int i = 0; i < array.Count; i++)
            {
                result.Add(ToPredicate(array[i]));
            }
            return result;
        }

        static List<NSExpression> ToSubexpressions(List<NSObject> array)
        {
            var result = new List<NSExpression>();
            for (int i = 0; i < array.Count; i++)
            {
                result.Add(ToExpression(array[i]));
            }
            return result;
        }

        static List<NSObject> SubArray(NSArray array, uint skip = 0)
        {
            var result = new List<NSObject>();
            for (uint i = skip; i < array.Count; i++)
            {
                result.Add(ObjCRuntime.Runtime.GetNSObject(array.ValueAt(i)));
            }
            return result;
        }

        static List<T> SubArray<T>(List<T> array, int skip = 0)
        {
            var result = new List<T>();
            for (int i = skip; i < array.Count; i++)
            {
                result.Add(array[i]);
            }
            return result;
        }

        static NSExpression ToExpression(NSObject @object)
        {
            switch (@object)
            {
                case NSArray array:
                    return ToExpression(array);
                case NSString @string:
                case NSNumber number:
                case NSValue value:
                    return NSExpression.FromConstant(@object);

                case NSDictionary dict:
                    {
                        var dictionary = new NSMutableDictionary();

                        foreach (var key in dict.Keys)
                        {
                            dictionary[key] = ToExpression(dict[key]);
                        }

                        return NSExpression.FromConstant(dictionary);
                    }
            }

            if (@object == null || @object == NSNull.Null)
            {
                return NSExpression.FromConstant(null);
            }

            return null;
        }
    }

    public static class UIColorAdditions
    {
        public static NSExpression FromRgb(NSExpression[] components)
        {
            var color = ColorFromRgb(components);
            if (color != null)
            {
                return NSExpression.FromConstant(color);
            }

            var colorClass = ObjCRuntime.Class.GetHandleIntrinsic(nameof(UIColor));
            var colorExpression = NSExpression.FromConstant(ObjCRuntime.Runtime.GetNSObject(colorClass));
            var alphaExpression = NSExpression.FromConstant(NSNumber.FromDouble(1));
            var rgbaArguments = new NSExpression[components.Length + 1];

            components.CopyTo(rgbaArguments, 0);
            Array.ConstrainedCopy(new[] { alphaExpression }, 0, rgbaArguments, components.Length, 1);

            return NSExpression.FromFunction(
                colorExpression,
                "colorWithRed:green:blue:alpha:",
                rgbaArguments
                );
        }

        public static NSExpression FromRgba(NSExpression[] components)
        {
            var color = ColorFromRgb(components);
            if (color != null)
            {
                return NSExpression.FromConstant(color);
            }


            var colorClass = ObjCRuntime.Class.GetHandleIntrinsic(nameof(UIColor));
            var colorExpression = NSExpression.FromConstant(ObjCRuntime.Runtime.GetNSObject(colorClass));

            return NSExpression.FromFunction(
                colorExpression,
                "colorWithRed:green:blue:alpha:",
                components
                );
        }

        public static UIColor ColorFromRgb(NSExpression[] components)
        {
            if (components.Length < 3 || components.Length > 4)
            {
                return null;
            }

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].ExpressionType != NSExpressionType.ConstantValue)
                    return null;

                if (components[i].ConstantValue is NSNumber == false)
                    return null;
            }

            var r = ((NSNumber)components[0].ConstantValue).DoubleValue / 255;
            var g = ((NSNumber)components[1].ConstantValue).DoubleValue / 255;
            var b = ((NSNumber)components[2].ConstantValue).DoubleValue / 255;
            var a = components.Length == 3 ? 1.0
                : ((NSNumber)components[3].ConstantValue).DoubleValue;

            return UIColor.FromRGBA((nfloat)r, (nfloat)g, (nfloat)b, (nfloat)a);
        }
    }
}
