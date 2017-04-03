using System;
namespace Naxam.Mapbox.Forms
{
	public class MultiPolylineAnnotation: Annotation
	{
		public MultiPolylineAnnotation()
		{
		}

		public Position[][] Coordinates
		{
			get;
			set;
		}

	}
}