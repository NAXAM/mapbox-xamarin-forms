using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.Mapbox.Mapboxsdk.Style.Expressions;
using Newtonsoft.Json;
using NxExpressions = Naxam.Mapbox.Expressions;
namespace Naxam.Mapbox.Platform.Droid.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression ToNative(this NxExpressions.Expression expression)
        {
            var json = JsonConvert.SerializeObject(expression.ToArray());
            System.Diagnostics.Debug.WriteLine(json);
            return Expression.Raw(json);
        }
    }
}
