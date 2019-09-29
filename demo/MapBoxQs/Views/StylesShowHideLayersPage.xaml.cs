
using System;
using Naxam.Controls.Forms;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;

namespace MapBoxQs.Views
{
    public partial class StylesShowHideLayersPage : ContentPage
    {
        public StylesShowHideLayersPage()
        {
            InitializeComponent();
            map.Center = new Naxam.Mapbox.LatLng(-13.517379300798098, -71.97722138410576);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 15;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private bool visible;
        private CircleLayer museumsLayer;
        private void HandleStyleLoaded(MapStyle obj)
        {
            var source = new VectorSource("museums_source", "mapbox://mapbox.2opop9hr");
            map.Functions.AddSource(source);

            museumsLayer = new CircleLayer("museums", "museums_source")
            {
                SourceLayer = "museum-cusco",
                Visibility = Expression.Visibility(true),
                CircleRadius = 8,
                CircleColor = Expression.Rgb(55, 148, 179)
            };

            map.Functions.AddLayer(museumsLayer);
        }

        private void MenuItem_OnClicked(object sender, EventArgs e)
        {
            if (museumsLayer == null) return;

            museumsLayer.Visibility = Expression.Visibility(visible = !visible);

            map.Functions.UpdateLayer(museumsLayer);
        }
    }
}
