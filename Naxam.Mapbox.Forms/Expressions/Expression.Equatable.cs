using System;
using System.Linq;

namespace Naxam.Mapbox.Expressions
{
    public partial class Expression : IEquatable<Expression>
    {
        /**
         * Indicates whether some other object is "equal to" this one.
         *
         * @param o the other object
         * @return true if equal, false if not
         */

        public bool Equals(Expression expression)
        {
            if (expression == null) return false;

            if (false == string.Equals(Operator, expression.Operator))
            {
                return false;
            }

            return Enumerable.SequenceEqual(Arguments, expression.Arguments);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj is Expression expression)
            {
                return Equals(expression);
            }

            return false;
        }

        /**
         * Returns a hash code value for the expression.
         *
         * @return a hash code value for this expression
         */
        public override int GetHashCode()
        {
            int result = Operator != null ? Operator.GetHashCode() : 0;
            result = 31 * result +  Arguments.GetHashCode();
            return result;
        }
    }
}
