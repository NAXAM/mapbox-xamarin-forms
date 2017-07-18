using System;
using Naxam.Controls.Forms;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

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

            map.DidTapOnMapCommand = new Command<Tuple<Position, Point>>((Tuple<Position, Point> obj) =>
            {
                var features = map.GetFeaturesAroundPoint.Invoke(obj.Item2, 6, null);
                var filtered = features.Where((arg) => arg.Attributes != null);
                foreach (IFeature feat in filtered) {
                    var str = JsonConvert.SerializeObject(feat);
        			System.Diagnostics.Debug.WriteLine(str);
                }

            });
            map.DidFinishLoadingStyleCommand = new Command<MapStyle>((obj) =>
            {
                foreach (Layer layer in obj.OriginalLayers)
				{
                    System.Diagnostics.Debug.WriteLine(layer.Id);
				}
            });
        }

        void ReloadStyle(object sender, EventArgs args)
        {
            map.ReloadStyleFunc?.Execute(sender);
        }

        void AddPolygon(object sender, EventArgs args)
        {
            var polygonCoords = new[] {
                new Position {
                    Lat = 21.0233,
                    Long = 105.7400
                },
                new Position {
                    Lat = 21.0233,
                    Long = 105.9600
                },
                new Position {
                    Lat = 21.0433,
                    Long = 105.9600
                },
                new Position {
                    Lat = 21.0433,
                    Long = 105.7400
                },
                new Position {
                    Lat = 21.0233,
                    Long = 105.7400
                },
            };

            var polygon = new PolylineAnnotation
            {
                Coordinates = polygonCoords,
                Title = "Polyline",
            };

            var annotations = new List<Annotation>();

            annotations.Add(polygon);

            map.Annotations = annotations;
        }
    }
}
