using System;
using System.Collections.Generic;

namespace Naxam.Mapbox.Forms
{
	public interface IFeature
	{
		Dictionary<string, object> Attributes
		{
			get;
			set;
		}
	}
}
