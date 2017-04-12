using System;
using Xamarin.Forms;

namespace Naxam.Mapbox.Forms
{
    public class CircleLayer: Layer
    {
        public CircleLayer (string id, string sourceId): base(id)
        {
            SourceId = sourceId;
            CircleColor = Color.Red;
            CircleRadius = 5.0;
            CircleOpacity = 0.8;
        }

        public string SourceId {
            get;
            private set;
        }

        public Color CircleColor {
            get;
            set;
        }

        public double CircleRadius {
            get;
            set;
        }

        public double CircleOpacity {
            get;
            set;
        }
    }
}
