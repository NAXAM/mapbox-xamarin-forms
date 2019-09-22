using System;
using System.Collections.Generic;
using System.Text;

namespace Naxam.Mapbox.Sources
{
    public class VectorSource : Source
    {
        public string Url { get; set; }

        public VectorSource(string id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}
