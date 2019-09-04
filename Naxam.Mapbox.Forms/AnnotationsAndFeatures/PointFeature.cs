using System;
using System.Collections.Generic;

namespace Naxam.Controls.Forms
{
	public class PointFeature: PointAnnotation, IFeature
	{
		public PointFeature()
		{
		}

		public PointFeature(PointAnnotation annotation)
		{
			Id = annotation.Id;
			Title = annotation.Title;
			SubTitle = annotation.SubTitle;
			Coordinate = annotation.Coordinate;
		}

		public Dictionary<string, object> Attributes
		{
			get;set;
		}
	}
}
