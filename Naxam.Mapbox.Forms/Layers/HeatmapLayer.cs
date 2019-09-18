using Naxam.Mapbox.Expressions;

namespace Naxam.Mapbox.Layers
{
    public class HeatmapLayer : StyleLayer
    {
        public Expression HeatmapColor { get; set; }
        public TransitionOptions HeatmapColorTransition { get; set; }

        public Expression HeatmapIntensity { get; set; }
        public TransitionOptions HeatmapIntensityTransition { get; set; }

        public Expression HeatmapOpacity { get; set; }
        public TransitionOptions HeatmapOpacityTransition { get; set; }

        public Expression HeatmapRadius { get; set; }
        public TransitionOptions HeatmapRadiusTransition { get; set; }

        public Expression HeatmapWeight { get; set; }

        public HeatmapLayer(string id, string sourceId) : base(id, sourceId)
        {
        }
    }
}