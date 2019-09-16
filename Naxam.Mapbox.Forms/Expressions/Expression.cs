using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Naxam.Mapbox.Expressions
{
    public partial class Expression
    {
        public string Operator { get; private set; }

        public Expression[] Arguments { get; private set; }

        /**
         * Creates an empty expression for expression literals
         */
        protected Expression()
        {
            Operator = null;
            Arguments = null;
        }

        /**
         * Creates an expression from its @operator and varargs expressions.
         *
         * @param @operator  the expression @operator
         * @param arguments expressions input
         */
        public Expression(string @operator, params Expression[] arguments)
        {
            this.Operator = @operator;
            this.Arguments = arguments;
        }

        /**
         * Create a literal number expression.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Literal(10.0f))
         * );
         * }
         * </pre>
         *
         * @param number the number
         * @return the expression
         */
        public static Expression Literal(double number)
        {
            // TODO Add any other number types
            return new ExpressionLiteral(number);
        }
        public static Expression Literal(int number)
        {
            // TODO Add any other number types
            return new ExpressionLiteral(number);
        }

        /**
         * Create a literal string expression.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(Literal("Text"))
         * );
         * }
         * </pre>
         *
         * @param string the string
         * @return the expression
         */
        public static Expression Literal(string @string)
        {
            return new ExpressionLiteral(@string);
        }

        /**
         * Create a literal bool expression.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillAntialias(Literal(true))
         * );
         * }
         * </pre>
         *
         * @param bool the bool
         * @return the expression
         */
        public static Expression Literal(bool @bool)
        {
            return new ExpressionLiteral(@bool);
        }

        /**
         * Create a literal object expression.
         *
         * @param object the object
         * @return the expression
         */
        public static Expression Literal(object @object)
        {
            switch (@object)
            {
                case IEnumerable enumberable:
                    return Literal(ToObjectArray(enumberable));
                case Expression expression:
                    throw new InvalidOperationException("Can't convert an expression to a literal");
            }

            return new ExpressionLiteral(@object);
        }

        /**
         * Create a literal array expression
         *
         * @param array the array
         * @return the expression
         */
        public static Expression Literal(IEnumerable array)
        {
            return new Expression("literal", new ExpressionLiteralArray(array));
        }

        /**
         * Expression literal utility method to convert a color int to an color expression
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(Color(Color.GREEN))
         * );
         * }
         * </pre>
         *
         * @param color the int color
         * @return the color expression
         */
        public static Expression Color(Color color)
        {
            return Rgba(color.R, color.G, color.B, color.A);
        }

        /**
         * Creates a color value from red, green, and blue components, which must range between 0 and 255,
         * and an alpha component of 1.
         * <p>
         * If any component is out of range, the expression is an error.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *         rgb(
         *             Literal(255.0f),
         *             Literal(255.0f),
         *             Literal(255.0f)
         *         )
         *     )
         * );
         * }
         * </pre>
         *
         * @param red   red color expression
         * @param green green color expression
         * @param blue  blue color expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-rgb">Style specification</a>
         */
        public static Expression Rgb(Expression red, Expression green, Expression blue)
        {
            return new Expression("rgb", red, green, blue);
        }

        /**
         * Creates a color value from red, green, and blue components, which must range between 0 and 255,
         * and an alpha component of 1.
         * <p>
         * If any component is out of range, the expression is an error.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *         rgb(255.0f, 255.0f, 255.0f)
         *     )
         * );
         * }
         * </pre>
         *
         * @param red   red color value
         * @param green green color value
         * @param blue  blue color value
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-rgb">Style specification</a>
         */
        public static Expression Rgb(int red, int green, int blue)
        {
            return Rgb(Literal(red), Literal(green), Literal(blue));
        }

        /**
         * Creates a color value from red, green, blue components, which must range between 0 and 255,
         * and an alpha component which must range between 0 and 1.
         * <p>
         * If any component is out of range, the expression is an error.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *         rgba(
         *             Literal(255.0f),
         *             Literal(255.0f),
         *             Literal(255.0f),
         *             Literal(1.0f)
         *         )
         *     )
         * );
         * }
         * </pre>
         *
         * @param red   red color value
         * @param green green color value
         * @param blue  blue color value
         * @param alpha alpha color value
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-rgba">Style specification</a>
         */
        public static Expression Rgba(Expression red, Expression green,
                                      Expression blue, Expression alpha)
        {
            return new Expression("rgba", red, green, blue, alpha);
        }

        /**
         * Creates a color value from red, green, blue components, which must range between 0 and 255,
         * and an alpha component which must range between 0 and 1.
         * <p>
         * If any component is out of range, the expression is an error.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *         rgba(255.0f, 255.0f, 255.0f, 1.0f)
         *     )
         * );
         * }
         * </pre>
         *
         * @param red   red color value
         * @param green green color value
         * @param blue  blue color value
         * @param alpha alpha color value
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-rgba">Style specification</a>
         */
        public static Expression Rgba(double red, double green, double blue, double alpha)
        {
            return Rgba(Literal(red), Literal(green), Literal(blue), Literal(alpha));
        }

        /**
         * Returns a four-element array containing the input color's red, green, blue, and alpha components, in that order.
         *
         * @param expression an expression to convert to a color
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-to-rgba">Style specification</a>
         */
        public static Expression ToRgba(Expression expression)
        {
            return new Expression("to-rgba", expression);
        }

        /**
         * Returns true if the input values are equal, false otherwise.
         * The inputs must be numbers, strings, or booleans, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Eq(get("keyToValue"), Get("keyToOtherValue"))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-==">Style specification</a>
         */
        public static Expression Eq(Expression compareOne, Expression compareTwo)
        {
            return new Expression("==", compareOne, compareTwo);
        }

        /**
         * Returns true if the input values are equal, false otherwise.
         * The inputs must be numbers, strings, or booleans, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Eq(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-==">Style specification</a>
         */
        public static Expression Eq(Expression compareOne, Expression compareTwo,
                                    Expression collator)
        {
            return new Expression("==", compareOne, compareTwo, collator);
        }

        /**
         * Returns true if the input values are equal, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Eq(get("keyToValue"), true)
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second bool
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-==">Style specification</a>
         */
        public static Expression Eq(Expression compareOne, bool compareTwo)
        {
            return Eq(compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the input values are equal, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Eq(get("keyToValue"), "value")
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-==">Style specification</a>
         */
        public static Expression Eq(Expression compareOne, string compareTwo)
        {
            return Eq(compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the input values are equal, false otherwise.
         * The inputs must be numbers, strings, or booleans, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Eq(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-==">Style specification</a>
         */
        public static Expression Eq(Expression compareOne, string compareTwo,
                                    Expression collator)
        {
            return Eq(compareOne, Literal(compareTwo), collator);
        }

        /**
         * Returns true if the input values are equal, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Eq(get("keyToValue"), 2.0f)
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-==">Style specification</a>
         */
        public static Expression Eq(Expression compareOne, double compareTwo)
        {
            return Eq(compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the input values are not equal, false otherwise.
         * The inputs must be numbers, strings, or booleans, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Neq(get("keyToValue"), Get("keyToOtherValue"))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-!=">Style specification</a>
         */
        public static Expression Neq(Expression compareOne, Expression compareTwo)
        {
            return new Expression("!=", compareOne, compareTwo);
        }

        /**
         * Returns true if the input values are not equal, false otherwise.
         * The inputs must be numbers, strings, or booleans, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Neq(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-!=">Style specification</a>
         */
        public static Expression Neq(Expression compareOne, Expression compareTwo,
                                     Expression collator)
        {
            return new Expression("!=", compareOne, compareTwo, collator);
        }

        /**
         * Returns true if the input values are equal, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Neq(get("keyToValue"), true)
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second bool
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-!=">Style specification</a>
         */
        public static Expression Neq(Expression compareOne, bool compareTwo)
        {
            return new Expression("!=", compareOne, Literal(compareTwo));
        }

        /**
         * Returns `true` if the input values are not equal, `false` otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Neq(get("keyToValue"), "value")
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-!=">Style specification</a>
         */
        public static Expression Neq(Expression compareOne, string compareTwo)
        {
            return new Expression("!=", compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the input values are not equal, false otherwise.
         * The inputs must be numbers, strings, or booleans, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Neq(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-!=">Style specification</a>
         */
        public static Expression Neq(Expression compareOne, string compareTwo,
                                     Expression collator)
        {
            return new Expression("!=", compareOne, Literal(compareTwo), collator);
        }

        /**
         * Returns `true` if the input values are not equal, `false` otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Neq(get("keyToValue"), 2.0f)
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-!=">Style specification</a>
         */
        public static Expression Neq(Expression compareOne, double compareTwo)
        {
            return new Expression("!=", compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the first input is strictly greater than the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Gt(get("keyToValue"), Get("keyToOtherValue"))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3E">Style specification</a>
         */
        public static Expression Gt(Expression compareOne, Expression compareTwo)
        {
            return new Expression(">", compareOne, compareTwo);
        }

        /**
         * Returns true if the first input is strictly greater than the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Gt(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3E">Style specification</a>
         */
        public static Expression Gt(Expression compareOne, Expression compareTwo,
                                    Expression collator)
        {
            return new Expression(">", compareOne, compareTwo, collator);
        }

        /**
         * Returns true if the first input is strictly greater than the second, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Gt(get("keyToValue"), 2.0f)
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3E">Style specification</a>
         */
        public static Expression Gt(Expression compareOne, double compareTwo)
        {
            return new Expression(">", compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the first input is strictly greater than the second, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Gt(get("keyToValue"), "value")
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3E">Style specification</a>
         */
        public static Expression Gt(Expression compareOne, string compareTwo)
        {
            return new Expression(">", compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the first input is strictly greater than the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Gt(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3E">Style specification</a>
         */
        public static Expression Gt(Expression compareOne, string compareTwo,
                                    Expression collator)
        {
            return new Expression(">", compareOne, Literal(compareTwo), collator);
        }

        /**
         * Returns true if the first input is strictly less than the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Lt(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3C">Style specification</a>
         */
        public static Expression Lt(Expression compareOne, Expression compareTwo)
        {
            return new Expression("<", compareOne, compareTwo);
        }

        /**
         * Returns true if the first input is strictly less than the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Lt(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second number
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3C">Style specification</a>
         */
        public static Expression Lt(Expression compareOne, Expression compareTwo,
                                    Expression collator)
        {
            return new Expression("<", compareOne, compareTwo, collator);
        }

        /**
         * Returns true if the first input is strictly less than the second, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Lt(get("keyToValue"), 2.0f)
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3C">Style specification</a>
         */
        public static Expression Lt(Expression compareOne, double compareTwo)
        {
            return new Expression("<", compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the first input is strictly less than the second, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Lt(get("keyToValue"), "value")
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3C">Style specification</a>
         */
        public static Expression Lt(Expression compareOne, string compareTwo)
        {
            return new Expression("<", compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the first input is strictly less than the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Lt(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3C">Style specification</a>
         */
        public static Expression Lt(Expression compareOne, string compareTwo,
                                    Expression collator)
        {
            return new Expression("<", compareOne, Literal(compareTwo), collator);
        }

        /**
         * Returns true if the first input is greater than or equal to the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     gte(get("keyToValue"), Get("keyToOtherValue"))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3E%3D">Style specification</a>
         */
        public static Expression Gte(Expression compareOne, Expression compareTwo)
        {
            return new Expression(">=", compareOne, compareTwo);
        }

        /**
         * Returns true if the first input is greater than or equal to the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     gte(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3E%3D">Style specification</a>
         */
        public static Expression Gte(Expression compareOne, Expression compareTwo,
                                     Expression collator)
        {
            return new Expression(">=", compareOne, compareTwo, collator);
        }

        /**
         * Returns true if the first input is greater than or equal to the second, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     gte(get("keyToValue"), 2.0f)
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3E%3D">Style specification</a>
         */
        public static Expression Gte(Expression compareOne, double compareTwo)
        {
            return new Expression(">=", compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the first input is greater than or equal to the second, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Neq(get("keyToValue"), "value")
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3E%3D">Style specification</a>
         */
        public static Expression Gte(Expression compareOne, string compareTwo)
        {
            return new Expression(">=", compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the first input is greater than or equal to the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     gte(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3E%3D">Style specification</a>
         */
        public static Expression Gte(Expression compareOne, string compareTwo,
                                     Expression collator)
        {
            return new Expression(">=", compareOne, Literal(compareTwo), collator);
        }

        /**
         * Returns true if the first input is less than or equal to the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Lte(get("keyToValue"), Get("keyToOtherValue"))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3C%3D">Style specification</a>
         */
        public static Expression Lte(Expression compareOne, Expression compareTwo)
        {
            return new Expression("<=", compareOne, compareTwo);
        }

        /**
         * Returns true if the first input is less than or equal to the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Lte(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second expression
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3C%3D">Style specification</a>
         */
        public static Expression Lte(Expression compareOne, Expression compareTwo,
                                     Expression collator)
        {
            return new Expression("<=", compareOne, compareTwo, collator);
        }

        /**
         * Returns true if the first input is less than or equal to the second, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Lte(get("keyToValue"), 2.0f)
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3C%3D">Style specification</a>
         */
        public static Expression Lte(Expression compareOne, double compareTwo)
        {
            return new Expression("<=", compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the first input is less than or equal to the second, false otherwise.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Lte(get("keyToValue"), "value")
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3C%3D">Style specification</a>
         */
        public static Expression Lte(Expression compareOne, string compareTwo)
        {
            return new Expression("<=", compareOne, Literal(compareTwo));
        }

        /**
         * Returns true if the first input is less than or equal to the second, false otherwise.
         * The inputs must be numbers or strings, and both of the same type.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Lte(get("keyToValue"), Get("keyToOtherValue"), Collator(true, false))
         * );
         * }
         * </pre>
         *
         * @param compareOne the first expression
         * @param compareTwo the second string
         * @param collator   the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%3C%3D">Style specification</a>
         */
        public static Expression Lte(Expression compareOne, string compareTwo,
                                     Expression collator)
        {
            return new Expression("<=", compareOne, Literal(compareTwo), collator);
        }

        /**
         * Returns `true` if all the inputs are `true`, `false` otherwise.
         * <p>
         * The inputs are evaluated in order, and evaluation is short-circuiting:
         * once an input expression evaluates to `false`,
         * the result is `false` and no further input expressions are evaluated.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     all(get("keyToValue"), Get("keyToOtherValue"))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-all">Style specification</a>
         */
        public static Expression All(params Expression[] input)
        {
            return new Expression("all", input);
        }

        /**
         * Returns `true` if any of the inputs are `true`, `false` otherwise.
         * <p>
         * The inputs are evaluated in order, and evaluation is short-circuiting:
         * once an input expression evaluates to `true`,
         * the result is `true` and no further input expressions are evaluated.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Any(get("keyToValue"), Get("keyToOtherValue"))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-any">Style specification</a>
         */
        public static Expression Any(params Expression[] input)
        {
            return new Expression("any", input);
        }

        /**
         * Logical negation. Returns `true` if the input is `false`, and `false` if the input is `true`.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Not(get("keyToValue"))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-!">Style specification</a>
         */
        public static Expression Not(Expression input)
        {
            return new Expression("!", input);
        }

        /**
         * Logical negation. Returns `true` if the input is `false`, and `false` if the input is `true`.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Not(false)
         * );
         * }
         * </pre>
         *
         * @param input bool input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-!">Style specification</a>
         */
        public static Expression Not(bool input)
        {
            return Not(Literal(input));
        }

        /**
         * Selects the first output whose corresponding test condition evaluates to true.
         * <p>
         * For each case a condition and an output should be provided.
         * The last parameter should provide the default output.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     iconSize(
         *         SwitchCase(
         *             Get(KEY_TO_BOOLEAN), Literal(3.0f),
         *             Get(KEY_TO_OTHER_BOOLEAN), Literal(5.0f),
         *             Literal(1.0f) // default value
         *         )
         *     )
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-case">Style specification</a>
         */
        public static Expression SwitchCase(params Expression[] input)
        {
            return new Expression("case", input);
        }

        /**
         * Selects the output whose label value matches the input value, or the fallback value if no match is found.
         * The `input` can be any string or number expression.
         * Each label can either be a single literal value or an array of values.
         * If types of the input and keys don't match, or the input value doesn't exist,
         * the expresion will fail without falling back to the default value.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textColor(
         *         Match(get("keyToValue"),
         *             Literal(1), rgba(255, 0, 0, 1.0f),
         *             Literal(2), rgba(0, 0, 255.0f, 1.0f),
         *             rgba(0.0f, 255.0f, 0.0f, 1.0f)
         *         )
         *     )
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-match">Style specification</a>
         */
        public static Expression Match(params Expression[] input)
        {
            return new Expression("match", input);
        }

        /**
         * Selects the output whose label value matches the input value, or the fallback value if no match is found.
         * The `input` can be any string or number expression.
         * Each label can either be a single literal value or an array of values.
         * If types of the input and keys don't match, or the input value doesn't exist,
         * the expresion will fail without falling back to the default value.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *   textColor(
         *     Match(get("keyToValue"), rgba(0.0f, 255.0f, 0.0f, 1.0f),
         *       stop(1f, rgba(255, 0, 0, 1.0f)),
         *       stop(2f, rgba(0, 0, 255.0f, 1.0f))
         *     )
         *   )
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-match">Style specification</a>
         */
        public static Expression Match(Expression input, Expression defaultOutput, params Stop[] stops)
        {
            return Match(Join(Join(new Expression[] { input }, Naxam.Mapbox.Expressions.Stop.ToExpressionArray(stops)), new Expression[] { defaultOutput }));
        }

        /**
         * Evaluates each expression in turn until the first non-null value is obtained, and returns that value.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textColor(
         *         Coalesce(
         *             Get("keyToNullValue"),
         *             Get("keyToNonNullValue")
         *         )
         *     )
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-coalesce">Style specification</a>
         */
        public static Expression Coalesce(params Expression[] input)
        {
            return new Expression("coalesce", input);
        }

        /**
         * Gets the feature properties object.
         * <p>
         * Note that in some cases, it may be more efficient to use {@link #get(Expression)}} instead.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(get("key-to-value", Properties()))
         * );
         * }
         * </pre>
         *
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-properties">Style specification</a>
         */
        public static Expression Properties()
        {
            return new Expression("properties");
        }

        /**
         * Gets the feature's geometry type: Point, MultiPoint, LineString, MultiLineString, Polygon, MultiPolygon.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(Concat(get("key-to-value"), Literal(" "), geometryType()))
         * );
         * }
         * </pre>
         *
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-geometry-types">Style specification</a>
         */
        public static Expression GeometryType()
        {
            return new Expression("geometry-type");
        }

        /**
         * Gets the feature's id, if it has one.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(id())
         * );
         * }
         * </pre>
         *
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-id">Style specification</a>
         */
        public static Expression Id()
        {
            return new Expression("id");
        }

        /**
         * Gets the kernel density estimation of a pixel in a heatmap layer,
         * which is a relative measure of how many data points are crowded around a particular pixel.
         * Can only be used in the `heatmap-color` property.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * HeatmapLayer layer = new HeatmapLayer("layer-id", "source-id");
         * layer.setProperties(
         *     heatmapColor(interpolate(linear(), heatmapDensity(),
         *         Literal(0), rgba(33, 102, 172, 0),
         *         Literal(0.2), rgb(103, 169, 207),
         *         Literal(0.4), rgb(209, 229, 240),
         *         Literal(0.6), rgb(253, 219, 199),
         *         Literal(0.8), rgb(239, 138, 98),
         *         Literal(1), rgb(178, 24, 43))
         *     )
         * );
         * }
         * </pre>
         *
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-heatmap-density">Style specification</a>
         */
        public static Expression HeatmapDensity()
        {
            return new Expression("heatmap-density");
        }

        /**
         * Gets the progress along a gradient line. Can only be used in the line-gradient property.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * LineLayer layer = new LineLayer("layer-id", "source-id");
         * layer.setProperties(
         *     lineGradient(interpolate(
         *         linear(), lineProgress(),
         *         stop(0f, rgb(0, 0, 255)),
         *         stop(0.5f, rgb(0, 255, 0)),
         *         stop(1f, rgb(255, 0, 0)))
         *     )
         * );
         * }
         * </pre>
         *
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-line-progress">Style specification</a>
         */
        public static Expression LineProgress()
        {
            return new Expression("line-progress");
        }

        /**
         * Retrieves an item from an array.
         *
         * @param number     the index expression
         * @param expression the array expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-at">Style specification</a>
         */
        public static Expression At(Expression number, Expression expression)
        {
            return new Expression("at", number, expression);
        }

        /**
         * Retrieves an item from an array.
         *
         * @param number     the index expression
         * @param expression the array expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-at">Style specification</a>
         */
        public static Expression At(double number, Expression expression)
        {
            return At(Literal(number), expression);
        }

        /**
         * Retrieves a property value from the current feature's properties,
         * or from another object if a second argument is provided.
         * Returns null if the requested property is missing.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(get("key-to-feature"))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-get">Style specification</a>
         */
        public static Expression Get(Expression input)
        {
            return new Expression("get", input);
        }

        /**
         * Retrieves a property value from the current feature's properties,
         * or from another object if a second argument is provided.
         * Returns null if the requested property is missing.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(get("key-to-feature"))
         * );
         * }
         * </pre>
         *
         * @param input string input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-get">Style specification</a>
         */
        public static Expression Get(string input)
        {
            return Get(Literal(input));
        }

        /**
         * Retrieves a property value from another object.
         * Returns null if the requested property is missing.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(get("key-to-property", Get("key-to-object")))
         * );
         * }
         * </pre>
         *
         * @param key    a property value key
         * @param object an expression object
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-get">Style specification</a>
         */
        public static Expression Get(Expression key, Expression @object)
        {
            return new Expression("get", key, @object);
        }

        /**
         * Retrieves a property value from another object.
         * Returns null if the requested property is missing.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(get("key-to-property", Get("key-to-object")))
         * );
         * }
         * </pre>
         *
         * @param key    a property value key
         * @param object an expression object
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-get">Style specification</a>
         */
        public static Expression Get(string key, Expression @object)
        {
            return Get(Literal(key), @object);
        }

        /**
         * Tests for the presence of an property value in the current feature's properties.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Has(get("keyToValue"))
         * );
         * }
         * </pre>
         *
         * @param key the expression property value key
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-has">Style specification</a>
         */
        public static Expression Has(Expression key)
        {
            return new Expression("has", key);
        }

        /**
         * Tests for the presence of an property value in the current feature's properties.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Has("keyToValue")
         * );
         * }
         * </pre>
         *
         * @param key the property value key
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-has">Style specification</a>
         */
        public static Expression Has(string key)
        {
            return Has(Literal(key));
        }

        /**
         * Tests for the presence of an property value from another object.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Has(get("keyToValue"), Get("keyToObject"))
         * );
         * }
         * </pre>
         *
         * @param key    the expression property value key
         * @param object an expression object
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-has">Style specification</a>
         */
        public static Expression Has(Expression key, Expression @object)
        {
            return new Expression("has", key, @object);
        }

        /**
         * Tests for the presence of an property value from another object.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setFilter(
         *     Has("keyToValue", Get("keyToObject"))
         * );
         * }
         * </pre>
         *
         * @param key    the property value key
         * @param object an expression object
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-has">Style specification</a>
         */
        public static Expression Has(string key, Expression @object)
        {
            return Has(Literal(key), @object);
        }

        /**
         * Gets the length of an array or string.
         *
         * @param expression an expression object or expression string
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-lenght">Style specification</a>
         */
        public static Expression Length(Expression expression)
        {
            return new Expression("length", expression);
        }

        /**
         * Gets the length of an array or string.
         *
         * @param input a string
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-lenght">Style specification</a>
         */
        public static Expression Length(string input)
        {
            return Length(Literal(input));
        }

        /**
         * Returns mathematical constant Ln(2).
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(product(Literal(10.0f), Ln2()))
         * );
         * }
         * </pre>
         *
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-ln2">Style specification</a>
         */
        public static Expression Ln2()
        {
            return new Expression("ln2");
        }

        /**
         * Returns the mathematical constant pi.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(product(Literal(10.0f), pi()))
         * );
         * }
         * </pre>
         *
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-pi">Style specification</a>
         */
        public static Expression Pi()
        {
            return new Expression("pi");
        }

        /**
         * Returns the mathematical constant e.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(product(Literal(10.0f), e()))
         * );
         * }
         * </pre>
         *
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-e">Style specification</a>
         */
        public static Expression E()
        {
            return new Expression("e");
        }

        /**
         * Returns the sum of the inputs.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(sum(Literal(10.0f), Ln2(), pi()))
         * );
         * }
         * </pre>
         *
         * @param numbers the numbers to calculate the sum for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-+">Style specification</a>
         */
        public static Expression Sum(params Expression[] numbers)
        {
            return new Expression("+", numbers);
        }

        /**
         * Returns the sum of the inputs.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(sum(10.0f, 5.0f, 3.0f))
         * );
         * }
         * </pre>
         *
         * @param numbers the numbers to calculate the sum for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-+">Style specification</a>
         */
        public static Expression Sum(params double[] numbers)
        {
            var numberExpression = new Expression[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                numberExpression[i] = Literal(numbers[i]);
            }
            return Sum(numberExpression);
        }

        /**
         * Returns the product of the inputs.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(product(Literal(10.0f), Ln2()))
         * );
         * }
         * </pre>
         *
         * @param numbers the numbers to calculate the product for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-*">Style specification</a>
         */
        public static Expression Product(params Expression[] numbers)
        {
            return new Expression("*", numbers);
        }

        /**
         * Returns the product of the inputs.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(product(10.0f, 2.0f))
         * );
         * }
         * </pre>
         *
         * @param numbers the numbers to calculate the product for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-*">Style specification</a>
         */
        public static Expression Product(params double[] numbers)
        {
            var numberExpression = new Expression[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                numberExpression[i] = Literal(numbers[i]);
            }
            return Product(numberExpression);
        }

        /**
         * Returns the result of subtracting a number from 0.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Subtract(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number subtract from 0
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions--">Style specification</a>
         */
        public static Expression Subtract(Expression number)
        {
            return new Expression("-", number);
        }

        /**
         * Returns the result of subtracting a number from 0.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Subtract(10.0f))
         * );
         * }
         * </pre>
         *
         * @param number the number subtract from 0
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions--">Style specification</a>
         */
        public static Expression Subtract(double number)
        {
            return Subtract(Literal(number));
        }

        /**
         * Returns the result of subtracting the second input from the first.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Subtract(Literal(10.0f), pi()))
         * );
         * }
         * </pre>
         *
         * @param first  the first number
         * @param second the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions--">Style specification</a>
         */
        public static Expression Subtract(Expression first, Expression second)
        {
            return new Expression("-", first, second);
        }

        /**
         * Returns the result of subtracting the second input from the first.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Subtract(10.0f, 20.0f))
         * );
         * }
         * </pre>
         *
         * @param first  the first number
         * @param second the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions--">Style specification</a>
         */
        public static Expression Subtract(double first, double second)
        {
            return Subtract(Literal(first), Literal(second));
        }

        /**
         * Returns the result of floating point division of the first input by the second.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Division(Literal(10.0f), pi()))
         * );
         * }
         * </pre>
         *
         * @param first  the first number
         * @param second the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-/">Style specification</a>
         */
        public static Expression Division(Expression first, Expression second)
        {
            return new Expression("/", first, second);
        }

        /**
         * Returns the result of floating point division of the first input by the second.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Division(10.0f, 20.0f))
         * );
         * }
         * </pre>
         *
         * @param first  the first number
         * @param second the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-/">Style specification</a>
         */
        public static Expression Division(double first, double second)
        {
            return Division(Literal(first), Literal(second));
        }

        /**
         * Returns the remainder after integer division of the first input by the second.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Mod(Literal(10.0f), pi()))
         * );
         * }
         * </pre>
         *
         * @param first  the first number
         * @param second the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%25">Style specification</a>
         */
        public static Expression Mod(Expression first, Expression second)
        {
            return new Expression("%", first, second);
        }

        /**
         * Returns the remainder after integer division of the first input by the second.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Mod(10.0f, 10.0f))
         * );
         * }
         * </pre>
         *
         * @param first  the first number
         * @param second the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%25">Style specification</a>
         */
        public static Expression Mod(double first, double second)
        {
            return Mod(Literal(first), Literal(second));
        }

        /**
         * Returns the result of raising the first input to the power specified by the second.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(pow(pi(), Literal(2.0f)))
         * );
         * }
         * </pre>
         *
         * @param first  the first number
         * @param second the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%5E">Style specification</a>
         */
        public static Expression Pow(Expression first, Expression second)
        {
            return new Expression("^", first, second);
        }

        /**
         * Returns the result of raising the first input to the power specified by the second.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(pow(5.0f, 2.0f))
         * );
         * }
         * </pre>
         *
         * @param first  the first number
         * @param second the second number
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-%5E">Style specification</a>
         */
        public static Expression Pow(double first, double second)
        {
            return Pow(Literal(first), Literal(second));
        }

        /**
         * Returns the square root of the input
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Sqrt(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number to take the square root from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-sqrt">Style specification</a>
         */
        public static Expression Sqrt(Expression number)
        {
            return new Expression("sqrt", number);
        }

        /**
         * Returns the square root of the input
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Sqrt(25.0f))
         * );
         * }
         * </pre>
         *
         * @param number the number to take the square root from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-sqrt">Style specification</a>
         */
        public static Expression Sqrt(double number)
        {
            return Sqrt(Literal(number));
        }

        /**
         * Returns the base-ten logarithm of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Log10(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number to take base-ten logarithm from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-log10">Style specification</a>
         */
        public static Expression Log10(Expression number)
        {
            return new Expression("log10", number);
        }

        /**
         * Returns the base-ten logarithm of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Log10(10))
         * );
         * }
         * </pre>
         *
         * @param number the number to take base-ten logarithm from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-log10">Style specification</a>
         */
        public static Expression Log10(double number)
        {
            return Log10(Literal(number));
        }

        /**
         * Returns the natural logarithm of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Ln(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number to take natural logarithm from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-ln">Style specification</a>
         */
        public static Expression Ln(Expression number)
        {
            return new Expression("ln", number);
        }

        /**
         * Returns the natural logarithm of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Ln(10))
         * );
         * }
         * </pre>
         *
         * @param number the number to take natural logarithm from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-ln">Style specification</a>
         */
        public static Expression Ln(double number)
        {
            return Ln(Literal(number));
        }

        /**
         * Returns the base-two logarithm of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Log2(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number to take base-two logarithm from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-log2">Style specification</a>
         */
        public static Expression Log2(Expression number)
        {
            return new Expression("log2", number);
        }

        /**
         * Returns the base-two logarithm of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Log2(2))
         * );
         * }
         * </pre>
         *
         * @param number the number to take base-two logarithm from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-log2">Style specification</a>
         */
        public static Expression Log2(double number)
        {
            return Log2(Literal(number));
        }

        /**
         * Returns the sine of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Sin(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the sine for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-sin">Style specification</a>
         */
        public static Expression Sin(Expression number)
        {
            return new Expression("sin", number);
        }

        /**
         * Returns the sine of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Sin(90.0f))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the sine for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-sin">Style specification</a>
         */
        public static Expression Sin(double number)
        {
            return Sin(Literal(number));
        }

        /**
         * Returns the cosine of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Cos(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the cosine for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-cos">Style specification</a>
         */
        public static Expression Cos(Expression number)
        {
            return new Expression("cos", number);
        }

        /**
         * Returns the cosine of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Cos(0))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the cosine for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-cos">Style specification</a>
         */
        public static Expression Cos(double number)
        {
            return new Expression("cos", Literal(number));
        }

        /**
         * Returns the tangent of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Tan(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the tangent for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-tan">Style specification</a>
         */
        public static Expression Tan(Expression number)
        {
            return new Expression("tan", number);
        }

        /**
         * Returns the tangent of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Tan(45.0f))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the tangent for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-tan">Style specification</a>
         */
        public static Expression Tan(double number)
        {
            return new Expression("tan", Literal(number));
        }

        /**
         * Returns the arcsine of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Asin(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the arcsine for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-asin">Style specification</a>
         */
        public static Expression Asin(Expression number)
        {
            return new Expression("asin", number);
        }

        /**
         * Returns the arcsine of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Asin(90))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the arcsine for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-asin">Style specification</a>
         */
        public static Expression Asin(double number)
        {
            return Asin(Literal(number));
        }

        /**
         * Returns the arccosine of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Acos(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the arccosine for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-acos">Style specification</a>
         */
        public static Expression Acos(Expression number)
        {
            return new Expression("acos", number);
        }

        /**
         * Returns the arccosine of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Acos(0))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the arccosine for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-acos">Style specification</a>
         */
        public static Expression Acos(double number)
        {
            return Acos(Literal(number));
        }

        /**
         * Returns the arctangent of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Asin(pi()))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the arctangent for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-atan">Style specification</a>
         */
        public static Expression Atan(Expression number)
        {
            return new Expression("atan", number);
        }

        /**
         * Returns the arctangent of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Atan(90))
         * );
         * }
         * </pre>
         *
         * @param number the number to calculate the arctangent for
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-atan">Style specification</a>
         */
        public static Expression Atan(double number)
        {
            return Atan(Literal(number));
        }

        /**
         * Returns the minimum value of the inputs.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Min(pi(), Literal(3.14f), Literal(3.15f)))
         * );
         * }
         * </pre>
         *
         * @param numbers varargs of numbers to get the minimum from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-min">Style specification</a>
         */
        public static Expression Min(params Expression[] numbers)
        {
            return new Expression("min", numbers);
        }

        /**
         * Returns the minimum value of the inputs.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Min(3.141, 3.14f, 3.15f))
         * );
         * }
         * </pre>
         *
         * @param numbers varargs of numbers to get the minimum from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-min">Style specification</a>
         */
        public static Expression Min(params double[] numbers)
        {
            var numberExpression = new Expression[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                numberExpression[i] = Literal(numbers[i]);
            }
            return Min(numberExpression);
        }

        /**
         * Returns the maximum value of the inputs.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Max(pi(), product(pi(), pi())))
         * );
         * }
         * </pre>
         *
         * @param numbers varargs of numbers to get the maximum from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-max">Style specification</a>
         */
        public static Expression Max(params Expression[] numbers)
        {
            return new Expression("max", numbers);
        }

        /**
         * Returns the maximum value of the inputs.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Max(3.141, 3.14f, 3.15f))
         * );
         * }
         * </pre>
         *
         * @param numbers varargs of numbers to get the maximum from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-max">Style specification</a>
         */
        public static Expression Max(params double[] numbers)
        {
            var numberExpression = new Expression[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                numberExpression[i] = Literal(numbers[i]);
            }
            return Max(numberExpression);
        }

        /**
         * Rounds the input to the nearest integer.
         * Halfway values are rounded away from zero.
         * For example `[\"round\", -1.5]` evaluates to -2.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Round(pi()))
         * );
         * }
         * </pre>
         *
         * @param expression number expression to round
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-round">Style specification</a>
         */
        public static Expression Round(Expression expression)
        {
            return new Expression("round", expression);
        }

        /**
         * Rounds the input to the nearest integer.
         * Halfway values are rounded away from zero.
         * For example `[\"round\", -1.5]` evaluates to -2.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Round(3.14159265359f))
         * );
         * }
         * </pre>
         *
         * @param number number to round
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-round">Style specification</a>
         */
        public static Expression Round(double number)
        {
            return Round(Literal(number));
        }

        /**
         * Returns the absolute value of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Abs(Subtract(pi())))
         * );
         * }
         * </pre>
         *
         * @param expression number expression to get absolute value from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-abs">Style specification</a>
         */
        public static Expression Abs(Expression expression)
        {
            return new Expression("abs", expression);
        }

        /**
         * Returns the absolute value of the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Abs(-3.14159265359f))
         * );
         * }
         * </pre>
         *
         * @param number number to get absolute value from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-abs">Style specification</a>
         */
        public static Expression Abs(double number)
        {
            return Abs(Literal(number));
        }

        /**
         * Returns the smallest integer that is greater than or equal to the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Ceil(pi()))
         * );
         * }
         * </pre>
         *
         * @param expression number expression to get value from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-abs">Style specification</a>
         */
        public static Expression Ceil(Expression expression)
        {
            return new Expression("ceil", expression);
        }

        /**
         * Returns the smallest integer that is greater than or equal to the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Ceil(3.14159265359))
         * );
         * }
         * </pre>
         *
         * @param number number to get value from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-abs">Style specification</a>
         */
        public static Expression Ceil(double number)
        {
            return Ceil(Literal(number));
        }

        /**
         * Returns the largest integer that is less than or equal to the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Floor(pi()))
         * );
         * }
         * </pre>
         *
         * @param expression number expression to get value from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-abs">Style specification</a>
         */
        public static Expression Floor(Expression expression)
        {
            return new Expression("floor", expression);
        }

        /**
         * Returns the largest integer that is less than or equal to the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(Floor(pi()))
         * );
         * }
         * </pre>
         *
         * @param number number to get value from
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-abs">Style specification</a>
         */
        public static Expression Floor(double number)
        {
            return Floor(Literal(number));
        }

        /**
         * Returns the IETF language tag of the locale being used by the provided collator.
         * This can be used to determine the default system locale,
         * or to determine if a requested locale was successfully loaded.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         * circleColor(switchCase(
         * Eq(Literal("it"), resolvedLocale(Collator(true, true, Locale.ITALY))), Literal(ColorUtils.colorToRgbaString
         * (Color.GREEN)),
         * Literal(ColorUtils.colorToRgbaString(Color.RED))))
         * );
         * }
         * </pre>
         *
         * @param collator the collator expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-resolved-locale">Style specification</a>
         */
        public static Expression ResolvedLocale(Expression collator)
        {
            return new Expression("resolved-locale", collator);
        }

        /**
         * Returns true if the input string is expected to render legibly.
         * Returns false if the input string contains sections that cannot be rendered without potential loss of meaning
         * (e.g. Indic scripts that require complex text shaping,
         * or right-to-left scripts if the the mapbox-gl-rtl-text plugin is not in use in Mapbox GL JS).
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * mapboxMap.getStyle().addLayer(new SymbolLayer("layer-id", "source-id")
         *   .withProperties(
         *     textField(
         *       SwitchCase(
         *         IsSupportedScript(get("name_property")), Get("name_property"),
         *         Literal("not-compatible")
         *       )
         *     )
         *   ));
         * }
         * </pre>
         *
         * @param expression the expression to evaluate
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-is-supported-script">Style
         * specification</a>
         */
        public static Expression IsSupportedScript(Expression expression)
        {
            return new Expression("is-supported-script", expression);
        }

        /**
         * Returns true if the input string is expected to render legibly.
         * Returns false if the input string contains sections that cannot be rendered without potential loss of meaning
         * (e.g. Indic scripts that require complex text shaping,
         * or right-to-left scripts if the the mapbox-gl-rtl-text plugin is not in use in Mapbox GL JS).
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * mapboxMap.getStyle().addLayer(new SymbolLayer("layer-id", "source-id")
         * .withProperties(
         *   textField(
         *     SwitchCase(
         *       IsSupportedScript("ಗೌರವಾರ್ಥವಾಗಿ"), Literal("ಗೌರವಾರ್ಥವಾಗಿ"),
         *       Literal("not-compatible"))
         *     )
         *   )
         * );
         * }
         * </pre>
         *
         * @param string the string to evaluate
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-is-supported-script">Style
         * specification</a>
         */
        public static Expression IsSupportedScript(string @string)
        {
            return new Expression("is-supported-script", Literal(@string));
        }

        /**
         * Returns the input string converted to uppercase.
         * <p>
         * Follows the Unicode Default Case Conversion algorithm
         * and the locale-insensitive case mappings in the Unicode Character Database.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(Upcase(get("key-to-string-value")))
         * );
         * }
         * </pre>
         *
         * @param string the string to upcase
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-upcase">Style specification</a>
         */
        public static Expression Upcase(Expression @string)
        {
            return new Expression("upcase", @string);
        }

        /**
         * Returns the input string converted to uppercase.
         * <p>
         * Follows the Unicode Default Case Conversion algorithm
         * and the locale-insensitive case mappings in the Unicode Character Database.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(Upcase("text"))
         * );
         * }
         * </pre>
         *
         * @param string string to upcase
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-upcase">Style specification</a>
         */
        public static Expression Upcase(string @string)
        {
            return Upcase(Literal(@string));
        }

        /**
         * Returns the input string converted to lowercase.
         * <p>
         * Follows the Unicode Default Case Conversion algorithm
         * and the locale-insensitive case mappings in the Unicode Character Database.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(downcase(get("key-to-string-value")))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-downcase">Style specification</a>
         */
        public static Expression downcase(Expression input)
        {
            return new Expression("downcase", input);
        }

        /**
         * Returns the input string converted to lowercase.
         * <p>
         * Follows the Unicode Default Case Conversion algorithm
         * and the locale-insensitive case mappings in the Unicode Character Database.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(Upcase("key-to-string-value"))
         * );
         * }
         * </pre>
         *
         * @param input string to downcase
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-downcase">Style specification</a>
         */
        public static Expression downcase(string input)
        {
            return downcase(Literal(input));
        }

        /**
         * Returns a string consisting of the concatenation of the inputs.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(Concat(get("key-to-string-value"), Literal("other string")))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-concat">Style specification</a>
         */
        public static Expression Concat(params Expression[] input)
        {
            return new Expression("concat", input);
        }

        /**
         * Returns a string consisting of the concatenation of the inputs.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(Concat("foo", "bar"))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-concat">Style specification</a>
         */
        public static Expression Concat(params string[] input)
        {
            var stringExpression = new Expression[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                stringExpression[i] = Literal(input[i]);
            }
            return Concat(stringExpression);
        }

        /**
         * Asserts that the input is an array (optionally with a specific item type and length).
         * If, when the input expression is evaluated, it is not of the asserted type,
         * then this assertion will cause the whole expression to be aborted.
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-array">Style specification</a>
         */
        public static Expression Array(Expression input)
        {
            return new Expression("array", input);
        }

        /**
         * Returns a string describing the type of the given value.
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-typeof">Style specification</a>
         */
        public static Expression TypeOf(Expression input)
        {
            return new Expression("typeof", input);
        }

        /**
         * Asserts that the input value is a string.
         * If multiple values are provided, each one is evaluated in order until a string value is obtained.
         * If none of the inputs are strings, the expression is an error.
         * The asserted input value is returned as result.
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-string">Style specification</a>
         */
        public static Expression String(params Expression[] input)
        {
            return new Expression("string", input);
        }

        /**
         * Asserts that the input value is a number.
         * If multiple values are provided, each one is evaluated in order until a number value is obtained.
         * If none of the inputs are numbers, the expression is an error.
         * The asserted input value is returned as result.
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-number">Style specification</a>
         */
        public static Expression Number(params Expression[] input)
        {
            return new Expression("number", input);
        }

        /**
         * Asserts that the input value is a bool.
         * If multiple values are provided, each one is evaluated in order until a bool value is obtained.
         * If none of the inputs are booleans, the expression is an error.
         * The asserted input value is returned as result.
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-bool">Style specification</a>
         */
        public static Expression Bool(params Expression[] input)
        {
            return new Expression("bool", input);
        }

        /**
         * Returns a collator for use in locale-dependent comparison operations.
         * The case-sensitive and diacritic-sensitive options default to false.
         * The locale argument specifies the IETF language tag of the locale to use.
         * If none is provided, the default locale is used. If the requested locale is not available,
         * the collator will use a system-defined fallback locale.
         * Use resolved-locale to test the results of locale fallback behavior.
         *
         * @param caseSensitive      case sensitive flag
         * @param diacriticSensitive diacritic sensitive flag
         * @param locale             locale
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-collator">Style specification</a>
         */
        public static Expression Collator(bool caseSensitive, bool diacriticSensitive, CultureInfo locale)
        {
            var map = new Dictionary<string, Expression>();
            map.Add("case-sensitive", Literal(caseSensitive));
            map.Add("diacritic-sensitive", Literal(diacriticSensitive));
            map.Add("locale", Literal(locale.Name));
            return new Expression("collator", new ExpressionMap(map));
        }

        /**
         * Returns a collator for use in locale-dependent comparison operations.
         * The case-sensitive and diacritic-sensitive options default to false.
         * The locale argument specifies the IETF language tag of the locale to use.
         * If none is provided, the default locale is used. If the requested locale is not available,
         * the collator will use a system-defined fallback locale.
         * Use resolved-locale to test the results of locale fallback behavior.
         *
         * @param caseSensitive      case sensitive flag
         * @param diacriticSensitive diacritic sensitive flag
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-collator">Style specification</a>
         */
        public static Expression Collator(bool caseSensitive, bool diacriticSensitive)
        {
            var map = new Dictionary<string, Expression>();
            map.Add("case-sensitive", Literal(caseSensitive));
            map.Add("diacritic-sensitive", Literal(diacriticSensitive));
            return new Expression("collator", new ExpressionMap(map));
        }

        /**
         * Returns a collator for use in locale-dependent comparison operations.
         * The case-sensitive and diacritic-sensitive options default to false.
         * The locale argument specifies the IETF language tag of the locale to use.
         * If none is provided, the default locale is used. If the requested locale is not available,
         * the collator will use a system-defined fallback locale.
         * Use resolved-locale to test the results of locale fallback behavior.
         *
         * @param caseSensitive      case sensitive flag
         * @param diacriticSensitive diacritic sensitive flag
         * @param locale             locale
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-collator">Style specification</a>
         */
        public static Expression Collator(Expression caseSensitive, Expression diacriticSensitive, Expression locale)
        {
            var map = new Dictionary<string, Expression>();
            map.Add("case-sensitive", caseSensitive);
            map.Add("diacritic-sensitive", diacriticSensitive);
            map.Add("locale", locale);
            return new Expression("collator", new ExpressionMap(map));
        }

        /**
         * Returns a collator for use in locale-dependent comparison operations.
         * The case-sensitive and diacritic-sensitive options default to false.
         * The locale argument specifies the IETF language tag of the locale to use.
         * If none is provided, the default locale is used. If the requested locale is not available,
         * the collator will use a system-defined fallback locale.
         * Use resolved-locale to test the results of locale fallback behavior.
         *
         * @param caseSensitive      case sensitive flag
         * @param diacriticSensitive diacritic sensitive flag
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-collator">Style specification</a>
         */
        public static Expression Collator(Expression caseSensitive, Expression diacriticSensitive)
        {
            var map = new Dictionary<string, Expression>();
            map.Add("case-sensitive", caseSensitive);
            map.Add("diacritic-sensitive", diacriticSensitive);
            return new Expression("collator", new ExpressionMap(map));
        }

        /**
         * Returns formatted text containing annotations for use in mixed-format text-field entries.
         * <p>
         * To build the expression, use {@link #formatEntry(Expression, FormatOption...)}.
         * <p>
         * "format" expression can be used, for example, with the {@link PropertyFactory#textField(Expression)}
         * and accepts unlimited numbers of formatted sections.
         * <p>
         * Each section consist of the input, the displayed text, and options, like font-scale and text-font.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *   textField(
         *     Format(
         *       formatEntry(
         *         Get("header_property"),
         *         FormatFontScale(2.0),
         *         FormatTextFont(new string[] {"DIN Offc Pro Regular", "Arial Unicode MS Regular"})
         *       ),
         *       formatEntry(Concat(Literal("\n"), Get("description_property")), FormatFontScale(1.5))
         *     )
         *   )
         * );
         * }
         * </pre>
         *
         * @param formatEntries format entries
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-format">Style specification</a>
         */
        public static Expression Format(params FormatEntry[] formatEntries)
        {
            return new ExpressionFormat(formatEntries);
            //// for each entry we are going to build an input and parameters
            //Expression[] mappedExpressions = new Expression[formatEntries.Length * 2];

            //int mappedIndex = 0;
            //foreach (var formatEntry in formatEntries)
            //{
            //    // input
            //    mappedExpressions[mappedIndex++] = formatEntry.Text;

            //    // parameters
            //    var map = new Dictionary<string, Expression>();

            //    if (formatEntry.Options?.Length > 0)
            //    {
            //        foreach (FormatOption option in formatEntry.Options)
            //        {
            //            map.Add(option.Type, option.Value);
            //        }
            //    }

            //    mappedExpressions[mappedIndex++] = new ExpressionMap(map);
            //}

            //return new Expression("format", mappedExpressions);
        }

        /**
         * Returns a format entry that can be used in {@link #Format(FormatEntry...)} to create formatted text fields.
         * <p>
         * Text is required to be of a resulting type string.
         * <p>
         * Text is required to be passed; {@link FormatOption}s are optional and will default to the base values defined
         * for the symbol.
         *
         * @param text          displayed text
         * @param formatOptions format options
         * @return format entry
         */
        public static FormatEntry FormatEntry(Expression text, params FormatOption[] formatOptions)
        {
            return new FormatEntry(text, formatOptions);
        }

        /**
         * Returns a format entry that can be used in {@link #Format(FormatEntry...)} to create formatted text fields.
         * <p>
         * Text is required to be of a resulting type string.
         * <p>
         * Text is required to be passed; {@link FormatOption}s are optional and will default to the base values defined
         * for the symbol.
         *
         * @param text displayed text
         * @return format entry
         */
        public static FormatEntry FormatEntry(Expression text)
        {
            return new FormatEntry(text, null);
        }

        /**
         * Returns a format entry that can be used in {@link #Format(FormatEntry...)} to create formatted text fields.
         * <p>
         * Text is required to be of a resulting type string.
         * <p>
         * Text is required to be passed; {@link FormatOption}s are optional and will default to the base values defined
         * for the symbol.
         *
         * @param text          displayed text
         * @param formatOptions format options
         * @return format entry
         */
        public static FormatEntry FormatEntry(string text, params FormatOption[] formatOptions)
        {
            return new FormatEntry(Literal(text), formatOptions);
        }

        /**
         * Returns a format entry that can be used in {@link #Format(FormatEntry...)} to create formatted text fields.
         * <p>
         * Text is required to be of a resulting type string.
         * <p>
         * Text is required to be passed; {@link FormatOption}s are optional and will default to the base values defined
         * for the symbol.
         *
         * @param text displayed text
         * @return format entry
         */
        public static FormatEntry FormatEntry(string text)
        {
            return new FormatEntry(Literal(text), null);
        }

        /**
         * Asserts that the input value is an object. If it is not, the expression is an error
         * The asserted input value is returned as result.
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-object">Style specification</a>
         */
        public static Expression @object(Expression input)
        {
            return new Expression("object", input);
        }

        /**
         * Converts the input value to a string.
         * If the input is null, the result is null.
         * If the input is a bool, the result is true or false.
         * If the input is a number, it is converted to a string by NumberToString in the ECMAScript Language Specification.
         * If the input is a color, it is converted to a string of the form "rgba(r,g,b,a)",
         * where `r`, `g`, and `b` are numerals ranging from 0 to 255, and `a` ranges from 0 to 1.
         * Otherwise, the input is converted to a string in the format specified by the JSON.stringify in the ECMAScript
         * Language Specification.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * SymbolLayer symbolLayer = new SymbolLayer("layer-id", "source-id");
         * symbolLayer.setProperties(
         *     textField(get("key-to-number-value"))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-to-string">Style specification</a>
         */
        public static Expression ToString(Expression input)
        {
            return new Expression("to-string", input);
        }

        /**
         * Converts the input value to a number, if possible.
         * If the input is null or false, the result is 0.
         * If the input is true, the result is 1.
         * If the input is a string, it is converted to a number as specified by the ECMAScript Language Specification.
         * If multiple values are provided, each one is evaluated in order until the first successful conversion is obtained.
         * If none of the inputs can be converted, the expression is an error.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(toNumber(get("key-to-string-value")))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-to-number">Style specification</a>
         */
        public static Expression ToNumber(Expression input)
        {
            return new Expression("to-number", input);
        }

        /**
         * Converts the input value to a bool. The result is `false` when then input is an empty string, 0, false,
         * null, or NaN; otherwise it is true.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(toBool(get("key-to-value")))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-to-bool">Style specification</a>
         */
        public static Expression ToBool(Expression input)
        {
            return new Expression("to-bool", input);
        }

        /**
         * Converts the input value to a color. If multiple values are provided,
         * each one is evaluated in order until the first successful conversion is obtained.
         * If none of the inputs can be converted, the expression is an error.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(toColor(get("keyStringValue")))
         * );
         * }
         * </pre>
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-types-to-color">Style specification</a>
         */
        public static Expression ToColor(Expression input)
        {
            return new Expression("to-color", input);
        }

        /**
         * Binds input to named variables,
         * which can then be referenced in the result expression using {@link #var(string)} or {@link #var(Expression)}.
         *
         * @param input expression input
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-let">Style specification</a>
         */
        public static Expression Let(params Expression[] input)
        {
            return new Expression("let", input);
        }

        /**
         * References variable bound using let.
         *
         * @param expression the variable naming expression that was bound with using let
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-var">Style specification</a>
         */
        public static Expression Var(Expression expression)
        {
            return new Expression("var", expression);
        }

        /**
         * References variable bound using let.
         *
         * @param variableName the variable naming that was bound with using let
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-var">Style specification</a>
         */
        public static Expression Var(string variableName)
        {
            return Var(Literal(variableName));
        }

        /**
         * Gets the current zoom level.
         * <p>
         * Note that in style layout and paint properties,
         * zoom may only appear as the input to a top-level step or interpolate expression.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *       interpolate(
         *         exponential(0.5f), zoom(),
         *         stop(1.0f, Color(Color.RED)),
         *         stop(5.0f, Color(Color.BLUE)),
         *         stop(10.0f, Color(Color.GREEN))
         *       )
         *     )
         * );
         * }
         * </pre>
         *
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-zoom">Style specification</a>
         */
        public static Expression Zoom()
        {
            return new Expression("zoom");
        }

        /**
         * Produces a stop value.
         * <p>
         * Can be used for {@link #stop(object, object)} as part of varargs parameter in
         * {@link #step(double, Expression, params Stop[])} or {@link #interpolate(Interpolator, Expression, params Stop[])}.
         * </p>
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(
         *         step(zoom(), Literal(0.0f),
         *         stop(1.0f, 2.5f),
         *         stop(10.0f, 5.0f))
         *     )
         * );
         * }
         * </pre>
         *
         * @param stop  the stop input
         * @param value the stop output
         * @return the stop
         */
        public static Stop Stop(object stop, object value)
        {
            return new Stop(stop, value);
        }

        /**
         * Produces discrete, stepped results by evaluating a piecewise-constant function defined by pairs of
         * input and output values (\"stops\"). The `input` may be any numeric expression (e.g., `[\"get\", \"population\"]`).
         * Stop inputs must be numeric literals in strictly ascending order.
         * Returns the output value of the stop just less than the input,
         * or the first input if the input is less than the first stop.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(
         *         step(zoom(), Literal(0.0f),
         *         Literal(1.0f), Literal(2.5f),
         *         Literal(10.0f), Literal(5.0f))
         *     )
         * );
         * }
         * </pre>
         *
         * @param input         the input value
         * @param defaultOutput the default output expression
         * @param stops         pair of input and output values
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-step">Style specification</a>
         */
        public static Expression Step(double input, Expression defaultOutput, params Expression[] stops)
        {
            return Step(Literal(input), defaultOutput, stops);
        }

        /**
         * Produces discrete, stepped results by evaluating a piecewise-constant function defined by pairs of
         * input and output values (\"stops\"). The `input` may be any numeric expression (e.g., `[\"get\", \"population\"]`).
         * Stop inputs must be numeric literals in strictly ascending order.
         * Returns the output value of the stop just less than the input,
         * or the first input if the input is less than the first stop.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(
         *         step(zoom(), Literal(0.0f),
         *         Literal(1.0f), Literal(2.5f),
         *         Literal(10.0f), Literal(5.0f))
         *     )
         * );
         * }
         * </pre>
         *
         * @param input         the input expression
         * @param defaultOutput the default output expression
         * @param stops         pair of input and output values
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-step">Style specification</a>
         */
        public static Expression Step(Expression input, Expression defaultOutput, params Expression[] stops)
        {
            return new Expression("step", Join(new Expression[] { input, defaultOutput }, stops));
        }

        /**
         * Produces discrete, stepped results by evaluating a piecewise-constant function defined by pairs of
         * input and output values (\"stops\"). The `input` may be any numeric expression (e.g., `[\"get\", \"population\"]`).
         * Stop inputs must be numeric literals in strictly ascending order.
         * Returns the output value of the stop just less than the input,
         * or the first input if the input is less than the first stop.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(
         *         step(zoom(), Literal(0.0f),
         *         stop(1, 2.5f),
         *         stop(10, 5.0f))
         *     )
         * );
         * }
         * </pre>
         *
         * @param input         the input value
         * @param defaultOutput the default output expression
         * @param stops         pair of input and output values
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-step">Style specification</a>
         */
        public static Expression Step(double input, Expression defaultOutput, params Stop[] stops)
        {
            return Step(Literal(input), defaultOutput, Naxam.Mapbox.Expressions.Stop.ToExpressionArray(stops));
        }

        /**
         * Produces discrete, stepped results by evaluating a piecewise-constant function defined by pairs of
         * input and output values (\"stops\"). The `input` may be any numeric expression (e.g., `[\"get\", \"population\"]`).
         * Stop inputs must be numeric literals in strictly ascending order.
         * Returns the output value of the stop just less than the input,
         * or the first input if the input is less than the first stop.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(
         *         step(zoom(), Literal(0.0f),
         *         stop(1, 2.5f),
         *         stop(10, 5.0f))
         *     )
         * );
         * }
         * </pre>
         *
         * @param input         the input value
         * @param defaultOutput the default output expression
         * @param stops         pair of input and output values
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-step">Style specification</a>
         */
        public static Expression Step(Expression input, Expression defaultOutput, params Stop[] stops)
        {
            return Step(input, defaultOutput, Naxam.Mapbox.Expressions.Stop.ToExpressionArray(stops));
        }

        /**
         * Produces discrete, stepped results by evaluating a piecewise-constant function defined by pairs of
         * input and output values (\"stops\"). The `input` may be any numeric expression (e.g., `[\"get\", \"population\"]`).
         * Stop inputs must be numeric literals in strictly ascending order.
         * Returns the output value of the stop just less than the input,
         * or the first input if the input is less than the first stop.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(
         *         step(1.0f, 0.0f,
         *         Literal(1.0f), Literal(2.5f),
         *         Literal(10.0f), Literal(5.0f))
         *     )
         * );
         * }
         * </pre>
         *
         * @param input         the input value
         * @param defaultOutput the default output expression
         * @param stops         pair of input and output values
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-step">Style specification</a>
         */
        public static Expression Step(double input, double defaultOutput, params Expression[] stops)
        {
            return Step(Literal(input), defaultOutput, stops);
        }

        /**
         * Produces discrete, stepped results by evaluating a piecewise-constant function defined by pairs of
         * input and output values (\"stops\"). The `input` may be any numeric expression (e.g., `[\"get\", \"population\"]`).
         * Stop inputs must be numeric literals in strictly ascending order.
         * Returns the output value of the stop just less than the input,
         * or the first input if the input is less than the first stop.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(
         *         step(zoom(), 0.0f,
         *         Literal(1.0f), Literal(2.5f),
         *         Literal(10.0f), Literal(5.0f))
         *     )
         * );
         * }
         * </pre>
         *
         * @param input         the input expression
         * @param defaultOutput the default output expression
         * @param stops         pair of input and output values
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-step">Style specification</a>
         */
        public static Expression Step(Expression input, double defaultOutput, params Expression[] stops)
        {
            return Step(input, Literal(defaultOutput), stops);
        }

        /**
         * Produces discrete, stepped results by evaluating a piecewise-constant function defined by pairs of
         * input and output values (\"stops\"). The `input` may be any numeric expression (e.g., `[\"get\", \"population\"]`).
         * Stop inputs must be numeric literals in strictly ascending order.
         * Returns the output value of the stop just less than the input,
         * or the first input if the input is less than the first stop.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(
         *         step(zoom(), 0.0f,
         *         stop(1, 2.5f),
         *         stop(10, 5.0f))
         *     )
         * );
         * }
         * </pre>
         *
         * @param input         the input value
         * @param defaultOutput the default output expression
         * @param stops         pair of input and output values
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-step">Style specification</a>
         */
        public static Expression Step(double input, double defaultOutput, params Stop[] stops)
        {
            return Step(Literal(input), defaultOutput, Naxam.Mapbox.Expressions.Stop.ToExpressionArray(stops));
        }

        /**
         * Produces discrete, stepped results by evaluating a piecewise-constant function defined by pairs of
         * input and output values (\"stops\"). The `input` may be any numeric expression (e.g., `[\"get\", \"population\"]`).
         * Stop inputs must be numeric literals in strictly ascending order.
         * Returns the output value of the stop just less than the input,
         * or the first input if the input is less than the first stop.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * CircleLayer circleLayer = new CircleLayer("layer-id", "source-id");
         * circleLayer.setProperties(
         *     circleRadius(
         *         step(zoom(), 0.0f,
         *         stop(1, 2.5f),
         *         stop(10, 5.0f))
         *     )
         * );
         * }
         * </pre>
         *
         * @param input         the input value
         * @param defaultOutput the default output expression
         * @param stops         pair of input and output values
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-step">Style specification</a>
         */
        public static Expression Step(Expression input, double defaultOutput, params Stop[] stops)
        {
            return Step(input, defaultOutput, Naxam.Mapbox.Expressions.Stop.ToExpressionArray(stops));
        }

        /**
         * Produces continuous, smooth results by interpolating between pairs of input and output values (\"stops\").
         * The `input` may be any numeric expression (e.g., `[\"get\", \"population\"]`).
         * Stop inputs must be numeric literals in strictly ascending order.
         * The output type must be `number`, `array&Lt;number&gt;`, or `color`.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *   fillColor(
         *     interpolate(exponential(0.5f), zoom(),
         *        stop(1.0f, Color(Color.RED)),
         *        stop(5.0f, Color(Color.BLUE)),
         *        stop(10.0f, Color(Color.GREEN)
         *       )
         *     )
         *   )
         * );
         * }
         * </pre>
         *
         * @param interpolation type of interpolation
         * @param number        the input expression
         * @param stops         pair of input and output values
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-interpolate">Style specification</a>
         */
        public static Expression Interpolate(Interpolator interpolation,
                                             Expression number, params Expression[] stops)
        {
            return new Expression("interpolate", Join(new Expression[] { interpolation, number }, stops));
        }

        /**
         * Produces continuous, smooth results by interpolating between pairs of input and output values (\"stops\").
         * The `input` may be any numeric expression (e.g., `[\"get\", \"population\"]`).
         * Stop inputs must be numeric literals in strictly ascending order.
         * The output type must be `number`, `array&Lt;number&gt;`, or `color`.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *       interpolate(
         *         exponential(0.5f), zoom(),
         *         stop(1.0f, Color(Color.RED)),
         *         stop(5.0f, Color(Color.BLUE)),
         *         stop(10.0f, Color(Color.GREEN))
         *       )
         *     )
         * );
         * }
         * </pre>
         *
         * @param interpolation type of interpolation
         * @param number        the input expression
         * @param stops         pair of input and output values
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-interpolate">Style specification</a>
         */
        public static Expression Interpolate(Interpolator interpolation,
                                             Expression number, params Stop[] stops)
        {
            return Interpolate(interpolation, number, Expressions.Stop.ToExpressionArray(stops));
        }

        /**
         * interpolates linearly between the pair of stops just less than and just greater than the input.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *       interpolate(
         *         linear(), zoom(),
         *         stop(1.0f, Color(Color.RED)),
         *         stop(5.0f, Color(Color.BLUE)),
         *         stop(10.0f, Color(Color.GREEN))
         *       )
         *     )
         * );
         * }
         * </pre>
         *
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-interpolate">Style specification</a>
         */
        public static Interpolator Linear()
        {
            return new Interpolator("linear");
        }

        /**
         * Interpolates exponentially between the stops just less than and just greater than the input.
         * `base` controls the rate at which the output increases:
         * higher values make the output increase more towards the high end of the range.
         * With values close to 1 the output increases linearly.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *       interpolate(
         *         exponential(0.5f), zoom(),
         *         stop(1.0f, Color(Color.RED)),
         *         stop(5.0f, Color(Color.BLUE)),
         *         stop(10.0f, Color(Color.GREEN))
         *       )
         *     )
         * );
         * }
         * </pre>
         *
         * @param base value controlling the route at which the output increases
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-interpolate">Style specification</a>
         */
        public static Interpolator Exponential(double @base)
        {
            return Exponential(Literal(@base));
        }

        /**
         * Interpolates exponentially between the stops just less than and just greater than the input.
         * The parameter controls the rate at which the output increases:
         * higher values make the output increase more towards the high end of the range.
         * With values close to 1 the output increases linearly.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *       interpolate(
         *         exponential(get("keyToValue")), zoom(),
         *         stop(1.0f, Color(Color.RED)),
         *         stop(5.0f, Color(Color.BLUE)),
         *         stop(10.0f, Color(Color.GREEN))
         *       )
         *     )
         * );
         * }
         * </pre>
         *
         * @param expression base number expression
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-interpolate">Style specification</a>
         */
        public static Interpolator Exponential(Expression expression)
        {
            return new Interpolator("exponential", expression);
        }

        /**
         * Interpolates using the cubic bezier curve defined by the given control points.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *       interpolate(
         *         cubicBezier(0.42f, 0.0f, 1.0f, 1.0f), zoom(),
         *         stop(1.0f, Color(Color.RED)),
         *         stop(5.0f, Color(Color.BLUE)),
         *         stop(10.0f, Color(Color.GREEN))
         *       )
         *     )
         * );
         * }
         * </pre>
         *
         * @param x1 x value of the first point of a cubic bezier, ranges from 0 to 1
         * @param y1 y value of the first point of a cubic bezier, ranges from 0 to 1
         * @param x2 x value of the second point of a cubic bezier, ranges from 0 to 1
         * @param y2 y value fo the second point of a cubic bezier, ranges from 0 to 1
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-interpolate">Style specification</a>
         */
        public static Interpolator CubicBezier(Expression x1, Expression y1,
                                               Expression x2, Expression y2)
        {
            return new Interpolator("cubic-bezier", x1, y1, x2, y2);
        }

        /**
         * Interpolates using the cubic bezier curve defined by the given control points.
         * <p>
         * Example usage:
         * </p>
         * <pre>
         * {@code
         * FillLayer fillLayer = new FillLayer("layer-id", "source-id");
         * fillLayer.setProperties(
         *     fillColor(
         *       interpolate(
         *         cubicBezier(0.42f, 0.0f, 1.0f, 1.0f), zoom(),
         *         stop(1.0f, Color(Color.RED)),
         *         stop(5.0f, Color(Color.BLUE)),
         *         stop(10.0f, Color(Color.GREEN))
         *       )
         *     )
         * );
         * }
         * </pre>
         *
         * @param x1 x value of the first point of a cubic bezier, ranges from 0 to 1
         * @param y1 y value of the first point of a cubic bezier, ranges from 0 to 1
         * @param x2 x value of the second point of a cubic bezier, ranges from 0 to 1
         * @param y2 y value fo the second point of a cubic bezier, ranges from 0 to 1
         * @return expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/#expressions-interpolate">Style specification</a>
         */
        public static Interpolator CubicBezier(double x1, double y1,
                                               double x2, double y2)
        {
            return CubicBezier(Literal(x1), Literal(y1), Literal(x2), Literal(y2));
        }

        /**
         * Joins two expressions arrays.
         * <p>
         * This flattens the object array output of an expression from a nested expression hierarchy.
         * </p>
         *
         * @param left  the left part of an expression
         * @param right the right part of an expression
         * @return the joined expression
         */

        private static Expression[] Join(Expression[] left, Expression[] right)
        {
            var output = new Expression[left.Length + right.Length];

            left.CopyTo(output, 0);
            System.Array.ConstrainedCopy(right, 0, output, left.Length, right.Length);

            return output;
        }

        /**
         * Converts the expression to object array representation.
         * <p>
         * The output will later be converted to a JSON object array.
         * </p>
         *
         * @return the converted object array expression
         */

        public virtual object[] ToArray()
        {
            var array = new List<object>
            {
                Operator
            };

            if (Arguments?.Length > 0)
            {
                foreach (Expression argument in Arguments)
                {
                    if (argument is ValueExpression valueExpression)
                    {
                        array.Add(valueExpression.ToValue());
                    }
                    else
                    {
                        array.Add(argument.ToArray());
                    }
                }
            }

            return array.ToArray();
        }

        /**
         * Returns a string representation of the object that matches the definition set in the style specification.
         * <p>
         * If this expression contains a coma (,) delimited literal, like 'rgba(r, g, b, a)`,
         * it will be enclosed with double quotes (").
         * </p>
         *
         * @return a string representation of the object.
         */

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[\"").Append(Operator).Append("\"");
            if (Arguments != null)
            {
                foreach (object argument in Arguments)
                {
                    builder.Append(", ");
                    builder.Append(argument.ToString());
                }
            }
            builder.Append("]");
            return builder.ToString();
        }

        /**
         * Returns a DSL equivalent of a raw expression.
         * <p>
         * If your raw expression contains a coma (,) delimited literal it has to be enclosed with double quotes ("),
         * for example
         * </p>
         * <pre>
         *   {@code
         *   ["to-color", "rgba(255, 0, 0, 255)"]
         *   }
         * </pre>
         *
         * @param rawExpression the raw expression
         * @return the resulting expression
         * @see <a href="https://www.mapbox.com/mapbox-gl-js/style-spec/">Style specification</a>
         */
        public static Expression Raw(string rawExpression)
        {
            return Converter.Convert(rawExpression);
        }

        /**
         * Converts an object that is a primitive array to an object[]
         *
         * @param object the object to convert to an object array
         * @return the converted object array
         */
        static object[] ToObjectArray(IEnumerable @object)
        {
            // object is a primitive array
            var result = new List<object>();
            var enumerator = @object.GetEnumerator();
            while (enumerator.MoveNext())
            {
                result.Add(enumerator.Current);
            }

            return result.ToArray();
        }
    }
}
