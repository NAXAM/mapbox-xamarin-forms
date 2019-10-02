using System.Timers;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesTransparentRenderSurface : ContentPage
    {
        public StylesTransparentRenderSurface()
        {
            InitializeComponent();

            map.Center = new LatLng(48.507879, 8.363795);
            map.MapStyle = "asset://no_bg_style.json";
            map.ZoomLevel = 2;
            map.ZoomMinLevel = 1.3;
            map.RenderTextureMode = true;
            map.RenderTextureTranslucentSurface = true;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            
        }
    }
}