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

    public class LineLayer : Layer
    {
        public LineLayer (string id, string sourceId) : base (id)
        {
            SourceId = sourceId;
            LineCap = MBLineCap.Round;
            LineColor = Color.Red;
            LineOpacity = 1.0;
            LineWidth = 1.0;
        }

        public string SourceId {
            get;
            private set;
        }

        public MBLineCap LineCap {
            get;
            set;
        }

        public Color LineColor {
            get;
            set;
        }

        public double LineOpacity {
            get;
            set;
        }

        public double LineWidth {
            get;
            set;
        }
    }
}
