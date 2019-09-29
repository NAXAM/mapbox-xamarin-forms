
using System;
using Naxam.Controls.Forms;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;

namespace MapBoxQs.Views
{
    public partial class StylesMapboxStudio : ContentPage
    {
        public StylesMapboxStudio()
        {
            InitializeComponent();
            map.Center = new Naxam.Mapbox.LatLng(41.89010, 12.49200);
            map.MapStyle = "mapbox://styles/mapbox/cj3kbeqzo00022smj7akz3o1e";
            map.ZoomLevel = 15;
        }

    }
}
