using System;
using Android.Content;
using Android.Graphics;
using Com.Mapbox.Mapboxsdk.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Naxam.Mapbox.Platform.Droid.Extensions
{
    public static class ImageSourceExtensions
    {
        public static Bitmap GetBitmap(this ImageSource source, Context context)
        {
            var handler =  Xamarin.Forms.Internals.Registrar.Registered
                .GetHandlerForObject<IImageSourceHandler>(source);

            return handler?
                .LoadImageAsync(source, context).Result;
        }
    }
}