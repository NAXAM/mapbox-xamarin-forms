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
                Expression.Linear(), 
                Expression.Zoom(),
                Expression.CreateStop(15, 0),
                Expression.CreateStop(18, 1)
            );

            var actual = shared.ToNative();
        }
    }
}
