using System;
namespace Naxam.Controls.Mapbox.Forms
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