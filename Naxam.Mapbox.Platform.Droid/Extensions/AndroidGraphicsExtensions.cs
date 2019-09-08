using Android.Graphics;

namespace Naxam.Mapbox.Platform.Droid.Extensions
{
    public static class AndroidGraphicsExtensions
    {
        public static RectF ToRectF(this PointF point, float radius)
        {
            return new RectF(
                point.X - radius,
                point.Y - radius,
                point.X+radius,
                point.Y+radius
            );
        }

        public static RectF ToRectF(this PointF tl, PointF br)
        {
            return new RectF(
                tl.X,
                tl.Y,
                br.X,
                br.Y
            );
        }

        public static PointF ToPointF(this Xamarin.Forms.Point point)
        {
            return new PointF(
                (float)point.X,
                (float)point.Y
            );
        }

        public static RectF ToRectF(this Xamarin.Forms.Rectangle rectangle)
        {
            return new RectF(
                (float)rectangle.Left,
                (float)rectangle.Top,
                (float)rectangle.Right,
                (float)rectangle.Bottom
            );
        }
    }
}
