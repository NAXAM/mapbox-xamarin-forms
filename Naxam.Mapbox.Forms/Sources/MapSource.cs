using System;
using Xamarin.Forms;
namespace Naxam.Controls.Forms
{
    public class MapSource: BindableObject
    {
        public string Id { get; set; }

        public MapSource()
        {
        }

        public MapSource(string id)
        {
            Id = id;
        }
    }
}
