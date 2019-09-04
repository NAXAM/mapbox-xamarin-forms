using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
    public class BackgroundLayer : StyleLayer
    {
        public Color BackgroundColor { get; set; } = Color.White;

        public BackgroundLayer(string id, string sourceId) : base(id, sourceId)
        {
        }
    }
}
