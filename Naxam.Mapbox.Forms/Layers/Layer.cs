using System;

namespace Naxam.Controls.Forms
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
}
