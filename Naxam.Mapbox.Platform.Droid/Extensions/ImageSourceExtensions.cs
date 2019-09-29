using System;
using Xamarin.Forms;

namespace Naxam.Mapbox.Platform.Droid.Extensions
{
    public static class ImageSourceExtensions
    {
        public static int GetResId(this ImageSource source)
        {
            var resId = 0;
            if (source is FileImageSource fileImageSource)
            {
                var name = fileImageSource.File.Split('.')[0];
                resId = Android.App.Application.Context.Resources.GetIdentifier(
                    name,
                    "drawable",
                    Android.App.Application.Context.PackageName
                );
            }

            if (resId == 0)
            {
                throw new InvalidOperationException("No resource with name: " + source);
            }

            return resId;
        }
    }
}