using System;
using System.Collections.Generic;

namespace Naxam.Controls.Mapbox.Forms
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
