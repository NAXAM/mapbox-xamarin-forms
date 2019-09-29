using System;
using UIKit;
using Xamarin.Forms;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class ImageSourceExtensions
    {
        public static UIImage GetImage(this ImageSource source)
        {
            UIImage localImage = null;

            if (source is FileImageSource fileImageSource)
            {
                localImage = UIImage.FromBundle(fileImageSource.File);
            }

            if (localImage == null)
            {
                throw new InvalidOperationException("No resource for name: " + source);
            }

            return localImage;
        }
    }
}