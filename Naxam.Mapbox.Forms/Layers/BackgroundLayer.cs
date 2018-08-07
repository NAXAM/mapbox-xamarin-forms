using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text;

namespace Naxam.Controls.Mapbox.Forms
{
    public class BackgroundLayer : StyleLayer
    {
        public Color BackgroundColor = Color.White;

        public BackgroundLayer(string id, string sourceId) : base(id, sourceId)
        {
        }
    }
}
