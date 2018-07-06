using System;
namespace Naxam.Controls.Mapbox.Forms
{
	public class PointAnnotation: Annotation
	{
		public PointAnnotation()
		{
		}

		public EventHandler<string> FinishDragged { get; set; }

		public Position Coordinate
		{
			get;
			set;
		}
	}
}
