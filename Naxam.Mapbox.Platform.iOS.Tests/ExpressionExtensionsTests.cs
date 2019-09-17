using System;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Platform.iOS.Extensions;
using NUnit.Framework;
using Xamarin.Forms;

namespace Naxam.Mapbox.Platform.iOS.Tests
{
    [TestFixture]
    public class ExpressionExteionsTests
    {
        [Test]
        public void ToExpressionTest()
        {
            var shared = Expression.Interpolate(
                        Expression.Exponential(1.0),
                        Expression.Get("mag"),
                        Expression.CreateStop(2.0f, Expression.Color(Color.Red)),
                        Expression.CreateStop(4.5f, Expression.Color(Color.Green)),
                        Expression.CreateStop(7.0f, Expression.Color(Color.Blue))
                    );

            var actual = shared.ToExpression();
        }
    }
}
