using System;
using System.Collections.Generic;

namespace Naxam.Mapbox.Forms
{
	public class PointFeature: PointAnnotation, Feature
	{
		public PointFeature()
		{
		}

		public Dictionary<string, object> Attributes
		{
			get;set;
		}
	}
}
