using System;
using System.Timers;
using GeoJSON.Net.Geometry;
using Naxam.Controls;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtrusionAdjust : ContentPage
    {
        public ExtrusionAdjust()
        {
            InitializeComponent();

            map.Center = new LatLng(40.7135, -74.0066);
            map.MapStyle = MapStyle.DARK;
            map.ZoomLevel = 16;
            map.Pitch = 45;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        
        private void HandleStyleLoaded(MapStyle obj)
        {
            var fillExtrusionLayer = new FillExtrusionLayer("3d-buildings", "composite")
            {
                SourceLayer = "building",
                Filter = Expression.Eq(Expression.Get("extrude"), "true"),
                MinZoom = 15,
                FillExtrusionColor = Color.LightGray,
                FillExtrusionHeight = Expression.Interpolate(
                    Expression.Exponential(1f),
                    Expression.Zoom(),
                    Expression.CreateStop(15, (0)),
                    Expression.CreateStop(16, Expression.Get("height"))
                ),
                FillExtrusionBase = Expression.Get("min_height"),
                FillExtrusionOpacity = 0.9f
            };

            map.Functions.AddLayer(fillExtrusionLayer);
            
            map.Functions.AnimateCamera(map.Camera, 1000);
        }

        private bool isInitPosition;
        private void ChangeLight(object sender, EventArgs e)
        {
            if (map.Functions == null) return;

            isInitPosition = !isInitPosition;
            
            map.Functions.UpdateLight(new Light
            {
                Position = isInitPosition 
                    ? new LightPosition(1.5f, 90, 80) 
                    : new LightPosition(1.15f, 210, 30)
            });
        }
        
        private bool isRedColor;
        private void ChangeColor(object sender, EventArgs e)
        {
            if (map.Functions == null) return;

            isRedColor = !isRedColor;
            
            map.Functions.UpdateLight(new Light
            {
                Color = isRedColor ? Color.Red : Color.Blue
            });
        }
        
        private bool isMapAnchorLight;
        private void ChangeAnchor(object sender, EventArgs e)
        {
            if (map.Functions == null) return;

            isMapAnchorLight = !isMapAnchorLight;
            
            map.Functions.UpdateLight(new Light
            {
                Anchor = isMapAnchorLight ? LayerProperty.ANCHOR_MAP : LayerProperty.ANCHOR_VIEWPORT
            });
        }
        
        
        private bool isLowIntensityLight;
        private void ChangeIntensity(object sender, EventArgs e)
        {
            if (map.Functions == null) return;

            isLowIntensityLight = !isLowIntensityLight;
            
            map.Functions.UpdateLight(new Light
            {
                Intensity = isLowIntensityLight ?  0.35f : 1.0f
            });
        }
    }
}