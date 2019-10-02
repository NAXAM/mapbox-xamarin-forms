using Android.Content;
using Com.Mapbox.Mapboxsdk.Constants;
using Com.Mapbox.Mapboxsdk.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Naxam.Mapbox.Platform.Droid.Extensions
{
    public static class MapboxOptionsExtensions
    {
        public static int[] ToArray(this Thickness thickness, Context context)
        {
            return new int[]
            {
                (int) context.ToPixels(thickness.Left),
                (int) context.ToPixels(thickness.Top),
                (int) context.ToPixels(thickness.Right),
                (int) context.ToPixels(thickness.Bottom),
            };
        }

        public static MapboxMapOptions CreateOptions(
            this Naxam.Controls.Forms.MapView element,
            Context context)
        {
            var mapboxMapOptions = new MapboxMapOptions();
            
            if (string.IsNullOrWhiteSpace(element.ApiBaseUri))
            {
                mapboxMapOptions.InvokeApiBaseUri(element.ApiBaseUri);
            }

            mapboxMapOptions.InvokeCamera(element.Camera.ToNative())
                .InvokeZoomGesturesEnabled(element.ZoomEnabled) //, true));
                .InvokeScrollGesturesEnabled(element.ScrollEnabled) //, true));
                .InvokeRotateGesturesEnabled(element.RotateEnabled) //, true));
                .InvokeTiltGesturesEnabled(element.PitchEnabled) //, true));
                .InvokeDoubleTapGesturesEnabled(element.DoubleTapEnabled) // , true));
                .InvokeQuickZoomGesturesEnabled(element.QuickZoomEnabled) //, true));
                .InvokeMaxZoomPreference(element.ZoomMaxLevel ?? MapboxConstants.MAXIMUM_ZOOM)
                .InvokeMinZoomPreference(element.ZoomMinLevel ?? MapboxConstants.MINIMUM_ZOOM)
                .InvokeCompassEnabled(element.CompassEnabled) //, true));
                .InvokeCompassGravity((int)element.CompassGravity) // Gravity.TOP | Gravity.END));
                .CompassMargins(element.CompassMargin.ToArray(context)) //FOUR_DP
                .CompassFadesWhenFacingNorth(element.CompassFadeFacingNorth); //, true));

            var compassDrawableName = (element.CompassDrawable as FileImageSource)?.File.Split('.')[0]
                                      ?? "mapbox_compass_icon";
            var compassDrawableId =
                context.Resources.GetIdentifier(compassDrawableName, "drawable", context.PackageName);
            if (compassDrawableId > 0)
            {
                var compassDrawable = context.Resources.GetDrawable(compassDrawableId, context.Theme);
                mapboxMapOptions.InvokeCompassImage(compassDrawable);
            }

            mapboxMapOptions.InvokeLogoEnabled(element.LogoEnabled) //, true));
                .InvokeLogoGravity((int)element.LogoGravity) //, Gravity.BOTTOM | Gravity.START));
                .LogoMargins(element.LogoMargin.ToArray(context)) //FOUR_DP
                .InvokeAttributionTintColor(element.AttributionTintColor.ToAndroid()) //, UNDEFINED_COLOR));
                .InvokeAttributionEnabled(element.AttributionEnabled) //, true));
                .InvokeAttributionGravity((int)element.AttributionGravity) //, Gravity.BOTTOM | Gravity.START));
                .AttributionMargins(element.AttributionMargin.ToArray(context)) // NINETY_TWO_DP FOUR_DP  FOUR_DP FOUR_DP
                .InvokeTextureMode(element.RenderTextureMode) //, false));
                .InvokeTranslucentTextureSurface(element.RenderTextureTranslucentSurface) //, false));
                .SetPrefetchesTiles(element.TilePrefetchEnabled) //, true));
                .InvokeRenderSurfaceOnTop(element.ZMediaOverlayEnabled); //, false));

            mapboxMapOptions.LocalIdeographFontFamilyEnabled(element.LocalIdeographFontFamilyEnabled)
                .InvokeLocalIdeographFontFamily(element.LocalIdeographFontFamilies ??
                                                new[] {MapboxConstants.DEFAULT_FONT}) //, true);
                .InvokePixelRatio(element.PixelRatio) //, 0);
                .InvokeForegroundLoadColor(element.ForegroundLoadColor.ToAndroid()) //, LIGHT_GRAY)
                .InvokeCrossSourceCollisions(element.CrossSourceCollisions); //, true)
            return mapboxMapOptions;
        }
    }
}