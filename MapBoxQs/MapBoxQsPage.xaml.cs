using System;
using Naxam.Controls.Forms;
using Xamarin.Forms;

namespace MapBoxQs
{
    public partial class MapBoxQsPage : ContentPage
    {
        public MapBoxQsPage()
        {
            InitializeComponent();

            var positions = new[] { 
                new Position {
					Lat = 21.0333,
					Long = 105.8500
                },
				new Position {
					Lat = 31.0333,
					Long = 105.8500
				}
            };

            var random = new Random();

            btnChangeLocation.Clicked += delegate {
                map.Center = positions[random.Next(2)%2];
            };
        }
    }


}
