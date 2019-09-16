using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.Mapbox.Mapboxsdk.Style.Expressions;
using NxExpressions = Naxam.Mapbox.Expressions;
namespace Naxam.Mapbox.Platform.Droid.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression ToExpression(this NxExpressions.Expression expression)
        {
            switch (expression)
            {
                case NxExpressions.ExpressionLiteral literal:
                    return Expression.Literal(literal.Value.ToJ());
                case NxExpressions.ExpressionFormat format:
                    return Expression.Format(format.Entries.Select(x => x.ToEntry()).ToArray());
            }
            switch (expression.Operator)
            {
                case "collator":
                    var mapExpressipn = (NxExpressions.ExpressionMap)expression.Arguments[0];

                    return mapExpressipn.Map.Keys.Count == 2
                        ? Expression.Collator(
                            mapExpressipn.Map["case-sensitive"].ToExpression(),
                            mapExpressipn.Map["diacritic-sensitive"].ToExpression()
                        )
                        : Expression.Collator(
                            mapExpressipn.Map["case-sensitive"].ToExpression(),
                            mapExpressipn.Map["diacritic-sensitive"].ToExpression(),
                            mapExpressipn.Map["locale"].ToExpression()
                        );
            }
            return new Expression(expression.Operator, expression.Arguments.Select(x => x.ToExpression()).ToArray());
        }

        public static Java.Lang.Object ToJ(this object obj)
        {
            switch (obj)
            {
                case byte byteValue:
                    return new Java.Lang.Byte((sbyte)byteValue);
                case int intValue:
                    return new Java.Lang.Integer(intValue);
                case long longValue:
                    return new Java.Lang.Long(longValue);
                case double doubleValue:
                    return new Java.Lang.Double(doubleValue);
                case float floatValue:
                    return new Java.Lang.Float(floatValue);
                case bool boolValue:
                    return new Java.Lang.Boolean(boolValue);
                case string stringValue:
                    return new Java.Lang.String(stringValue);
                case IDictionary<string, object> dict:
                    var hashMap = new Java.Util.HashMap();
                    foreach (var key in dict.Keys)
                    {
                        hashMap.Put(key, dict[key].ToJ());
                    }
                    return hashMap;
                case IEnumerable values:
                    var jValues = new Java.Util.ArrayList();
                    var enumerator = values.GetEnumerator();
                    while(enumerator.MoveNext())
                    {
                        jValues.Add(ToJ(enumerator.Current));
                    }

                    return jValues;
                case NxExpressions.Expression expression:
                    return expression.ToExpression();
                default:
                    if (obj == null)
                        return new Java.Lang.String(string.Empty);

                    throw new NotSupportedException("Object type isn't supported: " + obj.GetType());
            }
        }

        public static Expression.FormatEntry ToEntry(this NxExpressions.FormatEntry entry)
        {
            var options = entry.Options.Select(x => {
                switch (x.Type)
                {
                    case "font-scale":
                        return Expression.FormatOption.FormatFontScale(x.Value.ToExpression());
                    case "text-font":
                        return Expression.FormatOption.FormatTextFont(x.Value.ToExpression());
                    case "text-color":
                        return Expression.FormatOption.FormatTextColor(x.Value.ToExpression());
                    default:
                        throw new NotSupportedException("Option isn't supported: " + x.Type);
                }
            }).ToArray();

            return Expression.CreateFormatEntry(entry.Text.ToExpression(), options);
        }
    }
}
