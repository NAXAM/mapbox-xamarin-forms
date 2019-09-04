using System;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
    public enum MBLineCap : ulong
    {
        Butt,
        Round,
        Square
    }

    public class LineLayer : StyleLayer
    {
        public LineLayer(string id, string sourceId) : base(id, sourceId)
        {
        }

        public MBLineCap LineCap { get; set; } = MBLineCap.Round;

        public Color LineColor = Color.Red;

        private double lineOpacity = 1.0;
        public double LineOpacity
        {
            get => lineOpacity;
            set
            {
                lineOpacity = Math.Min(1.0, Math.Max(value, 0.0));
            }
        }

        private double lineWidth = 1.0;
        public double LineWidth
        {
            get => lineWidth;
            set
            {
                lineWidth = Math.Max(value, 0.0);
            }
        }

        public double[] Dashes { get; set; }
    }
}
