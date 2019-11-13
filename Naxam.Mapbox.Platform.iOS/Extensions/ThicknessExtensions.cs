using System;
using UIKit;
using Xamarin.Forms;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class ThicknessExtensions
    {
        public static UIEdgeInsets ToEdgeInsets(this Thickness thickness)
        {
            return new UIEdgeInsets(
                (nfloat)thickness.Top,
                (nfloat)thickness.Left,
                (nfloat)thickness.Bottom,
                (nfloat)thickness.Right);
        }
    }
}
