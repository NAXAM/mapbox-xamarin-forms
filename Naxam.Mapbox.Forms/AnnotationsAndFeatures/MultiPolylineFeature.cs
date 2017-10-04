using System;
using System.Collections.Generic;

namespace Naxam.Controls.Mapbox.Forms
{
	public class MultiPolylineFeature: MultiPolylineAnnotation, IFeature
	{
		public MultiPolylineFeature()
		{
		}

		public MultiPolylineFeature(MultiPolylineAnnotation annotation)
		{
			Id = annotation.Id;
			Title = annotation.Title;
			SubTitle = annotation.SubTitle;
			Coordinates = annotation.Coordinates;
		}

		public Dictionary<string, object> Attributes
		{
			get; set;
		}
	}
}
