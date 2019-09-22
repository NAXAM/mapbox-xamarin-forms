using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesChangeLayerColorPage : ContentPage
    {
        public StylesChangeLayerColorPage()
        {
            InitializeComponent();

            map.Center = new LatLng(45.4385, 12.338);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 17.4;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var waterLayer = new FillLayer("water", "built-in");
            var buildingLayer = new FillLayer("building", "built-in");
            var selectedLayerIndex = 0;

            picker.ItemsSource = new[] { "Water", "Building" };
            picker.SelectedIndexChanged += (sender, e) => {
                if (selectedLayerIndex == picker.SelectedIndex) return;

                selectedLayerIndex = picker.SelectedIndex;
                var expression = Expression.Rgb((int)red.Value, (int)green.Value, (int)blue.Value);
                switch (selectedLayerIndex)
                {
                    case 0:
                        waterLayer.FillColor = expression;
                        map.Functions.UpdateLayer(waterLayer);
                        break;
                    case 1:
                        buildingLayer.FillColor = expression;
                        map.Functions.UpdateLayer(buildingLayer);
                        break;

                }
            };
            picker.SelectedIndex = 0;

            red.ValueChanged += SliderValueChanged;
            green.ValueChanged += SliderValueChanged;
            blue.ValueChanged += SliderValueChanged;


            void SliderValueChanged(object sender, ValueChangedEventArgs e)
            {
                var expression = Expression.Rgb((int)red.Value, (int)green.Value, (int)blue.Value);
                switch (selectedLayerIndex)
                {
                    case 0:
                        waterLayer.FillColor = expression;
                        map.Functions.UpdateLayer(waterLayer);
                        break;
                    case 1:
                        buildingLayer.FillColor = expression;
                        map.Functions.UpdateLayer(buildingLayer);
                        break;

                }
            };
        }
    }
}