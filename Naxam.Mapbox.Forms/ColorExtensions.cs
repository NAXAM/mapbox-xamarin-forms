
using System.Globalization;
using Xamarin.Forms;

namespace Naxam.Controls
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color color)
        {
            int red = (int)(color.R * 255);
            int green = (int)(color.G * 255);
            int blue = (int)(color.B * 255);
            int alpha = (int)(color.A * 255);
            return string.Format("#{3:X2}{0:X2}{1:X2}{2:X2}", red, green, blue, alpha);
        }

        public static string ToRGBAString(this Color color)
        {
            int red = (int)(color.R * 255);
            int green = (int)(color.G * 255);
            int blue = (int)(color.B * 255);
            return string.Format(
                "rgba({0}, {1}, {2}, {3})",
                red, green, blue, color.A.ToString(CultureInfo.InvariantCulture));
        }
    }
}

