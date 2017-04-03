using System;
namespace Naxam.Mapbox.Forms
{
	public class PointAnnotation: Annotation
	{
		public PointAnnotation()
		{
		}

		public Position Coordinate
		{
			get;
			set;
		}
	}
}
