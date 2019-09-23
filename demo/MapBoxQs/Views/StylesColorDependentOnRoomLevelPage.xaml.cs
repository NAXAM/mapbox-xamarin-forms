using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesColorDependentOnRoomLevelPage : ContentPage
    {
        public StylesColorDependentOnRoomLevelPage()
        {
            InitializeComponent();
            
            map.Center = new LatLng(40.73581, -73.99155);
            map.ZoomLevel = 0;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var layer =new FillLayer("water", "built-in") {
                FillColor = Expression.Interpolate(
                    Expression.Exponential(1),
                    Expression.Zoom(),
                    Expression.CreateStop(1f, Expression.Rgb(0,209,22)),
                    Expression.CreateStop(8.5f, Expression.Rgb(10,88,255)),
                    Expression.CreateStop(10f, Expression.Rgb(255,10,10)),
                    Expression.CreateStop(18f, Expression.Rgb(251,255,0))
                )
            };

            map.Functions.UpdateLayer(layer);

            map.Functions.AnimiateCamera(map.Center, 12, 12000);
        }
    }
}