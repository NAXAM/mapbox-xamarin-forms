using System;
namespace Naxam.Controls.Forms
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
        public string Icon { get; set; }
    }
}
