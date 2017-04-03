using System;
using System.Collections.Generic;

namespace Naxam.Mapbox.Forms
{ 
	public sealed class PreserveAttribute : System.Attribute
	{
		public bool AllMembers;
		public bool Conditional;
	}

	[Preserve(AllMembers = true)]
	public class MapStyle
	{
		//"version": 8,
  //  "name": "Dark",
  //  "center": [
  //    0,
  //    -1.1368683772161603e-13
  //  ],
  //  "zoom": 0.8233671266790495,
  //  "bearing": 0,
  //  "pitch": 0,
  //  "created": "2017-02-21T07:28:33.252Z",
  //  "id": "cizf7m12200ik2spm99ack2pz",
  //  "modified": "2017-02-21T07:30:09.337Z",
  //  "owner": "hongha",
  //  "visibility": "private"

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
        get;
        set;
    }

//		public MapLayer[] Layers
//		{
//			get;
//			set;
//		}

		public MapStyle()
		{
		}

		public MapStyle(string id, string name,double[] center, string urlString, string owner)
		{
			Id = id;
			Name = name;
		    Center = center;
		    UrlString = urlString;
		    Owner = owner;
		}
	}
}
