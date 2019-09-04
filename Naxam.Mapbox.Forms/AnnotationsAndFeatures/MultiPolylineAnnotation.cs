using System;
namespace Naxam.Controls.Forms
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