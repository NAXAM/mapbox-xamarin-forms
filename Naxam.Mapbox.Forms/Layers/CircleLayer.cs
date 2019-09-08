using System;
using Xamarin.Forms;

namespace Naxam.Mapbox.Layers
{
    public class CircleLayer : StyleLayer
    {
        public Color CircleColor = Color.Red;

        private double circleRadius = 5.0;
        public double CircleRadius
        {
            get => circleRadius;
            set
            {
                circleRadius = Math.Max(value, 0.0);
            }
        }

        private double circleOpacity = 0.8;
        public double CircleOpacity
        {
            get => circleOpacity;
            set
            {
                circleOpacity = Math.Min(1.0, Math.Max(value, 0.0));
            }
        }

        public Color? StrokeColor { get; set; }

        private double strokeWidth = 0.0;
        public double StrokeWidth
        {
            get => strokeWidth;
            set
            {
                strokeWidth = Math.Max(value, 0.0);
            }
        }

        private double strokeOpacity = 1.0;
        public double StrokeOpacity
        {
            get => strokeOpacity;
            set
            {
                strokeOpacity = Math.Min(1.0, Math.Max(value, 0.0));
            }
        }

        public CircleLayer(string id, string sourceId) : base(id, sourceId)
        {
        }
    }
}
