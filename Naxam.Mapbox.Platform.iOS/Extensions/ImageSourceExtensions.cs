using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class ImageSourceExtensions
    {
        public static UIImage GetImage(this ImageSource source)
        {
            return new FileImageSourceHandler()
                .LoadImageAsync(source).Result;
        }
    }
}