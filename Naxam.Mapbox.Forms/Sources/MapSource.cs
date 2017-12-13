using System;
using Xamarin.Forms;
namespace Naxam.Controls.Mapbox.Forms
{
    public class MapSource: BindableObject
    {
        public MapSource()
        {
        }

        public MapSource(string id)
        {
            Id = id;
        }

        public string Id
        {
            get;
            set;
        }

    }
}
