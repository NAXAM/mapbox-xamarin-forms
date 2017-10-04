using System;
using Xamarin.Forms;

namespace Naxam.Controls.Mapbox.Forms
{
    public class FillLayer: StyleLayer
    {
        public FillLayer(string id, string sourceId) : base(id, sourceId)
        {
        }

        public Color FillColor = Color.Gray;

        private double fillOpactity = 0.8;
		public double FillOpacity
		{
			get => fillOpactity;
			set
			{
				fillOpactity = Math.Min(1.0, Math.Max(value, 0.0));
			}
		}
    }
}
