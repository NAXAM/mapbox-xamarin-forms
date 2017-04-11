using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Naxam.Mapbox.Forms
{ 
	public sealed class PreserveAttribute : System.Attribute
	{
		public bool AllMembers;
		public bool Conditional;
	}

	[Preserve(AllMembers = true)]
	public class MapStyle: BindableObject
	{
			public string Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Owner
		{
			get;
			set;
		}

		public double[] Center
		{
			get;
			set;
		}

		public string UrlString
		{
			get {
				if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Owner))
				{
					
					return "mapbox://styles/" + Owner + "/" + Id;
				}
				return null;
			}
		}

		public MapStyle()
		{
		}

		public MapStyle(string id, string name,double[] center = null, string owner = null)
		{
			Id = id;
			Name = name;
		    Center = center;
			Owner = owner;
		}

		public MapStyle(string urlString)
		{
			UpdateIdAndOwner(urlString);
		}

		public void SetUrl(string urlString)
		{
			UpdateIdAndOwner(urlString);
		}

		void UpdateIdAndOwner(string urlString)
		{
			if (!string.IsNullOrEmpty(urlString))
			{
				var segments = (new Uri(urlString)).Segments;
				if (string.IsNullOrEmpty(Id) && segments.Length != 0)
				{
					Id = segments[segments.Length - 1].Trim('/');
				}
				if (string.IsNullOrEmpty(Owner) && segments.Length > 1)
				{
					Owner = segments[segments.Length - 2].Trim('/');
				}
			}
		}

        public static readonly BindableProperty CustomSourcesProperty = BindableProperty.Create (
        		   nameof (CustomSources),
           typeof (IEnumerable<ShapeSource>),
           typeof (MapView),
           default (IEnumerable<ShapeSource>),
        		   BindingMode.TwoWay);

        public IEnumerable<ShapeSource> CustomSources {
        	get {
        		return (IEnumerable<ShapeSource>)GetValue (CustomSourcesProperty);
        	}
        	set {
        		SetValue (CustomSourcesProperty, (IList<ShapeSource>)value);
            }
        }
	}
}
