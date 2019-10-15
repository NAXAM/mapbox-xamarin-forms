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
    public partial class ExtrusionIndoor3DMap : ContentPage
    {
        public ExtrusionIndoor3DMap()
        {
            InitializeComponent();

            map.Center = new Naxam.Mapbox.LatLng(41.86625, -87.61694);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 15.99;
            map.Heading = 20;
            map.Pitch = 40;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.AddSource(new GeoJsonSource("room-data", "asset://indoor-3d-map.geojson"));

            map.Functions.AddLayer(new FillExtrusionLayer(
                "room-extrusion", "room-data"){
                FillExtrusionColor = Expression.Get("color"),
                FillExtrusionHeight = Expression.Get("height"),
                FillExtrusionBase = Expression.Get("base_height"),
                FillExtrusionOpacity = 0.5f
            });
            
            map.Functions.AnimateCamera(map.Camera, 2000);
        }
    }
}