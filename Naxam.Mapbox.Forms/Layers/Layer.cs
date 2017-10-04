using System;

namespace Naxam.Controls.Mapbox.Forms
{
    public class Layer
    {
        public Layer (string id)
        {
            Id = id;
            IsVisible = true;
        }

        public string Id {
            get;
            set;
        }

        public bool IsVisible {
            get;
            set;
        }
    }

    public class StyleLayer: Layer {
	    public StyleLayer(string id, string sourceId) : base(id)
        {
            SourceId = sourceId;
		}
		public string SourceId
		{
			get;
			private set;
		}
    }
}