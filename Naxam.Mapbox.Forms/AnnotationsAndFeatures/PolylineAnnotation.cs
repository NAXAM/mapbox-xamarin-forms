using System;
namespace Naxam.Mapbox.Forms
{
	public class PolylineAnnotation: Annotation
	{
		public PolylineAnnotation()
		{
		}

		public Position[] Coordinates
		{
			get;
			set;
		}
	}
}
