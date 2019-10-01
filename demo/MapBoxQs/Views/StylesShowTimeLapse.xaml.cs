using System.Timers;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesShowTimeLapse : ContentPage
    {
        public StylesShowTimeLapse()
        {
            InitializeComponent();

            map.Center = new LatLng(40.879, -76.476);
            map.MapStyle = MapStyle.DARK;
            map.ZoomLevel = 4.3;

            // TODO iOS not showing image yet
            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private System.Threading.Timer timer;
        private void HandleStyleLoaded(MapStyle obj)
        {
            string ID_IMAGE_SOURCE = "animated_image_source";
            string ID_IMAGE_LAYER = "animated_image_layer";

            LatLngQuad quad = new LatLngQuad(
                new LatLng(46.437, -80.425),
                new LatLng(46.437, -71.516),
                new LatLng(37.936, -71.516),
                new LatLng(37.936, -80.425)
            );

            // Add an ImageSource to the map
            var src = new MapboxImageSource(ID_IMAGE_SOURCE, quad, "southeast_radar_0.png");
            map.Functions.AddSource(src);

            // Create a raster layer and use the imageSource's ID as the layer's data. Then add a RasterLayer to the map.
            map.Functions.AddLayer(new RasterLayer(ID_IMAGE_LAYER, ID_IMAGE_SOURCE));

            int i = 0;
            timer = new System.Threading.Timer(x =>
             {
                 Device.BeginInvokeOnMainThread(() =>
                 {
                     map.Functions?.UpdateSource(src.Id, $"southeast_radar_{i}.png");
                 });
                 i++;

                 if (i > 3)
                 {
                     i = 0;
                 }
             }, i, 0, 100);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            timer?.Change(int.MaxValue, int.MaxValue);
            timer?.Dispose();
        }
    }
}