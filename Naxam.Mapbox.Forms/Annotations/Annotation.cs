using System.Collections.Generic;

namespace Naxam.Mapbox.Annotations
{
	public abstract class Annotation
    {
        public string HandleId { get; set; }

		public string Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Tooltip { get; set; }

        public Dictionary<string, object> Properties { get; private set; } = new Dictionary<string, object>();
    }
}
