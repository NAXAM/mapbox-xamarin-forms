
using Naxam.Mapbox.Expressions;

namespace Naxam.Mapbox.Layers
{
    public class RasterLayer: StyleLayer
    {
        public Expression RasterBrightnessMax { get; set; }
        public TransitionOptions RasterBrightnessMaxTransition { get; set; }

        public Expression RasterBrightnessMin { get; set; }
        public TransitionOptions RasterBrightnessMinTransition { get; set; }

        public Expression RasterContrast { get; set; }
        public TransitionOptions RasterContrastTransition { get; set; }

        public Expression RasterFadeDuration { get; set; }
        public Expression RasterHueRotate { get; set; }
        public TransitionOptions RasterHueRotateTransition { get; set; }

        public Expression RasterOpacity { get; set; }
        public TransitionOptions RasterOpacityTransition { get; set; }

        public Expression RasterResampling { get; set; }
        public Expression RasterSaturation { get; set; }
        public TransitionOptions RasterSaturationTransition { get; set; }

        public RasterLayer(string id, string sourceId) : base(id, sourceId) {}
    }
}
