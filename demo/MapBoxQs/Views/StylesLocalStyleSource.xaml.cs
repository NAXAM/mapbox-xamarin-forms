
using System;
using Naxam.Controls.Forms;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;

namespace MapBoxQs.Views
{
    public partial class StylesLocalStyleSource : ContentPage
    {
        public StylesLocalStyleSource()
        {
            InitializeComponent();
            map.Center = new Naxam.Mapbox.LatLng(8.853067, -73.846880);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 3.15;
        }
        
        private void LoadRasterStyle(object sender, EventArgs e)
        {
            if (map.Functions == null) return;

            map.MapStyle = "https://www.mapbox.com/android-docs/files/mapbox-raster-v8.json";
        }

        private void LoadLocalStyle(object sender, EventArgs e)
        {
            if (map.Functions == null) return;

            map.MapStyle = "asset://local_style_file.json";
        }
    }
}
