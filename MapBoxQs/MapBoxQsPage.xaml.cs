using System;
using Naxam.Controls.Forms;
using Newtonsoft.Json;
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

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>((MapStyle obj) =>
           {
                map.ResetPositionFunc.Execute(null);
           });

            map.MapStyle = new MapStyle()
            {
                Id = "ciyxczsj9004b2rtoji7t5hkj",
                Owner = "jesperdax"
            };
            map.DidTapOnMapCommand = new Command<Tuple<Position, Point>>((Tuple<Position, Point> obj) =>
            {
                var features = map.GetFeaturesAroundPoint.Invoke(obj.Item2, 6, null);
                var str = JsonConvert.SerializeObject(features);
                System.Diagnostics.Debug.WriteLine(str);
            });
        }
    }


}
