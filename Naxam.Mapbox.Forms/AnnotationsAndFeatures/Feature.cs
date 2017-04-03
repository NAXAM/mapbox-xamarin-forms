using System;
using System.Collections.Generic;

namespace Naxam.Mapbox.Forms
{
	public interface Feature
	{
		Dictionary<string, object> Attributes
		{
			get;
			set;
		}
	}
}
