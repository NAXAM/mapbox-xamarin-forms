using System;
using Naxam.Mapbox;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
    [Flags]
    public enum Gravity
    {
        AxisClip = 8,
        AxisPullAfter = 4,
        AxisPullBefore = 2,
        AxisSpecified = 1,
        AxisXShift = 0,
        AxisYShift = AxisPullAfter, // 0x00000004
        Bottom = 80, // 0x00000050
        Center = 17, // 0x00000011
        CenterHorizontal = AxisSpecified, // 0x00000001
        CenterVertical = 16, // 0x00000010
        ClipHorizontal = AxisClip, // 0x00000008
        ClipVertical = 128, // 0x00000080
        DisplayClipHorizontal = 16777216, // 0x01000000
        DisplayClipVertical = 268435456, // 0x10000000
        End = 8388613, // 0x00800005
        Fill = 119, // 0x00000077
        FillHorizontal = CenterHorizontal | AxisYShift | AxisPullBefore, // 0x00000007
        FillVertical = 112, // 0x00000070
        HorizontalGravityMask = FillHorizontal, // 0x00000007
        Left = CenterHorizontal | AxisPullBefore, // 0x00000003
        NoGravity = 0,
        RelativeHorizontalGravityMask = 8388615, // 0x00800007
        RelativeLayoutDirection = 8388608, // 0x00800000
        Right = CenterHorizontal | AxisYShift, // 0x00000005
        Start = RelativeLayoutDirection | Left, // 0x00800003
        Top = 48, // 0x00000030
        VerticalGravityMask = 112, // 0x00000070
    }

    partial class MapView
    {
        public static readonly BindableProperty ApiBaseUriProperty = BindableProperty.Create(
            nameof(ApiBaseUri),
            typeof(string),
            typeof(MapView),
            default(string),
            BindingMode.OneTime
        );

        public string ApiBaseUri
        {
            get => (string) GetValue(ApiBaseUriProperty);
            set => SetValue(ApiBaseUriProperty, value);
        }

        public CameraPosition Camera => new CameraPosition(Center, ZoomLevel, Pitch, Heading);

        public static readonly BindableProperty ZoomEnabledProperty = BindableProperty.Create(
            nameof(ZoomEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool ZoomEnabled
        {
            get => (bool) GetValue(ZoomEnabledProperty);
            set => SetValue(ZoomEnabledProperty, value);
        }

        public static readonly BindableProperty ScrollEnabledProperty = BindableProperty.Create(
            nameof(ScrollEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool ScrollEnabled
        {
            get => (bool) GetValue(ScrollEnabledProperty);
            set => SetValue(ScrollEnabledProperty, value);
        }

        public static readonly BindableProperty RotateEnabledProperty = BindableProperty.Create(
            nameof(RotateEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool RotateEnabled
        {
            get => (bool) GetValue(RotateEnabledProperty);
            set => SetValue(RotateEnabledProperty, value);
        }

        public static readonly BindableProperty PitchEnabledProperty = BindableProperty.Create(
            nameof(PitchEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool PitchEnabled
        {
            get => (bool) GetValue(PitchEnabledProperty);
            set => SetValue(PitchEnabledProperty, value);
        }

        public static readonly BindableProperty DoubleTapEnabledProperty = BindableProperty.Create(
            nameof(DoubleTapEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool DoubleTapEnabled
        {
            get => (bool) GetValue(DoubleTapEnabledProperty);
            set => SetValue(DoubleTapEnabledProperty, value);
        }

        public static readonly BindableProperty QuickZoomEnabledProperty = BindableProperty.Create(
            nameof(QuickZoomEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool QuickZoomEnabled
        {
            get => (bool) GetValue(QuickZoomEnabledProperty);
            set => SetValue(QuickZoomEnabledProperty, value);
        }

        public static readonly BindableProperty ZoomMaxLevelProperty = BindableProperty.Create(
            nameof(ZoomMaxLevel),
            typeof(double?),
            typeof(MapView),
            null,
            BindingMode.OneWay
        );

        public double? ZoomMaxLevel
        {
            get => (double?) GetValue(ZoomMaxLevelProperty);
            set => SetValue(ZoomMaxLevelProperty, value);
        }

        public static readonly BindableProperty ZoomMinLevelProperty = BindableProperty.Create(
            nameof(ZoomMinLevel),
            typeof(double?),
            typeof(MapView),
            null,
            BindingMode.OneWay
        );

        public double? ZoomMinLevel
        {
            get => (double?) GetValue(ZoomMinLevelProperty);
            set => SetValue(ZoomMinLevelProperty, value);
        }

        public static readonly BindableProperty CompassEnabledProperty = BindableProperty.Create(
            nameof(CompassEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool CompassEnabled
        {
            get => (bool) GetValue(CompassEnabledProperty);
            set => SetValue(CompassEnabledProperty, value);
        }

        public static readonly BindableProperty CompassGravityProperty = BindableProperty.Create(
            nameof(CompassGravity),
            typeof(Gravity),
            typeof(MapView),
            Gravity.Top | Gravity.End,
            BindingMode.OneWay
        );

        public Gravity CompassGravity
        {
            get => (Gravity) GetValue(CompassGravityProperty);
            set => SetValue(CompassGravityProperty, value);
        }

        public static readonly BindableProperty CompassMarginProperty = BindableProperty.Create(
            nameof(CompassMargin),
            typeof(Thickness),
            typeof(MapView),
            (Thickness)4,
            BindingMode.OneWay
        );

        public Thickness CompassMargin
        {
            get => (Thickness) GetValue(CompassMarginProperty);
            set => SetValue(CompassMarginProperty, value);
        }

        public static readonly BindableProperty CompassFadeFacingNorthProperty = BindableProperty.Create(
            nameof(CompassFadeFacingNorth),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool CompassFadeFacingNorth
        {
            get => (bool) GetValue(CompassFadeFacingNorthProperty);
            set => SetValue(CompassFadeFacingNorthProperty, value);
        }

        public static readonly BindableProperty CompassDrawableProperty = BindableProperty.Create(
            nameof(CompassDrawable),
            typeof(ImageSource),
            typeof(MapView),
            default(ImageSource),
            BindingMode.OneWay
        );

        public ImageSource CompassDrawable
        {
            get => (ImageSource) GetValue(CompassDrawableProperty);
            set => SetValue(CompassDrawableProperty, value);
        }

        public static readonly BindableProperty LogoEnabledProperty = BindableProperty.Create(
            nameof(LogoEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool LogoEnabled
        {
            get => (bool) GetValue(LogoEnabledProperty);
            set => SetValue(LogoEnabledProperty, value);
        }

        public static readonly BindableProperty LogoGravityProperty = BindableProperty.Create(
            nameof(LogoGravity),
            typeof(Gravity),
            typeof(MapView),
            Gravity.Bottom | Gravity.Left,
            BindingMode.OneWay
        );

        public Gravity LogoGravity
        {
            get => (Gravity) GetValue(LogoGravityProperty);
            set => SetValue(LogoGravityProperty, value);
        }

        public static readonly BindableProperty LogoMarginProperty = BindableProperty.Create(
            nameof(LogoMargin),
            typeof(Thickness),
            typeof(MapView),
            (Thickness)4,
            BindingMode.OneWay
        );

        public Thickness LogoMargin
        {
            get => (Thickness) GetValue(LogoMarginProperty);
            set => SetValue(LogoMarginProperty, value);
        }

        public static readonly BindableProperty AttributionTintColorProperty = BindableProperty.Create(
            nameof(AttributionTintColor),
            typeof(Color),
            typeof(MapView),
            Color.Default,
            BindingMode.OneWay
        );

        public Color AttributionTintColor
        {
            get => (Color) GetValue(AttributionTintColorProperty);
            set => SetValue(AttributionTintColorProperty, value);
        }

        public static readonly BindableProperty AttributionEnabledProperty = BindableProperty.Create(
            nameof(AttributionEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool AttributionEnabled
        {
            get => (bool) GetValue(AttributionEnabledProperty);
            set => SetValue(AttributionEnabledProperty, value);
        }

        public static readonly BindableProperty AttributionGravityProperty = BindableProperty.Create(
            nameof(AttributionGravity),
            typeof(Gravity),
            typeof(MapView),
            Gravity.Bottom | Gravity.Start,
            BindingMode.OneWay
        );

        public Gravity AttributionGravity
        {
            get => (Gravity) GetValue(AttributionGravityProperty);
            set => SetValue(AttributionGravityProperty, value);
        }

        public static readonly BindableProperty AttributionMarginProperty = BindableProperty.Create(
            nameof(AttributionMargin),
            typeof(Thickness),
            typeof(MapView),
            new Thickness(92, 4, 4, 4),
            BindingMode.OneWay
        );

        public Thickness AttributionMargin
        {
            get => (Thickness) GetValue(AttributionMarginProperty);
            set => SetValue(AttributionMarginProperty, value);
        }

        public static readonly BindableProperty RenderTextureModeProperty = BindableProperty.Create(
            nameof(RenderTextureMode),
            typeof(bool),
            typeof(MapView),
            false,
            BindingMode.OneWay
        );

        public bool RenderTextureMode
        {
            get => (bool) GetValue(RenderTextureModeProperty);
            set => SetValue(RenderTextureModeProperty, value);
        }

        public static readonly BindableProperty RenderTextureTranslucentSurfaceProperty = BindableProperty.Create(
            nameof(RenderTextureTranslucentSurface),
            typeof(bool),
            typeof(MapView),
            false,
            BindingMode.OneWay
        );

        public bool RenderTextureTranslucentSurface
        {
            get => (bool) GetValue(RenderTextureTranslucentSurfaceProperty);
            set => SetValue(RenderTextureTranslucentSurfaceProperty, value);
        }

        public static readonly BindableProperty TilePrefetchEnabledProperty = BindableProperty.Create(
            nameof(TilePrefetchEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool TilePrefetchEnabled
        {
            get => (bool) GetValue(TilePrefetchEnabledProperty);
            set => SetValue(TilePrefetchEnabledProperty, value);
        }

        public static readonly BindableProperty ZMediaOverlayEnabledProperty = BindableProperty.Create(
            nameof(ZMediaOverlayEnabled),
            typeof(bool),
            typeof(MapView),
            false,
            BindingMode.OneWay
        );

        public bool ZMediaOverlayEnabled
        {
            get => (bool) GetValue(ZMediaOverlayEnabledProperty);
            set => SetValue(ZMediaOverlayEnabledProperty, value);
        }

        public static readonly BindableProperty LocalIdeographFontFamilyEnabledProperty = BindableProperty.Create(
            nameof(LocalIdeographFontFamilyEnabled),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool LocalIdeographFontFamilyEnabled
        {
            get => (bool) GetValue(LocalIdeographFontFamilyEnabledProperty);
            set => SetValue(LocalIdeographFontFamilyEnabledProperty, value);
        }

        public static readonly BindableProperty LocalIdeographFontFamiliesProperty = BindableProperty.Create(
            nameof(LocalIdeographFontFamilies),
            typeof(string[]),
            typeof(MapView),
            default(string[]),
            BindingMode.OneWay
        );

        public string[] LocalIdeographFontFamilies
        {
            get => (string[]) GetValue(LocalIdeographFontFamiliesProperty);
            set => SetValue(LocalIdeographFontFamiliesProperty, value);
        }

        public static readonly BindableProperty PixelRatioProperty = BindableProperty.Create(
            nameof(PixelRatio),
            typeof(float),
            typeof(MapView),
            default(float),
            BindingMode.OneWay
        );

        public float PixelRatio
        {
            get => (float) GetValue(PixelRatioProperty);
            set => SetValue(PixelRatioProperty, value);
        }

        public static readonly BindableProperty ForegroundLoadColorProperty = BindableProperty.Create(
            nameof(ForegroundLoadColor),
            typeof(Color),
            typeof(MapView),
            Color.Default,
            BindingMode.OneWay
        );

        public Color ForegroundLoadColor
        {
            get => (Color) GetValue(ForegroundLoadColorProperty);
            set => SetValue(ForegroundLoadColorProperty, value);
        }

        public static readonly BindableProperty CrossSourceCollisionsProperty = BindableProperty.Create(
            nameof(CrossSourceCollisions),
            typeof(bool),
            typeof(MapView),
            true,
            BindingMode.OneWay
        );

        public bool CrossSourceCollisions
        {
            get => (bool) GetValue(CrossSourceCollisionsProperty);
            set => SetValue(CrossSourceCollisionsProperty, value);
        }
    }
}