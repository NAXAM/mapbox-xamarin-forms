using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Naxam.Mapbox.Expressions
{
    /**
     * ExpressionLiteral wraps an object to be used as a literal in an expression.
     * <p>
     * ExpressionLiteral is created with {@link #Literal(double)}, {@link #Literal(bool)},
     * {@link #Literal(string)} and {@link #Literal(object)}.
     * </p>
     */
     public class ExpressionLiteral<T> : Expression, IValueExpression, IEquatable<ExpressionLiteral<T>>
    {
        public T Value { get; private set; }

        public ExpressionLiteral(T @object)
        {
            Value = @object;
        }
        /**
         * Get the literal object.
         *
         * @return the literal object
         */

        public object ToValue()
        {
            if (Value is IValueExpression valueExpression)
            {
                return valueExpression.ToValue();
            }
            return Value;
        }

        public override object[] ToArray()
        {
            return new object[] { "literal", Value };
        }
        /**
         * Returns a string representation of the expression literal.
         *
         * @return a string representation of the object.
         */

        public override string ToString()
        {
            string @string;
            if (Value is string)
            {
                @string = "\"" + Value + "\"";
            }
            else
            {
                @string = Value.ToString();
            }
            return @string;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj is ExpressionLiteral<T> expressionLiteral)
            {
                return Equals(expressionLiteral);
            }

            return false;
        }

        /**
         * Returns a hash code value for the expression literal.
         *
         * @return a hash code value for this expression literal
         */
        public override int GetHashCode()
        {
            int result = base.GetHashCode();
            result = 31 * result + (Value != null ? Value.GetHashCode() : 0);
            return result;
        }

        public bool Equals(ExpressionLiteral<T> other)
        {
            if (other == null) return false;

            if (base.Equals(other) == false) return false;

            return Equals(Value, other.Value);
        }
    }

    /**
     * Expression interpolator type.
     * <p>
     * Is used for first parameter of {@link #interpolate(Interpolator, Expression, params Stop[])}.
     * </p>
     */
    public sealed class Interpolator : Expression
    {
        public Interpolator(string @operator, params Expression[] arguments) : base(@operator, arguments) { }
    }

    /**
     * Expression stop type.
     * <p>
     * Can be used for {@link #stop(object, object)} as part of varargs parameter in
     * {@link #step(double, Expression, params Stop[])} or {@link #interpolate(Interpolator, Expression, params Stop[])}.
     * </p>
     */
    public sealed class Stop
    {
        readonly object value;
        readonly object output;

        public Stop(object value, object output)
        {
            this.value = value;
            this.output = output;
        }

        /**
         * Converts a varargs of Stops to a Expression array.
         *
         * @param stops the stops to convert
         * @return the converted stops as an expression array
         */

        public static Expression[] ToExpressionArray(params Stop[] stops)
        {
            Expression[] expressions = new Expression[stops.Length * 2];
            Stop stop;
            object inputValue, outputValue;
            for (int i = 0; i < stops.Length; i++)
            {
                stop = stops[i];
                inputValue = stop.value;
                outputValue = stop.output;

                if (!(inputValue is Expression))
                {
                    inputValue = Expression.Literal(inputValue);
                }

                if (!(outputValue is Expression))
                {
                    outputValue = Expression.Literal(outputValue);
                }

                expressions[i * 2] = (Expression)inputValue;
                expressions[i * 2 + 1] = (Expression)outputValue;
            }
            return expressions;
        }
    }

    /**
     * Holds format entries used in a {@link #Format(FormatEntry...)} expression.
     */
    public sealed class FormatEntry
    {

        public Expression Text { get; private set; }

        public FormatOption[] Options { get; private set; }

        public FormatEntry(Expression text, FormatOption[] options)
        {
            this.Text = text;
            this.Options = options;
        }
    }

    /**
     * Holds format options used in a {@link #formatEntry(Expression, FormatOption...)} that builds
     * a {@link #Format(FormatEntry...)} expression.
     * <p>
     * If an option is not set, it defaults to the base value defined for the symbol.
     */
    public sealed class FormatOption
    {

        public string Type { get; private set; }

        public Expression Value { get; private set; }

        FormatOption(string type, Expression value)
        {
            this.Type = type;
            this.Value = value;
        }

        /**
         * If set, the font-scale argument specifies a scaling factor relative to the text-size
         * specified in the root layout properties.
         * <p>
         * "font-scale" is required to be of a resulting type number.
         *
         * @param expression expression
         * @return format option
         */

        public static FormatOption FormatFontScale(Expression expression)
        {
            return new FormatOption("font-scale", expression);
        }

        /**
         * If set, the font-scale argument specifies a scaling factor relative to the text-size
         * specified in the root layout properties.
         * <p>
         * "font-scale" is required to be of a resulting type number.
         *
         * @param scale value
         * @return format option
         */

        public static FormatOption FormatFontScale(double scale)
        {
            return new FormatOption("font-scale", Expression.Literal(scale));
        }

        /**
         * If set, the text-font argument overrides the font specified by the root layout properties.
         * <p>
         * "text-font" is required to be a literal array.
         * <p>
         * The requested font stack has to be a part of the used style.
         * For more information see <a href="https://www.mapbox.com/help/define-font-stack/">the documentation</a>.
         *
         * @param expression expression
         * @return format option
         */

        public static FormatOption FormatTextFont(Expression expression)
        {
            return new FormatOption("text-font", expression);
        }

        /**
         * If set, the text-font argument overrides the font specified by the root layout properties.
         * <p>
         * "text-font" is required to be a literal array.
         * <p>
         * The requested font stack has to be a part of the used style.
         * For more information see <a href="https://www.mapbox.com/help/define-font-stack/">the documentation</a>.
         *
         * @param fontStack value
         * @return format option
         */

        public static FormatOption FormatTextFont(string[] fontStack)
        {
            return new FormatOption("text-font", Expression.Literal(fontStack));
        }

        /**
         * If set, the text-color argument overrides the color specified by the root paint properties.
         *
         * @param expression expression
         * @return format option
         */

        public static FormatOption FormatTextColor(Expression expression)
        {
            return new FormatOption("text-color", expression);
        }

        /**
         * If set, the text-color argument overrides the color specified by the root paint properties.
         *
         * @param color value
         * @return format option
         */
        public static FormatOption FormatTextColor(Color color)
        {
            return new FormatOption("text-color", Expression.Color(color));
        }
    }

    ///**
    // * Converts a JsonArray or a raw expression to a Java expression.
    // */
    //public static class Converter
    //{
    //    /**
    //     * Converts a JsonArray to an expression
    //     *
    //     * @param jsonArray the json array to convert
    //     * @return the expression
    //     */
    //    public static Expression Convert(JArray jsonArray)
    //    {
    //        if (jsonArray.Count == 0)
    //        {
    //            throw new InvalidOperationException("Can't convert empty jsonArray expressions");
    //        }

    //        var @operator = jsonArray[0].Value<string>();
    //        var arguments = new List<Expression>();

    //        for (int i = 1; i < jsonArray.Count; i++)
    //        {
    //            var jsonElement = jsonArray[i];
    //            if (@operator.Equals("literal") && jsonElement is JArray jArray)
    //            {
    //                var array = new object[jArray.Count];
    //                for (int j = 0; j < jArray.Count; j++)
    //                {
    //                    var element = jArray[j];
    //                    switch (element.Type)
    //                    {
    //                        case JTokenType.Boolean:
    //                        case JTokenType.Float:
    //                        case JTokenType.Integer:
    //                        case JTokenType.String:
    //                            array[j] = ConvertToValue(element);
    //                            break;
    //                        default:
    //                            throw new InvalidOperationException("Nested literal arrays are not supported.");
    //                    }
    //                }

    //                arguments.Add(new ExpressionLiteralArray(array));
    //            }
    //            else
    //            {
    //                arguments.Add(Convert(jsonElement));
    //            }
    //        }
    //        return new Expression(@operator, arguments.ToArray());
    //    }

    //    /**
    //     * Converts a JsonElement to an expression
    //     *
    //     * @param jsonElement the json element to convert
    //     * @return the expression
    //     */
    //    public static Expression Convert(JToken jsonElement)
    //    {
    //        switch (jsonElement.Type)
    //        {
    //            case JTokenType.Array:
    //                return Convert((JArray)jsonElement);
    //            case JTokenType.None:
    //            case JTokenType.Null:
    //                return new ExpressionLiteral(string.Empty);
    //            case JTokenType.Boolean:
    //            case JTokenType.Float:
    //            case JTokenType.Integer:
    //            case JTokenType.String:
    //            case JTokenType.Uri:
    //                return new ExpressionLiteral(ConvertToValue(jsonElement));
    //            case JTokenType.Object:
    //                var map = new Dictionary<string, Expression>();
    //                var jObject = (JObject)jsonElement;
    //                foreach (var property in jObject.Properties())
    //                {
    //                    map.Add(property.Name, Convert(property.Value));
    //                }
    //                return new ExpressionMap(map);
    //            default:
    //                throw new InvalidOperationException("Unsupported expression conversion for " + jsonElement.Type);
    //        }
    //    }

    //    /**
    //     * Converts a JsonPrimitive to value
    //     *
    //     * @param jsonPrimitive the json primitive to convert
    //     * @return the value
    //     */
    //    private static object ConvertToValue(JToken jsonPrimitive)
    //    {
    //        switch (jsonPrimitive.Type)
    //        {
    //            case JTokenType.Boolean:
    //                return jsonPrimitive.Value<bool>();
    //            case JTokenType.Integer:
    //                return jsonPrimitive.Value<int>();
    //            case JTokenType.Float:
    //                return jsonPrimitive.Value<float>();
    //            case JTokenType.String:
    //                return jsonPrimitive.Value<string>();
    //            case JTokenType.Uri:
    //                return jsonPrimitive.Value<Uri>().ToString();
    //            default:
    //                throw new InvalidOperationException("Unsupported literal expression conversion for " + jsonPrimitive.Type);
    //        }
    //    }

    //    /**
    //     * Converts a raw expression to a DSL equivalent.
    //     *
    //     * @param rawExpression the raw expression to convert
    //     * @return the resulting expression
    //     * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/">Style specification</a>
    //     */
    //    public static Expression Convert(string rawExpression)
    //    {
    //        using (var stringReader = new StringReader(rawExpression))
    //        {
    //            using (var jsonReader = new JsonTextReader(stringReader))
    //            {
    //                return Convert(JArray.Load(jsonReader));
    //            }
    //        }
    //    }
    //}

    /**
     * Expression to wrap object[] as a literal
     */
    public sealed class ExpressionLiteralArray<T> : ExpressionLiteral<IEnumerable<T>>, IEquatable<ExpressionLiteralArray<T>>
    {

        /**
         * Create an expression literal.
         *
         * @param object the object to be treated as literal
         */
        public ExpressionLiteralArray(IEnumerable<T> @object) : base(@object)
        {
        }

        /**
         * Convert the expression array to a string representation.
         *
         * @return the string representation of the expression array
         */

        public override string ToString()
        {
            var array = Value?.ToArray() ?? new T[0];
            var builder = new StringBuilder("[");
            for (int i = 0; i < array.Length; i++)
            {
                object argument = array[i];
                if (argument is string)
                {
                    builder.Append("\"").Append(argument).Append("\"");
                }
                else
                {
                    builder.Append(argument);
                }

                if (i != array.Length - 1)
                {
                    builder.Append(", ");
                }
            }
            builder.Append("]");
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj is ExpressionLiteralArray<T> expressionLiteralArray)
            {
                // TODO Ensure arrays are equal
                return Equals(expressionLiteralArray);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(ExpressionLiteralArray<T> other)
        {
            if (other == null) return false;

            if (base.Equals(other) == false) return false;

            var left = Value;
            var right = other.Value;

            return Enumerable.SequenceEqual(left, right);
        }
    }

    /**
     * Wraps an expression value stored in a Map.
     */
    public sealed class ExpressionMap : Expression, IValueExpression, IEquatable<ExpressionMap>
    {
        public Dictionary<string, Expression> Map { get; private set; }

        public ExpressionMap(Dictionary<string, Expression> map)
        {
            Map = map;
        }

        public object ToValue()
        {
            var unwrappedMap = new Dictionary<string, object>();

            foreach (string key in Map.Keys)
            {
                var expression = Map[key];

                switch (expression)
                {
                    case IValueExpression valueExpression:
                        unwrappedMap[key] = valueExpression.ToValue();
                        break;
                    default:
                        unwrappedMap[key] = expression.ToArray();
                        break;
                }
            }

            return unwrappedMap;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{");
            foreach (string key in Map.Keys)
            {
                builder.Append("\"").Append(key).Append("\": ");
                builder.Append(Map[key]);
                builder.Append(", ");
            }

            if (Map.Count > 0)
            {
                builder.Remove(builder.Length - 2, builder.Length);
            }

            builder.Append("}");
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (!base.Equals(obj))
            {
                return false;
            }

            if (obj is ExpressionMap expressionMap)
            {
                return Equals(expressionMap);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int result = base.GetHashCode();
            result = 31 * result + (Map == null ? 0 : Map.GetHashCode());
            return result;
        }

        public bool Equals(ExpressionMap other)
        {
            if (other == null) return false;

            return ReferenceEquals(Map, other.Map);
        }
    }

    /**
     * Interface used to describe expressions that hold a Java value.
     */
    interface IValueExpression
    {
        object ToValue();
    }
}
